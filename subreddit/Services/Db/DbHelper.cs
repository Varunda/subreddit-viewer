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

        /// <summary>
        ///     Create a new <see cref="NpgsqlCommand"/> using a <see cref="NpgsqlConnection"/>
        /// </summary>
        /// <param name="conn">Connection to create the command on. The connection is opened</param>
        /// <param name="text">Text of the command</param>
        /// <returns>
        ///     A new <see cref="NpgsqlCommand"/> with
        ///     <see cref="NpgsqlCommand.CommandType"/>of <see cref="CommandType.Text"/>
        ///     and <see cref="NpgsqlCommand.CommandText"/> of <paramref name="text"/>
        /// </returns>
        public async Task<NpgsqlCommand> Command(NpgsqlConnection conn, string text) {
            await conn.OpenAsync();

            NpgsqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = text;

            return cmd;
        }

        /// <summary>
        ///     Get a connection to the DB
        /// </summary>
        /// <returns>
        ///     An unopened connection to the DB
        /// </returns>
        /// <exception cref="Exception">If no connection string for the DB was given</exception>
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
