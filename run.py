import psycopg2
import json
import sys
import datetime
import argparse
from pathlib import Path

parser = argparse.ArgumentParser(description='')
parser.add_argument('-v', '--verbose', action='store_true', help='Enable verbose output')
parser.add_argument('--subreddit-name', required=True, help='Subreddit name. Submissions and comments are loaded by this name')

args = parser.parse_args()

def verbose(msg):
    if (args.verbose == True):
        print('VERBOSE: {0}'.format(msg))

# check for the submission file
SUBMISSIONS_FILE = f"{args.subreddit_name}_submissions"
sub_f = Path(SUBMISSIONS_FILE).resolve()
if (sub_f.is_file() == False):
    print(f"ERROR: failed to load submission file at {sub_f}")
    sys.exit(1)

# check for the comments file
COMMENTS_FILE = f"{args.subreddit_name}_comments"
comment_f = Path(SUBMISSIONS_FILE).resolve()
if (comment_f.is_file() == False):
    print(f"ERROR: failed to load comment file at {comment_f}")
    sys.exit(1)

DB_SETTINGS_FILE = "db.json"
db_f = Path(DB_SETTINGS_FILE).resolve()
if (db_f.is_file() == False):
    print(f"ERROR: failed to find db.json. Make sure you rename the template file to match this!")
    sys.exit(1)

db_settings = None
with open(DB_SETTINGS_FILE, encoding="UTF-8") as f:
    db_str = f.read()
    verbose(f"db input: {db_str}")
    db_settings = json.loads(db_str)
    verbose(f"DB settings: {db_settings}")

if (db_settings == None):
    print(f"ERROR: db_settings is still None")
    sys.exit(1)

db_database = db_settings.get("database")
db_host = db_settings.get("host")
db_user = db_settings.get("user")
db_password = db_settings.get("password")
db_port = db_settings.get("port")

db_errors = []
if (db_database == None):
    db_errors.append(f"missing field 'database'")
if (db_host == None):
    db_errors.append(f"missing field 'host'")
if (db_user == None):
    db_errors.append(f"missing field 'user'")
if (db_password == None):
    db_errors.append(f"missing field 'password'")
if (db_port == None):
    db_errors.append(f"missing field 'port'")

if (len(db_errors) > 0):
    print(f"ERROR: errors with db.json:")
    for err in db_errors:
        print(f"\t{err}")
    sys.exit(1)

print(f"Connecting to database: {db_user}@{db_host}:{db_port}/{db_database}")

conn = psycopg2.connect(
    database=db_settings.get("database"),
    host=db_settings.get("host"),
    user=db_settings.get("user"),
    password=db_settings.get("password"),
    port=int(db_settings.get("port"))
)

def check_for_table(table_name):
    print(f"Checking if there is already rows in table {table_name}")
    try:
        cursor = conn.cursor()
        cursor.execute(f"SELECT 1 AS exists FROM {table_name} LIMIT 1;")
        result = cursor.fetchone()
        verbose(f"check_for_table({table_name}): {result}")
        cursor.close()
        return result != None
    except Exception as ex:
        print(f"Error checking for submission table. Is is created?\n{ex}")
        return False

# check to ensure all tables are empty
if (check_for_table("submissions") == True):
    print(f"ERROR: rows already exist in table 'submission'! Remove all rows from this table")
    sys.exit(1)

if (check_for_table("comments") == True):
    print(f"ERROR: rows already exist in table 'comments'! Remove all rows from this table")
    sys.exit(1)

# insert submissions
count = 0
with open(SUBMISSIONS_FILE, encoding="UTF-8") as subs:
    line = subs.readline()
    while line:
        j = json.loads(line)
        id = j.get("id")
        title = j.get("title")
        author = j.get("author")
        content = j.get("selftext") or j.get("url")
        posted_at = j.get("created") or j.get("created_utc") or j.get("retrieved_on")

        if not id:
            print(f"failed to find 'id' in:\n{j}")
            sys.exit(-1)
        if not title:
            print(f"Failed to find 'title' in:\n{j}")
            sys.exit(-1)
        if not author:
            print(f"Failed to find 'author' in:\n{j}")
            sys.exit(-1)
        if not content:
            print(f"Failed to find 'selftext' in:\n{j}")
            sys.exit(-1)
        if not posted_at:
            print(f"Failed to find 'created' in:\n{j}")
            sys.exit(-1)
        count += 1

        cursor = conn.cursor()
        cursor.execute("INSERT INTO submissions (id, title, posted_at, author, content, data) VALUES (%s, %s, %s, %s, %s, %s)",
            (id, title, datetime.datetime.fromtimestamp(posted_at), author, content, json.dumps(j))
        )
        conn.commit()
        cursor.close()

        if (count % 10000 == 0):
            print(f"Inserted {count} submissions")

        line = subs.readline()

# insert comments
count = 0
with open(COMMENTS_FILE, encoding="UTF-8") as comments:
    line = comments.readline()
    while line:
        count += 1

        j = json.loads(line)

        id = j.get("id")
        parent_id = j.get("parent_id")
        link_id = j.get("link_id")
        author = j.get("author")
        content = j.get("body")
        posted_at = int(j.get("created") or j.get("created_utc") or j.get("retrieved_on"))
        if not id:
            print(f"failed to find 'id' in:\n{j}")
            sys.exit(-1)
        if not author:
            print(f"Failed to find 'author' in:\n{j}")
            sys.exit(-1)
        if not content and content != "":
            print(f"Failed to find 'content' in:\n{j}")
            sys.exit(-1)
        if not posted_at or type(posted_at) is not int:
            print(f"Failed to find 'created' in:\n{j}")
            sys.exit(-1)
        if not link_id:
            print(f"Failed to find 'link_id' in:\n{j}")
            sys.exit(-1)

        if (count % 10000 == 0):
            print(f"Inserted {count} comments")

        try:
            cursor = conn.cursor()
            cursor.execute("INSERT INTO comments (id, parent_id, link_id, posted_at, author, content, data) VALUES (%s, %s, %s, %s, %s, %s, %s)",
                (id, parent_id, link_id, datetime.datetime.fromtimestamp(posted_at), author, content, json.dumps(j))
            )
            conn.commit()
        except Exception as e:
            print(f"Error updating {id}: {str(e)}\n{e}")
            if (str(e).startswith("duplicate key value violates unique constraint \"comments_pkey\"") == False):
                input("continue")
            else:
                # comments file has duplicate comments for some reason?
                print(f"duplicate comment {id}, skipping")
        finally:
            conn.rollback()
            cursor.close()

        line = comments.readline()

