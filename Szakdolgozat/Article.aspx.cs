using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.IO;

namespace Szakdolgozat
{
    public partial class Article : System.Web.UI.Page
    {
        private static string txtname = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                TbArticle.Visible = false;
                BtUpdateArticle.Visible = false;
                lbSuccess.Visible = false;
                LbNotExist.Visible = false;
                lbCalendarError.Visible = false;
                BtEditArticle.Visible = false;
            }
        }

        protected void BtSearchForArticle_Click(object sender, EventArgs e)
        {
            if (Calendar.SelectedDate != DateTime.MinValue)
            {
                string path = "~/" + HomeTeamName.Text + "_" + AwayTeamName.Text + "_" + Calendar.SelectedDate.ToString("yyyy_MM_dd") + ".txt";
                if (!File.Exists(Server.MapPath(path)))
                {
                    AwayTeamName.Text = "";
                    HomeTeamName.Text = "";
                    LbNotExist.Visible = true;
                    lbCalendarError.Visible = false;
                }
                else
                {
                    LbNotExist.Visible = false;
                    lbCalendarError.Visible = false;
                    HomeTeamLb.Visible = false;
                    HomeTeamName.Visible = false;
                    AwayTeamLb.Visible = false;
                    AwayTeamName.Visible = false;
                    Calendar.Visible = false;
                    BtSearchForArticle.Visible = false;
                    DayLb.Visible = false;
                    TbArticle.Visible = true;
                    TbArticle.ReadOnly = true;
                    BtEditArticle.Visible = true;
                    subTitle.Text = "Az automatikusan generált cikk: ";
                    txtname = Server.MapPath(path);
                    TbArticle.Text = File.ReadAllText(txtname);

                }
            }
            else
            {
                AwayTeamName.Text = "";
                HomeTeamName.Text = "";
                lbCalendarError.Visible = true;
                LbNotExist.Visible = false;
            }
        }
        protected void BtEditArticle_Click(object sender, EventArgs e)
        {
            TbArticle.ReadOnly = false;
            BtEditArticle.Visible = false;
            BtUpdateArticle.Visible = true;
        }
        protected void BtUpdateArticle_Click(object sender, EventArgs e)
        {
            File.WriteAllText(txtname, TbArticle.Text);
            lbSuccess.Visible = true;
        }

        
    }
}