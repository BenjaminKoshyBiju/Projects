using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Flat_Visitor_Management_WebApp.Pages.visitors
{
    public class EditModel : PageModel
    {
        public VisitorInfo visitorInfo = new VisitorInfo();
        public String ErrorMsg = "";
        public String SucMsg = "";
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Visitors;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "select * from visit where id=@id";
                    using (SqlCommand command=new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                visitorInfo.id=""+reader.GetInt32(0);
                                visitorInfo.name=reader.GetString(1);
                                visitorInfo.email = reader.GetString(2);
                                visitorInfo.phone=reader.GetString(3);
                                visitorInfo.address=reader.GetString(4);
                                visitorInfo.flat = reader.GetString(5);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
            }
        }
        public void OnPost()
        {
            visitorInfo.id = Request.Form["id"];
            visitorInfo.name = Request.Form["name"];
            visitorInfo.email = Request.Form["email"];
            visitorInfo.phone = Request.Form["phone"];
            visitorInfo.address = Request.Form["address"];
            visitorInfo.flat = Request.Form["flat"];

            if (visitorInfo.id.Length == 0 || visitorInfo.name.Length == 0 || visitorInfo.email.Length == 0 ||
            visitorInfo.phone.Length == 0 || visitorInfo.address.Length == 0 ||
            visitorInfo.flat.Length == 0)
            {
                ErrorMsg = "Please Enter all fields";
                return;
            }

            //saving in database

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Visitors;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE visit SET name = @name,email=@email, phone=@phone, address=@address, flat=@flat WHERE id=@id;";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", visitorInfo.name);
                        command.Parameters.AddWithValue("@email", visitorInfo.email);
                        command.Parameters.AddWithValue("@phone", visitorInfo.phone);
                        command.Parameters.AddWithValue("@address", visitorInfo.address);
                        command.Parameters.AddWithValue("@flat", visitorInfo.flat);
                        command.Parameters.AddWithValue("@id", visitorInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMsg = ex.Message;
                return;
            }
           
            Response.Redirect("/visitors/Index");

        }
    }
}
