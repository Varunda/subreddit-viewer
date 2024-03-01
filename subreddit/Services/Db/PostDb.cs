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

        /// <summary>
        ///     Get a specific post from the DB
        /// </summary>
        /// <param name="ID">ID of the post to get. Do not include a leading t1_</param>
        /// <returns>
        ///     The <see cref="RedditPost"/> with <see cref="RedditPost.ID"/> of <paramref name="ID"/>,
        ///     or <c>null</c> if it does not exists
        /// </returns>
        public async Task<RedditPost?> GetByID(string ID) {
            using NpgsqlConnection conn = _DbHelper.Connection();
            using NpgsqlCommand cmd = await _DbHelper.Command(conn, @"
                SELECT *, CAST(data->>'score' AS INT) as score, data->>'author_flair_text' AS author_flair
                    FROM submissions
                    WHERE id = @ID;
            ");

            cmd.AddParameter("ID", ID);

            RedditPost? post = await _Reader.ReadSingle(cmd);

            return post;
        }

    }
}
