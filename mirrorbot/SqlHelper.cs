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
            connection.Open();
        }
        public void createTable(string tableName, string[] columns)
        {
            string sqlString = $"create table {tableName} (";
            foreach(var column in columns)
            {
                sqlString += column + ",";
            }
            sqlString = sqlString.Substring(0, sqlString.Length - 1);
            sqlString += ");";
            Console.WriteLine(sqlString);
            MySqlCommand command = new MySqlCommand(sqlString, connection);
            command.ExecuteNonQuery();
        }
        public void addData(string table, string[] column, object[] data)
        {
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
        }
        public void removeData(string table, string where, object whereData)
        {
            string cmd = $"DROP FROM {table} WHERE {where}='{whereData}';";
            MySqlCommand command = new MySqlCommand(cmd, connection);
            command.ExecuteNonQuery();
        }
        public object getData(string table, string whereColumn, object whereData, string getColumn)
        {
            string cmd = $"select * from {table} where {whereColumn} = '{whereData}';";
            // Console.WriteLine(cmd);
            MySqlCommand command = new MySqlCommand(cmd, connection);
            var reader = command.ExecuteReader();
            object turn = null;
            while (reader.Read())
            {
                turn =  reader[getColumn];
            }
            return turn;
        }
    }
}