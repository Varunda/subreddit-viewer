using Npgsql;

namespace subreddit.Services.Db {

    public interface IDbHelper {

        NpgsqlConnection Connection();

        Task<NpgsqlCommand> Command(NpgsqlConnection conn, string text);

    }
}
