using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace FinalProject.Database
{
	public abstract class DBAccessor
	{
		protected MySqlConnection connection;

		public DBAccessor()
		{
			// get environemnt variable
			string dbHost = Environment.GetEnvironmentVariable("DB_HOST");
			string dbUser = Environment.GetEnvironmentVariable("DB_USER");
			string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
			var builder = new MySqlConnectionStringBuilder
			{
				Server = dbHost,
				UserID = dbUser,
				Password = dbPassword,
				Database = "final", // Use maria db to create a database called library
			};

			connection = new MySqlConnection(builder.ConnectionString);
		}

		public abstract void CreateTable();
	}
}
