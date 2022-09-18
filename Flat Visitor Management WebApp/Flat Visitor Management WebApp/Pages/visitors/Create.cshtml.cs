using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Flat_Visitor_Management_WebApp.Pages.visitors
{
    public class CreateModel : PageModel
    {
        public VisitorInfo visitorInfo=new VisitorInfo();
        public String ErrorMsg = "";
        public String SucMsg = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            visitorInfo.name = Request.Form["name"];
            visitorInfo.email = Request.Form["email"];
            visitorInfo.phone = Request.Form["phone"];
            visitorInfo.address = Request.Form["address"];
            visitorInfo.flat = Request.Form["flat"];

            if (visitorInfo.name.Length == 0 || visitorInfo.email.Length == 0 ||
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
                using (SqlConnection connection= new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "insert into visit" +"(name,email,phone,address,flat) VALUES"+ "(@name,@email,@phone,@address,@flat);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", visitorInfo.name);
                        command.Parameters.AddWithValue("@email",visitorInfo.email);
                        command.Parameters.AddWithValue("@phone", visitorInfo.phone);
                        command.Parameters.AddWithValue("@address", visitorInfo.address);
                        command.Parameters.AddWithValue("@flat", visitorInfo.flat);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception e)
            {
                ErrorMsg=e.Message;
                return;
            }
                visitorInfo.name = ""; visitorInfo.email = ""; visitorInfo.phone = ""; visitorInfo.address = "";
                visitorInfo.flat = "";
                SucMsg = "Visitor has been entered in Database";
            Response.Redirect("/visitors/Index");
        }
    }
}
 
