# subreddit-viewer
View an archive of a subreddit (some assembly required)

## Setup

Download the subreddit from https://reddit-top20k.cworld.ai/

Extract the .zst files using https://github.com/facebook/zstd/releases

### Create the tables

The database instance is Postgres 14. Other versions probably work as the features used are pretty basic

```sql
CREATE TABLE IF NOT EXISTS submissions (
	id varchar NOT NULL PRIMARY KEY,
	title varchar NOT NULL,
	posted_at timestamptz NOT NULL,
	author varchar NOT NULL,
	content varchar NULL,
	data jsonb NOT NULL
);

CREATE TABLE IF NOT EXISTS comments (
	id varchar NOT NULL PRIMARY KEY,
	link_id varchar NOT NULL,
	parent_id varchar NOT NULL,
	posted_at timestamptz NOT NULL,
	author varchar NOT NULL,
	content varchar NOT NULL,
	data jsonb NOT NULL
);
```

### Import the data:

Copy `db.template.json` into `db.json`

Fill out the newly created `db.json` with your server properties

Run the script `run.py`, specifying the name of the files, which will be `{SUBREDDIT_NAME}_submissions` and `{SUBREDDIT_NAME}_comments`. These file have to be in the same directory as im too lazy to make it use paths

### Create indexes

```sql
CREATE EXTENSION IF NOT EXISTS pg_trgm;

CREATE INDEX IF NOT EXISTS idx_submissions_title ON submissions USING gin (lower(title) gin_trgm_ops);

CREATE INDEX IF NOT EXISTS idx_submissions_content ON submissions USING gin (lower(content) gin_trgm_ops);

CREATE INDEX IF NOT EXISTS idx_submissions_author ON submissions(lower(author));

CREATE INDEX IF NOT EXISTS idx_comments_content ON comments USING gin(lower(content) gin_trgm_ops);

CREATE INDEX IF NOT EXISTS idx_comments_link_id ON comments (link_id);

CREATE INDEX IF NOT EXISTS idx_comments_author ON comments(lower(author));
```
