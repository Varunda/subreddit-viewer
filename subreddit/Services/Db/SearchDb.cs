using Npgsql;
using subreddit.Code.ExtensionMethods;
using subreddit.Models;

namespace subreddit.Services.Db {

    public class SearchDb {

        private readonly ILogger<SearchDb> _Logger;
        private readonly IDbHelper _DbHelper;
        private readonly IDataReader<SearchResult> _Reader;

        public SearchDb(ILogger<SearchDb> logger,
            IDataReader<SearchResult> reader, IDbHelper dbHelper) {

            _Logger = logger;

            _Reader = reader;
            _DbHelper = dbHelper;
        }

        public async Task<List<SearchResult>> Search(List<string> terms, int offset, CancellationToken cancel) {
            _Logger.LogInformation($"Searching for {terms.Count} terms: [{string.Join(", ", terms)}]");
            if (terms.Count == 0) {
                return new List<SearchResult>();
            }

            if (terms.Count > 50) {
                throw new Exception($"Too many search terms, max 50, had {terms.Count}");
            }

            using NpgsqlConnection conn = _DbHelper.Connection();
            using NpgsqlCommand cmd = await _DbHelper.Command(conn, @"
                SELECT
                    id AS post_id, id, title, posted_at, content, 'post' AS type, author, CAST(data->>'score' AS int) AS score
                FROM
                    submissions 
                WHERE (
                    {0}
                )
                UNION ALL
                SELECT
                    c.link_id AS post_id, c.id, COALESCE(s.title, ''), COALESCE(s.posted_at, c.posted_at), c.content, 'comment' AS type, c.author, CAST(c.data->>'score' AS int) AS score
                FROM
                    comments c
                    LEFT JOIN submissions s ON ('t3_' || s.id) = c.link_id
                WHERE (
                    {1}
                )
                LIMIT 1000 OFFSET {2};
            ");

            string submissionSearch = string.Join(" AND ", terms.Select((iter, i) => {
                return $" ( (lower(content) LIKE '%' || @Term{i} || '%') OR (lower(title) LIKE '%' || @Term{i} || '%') ) ";
            }));

            string commentSearch = string.Join(" AND ", terms.Select((iter, i) => {
                return $" (lower(c.content) LIKE '%' || @Term{i} || '%') ";
            }));

            cmd.CommandText = string.Format(cmd.CommandText, submissionSearch, commentSearch, offset);

            for (int i = 0; i < terms.Count; ++i) {
                cmd.AddParameter($"@Term{i}", terms[i]);
            }

            _Logger.LogDebug(cmd.Print());

            List<SearchResult> results = await cmd.ExecuteReadList(_Reader, cancel);

            return results;
        }

    }
}
