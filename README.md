# subreddit-viewer
View an archive of a subreddit (some assembly required)

## Setup

Download the subreddit from https://reddit-top20k.cworld.ai/

Extract the .zst files using https://github.com/facebook/zstd/releases

Create the tables

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

Import the data:
TODO: write how to do this

Create indexes

```sql
CREATE EXTENSION IF NOT EXISTS pg_trgm;

CREATE INDEX IF NOT EXISTS idx_submissions_title ON submissions USING gin (lower(title) gin_trgm_ops);

CREATE INDEX IF NOT EXISTS idx_submissions_content ON submissions USING gin (lower(content) gin_trgm_ops);

CREATE INDEX IF NOT EXISTS idx_comments_content ON comments USING gin(lower(content) gin_trgm_ops);

CREATE INDEX IF NOT EXISTS idx_comments_link_id ON comments (link_id);
```
