using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace CustomPaging
{
    public partial class Defaultv2 : System.Web.UI.Page
    {

        private int pageSize = 10;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetCustomersPageWise(1);
            }
        }

        private int GetTotalCustomers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            int total = 0;

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM tblcustomer", conn);

                conn.Open();

                var retTotal = cmd.ExecuteScalar();

                if (retTotal != null)
                {
                    total = int.Parse(retTotal.ToString());
                }
            }

            return total;
        }

        private void GetCustomersPageWise(int pageIndex)
        {
            string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

            DataTable dt = new DataTable();

            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                MySqlDataAdapter adp = new MySqlDataAdapter("SELECT * FROM tblcustomer limit @pageSize offset @offset", conn);
                adp.SelectCommand.Parameters.AddWithValue("@pageSize", pageSize);
                adp.SelectCommand.Parameters.AddWithValue("@offset", (pageIndex - 1)*pageSize);

                adp.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    lvCustomers.DataSource = dt;
                    lvCustomers.DataBind();
                }

                //int recordCount = Convert.ToInt32(cmd.Parameters["_RecordCount"].Value);
                PopulatePager(GetTotalCustomers(), pageIndex);
            }
        }

        private void PopulatePager(int recordCount, int currentPage)
        {
            List<ListItem> pages = new List<ListItem>();

            int startIndex, endIndex;
            int pagerSpan = 5;

            //Calculate the Start and End Index of pages to be displayed.
            double dblPageCount = (double)(recordCount / Convert.ToDecimal(pageSize));
            int pageCount = (int)Math.Ceiling(dblPageCount);
            startIndex = currentPage > 1 && currentPage + pagerSpan - 1 < pagerSpan ? currentPage : 1;
            endIndex = pageCount > pagerSpan ? pagerSpan : pageCount;
            if (currentPage > pagerSpan % 2)
            {
                if (currentPage == 2)
                {
                    endIndex = 5;
                }
                else
                {
                    endIndex = currentPage + 2;
                }
            }
            else
            {
                endIndex = (pagerSpan - currentPage) + 1;
            }

            if (endIndex - (pagerSpan - 1) > startIndex)
            {
                startIndex = endIndex - (pagerSpan - 1);
            }

            if (endIndex > pageCount)
            {
                endIndex = pageCount;
                startIndex = ((endIndex - pagerSpan) + 1) > 0 ? (endIndex - pagerSpan) + 1 : 1;
            }

            //Add the First Page Button.
            if (currentPage > 1)
            {
                pages.Add(new ListItem("First", "1"));
            }

            //Add the Previous Button.
            if (currentPage > 1)
            {
                pages.Add(new ListItem("<<", (currentPage - 1).ToString()));
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
            }

            //Add the Next Button.
            if (currentPage < pageCount)
            {
                pages.Add(new ListItem(">>", (currentPage + 1).ToString()));
            }

            //Add the Last Button.
            if (currentPage < pageCount)
            {
                pages.Add(new ListItem("Last", pageCount.ToString()));
            }
            rptPager.DataSource = pages;
            rptPager.DataBind();
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
            GetCustomersPageWise(pageIndex);
        }
    }
}
