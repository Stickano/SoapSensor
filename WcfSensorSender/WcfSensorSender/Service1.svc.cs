using System.Collections.Generic;
using System.Data.SqlClient;
using System.ServiceModel;

namespace WcfSensorSender
{
    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class Service1 : IService1
    {

        SqlConnection conn;
        string connString = "Server=tcp:sensorreceiver.database.windows.net,1433;Initial Catalog=SoapSensorReceiver;Persist Security Info=False;User ID={USERNAME};Password={PASSWORD};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        /**
         * This will INSERT data into our Azure database
         * @param int light         The light value
         * @param int temp          The temp value
         * @param int Resistence    The resistence value
         * @param int analog        The analog value
         * @return int rowsAffected How many rows has been changed
         */
        public int Insert(int light, int temp, int resistence, int analog)
        {
            const string sql = "INSERT INTO SensorData (light,temp,resistence,analog) VALUES (@light, @temp, @resistence, @analog)";
            using (conn = new SqlConnection(connString))
            {
                using (SqlCommand insert = new SqlCommand(sql, conn))
                {
                    insert.Parameters.AddWithValue("@light", light);
                    insert.Parameters.AddWithValue("@temp", temp);
                    insert.Parameters.AddWithValue("@resistence", resistence);
                    insert.Parameters.AddWithValue("@analog", analog);

                    conn.Open();
                    int rowsAffected = insert.ExecuteNonQuery();
                    conn.Close();
                    return rowsAffected;
                }
            }
        }

        /**
         * Receives (read) all the data from the database.
         * Returns a List of nicely formatted string
         */
        public List<string> Receive()
        {
            const string sql = "SELECT * FROM SensorData";
            using (conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand query = new SqlCommand(sql, conn))
                {
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        List<string> result = new List<string>();
                        while (reader.Read())
                        {
                            string res = "id=" + reader["id"] + " temp=" + reader["light"] + " light=" + reader["light"] + " resistence=" + reader["resistence"] + " analog=" + reader["analog"];
                            result.Add(res);
                        }
                        return result;
                    }
                }
            }
        }

        // FIXME: Isn't working
        // This also the place you got to!
        public string Avarage(List<string> values)
        {
            List<int> temp = new List<int>();
            List<int> light = new List<int>();
            List<int> resistence = new List<int>();
            List<int> analog = new List<int>();
            string tester = "";
            foreach(string val in values)
            {
                string[] spltString = val.Split(' ');
                foreach(string value in spltString)
                {
                    string[] splitValues = value.Split('=');
                    tester+=splitValues+"/ ";
                }
            }
            return tester;
        }
    }
}
