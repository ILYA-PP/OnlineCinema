using System;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace OlineCinema
{
    class DB
    {
        private static SqlConnection connection;
        private static string connectionString;
        private SqlCommand command;
        private SqlDataReader reader;

        public DB(string connStr)
        {
            connectionString = connStr;
            connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch
            {
                MessageBox.Show($"Ошибка подключения!");
            }
            command = new SqlCommand();
            command.Connection = connection;
        }

        public SqlConnection GetConnection()
        {
            return connection;
        }

        public int ExecuteQuery(string query)
        {
            try
            {
                command.CommandText = query;
                return command.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Ошибка при выполнении запроса!");
                return -1;
            }
        }

        public object ExecuteScalarQuery(string query)
        {
            command.CommandText = query;
            return command.ExecuteScalar();
        }

        public void ExecuteReaderQuery(string query, IAddable table)
        {
            command.CommandText = query;
            try
            {
                reader = command.ExecuteReader();
                while (reader.Read())
                    for (int i = 0; i < reader.FieldCount; i++)
                        table.AddData(reader.GetName(i), reader.GetValue(i));
                reader.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Ошибка при выполнении запроса!" + e);
            }
        }

        static public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
    }
}
