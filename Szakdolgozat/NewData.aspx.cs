using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Data;

namespace Szakdolgozat
{
    public partial class NewData : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MatchesDatabase.mdf;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            ltHiba.Visible = false;
            if (!Page.IsPostBack)
            {
                lbSuccess.Visible = false;
            }
        }

        protected void AddPlayer_Click(object sender, EventArgs e)
        {
            lbSuccess.Visible = false;
            String checkPlayer = "select count(*) from Players where Name LIKE '" + PlayerName.Text + "' AND TeamName LIKE '" + TeamName.Text + "'";
            SqlCommand com = new SqlCommand(checkPlayer, con);
            int temp = Convert.ToInt32(com.ExecuteScalar().ToString());
            if (temp == 0)
            {
                String checkPlayerNumber = "select count(*) from Players where TeamName LIKE '" + TeamName.Text + "' AND KitNumber='" + Int32.Parse(KitNumber.Text) + "'";
                SqlCommand com2 = new SqlCommand(checkPlayerNumber, con);
                int temp2 = Convert.ToInt32(com2.ExecuteScalar().ToString());
                if (temp2 == 0)
                {
                    ltHiba.Visible = false;
                    lbSuccess.Visible = true;
                    SqlCommand cmd = new SqlCommand("INSERT INTO Players(Name,TeamName,KitNumber) values ('" + PlayerName.Text + "', '" + TeamName.Text + "', '" + Int32.Parse(KitNumber.Text) + "')", con);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    ltHiba.Visible = true;
                    ltHiba.Text = "A megadott mezszám már foglalt a csapatban";
                    PlayerName.Text = "";
                    TeamName.Text = "";
                    KitNumber.Text = "";
                }
            }
            else
            {
                ltHiba.Visible = true;
                ltHiba.Text = "A megadott játékos már szerepel az adatbázisban";
                PlayerName.Text = "";
                TeamName.Text = "";
                KitNumber.Text = "";
            }
        }
    }
}