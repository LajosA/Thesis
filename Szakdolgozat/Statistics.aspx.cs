using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace Szakdolgozat
{
    public partial class Statistics : System.Web.UI.Page
    {
        SqlConnection statcon = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\MatchesDatabase.mdf;Integrated Security=True");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                NotEnoughData.Visible = false;
            }
        }

        protected void ToplistaLekerdezesBt_Click(object sender, EventArgs e)
        {
            ClearGV();
            DataLb.Text = "";
            if (ToplistaRBL.SelectedIndex > -1)
            {
                NotEnoughData.Visible = false;
                EmptyRBL.Visible = false;
                string commandstring = "";
                if (ToplistaRBL.SelectedIndex < 4)
                {
                    commandstring = "SELECT b.Name as Név, b.TeamName as Csapatnév, a." + ToplistaRBL.SelectedItem.Text + " as " + ToplistaRBL.SelectedItem.Text + " FROM (SELECT top 10 JatekosId, sum(" + ToplistaRBL.SelectedValue + ") AS " + ToplistaRBL.SelectedItem.Text + " FROM PlayerStatistics GROUP BY JatekosId HAVING sum(" + ToplistaRBL.SelectedValue + ")>0 ORDER BY sum(" + ToplistaRBL.SelectedValue + ") desc) a , Players b WHERE a.JatekosId=b.Id";
                }
                else if (ToplistaRBL.SelectedIndex == 4)
                {
                    commandstring = "SELECT b.Name as Név, b.TeamName as Csapatnév, a.Mérkőzésszám as Mérkőzésszám FROM (SELECT top 10 JatekosId, sum(Kezdocsapatszam)+sum(Becserelesszam) AS Mérkőzésszám FROM PlayerStatistics GROUP BY JatekosId HAVING sum(Kezdocsapatszam)+sum(Becserelesszam)>0 ORDER BY sum(Kezdocsapatszam)+sum(Becserelesszam) desc) a , Players b WHERE a.JatekosId=b.Id";
                }
                else if (ToplistaRBL.SelectedIndex == 5)
                {
                    commandstring = "SELECT b.Name as Név, b.TeamName as Csapatnév, a.PasszPontosság as Passzpontosság FROM (SELECT top 10 JatekosId, FORMAT( round(CAST(sum(SikeresPasszSzam) AS FLOAT)/CAST(sum(OsszesPasszSzam) AS FLOAT),4),'P') AS PasszPontosság FROM PlayerStatistics GROUP BY JatekosId HAVING sum(OsszesPasszSzam)>0 AND CAST(sum(SikeresPasszSzam) AS FLOAT)/(CAST(sum(OsszesPasszSzam) AS FLOAT)+1)>0 ORDER BY CAST(sum(SikeresPasszSzam) AS FLOAT)/CAST(sum(OsszesPasszSzam) AS FLOAT) desc) a , Players b WHERE a.JatekosId=b.Id";
                }
                statcon.Open();

                using (statcon)
                {
                    SqlCommand command = new SqlCommand(commandstring, statcon);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataSet dataSet = new DataSet();
                    dataAdapter.Fill(dataSet);
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        StatisztikaGV.DataSource = dataSet;
                        StatisztikaGV.DataBind();
                    }
                }
                statcon.Close();


            }
            else
            {
                EmptyRBL.Visible = true;
            }

        }

        protected void SummedStatsBt_Click(object sender, EventArgs e)
        {
            EmptyRBL.Visible = false;
            ClearGV();
            DataLb.Text = "";
            if (SearchedTeamTb.Text.Length > 0)
            {
                if (SearchedPlayerTb.Text.Length > 0)
                {
                    NotEnoughData.Visible = false;
                    statcon.Open();
                    String check = "select count(*) from Players where TeamName LIKE '" + SearchedTeamTb.Text + "' AND Name LIKE '" + SearchedPlayerTb.Text + "'";
                    SqlCommand CheckExists = new SqlCommand(check, statcon);
                    int CE = Convert.ToInt32(CheckExists.ExecuteScalar().ToString());
                    if (CE != 1)
                    {
                        NotEnoughData.Visible = true;
                        NotEnoughData.Text = "Nem létezik ilyen játékos";
                    }
                    else
                    {
                        DataLb.Text = SearchedTeamTb.Text + " : " + SearchedPlayerTb.Text;
                        String PlayerIDSearchString = "select Id from Players where TeamName LIKE '" + SearchedTeamTb.Text + "' AND Name LIKE '" + SearchedPlayerTb.Text + "'";
                        SqlCommand PlayerIDSearch = new SqlCommand(PlayerIDSearchString, statcon);
                        int PlayerID = Convert.ToInt32(PlayerIDSearch.ExecuteScalar().ToString());

                        string WhatToSelect = "sum(KezdocsapatSzam)+sum(BecserelesSzam) AS Mérkőzésszám,";
                        for (int i = 0; i < 5; i++)
                        {
                            if (NeededStatsCBL.Items[i].Selected)
                            {
                                if (i < 4)
                                {
                                    WhatToSelect = WhatToSelect + "sum(" + NeededStatsCBL.Items[i].Value + ") AS " + NeededStatsCBL.Items[i].Text + ",";
                                    if (i == 2)
                                    {
                                        WhatToSelect = WhatToSelect + "FORMAT(round(ISNULL((CAST (sum(Golszam) AS FLOAT)) / NULLIF((CAST (sum(KapuraLovesSzam) AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Lövési_Pontosság,";
                                    }
                                }
                                else
                                {
                                    WhatToSelect = WhatToSelect + "sum(SikeresPasszSzam) as Pontos_Passzok, sum(OsszesPasszSzam) as Összes_Passzok, FORMAT(round(ISNULL((CAST (sum(SikeresPasszSzam) AS FLOAT)) / NULLIF((CAST (sum(OsszesPasszSzam) AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság,";
                                }
                            }
                        }
                        WhatToSelect = WhatToSelect.Remove(WhatToSelect.Length - 1, 1);
                        string commandstring = "SELECT " + WhatToSelect + " FROM PlayerStatistics WHERE JatekosId='" + PlayerID + "' GROUP BY JatekosId";
                        using (statcon)
                        {

                            SqlCommand command = new SqlCommand(commandstring, statcon);
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                StatisztikaGV.DataSource = dataSet;
                                StatisztikaGV.DataBind();
                            }
                        }

                    }
                    statcon.Close();
                }
                else
                {
                    NotEnoughData.Visible = false;
                    statcon.Open();
                    String check = "select count(*) from Players where TeamName LIKE '" + SearchedTeamTb.Text + "'";
                    SqlCommand CheckExists = new SqlCommand(check, statcon);
                    int CE = Convert.ToInt32(CheckExists.ExecuteScalar().ToString());
                    if (CE < 1)
                    {
                        NotEnoughData.Visible = true;
                        NotEnoughData.Text = "Nem létezik ilyen csapat";
                    }
                    else
                    {
                        DataLb.Text = SearchedTeamTb.Text;
                        string WhatToSelect = "JatekosId, sum(KezdocsapatSzam)+sum(BecserelesSzam) AS Mérkőzésszám,";
                        string WhatToSelectFinal = "a.Name as Név, b.Mérkőzésszám,";
                        for (int i = 0; i < 5; i++)
                        {
                            if (NeededStatsCBL.Items[i].Selected)
                            {
                                if (i < 4)
                                {
                                    WhatToSelect = WhatToSelect + "sum(" + NeededStatsCBL.Items[i].Value + ") AS " + NeededStatsCBL.Items[i].Text + ",";
                                    WhatToSelectFinal = WhatToSelectFinal + "b." + NeededStatsCBL.Items[i].Text + ",";
                                    if (i == 2)
                                    {
                                        WhatToSelect = WhatToSelect + "FORMAT(round(ISNULL((CAST (sum(Golszam) AS FLOAT)) / NULLIF((CAST (sum(KapuraLovesSzam) AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Lövési_Pontosság,";
                                        WhatToSelectFinal = WhatToSelectFinal + "b.Lövési_Pontosság,";
                                    }
                                }
                                else
                                {
                                    WhatToSelect = WhatToSelect + "sum(SikeresPasszSzam) as Pontos_Passzok, sum(OsszesPasszSzam) as Összes_Passzok, FORMAT(round(ISNULL((CAST (sum(SikeresPasszSzam) AS FLOAT)) / NULLIF((CAST (sum(OsszesPasszSzam) AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság,";
                                    WhatToSelectFinal = WhatToSelectFinal + "b.Pontos_Passzok, b.Összes_Passzok, b.Passzpontosság,";
                                }
                            }
                        }
                        WhatToSelect = WhatToSelect.Remove(WhatToSelect.Length - 1, 1);
                        WhatToSelectFinal = WhatToSelectFinal.Remove(WhatToSelectFinal.Length - 1, 1);
                        string commandstring = "SELECT " + WhatToSelectFinal + " FROM (SELECT Name, Id FROM Players Where TeamName LIKE '" + SearchedTeamTb.Text + "') a , (SELECT " + WhatToSelect + " FROM PlayerStatistics WHERE JatekosID IN (SELECT Id From Players WHERE TeamName LIKE '" + SearchedTeamTb.Text + "') GROUP BY JatekosId) b WHERE a.Id=b.JatekosId";
                        using (statcon)
                        {

                            SqlCommand command = new SqlCommand(commandstring, statcon);
                            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                            DataSet dataSet = new DataSet();
                            dataAdapter.Fill(dataSet);
                            if (dataSet.Tables[0].Rows.Count > 0)
                            {
                                StatisztikaGV.DataSource = dataSet;
                                StatisztikaGV.DataBind();
                            }
                        }

                    }
                    statcon.Close();
                }
            }
            else
            {
                NotEnoughData.Visible = true;
                NotEnoughData.Text = "Adja meg a keresett csapat nevét, vagy a keresett csapat és játékos nevét is";
            }
        }

        protected void PlayerMatchStatsBt_Click(object sender, EventArgs e)
        {
            EmptyRBL.Visible = false;
            ClearGV();
            DataLb.Text = "";
            if (SearchedPlayerTb.Text.Length > 0 && SearchedTeamTb.Text.Length > 0)
            {
                NotEnoughData.Visible = false;
                statcon.Open();

                String check = "select count(*) from Players where TeamName LIKE '" + SearchedTeamTb.Text + "' AND Name LIKE '" + SearchedPlayerTb.Text + "'";
                SqlCommand CheckExists = new SqlCommand(check, statcon);
                int CE = Convert.ToInt32(CheckExists.ExecuteScalar().ToString());
                if (CE != 1)
                {
                    NotEnoughData.Visible = true;
                    NotEnoughData.Text = "Nem létezik ilyen játékos";
                }
                else
                {
                    DataLb.Text = SearchedTeamTb.Text + " : " + SearchedPlayerTb.Text;
                    //string WhatToSelect = "c.HomeTeam AS Hazai_Csapat ,c.AwayTeam AS Vendég_Csapat,c.HomeTeamScore AS Eredmény ,c.AwayTeamScore AS [ ],c.Date AS Dátum,";
                    string WhatToSelect = "c.HomeTeam AS Hazai_Csapat ,c.AwayTeam AS Vendég_Csapat,c.HomeTeamScore AS Eredmény ,c.AwayTeamScore AS [ ],convert(varchar(10), c.Date, 120) AS Dátum,";
                    for (int i = 0; i < 5; i++)
                    {
                        if (NeededStatsCBL.Items[i].Selected)
                        {
                            if (i < 4)
                            {
                                WhatToSelect = WhatToSelect + "b." + NeededStatsCBL.Items[i].Value + " AS " + NeededStatsCBL.Items[i].Text + ",";
                                if (i == 2)
                                {
                                    WhatToSelect = WhatToSelect + "FORMAT(round(ISNULL((CAST(b.Golszam AS FLOAT)) / NULLIF((CAST(b.KapuraLovesSzam AS FLOAT)), (CAST(0 AS FLOAT))), 0), 4), 'P') AS Lövési_Pontosság,";
                                }
                            }
                            else
                            {
                                WhatToSelect = WhatToSelect + "b.SikeresPasszSzam as Pontos_Passzok, b.OsszesPasszSzam as Összes_Passzok, FORMAT(round(ISNULL((CAST (b.SikeresPasszSzam AS FLOAT)) / NULLIF((CAST (b.OsszesPasszSzam AS FLOAT)),(CAST (0 AS FLOAT))),0),4),'P') AS Passzpontosság ,";
                            }
                        }
                    }

                    WhatToSelect = WhatToSelect.Remove(WhatToSelect.Length - 1, 1);
                    string commandstring = "SELECT " + WhatToSelect + " FROM (SELECT Id from Players where TeamName LIKE '" + SearchedTeamTb.Text + "' AND Name LIKE '" + SearchedPlayerTb.Text + "') a , (SELECT * FROM PlayerStatistics) b , (SELECT * FROM ScoreLines) c WHERE a.Id=b.JatekosId AND b.MerkozesId=c.Id";
                    using (statcon)
                    {

                        SqlCommand command = new SqlCommand(commandstring, statcon);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet);
                        if (dataSet.Tables[0].Rows.Count > 0)
                        {
                            StatisztikaGV.DataSource = dataSet;
                            StatisztikaGV.DataBind();
                        }
                    }
                }
                statcon.Close();
            }
            else
            {
                NotEnoughData.Visible = true;
                NotEnoughData.Text = "Adja meg a keresett játékos nevét, és csapatának nevét is";
            }
        }

        protected void ClearGV()
        {
            StatisztikaGV.DataSource = null;
            StatisztikaGV.DataBind();
        }

    }
}