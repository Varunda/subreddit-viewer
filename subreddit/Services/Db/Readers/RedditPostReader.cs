using Npgsql;
using subreddit.Models;
using System.Data;

namespace subreddit.Services.Db.Readers {

    public class RedditPostReader : IDataReader<RedditPost> {

        public override RedditPost? ReadEntry(NpgsqlDataReader reader) {
            RedditPost post = new();

            post.ID = reader.GetString("id");
            post.Title = reader.GetString("title");
            post.PostedAt = reader.GetDateTime("posted_at");
            post.Author = reader.GetString("author");
            post.Content = reader.GetString("content");
            post.Score = reader.GetInt32("score");

            return post;
        }

    }
}
