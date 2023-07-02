using Npgsql;
using subreddit.Models;
using System.Data;

namespace subreddit.Services.Db.Readers {

    public class RedditCommentReader : IDataReader<RedditComment> {

        public override RedditComment? ReadEntry(NpgsqlDataReader reader) {
            RedditComment c = new();

            c.ID = reader.GetString("id");
            c.LinkID = reader.GetString("link_id");
            c.ParentID = reader.GetString("parent_id");
            c.PostedAt = reader.GetDateTime("posted_at");
            c.Author = reader.GetString("author");
            c.Content = reader.GetString("content");

            return c;
        }

    }
}
