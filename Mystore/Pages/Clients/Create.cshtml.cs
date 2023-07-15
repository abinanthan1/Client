using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace Mystore.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo ClientInfo=new ClientInfo();
        public String errorMessage = "";
        public String successMessage= "";
        public void OnGet()
        {
        }
        public void OnPost() 
        {
            ClientInfo.name = Request.Form["name"]; 
            ClientInfo.email = Request.Form["email"];
            ClientInfo.phone = Request.Form["phone"];
            ClientInfo.address = Request.Form["address"];  

            if(ClientInfo.name.Length==0|| ClientInfo.email.Length == 0 || ClientInfo.phone.Length == 0 || ClientInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";      
            }

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=mystore;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO clients" + "(name,email,phone,address)VALUES" + "(@name,@email,@phone,@address);";

                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("name", ClientInfo.name);
                        command.Parameters.AddWithValue("email",ClientInfo.email);  
                        command.Parameters.AddWithValue ("phone", ClientInfo.phone);
                        command.Parameters.AddWithValue("address",ClientInfo.address);  
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            ClientInfo.name = ""; ClientInfo.email = "";ClientInfo.phone = "";ClientInfo.address = "";
            successMessage = "New client added correctly";

            Response.Redirect("/clients/Index");
        }
    }
}
