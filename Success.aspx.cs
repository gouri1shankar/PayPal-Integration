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

public partial class Success : System.Web.UI.Page
{

    SqlConnection sqlcon = new SqlConnection(ConfigurationManager.ConnectionStrings["Con"].ToString());
    SqlCommand sqlcmd = new SqlCommand();
    SqlDataAdapter da = new SqlDataAdapter();
    DataTable dt = new DataTable();
    string query;

    protected void Page_Load(object sender, EventArgs e)
    {
        string tranref, transtat, tranamt, trancur,output;
        if (!Page.IsPostBack)
        {
            try
            {
                tranref = Request.QueryString["tx"].ToString();
                transtat = Request.QueryString["st"].ToString();
                tranamt = Request.QueryString["amt"].ToString();
                trancur = Request.QueryString["cc"].ToString();
                
                //insert into table for future reference - Session["user"] is logged in user
                query = "insert into tranSuccess(tranref,transtat,tranamt,trancur,tranusr) values('" + tranref + "','" + transtat + "','" + tranamt + "','" + trancur + "','" + Session["user"].ToString() + "')";
                sqlcon.Open();
                sqlcmd = new SqlCommand(query, sqlcon);
                sqlcmd.ExecuteNonQuery();
                sqlcon.Close();

                //Display transaction details
                output =  "<table width='500' align='center' cellpadding='0' cellspacing='0' border='1'>";
                output += "<tr><td colspan='2' height='40' align='center'><b>Transaction Details</b><td></tr>";
                output += "<tr><td height='40'>Reference No.<td><td>" +  tranref + "</td>";
                output += "<tr><td height='40'>Status<td><td>" + transtat + "</td>";
                output += "<tr><td height='40'>Amount<td><td>" + tranamt + "</td>";
                output += "<tr><td height='40'>Currency<td><td>" + trancur + "</td>";
                output += "<tr><td height='40'>User<td><td>" + Session["user"].ToString() + "</td>";
                output += "</table>";

                lblDet.Text = output;
            }
            catch (Exception ex)
            { 
            
            }           
        }
    }
}
