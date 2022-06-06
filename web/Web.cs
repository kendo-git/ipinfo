using System;
using IPInfoLibraries;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Web
{
    class Web
    {
        //private static IMemoryCache memoryCache;
        //public static web.CacheController cacheCtrl;

        static async Task Main(string[] args)
        {
            //cacheCtrl = new web.CacheController(memoryCache);
            IPInfoLibrary sl = new IPInfoLibrary("", "7d53e8b076b0c0f5e1c18ae6bdbd9a79");
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = "localhost,1439",
                UserID = "SA",
                Password = "Test123!@#",
                InitialCatalog = "master"
            };
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            connection.Open();

            do
            {
                Console.WriteLine("Please enter an IP (examples: 94.68.117.55 = Athens | 185.230.46.143 = Manhattan | 18.182.194.89 = Tokyo)");
                Console.Write("IP:");
                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input)) break;

                var body = await GetType("http://api.ipstack.com/" + input, sl, connection);
                if (body.Body != null)
                {
                    Console.WriteLine("Result :" + body.Body.Printable());
                }
                else
                {
                    Console.WriteLine("Error :" + body.Error);
                }
                Console.WriteLine("");

            } while (true);
        }

        static async Task<iplibrary.Result<iplibrary.IpDetails>> GetType(string ip, IPInfoLibrary sl, SqlConnection connection)
        {

            //check if ip in cache, if so, return details
            //var cacheresult = cacheCtrl.GetCache(ip);
            //if (cacheresult != null)
            //{
            //    return new iplibrary.Result<iplibrary.IpDetails>(true, cacheresult, null);
            //}

            //check if ip in db, if so return details
            String sql;
            StringBuilder sb = new StringBuilder();
            sb.Append("USE ips;");
            sb.Append("SELECT * FROM ipDetails;");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ipd = new iplibrary.IpDetails(reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetString(3), reader.GetInt32(4), reader.GetInt32(5));
                        if (ipd.Ip == ip)
                        {
                            return new iplibrary.Result<iplibrary.IpDetails>(true, ipd, null);
                        }
                    }
                }
            }

            //since of the above are true, get from the actual library
            var body = await sl.GetDetails(ip);

            //put ip in db
            sb.Clear();
            sb.Append("INSERT ipDetails (ip, country, city, continent, lat, long) ");
            sb.Append("VALUES (@ip, @country, @city, @continent, @lat, @long);");
            sql = sb.ToString();
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ip", body.Body.Ip);
                command.Parameters.AddWithValue("@country", body.Body.Country);
                command.Parameters.AddWithValue("@continent", body.Body.Continent);
                command.Parameters.AddWithValue("@city", body.Body.City);
                command.Parameters.AddWithValue("@lat", body.Body.Lat);
                command.Parameters.AddWithValue("@long", body.Body.Long);
                int rowsAffected = command.ExecuteNonQuery();
            }

            //put ip in cache

            //cacheCtrl.SetCache(body.Body);

            return body;
        }
    }
}
