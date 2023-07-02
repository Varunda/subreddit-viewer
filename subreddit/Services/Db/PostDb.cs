using Npgsql;
using subreddit.Code.ExtensionMethods;
using subreddit.Models;

namespace subreddit.Services.Db {

    public class PostDb {

        private readonly ILogger<PostDb> _Logger;
        private readonly IDbHelper _DbHelper;
        private readonly IDataReader<RedditPost> _Reader;

        public PostDb(ILogger<PostDb> logger,
            IDbHelper dbHelper, IDataReader<RedditPost> reader) {

            _Logger = logger;

            _DbHelper = dbHelper;
            _Reader = reader;
        }

        public async Task<RedditPost?> GetByID(string ID) {
            using NpgsqlConnection conn = _DbHelper.Connection();
            using NpgsqlCommand cmd = await _DbHelper.Command(conn, @"
                SELECT *
                    FROM submissions
                    WHERE id = @ID;
            ");

            cmd.AddParameter("ID", ID);

            RedditPost? post = await _Reader.ReadSingle(cmd);

            return post;
        }

    }
}
