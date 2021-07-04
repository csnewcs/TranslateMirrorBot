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
        public bool tableExits(string tableName)
        {
            connection.Open();
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
            connection.Open();
            string sqlString = $"create table if not exists {tableName} (";
            foreach(var column in columns)
            {
                sqlString += column + ",";
            }
            sqlString = sqlString.Substring(0, sqlString.Length - 1);
            sqlString += ");";
            Console.WriteLine(sqlString);
            MySqlCommand command = new MySqlCommand(sqlString, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public void addData(string table, string[] column, object[] data)
        {
            connection.Open();
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
            connection.Open();
            string cmd = $"DELETE FROM {table} WHERE {where}='{whereData}';";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public object getData(string table, string whereColumn, object whereData, string getColumn)
        {
            connection.Open();
            string cmd = $"select * from {table} where {whereColumn} = '{whereData}';";
            // Console.WriteLine(cmd);
            MySqlCommand command = new MySqlCommand(cmd, connection);
            var reader = command.ExecuteReader();
            object turn = null;
            while (reader.Read())
            {
                turn =  reader[getColumn];
            }
            reader.Close();
            connection.Close();
            return turn;
        }
    }
}