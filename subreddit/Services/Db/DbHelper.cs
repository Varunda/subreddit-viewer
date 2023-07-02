using Npgsql;
using System.Data;

namespace subreddit.Services.Db {

    public class DbHelper : IDbHelper {

        private readonly ILogger<DbHelper> _Helper;
        private readonly IConfiguration _Configuration;

        public DbHelper(ILogger<DbHelper> helper, IConfiguration configuration) {
            _Helper = helper;
            _Configuration = configuration;
        }

        public async Task<NpgsqlCommand> Command(NpgsqlConnection conn, string text) {
            await conn.OpenAsync();

            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = text;

            return cmd;
        }

        public NpgsqlConnection Connection() {
            IConfigurationSection allStrings = _Configuration.GetSection("ConnectionStrings");
            string? connStr = allStrings["db"];

            if (string.IsNullOrEmpty(connStr)) {
                throw new Exception($"No connection string for db exists. Currently have [{string.Join(", ", allStrings.GetChildren().ToList().Select(iter => iter.Path))}]. "
                    + $"Set this value in config, or by using 'dotnet user-secrets set ConnectionStrings:db {{connection string}}");
            }

            NpgsqlConnection conn = new(connStr);
            return conn;
        }

    }
}
