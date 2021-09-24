using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;

namespace SqlHelper
{
    public class MariaDB
    {
        readonly MySqlConnection connection;
        public MariaDB()
        {
            string connectString = "server=localhost;Database=translatemirrorbot;Uid=translatemirrorbot";
            connection = new MySqlConnection(connectString);
            
        }
        public Dictionary<string, object> getChannelData(ulong channel_id, ulong guild_id)
        {

            try
            {
                connection.Open();
            } catch{}
            string sqlCommand = $"SELECT * FROM guild_{guild_id} WHERE StartChannel = {channel_id}";
            MySqlCommand command = new MySqlCommand(sqlCommand, connection);
            MySqlDataReader reader = command.ExecuteReader();
            Dictionary<string, object> channelData = new Dictionary<string, object>();
            while (reader.Read())
            {
                channelData.Add("EndChannel", reader["EndChannel"]);
                channelData.Add("StartLang", reader["StartLang"]);
                channelData.Add("EndLang", reader["EndLang"]);
            }
            reader.Close();
            connection.Close();
            return channelData;
        }

        public bool tableExits(string tableName)
        {
            try
            {
                connection.Open();
            } catch{}
            string hasTableCommand = $"SELECT 1 FROM Information_schema.tables WHERE table_schema='translatemirrorbot' AND table_name='{tableName}';";
            MySqlCommand cmd = new MySqlCommand(hasTableCommand, connection);
            var result = cmd.ExecuteReader();
            bool exists = false;
            while (result.Read())
            {
                if ((int)result["1"] == 1) exists = true;
                break;
            }
            result.Close();
            connection.Close();
            return exists;
        }
        public void createTable(string tableName, string[] columns)
        {
            try
            {
                connection.Open();
            } catch{}
            string sqlString = $"create table if not exists {tableName} (";
            foreach(var column in columns)
            {
                sqlString += column + ",";
            }
            sqlString = sqlString.Substring(0, sqlString.Length - 1);
            sqlString += ");";
            Console.WriteLine(sqlString);
            MySqlCommand command = new MySqlCommand(sqlString, connection);
            try
            {
                command.ExecuteNonQuery();
            } catch{}
            connection.Close();
        }
        public void addData(string table, string[] column, object[] data)
        {
            try
            {
                connection.Open();
            } catch{}
            string cmd = $"INSERT INTO {table} (";
            foreach (var a in column) cmd += a + ",";
            cmd = cmd.Substring(0, cmd.Length - 1);
            cmd += ") VALUES (";
            foreach (var a in data) 
            {                
                cmd += $"'{a}',";
            }
            cmd = cmd.Substring(0,cmd.Length - 1);
            cmd += ");";
            
            MySqlCommand command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void removeData(string table, string where, object whereData)
        {
            try
            {
                connection.Open();
            } catch{}
            string cmd = $"DELETE FROM {table} WHERE {where}='{whereData}';";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public object getData(string table, string whereColumn, object whereData, string getColumn)
        {
            try
            {
                connection.Open();
            } catch{}
            string cmd = $"select * from {table} where {whereColumn} = '{whereData}';";
            // Console.WriteLine(cmd);
            MySqlCommand command = new MySqlCommand(cmd, connection);
            
            object turn = null;
            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    turn = reader[getColumn];
                    break;
                }
            }
            // reader.Close();
            connection.Close();
            return turn;
        }
        public MySqlDataReader getAllTableData(string table)
        {
            try
            {
                connection.Open();
            } catch{}
            string cmd = $"select * from {table};";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            connection.Close();
            return command.ExecuteReader();
        }
        public bool channelExist(ulong guildId)
        {
            if(!tableExits("guild_" + guildId)) return false;
            try
            {
                connection.Open();
            } catch{}
            string cmd = $"select * from guild_{guildId}";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            var reader = command.ExecuteReader();
            bool exists = false;
            while (reader.Read())
            {
                Console.WriteLine(reader["EndChannel"]);
                if (reader["EndChannel"].ToString() != "") exists = true;
                break;
            }
            reader.Close();
            connection.Close();
            return exists;
        }
        public bool dataExist(string table, string column, object data)
        {
            try
            {
                connection.Open();
            } catch{}
            string cmd = $"SELECT count(*) as count FROM {table} WHERE {column} LIKE '{data}';";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            var reader = command.ExecuteReader();
            bool exists = false;
            while (reader.Read())
            {
                if ((long)reader["count"] != 0) exists = true;
                break;
            }
            reader.Close();
            connection.Close();
            return exists;
        }
    }
}