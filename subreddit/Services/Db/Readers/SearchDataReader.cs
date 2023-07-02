using Npgsql;
using subreddit.Models;
using System.Data;

namespace subreddit.Services.Db {

    public class SearchDataReader : IDataReader<SearchResult> {
        public override SearchResult? ReadEntry(NpgsqlDataReader reader) {
            SearchResult r = new();

            r.Type = reader.GetString("type");

            if (r.Type == "comment") {
                // t3_1gxupn => 1gxupn
                r.PostID = reader.GetString("post_id")[3..];
            } else if (r.Type == "post") {
                r.PostID = reader.GetString("post_id");
            } else {
                throw new Exception($"unhandled type '{r.Type}'");
            }

            r.Title = reader.GetString("title");
            r.Content = reader.GetString("content");
            r.PostedAt = reader.GetDateTime("posted_at");
            r.Author = reader.GetString("author");
            r.Score = reader.GetInt32("score");

            return r;
        }
    }
}
