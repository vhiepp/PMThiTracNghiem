using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemThiTracNghiem
{
    class KetNoiDuLieu
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string username;
        private string password;    
        public KetNoiDuLieu()
        {
            this.server = "14.225.254.248";
            this.database = "winform";
            this.username = "wf_user";
            this.password = "Hiep12345@@";
            string connectString = $"SERVER={this.server};DATABASE={this.database};UID={this.username};PASSWORD={this.password};";
            this.connection = new MySqlConnection(connectString);
        }

        public bool OpenConnection()
        {
            try
            {
                this.connection.Open();
                return true;
            }
            catch (MySqlException ex)
            { 
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                this.connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }

        public DataTable SelectDuLieu(string sql)
        {
            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                this.CloseConnection();
                return dt;
            }
            return null;
        }

        public bool ExecuteNonQuery(string sql)
        {
            if (this.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(sql, this.connection);
                    cmd.ExecuteNonQuery();
                try
                {
                    this.CloseConnection();
                    return true;
                } catch (MySqlException ex) { }
            }
            return false;
        }
    }
}
