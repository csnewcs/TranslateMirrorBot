using System;
using System.IO;
using System.Collections.Generic;
using MySql.Data;
using MySql;
using MySql.Data.MySqlClient;
using Logging;

namespace SqlHelper
{
    public class MariaDB
    {
        readonly MySqlConnection connection;
        public MariaDB()
        {
            string connectString = "server=localhost;Database=translatemirrorbot;Uid=translatemirrorbot";
            connection = new MySqlConnection(connectString);
            // connection.Open();
        }
        public Dictionary<string, object> getChannelData(ulong channel_id, ulong guild_id)
        {
            try
            {
                string sqlCommand = $"SELECT * FROM guild_{guild_id} WHERE StartChannel = {channel_id};";
                // try
                // {
                //     // connection.Open();
                // } catch{}
                connection.Open();
                MySqlCommand command = new MySqlCommand(sqlCommand, connection);
                MySqlDataReader reader = command.ExecuteReader();
                Dictionary<string, object> channelData = new Dictionary<string, object>();
                try
                {
                    // connection.Open();
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
                catch (Exception e)
                {
                    Log.log(e.ToString());
                    reader.Close();
                    connection.Close();   
                    return new Dictionary<string, object>() { {"Error", e.ToString()}};
                }
            }
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();   
                return new Dictionary<string, object>() { {"Error", e.ToString()}};
            }
        }

        public bool tableExits(string tableName)
        {
            // try
            // {
            //     // connection.Open();
            // } catch{}
            try
            {
                connection.Open();
                string hasTableCommand = $"SELECT 1 FROM Information_schema.tables WHERE table_schema='translatemirrorbot' AND table_name='{tableName}';";
                MySqlCommand cmd = new MySqlCommand(hasTableCommand, connection);
                var result = cmd.ExecuteReader();
                bool exists = false;
                try
                {
                    while (result.Read())
                    {
                        if ((int)result["1"] == 1) exists = true;
                        break;
                    }
                    result.Close();
                    connection.Close();
                    return exists;
                }
                catch (Exception e)
                {
                    Log.log(e.ToString());
                    result.Close();
                    connection.Close();
                    return false;
                }
            }
            catch(Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
                return false;
            }
        }
        public void createTable(string tableName, string[] columns)
        {
            try
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
                // try
                // {
                    command.ExecuteNonQuery();
                // } catch{}
                // connection.Open();
                connection.Close();
            } 
            catch(Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
            }
            // connection.Close();
        }
        public void addData(string table, string[] column, object[] data)
        {
            try
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
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
            }
            // connection.Close();
        }
        public void removeData(string table, string where, object whereData)
        {
            try
            {
                connection.Open();
                string cmd = $"DELETE FROM {table} WHERE {where}='{whereData}';";
                MySqlCommand command = new MySqlCommand(cmd, connection);
                command.ExecuteNonQuery();
                connection.Close();
            } 
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
            }
            // connection.Close();
        }
        public object getData(string table, string whereColumn, object whereData, string getColumn)
        {
            if(!tableExits(table))
            {
                return false;
            }
            try
            {
                string cmd = $"select * from {table} where {whereColumn} = '{whereData}';";
                // Console.WriteLine(cmd);
                MySqlCommand command = new MySqlCommand(cmd, connection);
                connection.Open();
                object turn = null;
                using(var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        turn = reader[getColumn];
                        break;
                    }
                }
                connection.Close();
                return turn;
                // connection.Open();
            } 
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
                return e;
            }
            // reader.Close();
            
            // connection.Close();
        }
        public Dictionary<ulong, ulong> allGuildAndChannel(string table)
        {
            try
            {
                connection.Open();
                string cmd = $"select * from {table};";
                MySqlCommand command = new MySqlCommand(cmd, connection);
                var reader = command.ExecuteReader();
                try
                {
                    Dictionary<ulong, ulong> guildAndChannel = new Dictionary<ulong, ulong>();
                    while (reader.Read())
                    {
                        guildAndChannel.Add((ulong)reader["StartChannel"], (ulong)reader["EndChannel"]);
                    }
                    reader.Close();
                    connection.Close();
                    return guildAndChannel;
                }
                catch (Exception e)
                {
                    Log.log(e.ToString());
                    reader.Close();
                    connection.Close();
                    return new Dictionary<ulong, ulong>() { { 0, 0 } };
                }
            } 
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
                return new Dictionary<ulong, ulong>() { { 0, 0 } };
            }
            // connection.Close();
            // return command.ExecuteReader();
        }
        public bool channelExist(ulong guildId, out int channels)
        {
            channels = 0;
            if(!tableExits("guild_" + guildId)) 
            {
                return false;
            }
            try
            {
                connection.Open();
                string cmd = $"select * from guild_{guildId};";
                MySqlCommand command = new MySqlCommand(cmd, connection);
                var reader = command.ExecuteReader();
                bool exists = false;
                try
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader["EndChannel"]);
                        if (reader["EndChannel"].ToString() != "") {
                            exists = true;
                            channels++;
                        }

                    }
                    reader.Close();
                    connection.Close();
                    return exists;
                }
                catch(Exception e)
                {
                    Log.log(e.ToString());
                    reader.Close();
                    connection.Close();
                    return false;
                }
            } 
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
                return false;
            }
        }
        public bool dataExist(string table, string column, object data)
        {
            try
            {
                connection.Open();
                string cmd = $"SELECT count(*) as count FROM {table} WHERE {column} LIKE '{data}';";
                MySqlCommand command = new MySqlCommand(cmd, connection);
                var reader = command.ExecuteReader();
                bool exists = false;
                try
                {
                    while (reader.Read())
                    {
                        if ((long)reader["count"] != 0) exists = true;
                        break;
                    }
                    reader.Close();
                    connection.Close();
                    return exists;
                }
                catch (Exception e)
                {
                    Log.log(e.ToString());
                    reader.Close();
                    connection.Close();
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
                return false;
            }
        }
        public void updateData(string table, string column, object data, string whereColumn, object whereData)
        {
            try
            {
                connection.Open();
                string cmd = $"UPDATE {table} SET {column} = '{data}' WHERE {whereColumn} = '{whereData}';";
                MySqlCommand command = new MySqlCommand(cmd, connection);
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
            }
        }
        public int[] getUsed()
        {
            int[] used = new int[2] {0, 0};
            DateTime dt = DateTime.Now;
            string cmd = $"select * from Used where day='{dt.ToString("yyyy-MM-dd")}';";
            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(cmd, connection);
                var reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        used[0] = (int)reader["papago"];
                        used[1] = (int)reader["kakao"];
                        break;
                    }
                    reader.Close();
                    connection.Close();
                    return used;
                }
                catch (Exception e)
                {
                    Log.log(e.ToString());
                    reader.Close();
                    connection.Close();
                    return new int[2] { -1, -1 };
                }
            }
            catch(Exception e)
            {
                Log.log(e.ToString());
                connection.Close();
                return new int[] {-1, -1};
            }
        }
    }
}