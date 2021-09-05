using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Threading.Tasks;
using Microsoft.Ajax.Utilities;

namespace Szakdolgozat
{
    public partial class Match : System.Web.UI.Page
    {
        private static int HomeTeamScore;
        private static int AwayTeamScore;
        private static string HalfTimeScore;
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MatchesDatabase.mdf;Integrated Security=True");
        static List<string> HomeTeamsheet = new List<string>();
        static List<string> AwayTeamsheet = new List<string>();
        static List<string> HomeStartingEleven = new List<string>();
        static List<string> AwayStartingEleven = new List<string>();
        static List<string> HomeYellowCards = new List<string>();
        static List<string> AwayYellowCards = new List<string>();
        static List<GoalScorer> HomeGoalScorers = new List<GoalScorer>();
        static List<GoalScorer> AwayGoalScorers = new List<GoalScorer>();
        static List<Event> MatchEvents = new List<Event>();
        private static int HomeTeamRemainingSubs = 3;
        private static int AwayTeamRemainingSubs = 3;
        private static bool DetailedStats;
        private static bool ArticleMaking;
        private static string path;
        private static Random r = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                StartMatchBtn.Visible = false;
                SameTeamLb.Visible = false;
                HomeTeamPlayers.Visible = false;
                HomeTeamEleven.Visible = false;
                AwayTeamPlayers.Visible = false;
                AwayTeamEleven.Visible = false;
                BtnHomeTeamGoal.Visible = false;
                BtnAwayTeamGoal.Visible = false;
                AddToHome11.Visible = false;
                HT11Error.Visible = false;
                ThereWasAMatch.Visible = false;
                HomeTeam11Lb.Visible = false;
                HomeTeamsheetLb.Visible = false;
                AddToAway11.Visible = false;
                AT11Error.Visible = false;
                AwayTeam11Lb.Visible = false;
                AwayTeamsheetLb.Visible = false;
                RemoveAwayEleven.Visible = false;
                RemoveHomeEleven.Visible = false;
                TooFewAwayPlayers.Visible = false;
                TooFewHomePlayers.Visible = false;
                TooFewLink.Visible = false;
                BtnSecondHalf.Visible = false;
                ltVégeredmény.Visible = false;
                BtnHomeTeamYellow.Visible = false;
                BtnHomeTeamRed.Visible = false;
                BtnAwayTeamYellow.Visible = false;
                BtnAwayTeamRed.Visible = false;
                HomeTeamSub.Visible = false;
                AwayTeamSub.Visible = false;
                BtnPostponed.Visible = false;
                BtnHomeTeamOwnGoal.Visible = false;
                BtnAwayTeamOwnGoal.Visible = false;
                DetailedStatsButtons(false);
                DetailedStatsGV.Visible = false;
                ArticleLb.Visible = false;
            }
        }

        //getting players into starting elevens
        protected void StartingElevensBtn_Click(object sender, EventArgs e)
        {
            HomeTeamScore=0;
            AwayTeamScore=0;
            HomeTeamsheet.Clear();
            AwayTeamsheet.Clear();
            HomeStartingEleven.Clear();
            AwayStartingEleven.Clear();
            HomeYellowCards.Clear();
            AwayYellowCards.Clear();
            HomeGoalScorers.Clear();
            AwayGoalScorers.Clear();
            MatchEvents.Clear();
            HomeTeamRemainingSubs = 3;
            AwayTeamRemainingSubs = 3;
            if (HomeTeam.Text == AwayTeam.Text)
            {
                SameTeamLb.Visible = true;
                ThereWasAMatch.Visible = false;
                TooFewAwayPlayers.Visible = false;
                TooFewHomePlayers.Visible = false;
                TooFewLink.Visible = false;
                HomeTeam.Text = "";
                AwayTeam.Text = "";
            }
            else
            {
                con.Open();
                String checkWasThereAMatch = "select count(*) from Scorelines where HomeTeam LIKE '" + HomeTeam.Text + "' AND AwayTeam LIKE '" + AwayTeam.Text + "' AND Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                SqlCommand cHWTAM = new SqlCommand(checkWasThereAMatch, con);
                int MC = Convert.ToInt32(cHWTAM.ExecuteScalar().ToString());
                if (MC != 0)
                {
                    ThereWasAMatch.Visible = true;
                    SameTeamLb.Visible = false;
                    TooFewAwayPlayers.Visible = false;
                    TooFewHomePlayers.Visible = false;
                    TooFewLink.Visible = false;
                    HomeTeam.Text = "";
                    AwayTeam.Text = "";
                }
                else
                {
                    SameTeamLb.Visible = false;
                    ThereWasAMatch.Visible = false;
                    String checkHTEnoughPlayers = "select count(*) from Players where TeamName LIKE '" + HomeTeam.Text + "'";
                    SqlCommand cHTEP = new SqlCommand(checkHTEnoughPlayers, con);
                    int HTP = Convert.ToInt32(cHTEP.ExecuteScalar().ToString());

                    String checkATEnoughPlayers = "select count(*) from Players where TeamName LIKE '" + AwayTeam.Text + "'";
                    SqlCommand cATEP = new SqlCommand(checkATEnoughPlayers, con);
                    int ATP = Convert.ToInt32(cATEP.ExecuteScalar().ToString());

                    if (HTP < 11 || ATP < 11)
                    {
                        if (HTP < 11)
                        {
                            TooFewHomePlayers.Visible = true;
                        }
                        else
                        {
                            TooFewHomePlayers.Visible = false;
                        }
                        if (ATP < 11)
                        {
                            TooFewAwayPlayers.Visible = true;
                        }
                        else
                        {
                            TooFewAwayPlayers.Visible = false;
                        }
                        HomeTeam.Text = "";
                        AwayTeam.Text = "";
                        TooFewLink.Visible = true;
                    }
                    else
                    {
                        HomeTeam.Visible = false;
                        HomeTeamName.Visible = false;
                        AwayTeam.Visible = false;
                        AwayTeamName.Visible = false;
                        DetailedStats = DetailedStatsCB.Checked;
                        ArticleMaking = ArticleNeededCB.Checked;
                        ArticleNeededCB.Visible = false;
                        DetailedStatsCB.Visible = false;
                        TooFewAwayPlayers.Visible = false;
                        TooFewHomePlayers.Visible = false;
                        TooFewLink.Visible = false;
                        ltCsapatok.Text = HomeTeam.Text + " - " + AwayTeam.Text;
                        using (con)
                        {
                            string qry = "SELECT Name FROM Players WHERE TeamName LIKE '" + HomeTeam.Text + "'";
                            var cmd = new SqlCommand(qry, con);
                            cmd.CommandType = CommandType.Text;
                            using (SqlDataReader objReader = cmd.ExecuteReader())
                            {
                                if (objReader.HasRows)
                                {
                                    while (objReader.Read())
                                    {
                                        string item = objReader.GetString(objReader.GetOrdinal("Name"));
                                        HomeTeamsheet.Add(item);
                                    }
                                }
                            }
                            string qry2 = "SELECT Name FROM Players WHERE TeamName LIKE '" + AwayTeam.Text + "'";
                            var cmd2 = new SqlCommand(qry2, con);
                            cmd2.CommandType = CommandType.Text;
                            using (SqlDataReader objReader2 = cmd2.ExecuteReader())
                            {
                                if (objReader2.HasRows)
                                {
                                    while (objReader2.Read())
                                    {
                                        string item = objReader2.GetString(objReader2.GetOrdinal("Name"));
                                        AwayTeamsheet.Add(item);
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < HomeTeamsheet.Count; i++)
                        {
                            HomeTeamPlayers.Items.Insert(i, HomeTeamsheet[i]);
                        }
                        for (int i = 0; i < AwayTeamsheet.Count; i++)
                        {
                            AwayTeamPlayers.Items.Insert(i, AwayTeamsheet[i]);
                        }

                        HomeTeamPlayers.Visible = true;
                        HomeTeamEleven.Visible = true;
                        StartingElevensBtn.Visible = false;
                        AddToHome11.Visible = true;
                        HomeTeam11Lb.Visible = true;
                        HomeTeamsheetLb.Visible = true;

                        AwayTeamPlayers.Visible = true;
                        AwayTeamEleven.Visible = true;
                        AddToAway11.Visible = true;
                        AwayTeam11Lb.Visible = true;
                        AwayTeamsheetLb.Visible = true;
                    }
                }
                con.Close();
            }
        }

        protected void AddToHome11_Click(object sender, EventArgs e)
        {
            if (HomeStartingEleven.Contains(HomeTeamPlayers.SelectedValue))
            {
                HT11Error.Visible = true;
            }
            else
            {
                HT11Error.Visible = false;
                HomeStartingEleven.Add(HomeTeamPlayers.SelectedValue);
                HomeTeamEleven.Items.Add(HomeTeamPlayers.SelectedValue);
                RemoveHomeEleven.Visible = true;
                if (HomeStartingEleven.Count == 11)
                {
                    AddToHome11.Visible = false;
                    if (AwayStartingEleven.Count == 11)
                    {
                        StartMatchBtn.Visible = true;
                    }
                }
            }
        }

        protected void AddToAway11_Click(object sender, EventArgs e)
        {
            if (AwayStartingEleven.Contains(AwayTeamPlayers.SelectedValue))
            {
                AT11Error.Visible = true;
            }
            else
            {
                AT11Error.Visible = false;
                AwayStartingEleven.Add(AwayTeamPlayers.SelectedValue);
                AwayTeamEleven.Items.Add(AwayTeamPlayers.SelectedValue);
                RemoveAwayEleven.Visible = true;
                if (AwayStartingEleven.Count == 11)
                {
                    AddToAway11.Visible = false;
                    if (HomeStartingEleven.Count == 11)
                    {
                        StartMatchBtn.Visible = true;
                    }

                }
            }
        }


        protected void RemoveAwayEleven_Click(object sender, EventArgs e)
        {
            AwayStartingEleven.Remove(AwayTeamEleven.SelectedValue);
            AwayTeamEleven.Items.Remove(AwayTeamEleven.SelectedValue);
            if (AwayStartingEleven.Count == 0)
            {
                RemoveAwayEleven.Visible = false;
            }
            AddToAway11.Visible = true;
            StartMatchBtn.Visible = false;
        }

        protected void RemoveHomeEleven_Click(object sender, EventArgs e)
        {
            HomeStartingEleven.Remove(HomeTeamEleven.SelectedValue);
            HomeTeamEleven.Items.Remove(HomeTeamEleven.SelectedValue);
            if (HomeStartingEleven.Count == 0)
            {
                RemoveHomeEleven.Visible = false;
            }
            AddToHome11.Visible = true;
            StartMatchBtn.Visible = false;
        }

        //starting match
        protected void StartMatchBtn_Click(object sender, EventArgs e)
        {

            con.Open();
            string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
            SqlCommand matchcmd = new SqlCommand("INSERT INTO Scorelines(HomeTeam,AwayTeam,HomeTeamScore,AwayTeamScore,Date) values ('" + HomeTeam.Text + "', '" + AwayTeam.Text + "', '" + HomeTeamScore + "', '" + AwayTeamScore + "', '" + dateTime + "')", con);
            matchcmd.ExecuteNonQuery();
            int MerkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);

            for (int i = 0; i < 11; i++)
            {
                int JatekosID = GetJatekosId(HomeStartingEleven[i], HomeTeam.Text);
                SqlCommand cmd = new SqlCommand("INSERT INTO PlayerStatistics(MerkozesId,JatekosId,KezdocsapatSzam) values ('" + MerkozesID + "' , '" + JatekosID + "' , '" + 1 + "')", con);
                cmd.ExecuteNonQuery();


                JatekosID = GetJatekosId(AwayStartingEleven[i], AwayTeam.Text);
                SqlCommand cmd2 = new SqlCommand("INSERT INTO PlayerStatistics(MerkozesId,JatekosId,KezdocsapatSzam) values ('" + MerkozesID + "' , '" + JatekosID + "' , '" + 1 + "')", con);
                cmd2.ExecuteNonQuery();
            }
            con.Close();


            ScoreChange();
            StartMatchBtn.Visible = false;
            RemoveAwayEleven.Visible = false;
            RemoveHomeEleven.Visible = false;
            HomeTeamsheetLb.Text = "Hazai csapat cseréi: ";
            HT11Error.Visible = false;
            AwayTeamsheetLb.Text = "Vendég csapat cseréi: ";
            AT11Error.Visible = false;
            HomeTeam.Visible = false;
            AwayTeam.Visible = false;
            HomeTeamName.Visible = false;
            AwayTeamName.Visible = false;
            AddToHome11.Visible = false;
            AddToAway11.Visible = false;
            HomeTeamSub.Visible = true;
            AwayTeamSub.Visible = true;
            for (int i = 0; i < 11; i++)
            {
                HomeTeamPlayers.Items.Remove(HomeTeamEleven.Items[i]);
                AwayTeamPlayers.Items.Remove(AwayTeamEleven.Items[i]);
            }
            BtnHomeTeamGoal.Visible = true;
            BtnAwayTeamGoal.Visible = true;
            BtnHomeTeamOwnGoal.Visible = true;
            BtnAwayTeamOwnGoal.Visible = true;
            BtnHomeTeamYellow.Visible = true;
            BtnHomeTeamRed.Visible = true;
            BtnAwayTeamYellow.Visible = true;
            BtnAwayTeamRed.Visible = true;
            BtnPostponed.Visible = true;
            LTFelido.Text = "1. félidő - ";
            LTPerc.Text = "1";
            if (DetailedStats)
            {
                DetailedStatsButtons(true);
            }
            if (ArticleMaking)
            {
                path = "~/" + HomeTeam.Text + "_" + AwayTeam.Text + "_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
            }
        }

        protected void ScoreChange()
        {
            ltEredmeny.Text = HomeTeamScore.ToString() + " - " + AwayTeamScore.ToString();
        }

        //getting the id of match and player
        protected int GetMerkozesId(string HomeTeam, string AwayTeam)
        {
            string matchid = "SELECT Id FROM Scorelines WHERE HomeTeam= '" + HomeTeam + "' AND  AwayTeam= '" + AwayTeam + "' AND Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
            SqlCommand matchidcom = new SqlCommand(matchid, con);
            int MerkozesID = Convert.ToInt32(matchidcom.ExecuteScalar().ToString());
            return MerkozesID;
        }

        protected int GetJatekosId(string JatekosNev, string CsapatNev)
        {
            string qry = "SELECT Id FROM Players WHERE Name LIKE '" + JatekosNev + "' AND TeamName LIKE '" + CsapatNev + "'";
            SqlCommand com = new SqlCommand(qry, con);
            int JatekosID = Convert.ToInt32(com.ExecuteScalar().ToString());
            return JatekosID;
        }

        //updata data in database after events
        protected void UpdateStatistics(string Adattag, int JatekosID, int MerkozesID, bool plusz)
        {
            string qry = "SELECT " + Adattag + " FROM PlayerStatistics WHERE MerkozesId = '" + MerkozesID + "' AND JatekosId = '" + JatekosID + "'";
            SqlCommand com = new SqlCommand(qry, con);
            int AdattagSzam = Convert.ToInt32(com.ExecuteScalar().ToString());
            if (plusz)
            {
                AdattagSzam = AdattagSzam + 1;
            }
            else
            {
                AdattagSzam = AdattagSzam - 1;
            }
            string qry2 = "UPDATE PlayerStatistics SET " + Adattag + " = '" + AdattagSzam + "' WHERE MerkozesId = '" + MerkozesID + "' AND JatekosId = '" + JatekosID + "'";
            SqlCommand cmd = new SqlCommand(qry2, con);
            cmd.ExecuteNonQuery();
        }

        //function for data gathering for articles
        protected void CikkEsemeny(string jatekos, string csapat, string esemeny, string ido)
        {
            MatchEvents.Add(new Event(jatekos, csapat, esemeny, ido));
        }

        //goal buttons clicks
        protected void BtnHomeTeamGoal_Click(object sender, EventArgs e)
        {
            HomeTeamScore += 1;
            ScoreChange();
            int i = 0;
            while (i < HomeGoalScorers.Count && HomeGoalScorers[i].getName() != HomeTeamEleven.SelectedValue)
            {
                i = i + 1;
            }
            if (i < HomeGoalScorers.Count)
            {
                HomeGoalScorers[i].setTimes(HomeGoalScorers[i].getTimes() + ", " + hf.Value);
            }
            else
            {
                HomeGoalScorers.Add(new GoalScorer(HomeTeamEleven.SelectedValue, hf.Value));
            }
            con.Open();
            UpdateStatistics("Golszam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            UpdateStatistics("KapuraLovesSzam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Gól", hf.Value);
            con.Close();
            timerDisplay();
        }

        protected void BtnAwayTeamGoal_Click(object sender, EventArgs e)
        {
            AwayTeamScore += 1;
            ScoreChange();
            int i = 0;
            while (i < AwayGoalScorers.Count && AwayGoalScorers[i].getName() != AwayTeamEleven.SelectedValue)
            {
                i = i + 1;
            }
            if (i < AwayGoalScorers.Count)
            {
                AwayGoalScorers[i].setTimes(AwayGoalScorers[i].getTimes() + ", " + hf.Value);
            }
            else
            {
                AwayGoalScorers.Add(new GoalScorer(AwayTeamEleven.SelectedValue, hf.Value));
            }
            con.Open();
            UpdateStatistics("Golszam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            UpdateStatistics("KapuraLovesSzam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Gól", hf.Value);
            con.Close();
            timerDisplay();
        }

        //half time end and start buttons
        protected void BtnHalfTime_Click(object sender, EventArgs e)
        {
            HalfTimeScore = HomeTeamScore.ToString() + " - " + AwayTeamScore.ToString();
            BtnHalfTime.Visible = false;
            BtnSecondHalf.Visible = true;
            LTFelido.Text = "Félidő ";
            LTPerc.Text = "";
            hf.Value = "Félidő";
            BtnHomeTeamGoal.Visible = false;
            BtnAwayTeamGoal.Visible = false;
            BtnHomeTeamOwnGoal.Visible = false;
            BtnAwayTeamOwnGoal.Visible = false;
            if (DetailedStats)
            {
                DetailedStatsButtons(false);
            }
        }

        protected void BtnSecondHalf_Click(object sender, EventArgs e)
        {
            BtnSecondHalf.Visible = false;
            BtnHomeTeamGoal.Visible = true;
            BtnAwayTeamGoal.Visible = true;
            BtnHomeTeamOwnGoal.Visible = true;
            BtnAwayTeamOwnGoal.Visible = true;
            if (DetailedStats)
            {
                DetailedStatsButtons(true);
            }
            LTFelido.Text = "2. félidő - ";
            LTPerc.Text = "46";

        }

        //ending the match
        protected void BtnFullTime_Click(object sender, EventArgs e)
        {
            BtnHomeTeamGoal.Visible = false;
            BtnAwayTeamGoal.Visible = false;
            BtnFullTime.Visible = false;
            ltEredmeny.Visible = false;
            HomeTeam11Lb.Visible = false;
            HomeTeamEleven.Visible = false;
            AddToHome11.Visible = false;
            AwayTeam11Lb.Visible = false;
            AwayTeamEleven.Visible = false;
            AddToAway11.Visible = false;
            LTFelido.Visible = false;
            LTPerc.Visible = false;
            BtnHomeTeamOwnGoal.Visible = false;
            BtnAwayTeamOwnGoal.Visible = false;
            BtnHomeTeamYellow.Visible = false;
            BtnHomeTeamRed.Visible = false;
            BtnAwayTeamYellow.Visible = false;
            BtnAwayTeamRed.Visible = false;
            HomeTeamSub.Visible = false;
            AwayTeamSub.Visible = false;
            BtnPostponed.Visible = false;
            HomeTeamsheetLb.Visible = false;
            HomeTeamPlayers.Visible = false;
            AwayTeamsheetLb.Visible = false;
            AwayTeamPlayers.Visible = false;
            ltVégeredmény.Visible = true;
            if (DetailedStats)
            {
                DetailedStatsButtons(false);
                DetailedStatsGV.Visible = true;
            }
            ltVégeredmény.Text = HomeTeamScore.ToString() + " - " + AwayTeamScore.ToString() + " ( " + HalfTimeScore + " )";
            if (HomeTeamScore != 0 || AwayTeamScore != 0)
            {
                string goalscorers = "Gólszerzők: ";
                for (int i = 0; i < HomeGoalScorers.Count; i++)
                {
                    goalscorers = goalscorers + HomeGoalScorers[i].getName() + " " + HomeGoalScorers[i].getTimes();
                    if (i < HomeGoalScorers.Count - 1)
                    {
                        goalscorers = goalscorers + " ; ";
                    }
                }
                if (HomeTeamScore != 0 && AwayTeamScore != 0)
                {
                    goalscorers = goalscorers + " ill. ";
                }
                for (int i = 0; i < AwayGoalScorers.Count; i++)
                {
                    goalscorers = goalscorers + AwayGoalScorers[i].getName() + " " + AwayGoalScorers[i].getTimes();
                    if (i < AwayGoalScorers.Count - 1)
                    {
                        goalscorers = goalscorers + " ; ";
                    }
                }
                LtGólszerzők.Text = goalscorers;
            }

            con.Open();
            using (con)
            {
                if (DetailedStats)
                {
                    int merkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);
                    string detailedstatscmd = "SELECT a.TeamName AS Csapat, a.Name AS Játékos, b.Golszam AS Gólszám, b.Ongolszam AS Öngólszám, b.SikeresPasszSzam AS Sikeres_Passzok, b.OsszesPasszSzam AS Összes_Passz, FORMAT(round(ISNULL((CAST (b.SikeresPasszSzam AS FLOAT)) / NULLIF((CAST (b.OsszesPasszSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság, b.KapuraLovesSzam AS Kapuralövési_Kísérletek, FORMAT(round(ISNULL((CAST (b.Golszam AS FLOAT)) / NULLIF((CAST (b.KapuraLovesSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Lövési_Pontosság, b.SzerelesSzam AS Szerelésszám FROM (SELECT * FROM Players WHERE Id IN(SELECT JatekosId FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "')) a , (SELECT * FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "') b WHERE a.Id=b.JatekosId ORDER BY a.TeamName, a.Name";
                    SqlCommand command = new SqlCommand(detailedstatscmd, con);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        DetailedStatsGV.DataSource = dataSet;
                        DetailedStatsGV.DataBind();
                    }
                }
                string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                SqlCommand cmd = new SqlCommand("UPDATE Scorelines SET HomeTeamScore = '" + HomeTeamScore + "', AwayTeamScore = '" + AwayTeamScore + "' WHERE HomeTeam= '" + HomeTeam.Text + "' AND  AwayTeam= '" + AwayTeam.Text + "' AND Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'", con);
                cmd.ExecuteNonQuery();
            }
            con.Close();

            if (ArticleMaking)
            {
                File.WriteAllText(Server.MapPath(path), MakeArticle());
                ArticleLb.Visible = true;
            }

        }

        //yellow and red card button clicks
        protected void BtnHomeTeamYellow_Click(object sender, EventArgs e)
        {
            if (HomeYellowCards.Contains(HomeTeamEleven.SelectedValue))
            {
                HomeYellowCards.Remove(HomeTeamEleven.SelectedValue);
                CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Piros lap", hf.Value);
                RemoveHomeTeamPlayer();
                if (HomeTeamEleven.Items.Count < 7)
                {
                    postpone("Hazai");
                }
            }
            else
            {
                HomeYellowCards.Add(HomeTeamEleven.SelectedValue);
                CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Sárga lap", hf.Value);
            }
            timerDisplay();
        }

        protected void BtnAwayTeamYellow_Click(object sender, EventArgs e)
        {
            if (AwayYellowCards.Contains(AwayTeamEleven.SelectedValue))
            {
                AwayYellowCards.Remove(AwayTeamEleven.SelectedValue);
                CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Piros lap", hf.Value);
                RemoveAwayTeamPlayer();
                if (AwayTeamEleven.Items.Count < 7)
                {
                    postpone("Vendég");
                }
            }
            else
            {
                AwayYellowCards.Add(AwayTeamEleven.SelectedValue);
                CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Sárga lap", hf.Value);
            }
            timerDisplay();
        }

        protected void BtnHomeTeamRed_Click(object sender, EventArgs e)
        {
            CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Piros lap", hf.Value);
            RemoveHomeTeamPlayer();
            if (HomeTeamEleven.Items.Count < 7)
            {
                postpone("Hazai");
            }
            timerDisplay();
        }

        protected void BtnAwayTeamRed_Click(object sender, EventArgs e)
        {
            CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Piros lap", hf.Value);
            RemoveAwayTeamPlayer();
            if (AwayTeamEleven.Items.Count < 7)
            {
                postpone("Vendég");
            }
            timerDisplay();
        }

        //edit dropdownlists
        protected void RemoveHomeTeamPlayer()
        {
            HomeStartingEleven.Remove(HomeTeamEleven.SelectedValue);
            HomeTeamEleven.Items.Remove(HomeTeamEleven.SelectedValue);
        }

        protected void RemoveAwayTeamPlayer()
        {
            AwayStartingEleven.Remove(AwayTeamEleven.SelectedValue);
            AwayTeamEleven.Items.Remove(AwayTeamEleven.SelectedValue);
        }

        //substitution buttons clicks
        protected void HomeTeamSub_Click(object sender, EventArgs e)
        {
            if (HomeTeamRemainingSubs == 0 || HomeTeamPlayers.Items.Count == 0)
            {
                con.Open();
                CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Sérülés", hf.Value);
                con.Close();
                RemoveHomeTeamPlayer();
                if (HomeTeamEleven.Items.Count < 7)
                {
                    postpone("Hazai");
                }
            }
            else
            {
                con.Open();
                CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Lecserélés", hf.Value);
                CikkEsemeny(HomeTeamPlayers.SelectedValue, HomeTeam.Text, "Becserélés", hf.Value);
                RemoveHomeTeamPlayer();
                HomeStartingEleven.Add(HomeTeamPlayers.SelectedValue);
                HomeTeamEleven.Items.Add(HomeTeamPlayers.SelectedValue);
                int MerkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);
                int JatekosID = GetJatekosId(HomeTeamPlayers.SelectedValue, HomeTeam.Text);
                SqlCommand cmd = new SqlCommand("INSERT INTO PlayerStatistics(MerkozesId,JatekosId,BecserelesSzam) values ('" + MerkozesID + "' , '" + JatekosID + "' , '" + 1 + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                HomeTeamPlayers.Items.Remove(HomeTeamPlayers.SelectedValue);
                if (HomeTeamRemainingSubs == 1 || HomeTeamPlayers.Items.Count == 0)
                {
                    HomeTeamPlayers.Visible = false;
                    HomeTeamsheetLb.Visible = false;
                    HomeTeamSub.Text = "Hazai játékos sérülés";
                }
                HomeTeamRemainingSubs -= 1;
            }
            if (hf.Value != "Félidő")
            {
                LTPerc.Text = hf.Value;
            }
            timerDisplay();
        }

        protected void AwayTeamSub_Click(object sender, EventArgs e)
        {
            if (AwayTeamRemainingSubs == 0 || AwayTeamPlayers.Items.Count == 0)
            {
                con.Open();
                CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Sérülés", hf.Value);
                con.Close();
                RemoveAwayTeamPlayer();
                if (AwayTeamEleven.Items.Count < 7)
                {
                    postpone("Vendég");
                }
            }
            else
            {
                con.Open();
                CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Lecserélés", hf.Value);
                CikkEsemeny(AwayTeamPlayers.SelectedValue, AwayTeam.Text, "Becserélés", hf.Value);
                RemoveAwayTeamPlayer();
                AwayStartingEleven.Add(AwayTeamPlayers.SelectedValue);
                AwayTeamEleven.Items.Add(AwayTeamPlayers.SelectedValue);
                int MerkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);
                int JatekosID = GetJatekosId(AwayTeamPlayers.SelectedValue, AwayTeam.Text);
                SqlCommand cmd = new SqlCommand("INSERT INTO PlayerStatistics(MerkozesId,JatekosId,BecserelesSzam) values ('" + MerkozesID + "' , '" + JatekosID + "' , '" + 1 + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                AwayTeamPlayers.Items.Remove(AwayTeamPlayers.SelectedValue);
                if (AwayTeamRemainingSubs == 1 || AwayTeamPlayers.Items.Count == 0)
                {
                    AwayTeamPlayers.Visible = false;
                    AwayTeamsheetLb.Visible = false;
                    AwayTeamSub.Text = "Vendég játékos sérülés";
                }
                AwayTeamRemainingSubs -= 1;
            }
            timerDisplay();
        }

        //match postponed
        protected void BtnPostponed_Click(object sender, EventArgs e)
        {
            postpone("egyéb");
        }

        protected void postpone(string asd)
        {
            if (DetailedStats)
            {
                DetailedStatsButtons(false);
                DetailedStatsGV.Visible = true;
            }
            if (asd == "Hazai")
            {
                LbPostponed.Text = "A hazai csapatban nincs elegendő játékos, a mérkőzés így félbeszakad. Az eredmény 3-0 a vendégcsapatnak. Az egyéni statisztikák elmentésre kerültek";
                con.Open();
                using (con)
                {
                    if (DetailedStats)
                    {
                        int merkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);
                        string detailedstatscmd = "SELECT a.TeamName AS Csapat, a.Name AS Játékos, b.Golszam AS Gólszám, b.Ongolszam AS Öngólszám, b.SikeresPasszSzam AS Sikeres_Passzok, b.OsszesPasszSzam AS Összes_Passz, FORMAT(round(ISNULL((CAST (b.SikeresPasszSzam AS FLOAT)) / NULLIF((CAST (b.OsszesPasszSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság, b.KapuraLovesSzam AS Kapuralövési_Kísérletek, FORMAT(round(ISNULL((CAST (b.Golszam AS FLOAT)) / NULLIF((CAST (b.KapuraLovesSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Lövési_Pontosság, b.SzerelesSzam AS Szerelésszám FROM (SELECT * FROM Players WHERE Id IN(SELECT JatekosId FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "')) a , (SELECT * FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "') b WHERE a.Id=b.JatekosId ORDER BY a.TeamName, a.Name";
                        SqlCommand command = new SqlCommand(detailedstatscmd, con);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            DetailedStatsGV.DataSource = dataSet;
                            DetailedStatsGV.DataBind();
                        }
                    }
                    string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                    SqlCommand cmd = new SqlCommand("UPDATE Scorelines SET HomeTeamScore = '" + 0 + "', AwayTeamScore = '" + 3 + "' WHERE HomeTeam= '" + HomeTeam.Text + "' AND  AwayTeam= '" + AwayTeam.Text + "' AND Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'", con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            else if (asd == "Vendég")
            {
                LbPostponed.Text = "A vendég csapatban nincs elegendő játékos, a mérkőzés így félbeszakad. Az eredmény 3-0 a hazai csapatnak. Az egyéni statisztikák elmentésre kerültek";
                con.Open();
                using (con)
                {
                    if (DetailedStats)
                    {
                        int merkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);
                        string detailedstatscmd = "SELECT a.TeamName AS Csapat, a.Name AS Játékos, b.Golszam AS Gólszám, b.Ongolszam AS Öngólszám, b.SikeresPasszSzam AS Sikeres_Passzok, b.OsszesPasszSzam AS Összes_Passz, FORMAT(round(ISNULL((CAST (b.SikeresPasszSzam AS FLOAT)) / NULLIF((CAST (b.OsszesPasszSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság, b.KapuraLovesSzam AS Kapuralövési_Kísérletek, FORMAT(round(ISNULL((CAST (b.Golszam AS FLOAT)) / NULLIF((CAST (b.KapuraLovesSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Lövési_Pontosság, b.SzerelesSzam AS Szerelésszám FROM (SELECT * FROM Players WHERE Id IN(SELECT JatekosId FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "')) a , (SELECT * FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "') b WHERE a.Id=b.JatekosId ORDER BY a.TeamName, a.Name";
                        SqlCommand command = new SqlCommand(detailedstatscmd, con);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            DetailedStatsGV.DataSource = dataSet;
                            DetailedStatsGV.DataBind();
                        }
                    }
                    string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                    SqlCommand cmd = new SqlCommand("UPDATE Scorelines SET HomeTeamScore = '" + 3 + "', AwayTeamScore = '" + 0 + "' WHERE HomeTeam= '" + HomeTeam.Text + "' AND  AwayTeam= '" + AwayTeam.Text + "' AND Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'", con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            else
            {
                LbPostponed.Text = "A mérkőzés félbeszakadt. Az egyéni statisztikák, és a mérkőzés végeredménye elmentésre kerültek";
                con.Open();
                using (con)
                {
                    if (DetailedStats)
                    {
                        int merkozesID = GetMerkozesId(HomeTeam.Text, AwayTeam.Text);
                        string detailedstatscmd = "SELECT a.TeamName AS Csapat, a.Name AS Játékos, b.Golszam AS Gólszám, b.Ongolszam AS Öngólszám, b.SikeresPasszSzam AS Sikeres_Passzok, b.OsszesPasszSzam AS Összes_Passz, FORMAT(round(ISNULL((CAST (b.SikeresPasszSzam AS FLOAT)) / NULLIF((CAST (b.OsszesPasszSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság, b.KapuraLovesSzam AS Kapuralövési_Kísérletek, FORMAT(round(ISNULL((CAST (b.Golszam AS FLOAT)) / NULLIF((CAST (b.KapuraLovesSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Lövési_Pontosság, b.SzerelesSzam AS Szerelésszám FROM (SELECT * FROM Players WHERE Id IN(SELECT JatekosId FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "')) a , (SELECT * FROM PlayerStatistics WHERE MerkozesId = '" + merkozesID + "') b WHERE a.Id=b.JatekosId ORDER BY a.TeamName, a.Name";
                        SqlCommand command = new SqlCommand(detailedstatscmd, con);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            DetailedStatsGV.DataSource = dataSet;
                            DetailedStatsGV.DataBind();
                        }
                    }
                    string dateTime = DateTime.Now.ToString("yyyy-MM-dd");
                    SqlCommand cmd = new SqlCommand("UPDATE Scorelines SET HomeTeamScore = '" + HomeTeamScore + "', AwayTeamScore = '" + AwayTeamScore + "' WHERE HomeTeam= '" + HomeTeam.Text + "' AND  AwayTeam= '" + AwayTeam.Text + "' AND Date ='" + DateTime.Now.ToString("yyyy-MM-dd") + "'", con);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            HomeTeamSub.Visible = false;
            AwayTeamSub.Visible = false;
            BtnHomeTeamGoal.Visible = false;
            BtnAwayTeamGoal.Visible = false;
            BtnHomeTeamYellow.Visible = false;
            BtnHomeTeamRed.Visible = false;
            BtnAwayTeamYellow.Visible = false;
            BtnAwayTeamRed.Visible = false;
            LTFelido.Visible = false;
            LTPerc.Visible = false;
            BtnFullTime.Visible = false;
            BtnSecondHalf.Visible = false;
            BtnHalfTime.Visible = false;
            BtnPostponed.Visible = false;
            ltCsapatok.Visible = false;
            ltEredmeny.Visible = false;
            HomeTeamsheetLb.Visible = false;
            HomeTeamPlayers.Visible = false;
            HomeTeam11Lb.Visible = false;
            HomeTeamEleven.Visible = false;
            AwayTeamsheetLb.Visible = false;
            AwayTeamPlayers.Visible = false;
            AwayTeam11Lb.Visible = false;
            AwayTeamEleven.Visible = false;
            BtnHomeTeamOwnGoal.Visible = false;
            BtnAwayTeamOwnGoal.Visible = false;
            if (ArticleMaking)
            {
                CikkEsemeny("-", "-", "Félbeszakad", "-");
                File.WriteAllText(Server.MapPath(path), MakeArticle());
                ArticleLb.Visible = true;
            }
        }

        //detailed statistics buttons clicks
        protected void BtnHomeTeamGoodPass_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("SikeresPasszSzam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            UpdateStatistics("OsszesPasszSzam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnAwayTeamGoodPass_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("SikeresPasszSzam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            UpdateStatistics("OsszesPasszSzam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnHomeTeamBadPass_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("OsszesPasszSzam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnAwayTeamBadPass_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("OsszesPasszSzam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnHomeTeamShot_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("KapuraLovesSzam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnAwayTeamShot_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("KapuraLovesSzam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnHomeTeamTackle_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("SzerelesSzam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        protected void BtnAwayTeamTackle_Click(object sender, EventArgs e)
        {
            con.Open();
            UpdateStatistics("SzerelesSzam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            con.Close();
            timerDisplay();
        }

        //showing or not showing the group of buttons needed for detailed statistics
        protected void DetailedStatsButtons(bool x)
        {
            BtnHomeTeamGoodPass.Visible = x;
            BtnAwayTeamGoodPass.Visible = x;
            BtnHomeTeamBadPass.Visible = x;
            BtnAwayTeamBadPass.Visible = x;
            BtnHomeTeamShot.Visible = x;
            BtnAwayTeamShot.Visible = x;
            BtnHomeTeamTackle.Visible = x;
            BtnAwayTeamTackle.Visible = x;
        }

        //own goal buttons
        protected void BtnHomeTeamOwnGoal_Click(object sender, EventArgs e)
        {
            AwayTeamScore += 1;
            ScoreChange();
            string OwnGoalScorer = HomeTeamEleven.SelectedValue + "[OG]";
            int i = 0;
            while (i < AwayGoalScorers.Count && AwayGoalScorers[i].getName() != OwnGoalScorer)
            {
                i = i + 1;
            }
            if (i < AwayGoalScorers.Count)
            {
                AwayGoalScorers[i].setTimes(AwayGoalScorers[i].getTimes() + ", " + hf.Value);
            }
            else
            {
                AwayGoalScorers.Add(new GoalScorer(OwnGoalScorer, hf.Value));
            }
            con.Open();
            UpdateStatistics("Ongolszam", GetJatekosId(HomeTeamEleven.SelectedValue, HomeTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            CikkEsemeny(HomeTeamEleven.SelectedValue, HomeTeam.Text, "Öngól", hf.Value);
            con.Close();
            timerDisplay();
        }

        protected void BtnAwayTeamOwnGoal_Click(object sender, EventArgs e)
        {
            HomeTeamScore += 1;
            ScoreChange();
            string OwnGoalScorer = AwayTeamEleven.SelectedValue + "[OG]";
            int i = 0;
            while (i < HomeGoalScorers.Count && HomeGoalScorers[i].getName() != OwnGoalScorer)
            {
                i = i + 1;
            }
            if (i < HomeGoalScorers.Count)
            {
                HomeGoalScorers[i].setTimes(HomeGoalScorers[i].getTimes() + ", " + hf.Value);
            }
            else
            {
                HomeGoalScorers.Add(new GoalScorer(OwnGoalScorer, hf.Value));
            }
            con.Open();
            UpdateStatistics("Ongolszam", GetJatekosId(AwayTeamEleven.SelectedValue, AwayTeam.Text), GetMerkozesId(HomeTeam.Text, AwayTeam.Text), true);
            CikkEsemeny(AwayTeamEleven.SelectedValue, AwayTeam.Text, "Öngól", hf.Value);
            con.Close();
            timerDisplay();
        }

        //fixing the time and button display in niche cases
        protected void timerDisplay()
        {
            if (hf.Value != "Félidő")
            {
                LTPerc.Text = hf.Value;
            }
            if (hf.Value == "45")
            {
                BtnHalfTime.Style["visibility"] = "visible";
            }
            if (hf.Value == "90")
            {
                BtnFullTime.Style["visibility"] = "visible";
            }
        }

        //creating the automatic article
        protected string MakeArticle()
        {
            string article = "";
            article = article + ArticleStarter();
            article = article + ArticleEvents();
            article = article + ArticleFinisher();
            return article;
        }

        protected string ArticleStarter()
        {
            string starter = "";
            List<string> startingsentenceList = new List<string> { "HT és AT közötti összecsapást rendeztek DT napon. ","HT és AT között mérkőzést rendeztek DT napon. ","HT és AT közötti összecsapásra került sor DT napon. ","HT-AT mérkőzést rendeztek DT napon. ", "DT napon zajlott a/az HT és AT közötti mérkőzés. " };
            int index = r.Next(startingsentenceList.Count);
            starter = starter + startingsentenceList[index];
            starter = starter.Replace("HT", HomeTeam.Text);
            starter = starter.Replace("AT", AwayTeam.Text);
            starter = starter.Replace("DT", DateTime.Now.ToString("yyyy.MM.dd"));
            starter = starter + '\n';
            return starter;
        }

        protected string ArticleEvents()
        {
            string articleevents = "";
            List<string> goalsentenceList = new List<string> { "A TIME. percben gólt szerzett a/az TEAM játékosa, PLAYER. ", "A TIME. percben növelte góljainak számát a/az TEAM, PLAYER révén. ", "PLAYER TIME. percben szerzett gólja növelte TEAM góljainak számát. ", "PLAYER a hálóba talált a TIME. percben, ezzel növelve TEAM góljainak számát. ", "A/az TEAM gólját a TIME. percben PLAYER szerzte. " };
            List<string> ycsentenceList = new List<string> { "A TIME. percben sárga lapos figyelmeztetést kapott a/az TEAM játékosa, PLAYER. ", "A TIME. percben sárga lapos figyelmeztetésben részesült a/az TEAM játékosa, PLAYER. ", "A TIME. percben a bíró sárga lapot mutatott fel. Kapta a/az TEAM játékosa, PLAYER. ", "A TIME. percben a/az TEAM játékosa PLAYER kapott sárga lapos figyelmeztetést. " };
            List<string> ychtsentenceList = new List<string> { "A félidőben sárga lapos figyelmeztetést kapott a/az TEAM játékosa, PLAYER. ", "A félidőben sárga lapos figyelmeztetésben részesült a/az TEAM játékosa, PLAYER. ", "A félidőben a bíró sárga lapot mutatott fel. Kapta a/az TEAM játékosa, PLAYER. ", "A félidőben a/az TEAM játékosa PLAYER kapott sárga lapos figyelmeztetést. " };
            List<string> rcsentenceList = new List<string> { "A TIME. percben PLAYER elhagyta a játékteret egy piros lapot követően. ", "A TIME. percben kapott piros lap után elhagyta a játékteret a/az TEAM játékosa PLAYER. ", "Miután a bíró a TIME. percben felmutatta a piros lapot, PLAYER hagyta el a játékteret. ", "PLAYER a TIME. percben piros lapot kapott, így megfogyatkozott a/az TEAM csapata. " };
            List<string> rchtsentenceList = new List<string> { "A félidőben PLAYER elhagyta a játékteret egy piros lapot követően. ", "A félidőben kapott piros lap után elhagyta a játékteret a/az TEAM játékosa PLAYER. ", "Miután a bíró a félidőben felmutatta a piros lapot, PLAYER hagyta el a játékteret. ", "PLAYER a félidőben piros lapot kapott, így megfogyatkozott a/az TEAM csapata. " };
            List<string> suboffsentenceList = new List<string> { "A TIME. percben cserélt a/az TEAM, PLAYER hagyta el a játékteret. ", "A TIME. percben cserét hajtott végre a/az TEAM, PLAYER hagyta el a játékteret. ", "A TIME. percben történt csere után PLAYER hagyta el a pályát. ", "A TIME. percben a/az TEAM csapata hajtott végre cserét, lejött a pályáról PLAYER. ", "A TIME. percben végrehajtott TEAM csere után PLAYER hagyta el a játékteret. "  };
            List<string> suboffhtsentenceList = new List<string> { "A félidőben cserélt a/az TEAM, PLAYER hagyta el a játékteret. ", "A félidőben cserét hajtott végre a/az TEAM, PLAYER hagyta el a játékteret. ", "A félidőben történt csere után PLAYER hagyta el a pályát. ", "A félidőben a/az TEAM csapata hajtott végre cserét, lejött a pályáról PLAYER. ", "A félidőben végrehajtott TEAM csere után PLAYER hagyta el a játékteret. " };
            List<string> subonsentenceList = new List<string> { "A helyén PLAYER folytatta a játékot. " , "Helyére PLAYER állt be. " , "A helyét a pályán PLAYER vette át. ", "A továbbiakban PLAYER folytatta a játékot. ", "Helyette PLAYER érkezett a játéktérre. "};
            List<string> injurysentenceList = new List<string> { "A TIME. percben sérülés után elhagyta a játékteret a/az TEAM játékosa, PLAYER. " , "PLAYER sérülés miatt nem tudta folytatni a játékot, így a TIME. percben elhagyta a játékteret. ", "PLAYER sérülése után megfogyatkozott TEAM csapata. " };
            List<string> injuryhtsentenceList = new List<string> { "A félidőben sérülés után elhagyta a játékteret a/az TEAM játékosa, PLAYER. ", "PLAYER sérülés miatt nem tudta folytatni a játékot, így a félidőben elhagyta a játékteret. ", "PLAYER sérülése után megfogyatkozott TEAM csapata. " };
            List<string> owngoalsentenceList = new List<string> { "A TIME. percben öngólt szerzett PLAYER. ", "A TIME. percben öngólt vétett PLAYER. ", "A TIME. percben TEAM kárára öngólt szerzett PLAYER. ", "A TIME. percben TEAM kárára öngólt vétett PLAYER. " };
            List<string> postponedsentenceList = new List<string> { "A mérkőzés félbeszakadt. " };
            for (int i = 0; i < MatchEvents.Count; i++)
            {
                if (MatchEvents[i].getEventName() == "Gól")
                {
                    articleevents = articleevents + EventToSentence(MatchEvents[i], goalsentenceList);
                }
                else if (MatchEvents[i].getEventName() == "Sárga lap")
                {
                    if (MatchEvents[i].getTime() == "Félidő")
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], ychtsentenceList);
                    }
                    else
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], ycsentenceList);
                    }
                }
                else if (MatchEvents[i].getEventName() == "Piros lap")
                {
                    if (MatchEvents[i].getTime() == "Félidő")
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], rchtsentenceList);
                    }
                    else
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], rcsentenceList);
                    }
                }
                else if (MatchEvents[i].getEventName() == "Lecserélés")
                {
                    if (MatchEvents[i].getTime() == "Félidő")
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], suboffhtsentenceList);
                    }
                    else
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], suboffsentenceList);
                    }
                }
                else if (MatchEvents[i].getEventName() == "Becserélés")
                {
                    articleevents = articleevents + EventToSentence(MatchEvents[i], subonsentenceList);
                }
                else if (MatchEvents[i].getEventName() == "Sérülés")
                {
                    if (MatchEvents[i].getTime() == "Félidő")
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], injuryhtsentenceList);
                    }
                    else
                    {
                        articleevents = articleevents + EventToSentence(MatchEvents[i], injurysentenceList);
                    }
                }
                else if (MatchEvents[i].getEventName() == "Öngól")
                {
                    articleevents = articleevents + EventToSentence(MatchEvents[i], owngoalsentenceList);
                }
                else
                {
                    articleevents = articleevents + EventToSentence(MatchEvents[i], postponedsentenceList);
                }
                if (MatchEvents[i].getTime() != "Félidő" && i < MatchEvents.Count - 1)
                {
                    if (Int32.Parse(MatchEvents[i].getTime()) < 46)
                    {
                        if (MatchEvents[i + 1].getTime() == "-")
                        {
                            articleevents = articleevents + "LLLLLL\n";
                        }
                        else if (MatchEvents[i + 1].getTime() == "Félidő")
                        {
                            articleevents = articleevents + HalfTimeSentence();
                        }
                        else if (Int32.Parse(MatchEvents[i + 1].getTime()) > 45)
                        {
                            articleevents = articleevents + HalfTimeSentence();
                        }
                    }
                }

            }
            return articleevents;
        }

        protected string EventToSentence(Event esemeny, List<string> templates)
        {
            string sentence = "";
            int index = r.Next(templates.Count);
            sentence = sentence + templates[index];
            sentence = sentence.Replace("PLAYER", esemeny.getPlayerName());
            sentence = sentence.Replace("TEAM", esemeny.getTeamName());
            sentence = sentence.Replace("TIME", esemeny.getTime());
            return sentence;
        }

        protected string HalfTimeSentence()
        {
            string ht = "";
            ht = ht + '\n';
            List<string> htsentenceList = new List<string> { "A félidei eredmény így HTS . ", "A félidei eredmény tehát HTS. ", "HTS eredménnyel fordulhattak a csapatok. ", "A csapatok HTS eredményű első félidő után vonulhattak szünetre. ", "A HTS eredményű félidő után következett a második. " };
            int index = r.Next(htsentenceList.Count);
            ht = ht + htsentenceList[index];
            ht = ht.Replace("HTS", HalfTimeScore);
            ht = ht + '\n';
            return ht;
        }

        protected string ArticleFinisher()
        {
            string last = "";
            last = last +'\n';
            List<string> lastsentenceList = new List<string> { "A/az HT - AT mérkőzés végeredménye tehát HG-AG. " , "A/az HT - AT összecsapás végeredménye HG-AG. ", "A/az HT és AT között rendezett összecsapás végeredménye így HG-AG. ", "A mérkőzés végeredménye így HG-AG. ", "A végeredmény tehát HG-AG. " };
            int index = r.Next(lastsentenceList.Count);
            last = last + lastsentenceList[index];
            last = last.Replace("HT", HomeTeam.Text);
            last = last.Replace("AT", AwayTeam.Text);
            last = last.Replace("HG", HomeTeamScore.ToString());
            last = last.Replace("AG", AwayTeamScore.ToString());
            return last;
        }
    }

    //class to gather the goalscorers data
    public class GoalScorer
    {
        private string Name;
        private string Times;

        public string getName()
        {
            return Name;
        }
        public string getTimes()
        {
            return Times;
        }
        public void setTimes(string Time)
        {
            Times = Time;
        }
        public GoalScorer(string PlayerName, string Time)
        {
            Name = PlayerName;
            Times = Time;
        }

    }

    //class for article making
    public class Event
    {
        private string PlayerName;
        private string TeamName;
        private string EventName;
        private string Time;

        public string getPlayerName()
        {
            return PlayerName;
        }

        public string getTeamName()
        {
            return TeamName;
        }
        public string getEventName()
        {
            return EventName;
        }
        public string getTime()
        {
            return Time;
        }
        public Event(string Player, string Team, string Event, string TimeofE)
        {
            PlayerName = Player;
            TeamName = Team;
            EventName = Event;
            Time = TimeofE;
        }

    }
}