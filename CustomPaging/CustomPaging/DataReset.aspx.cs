using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace CustomPaging
{
    public partial class DataReset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

                int total = 0;

                using (MySqlConnection conn = new MySqlConnection(connStr))
                {
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM tblcustomer", conn);

                    conn.Open();
                    cmd.ExecuteNonQuery();

                    for (int i = 0; i < 33317; i++)
                    {
                        Random rnd = new Random();

                        MySqlCommand cmd2 = new MySqlCommand("INSERT INTO tblcustomer (custName, custMobile) VALUES (@custName, @custMobile)", conn);

                        cmd2.Parameters.AddWithValue("@custName", Guid.NewGuid());
                        cmd2.Parameters.AddWithValue("@custMobile", rnd.Next(12345678, 99999999));

                        cmd2.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}