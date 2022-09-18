using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Flat_Visitor_Management_WebApp.Pages.visitors
{
    public class IndexModel : PageModel
{
        public List<VisitorInfo> listVisitors=new List<VisitorInfo>();
    public void OnGet()
    {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Visitors;Integrated Security=True";
                using (SqlConnection connection= new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM visit ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                VisitorInfo visitorinfo=new VisitorInfo();
                                visitorinfo.id = "" + reader.GetInt32(0);
                                visitorinfo.name = reader.GetString(1);
                                visitorinfo.email = reader.GetString(2);
                                visitorinfo.phone = reader.GetString(3);
                                visitorinfo.address = reader.GetString(4);
                                visitorinfo.flat = reader.GetString(5);
                                visitorinfo.created_at = reader.GetDateTime(6).ToString();
                                listVisitors.Add(visitorinfo);
    
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception :" + e.ToString());
            }
    }
}
    public class VisitorInfo
    {
        public String id;
        public String name;
        public String email;
        public String phone;
        public String address;
        public String flat;
        public String created_at;

    }

}
