using Npgsql;
using subreddit.Code.ExtensionMethods;
using subreddit.Models;

namespace subreddit.Services.Db {

    public class CommentDb {

        private readonly ILogger<CommentDb> _Logger;
        private readonly IDbHelper _DbHelper;
        private readonly IDataReader<RedditComment> _Reader;

        public CommentDb(ILogger<CommentDb> logger, IDbHelper dbHelper, IDataReader<RedditComment> reader) {
            _Logger = logger;
            _DbHelper = dbHelper;
            _Reader = reader;
        }

        public async Task<List<RedditComment>> GetByPostID(string ID) {
            if (ID.StartsWith("t3_") == false) {
                ID = "t3_" + ID;
            }

            using NpgsqlConnection conn = _DbHelper.Connection();
            using NpgsqlCommand cmd = await _DbHelper.Command(conn, @"
                SELECT *
                    FROM comments
                    WHERE link_id = @LinkID;
            ");

            cmd.AddParameter("LinkID", ID);

            List<RedditComment> comments = await cmd.ExecuteReadList(_Reader, CancellationToken.None);
            return comments;
        }

    }
}
