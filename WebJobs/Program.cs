using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebJobs
{
    class Program
    {
        static void Main()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["dougbotdb"].ConnectionString))
            using (var command = new SqlCommand("DailyReset", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                conn.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
