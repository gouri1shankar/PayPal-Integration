using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Configuration;

public partial class _Default : System.Web.UI.Page
{
    SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ToString());
    SqlCommand sqlcmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    DataTable dt = new DataTable();
    DataRow dr;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            //Add some column to datatable display some products           
            dt.Columns.Add("pname");
            dt.Columns.Add("pdesc");
            dt.Columns.Add("price");

            //Add rows with datatable and bind in the grid view
            dr = dt.NewRow();          
            dr["pname"] = "Laptop";
            dr["pdesc"] = "Professional laptop";
            dr["price"] = "$100";
            dt.Rows.Add(dr);

            dr = dt.NewRow();            
            dr["pname"] = "Laptop";
            dr["pdesc"] = "Personal Laptop";
            dr["price"] = "$120";
            dt.Rows.Add(dr);

            dr = dt.NewRow();           
            dr["pname"] = "CPU";
            dr["pdesc"] = "Comptuter accessories";
            dr["price"] = "$40";
            dt.Rows.Add(dr);

            dr = dt.NewRow();           
            dr["pname"] = "Desktop";
            dr["pdesc"] = "Home PC";
            dr["price"] = "$150";
            dt.Rows.Add(dr);

            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "buy")
        {
            ImageButton ib = (ImageButton)e.CommandSource;
            int index = Convert.ToInt32(ib.CommandArgument);
            GridViewRow row = GridView1.Rows[index];

            //Get each Column label value from grid view and store it in label
            Label l1 = (Label)row.FindControl("Label1");
            Label l2 = (Label)row.FindControl("Label2");
            Label l3 = (Label)row.FindControl("Label3");

            //here i temporary use my name as logged in user you can create login page after only make an order
            Session["user"] = "ravi";

            //After user clik buy now button store that details into the sql server "purchase" table for reference            
            string query = "";
            query = "insert into purchase(pname,pdesc,price,uname) values('" + l1.Text + "','" + l2.Text + "','" +  l3.Text.Replace("$","") + "','" + Session["user"].ToString() + "')";
            sqlcon.Open();
            sqlcmd = new SqlCommand(query, sqlcon);
            sqlcmd.ExecuteNonQuery();
            sqlcon.Close();

            //Pay pal process Refer for what are the variable are need to send http://www.paypalobjects.com/IntegrationCenter/ic_std-variable-ref-buy-now.html
            
            string redirecturl = "";

            //Mention URL to redirect content to paypal site
            redirecturl += "https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick&business=" + ConfigurationManager.AppSettings["paypalemail"].ToString();            

            //First name i assign static based on login details assign this value
            redirecturl += "&first_name=ravindran";

            //City i assign static based on login user detail you change this value
            redirecturl += "&city=chennai";

            //State i assign static based on login user detail you change this value
            redirecturl += "&state=tamilnadu";

            //Product Name
            redirecturl += "&item_name=" + l1.Text;

            //Product Amount
            redirecturl += "&amount=" + l3.Text;           

            //Business contact id
            //redirecturl += "&business=nravindranmca@gmail.com";

            //Shipping charges if any
            redirecturl += "&shipping=5";

            //Handling charges if any
            redirecturl += "&handling=5";

            //Tax amount if any
            redirecturl += "&tax=5";
            
            //Add quatity i added one only statically 
            redirecturl += "&quantity=1";

            //Currency code 
            redirecturl += "&currency=USD";

            //Success return page url
            redirecturl += "&return=" + ConfigurationManager.AppSettings["SuccessURL"].ToString();

            //Failed return page url
            redirecturl += "&cancel_return=" + ConfigurationManager.AppSettings["FailedURL"].ToString();

            Response.Redirect(redirecturl);
        }
    }
}

