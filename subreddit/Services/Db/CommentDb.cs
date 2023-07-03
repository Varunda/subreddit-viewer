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

        /// <summary>
        ///     Get all <see cref="RedditComment"/> of a post
        /// </summary>
        /// <param name="ID">
        ///     ID of the post. Can include the leading t3_ or not.
        ///     If the string does not start with t3_, it is appended for you
        /// </param>
        /// <returns>
        ///     A list of <see cref="RedditComment"/>s with <see cref="RedditComment.LinkID"/> of <paramref name="ID"/>
        /// </returns>
        public async Task<List<RedditComment>> GetByPostID(string ID) {
            if (ID.StartsWith("t3_") == false) {
                ID = "t3_" + ID;
            }

            using NpgsqlConnection conn = _DbHelper.Connection();
            using NpgsqlCommand cmd = await _DbHelper.Command(conn, @"
                SELECT *, CAST(data->>'score' AS INT) as score
                    FROM comments
                    WHERE link_id = @LinkID;
            ");

            cmd.AddParameter("LinkID", ID);

            List<RedditComment> comments = await cmd.ExecuteReadList(_Reader, CancellationToken.None);
            return comments;
        }

    }
}
