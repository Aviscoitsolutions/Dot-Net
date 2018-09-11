using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using MySql.Data.MySqlClient;
using Npgsql;

namespace ExampleService.DataAccess
{
    internal class QueryGenerator
    {
        private string Table;
        private NpgsqlConnection ConnectionNpgSql;
        private MySqlConnection ConnectionMySql;
        private SqlConnection ConnectionSql;
        private Dictionary<string, InsertValue> Values;

        private struct InsertValue
        {
            public object Value;
            public NpgsqlTypes.NpgsqlDbType Type;
        }

        public QueryGenerator(string table, NpgsqlConnection connection)
        {
            this.Table = table;
            this.ConnectionNpgSql = connection;
            this.Values = new Dictionary<string, InsertValue>();
        }

        public QueryGenerator(string table, MySqlConnection connection)
        {
            this.Table = table;
            this.ConnectionMySql = connection;
            this.Values = new Dictionary<string, InsertValue>();
        }

        public QueryGenerator(string table, SqlConnection connection)
        {
            this.Table = table;
            this.ConnectionSql = connection;
            this.Values = new Dictionary<string, InsertValue>();
        }

        public void AddParam(string name, object val, NpgsqlTypes.NpgsqlDbType type)
        {
            if (val != null & !Values.ContainsKey(name))
                this.Values.Add(name, new InsertValue { Type = type, Value = val });
        }

        public void AddParam(string name, object val)
        {
            if (val != null & !Values.ContainsKey(name))
                this.Values.Add(name, new InsertValue { Value = val });
        }

        public string GenerateQuery()
        {
            var query = "INSERT INTO " + Table;
            query += " (" + string.Join(",", Values.Keys) + ")";
            query += " VALUES (" + string.Join(",", Values.Select(x => "@" + x.Key).ToArray()) + ");";
            return query;
        }

        public void ExecuteNpgSql()
        {
            using (NpgsqlCommand command = new NpgsqlCommand(GenerateQuery(), this.ConnectionNpgSql))
            {
                foreach (var item in Values)
                    command.Parameters.Add("@" + item.Key, item.Value.Type).Value = item.Value.Value;
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteMySql()
        {
            using (MySqlCommand command = new MySqlCommand(GenerateQuery(), this.ConnectionMySql))
            {
                foreach (var item in Values)
                    command.Parameters.AddWithValue("@" + item.Key, item.Value.Value);
                command.ExecuteNonQuery();
            }
        }

        public void ExecuteSql(SqlTransaction transaction)
        {
            using (SqlCommand command = new SqlCommand(GenerateQuery(), this.ConnectionSql, transaction))
            {
                foreach (var item in Values)
                {
                    var type = item.Value.Value.GetType();
                    if (type == typeof(ushort))
                        command.Parameters.AddWithValue("@" + item.Key, Convert.ToInt32(item.Value.Value));
                    else if (type == typeof(uint))
                        command.Parameters.AddWithValue("@" + item.Key, Convert.ToInt64(item.Value.Value));
                    else if (type == typeof(ulong))
                        command.Parameters.AddWithValue("@" + item.Key, Convert.ToInt64(item.Value.Value));
                    else
                        command.Parameters.AddWithValue("@" + item.Key, item.Value.Value);
                }

                command.ExecuteNonQuery();
            }
        }
    }
}