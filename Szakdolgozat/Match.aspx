<%@ Page Title="Mérkőzés" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Match.aspx.cs" Inherits="Szakdolgozat.Match" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script src="/Scripts/Timer.js"></script>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"> 
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <div>
                    <asp:CheckBox CssClass="aboutCb" ID="DetailedStatsCB" runat="server" Text="Részletes Statisztikák" />
                    <asp:CheckBox CssClass="aboutCb" ID="ArticleNeededCB" runat="server" Text="Automatikusan generált cikk készítése a mérkőzésről" />
                </div>
                <div>
                    <asp:Label CssClass="contactlb" ID="HomeTeamName" runat="server" Text="Hazai csapat: "></asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="HomeTeam" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvHomeTeam" ControlToValidate="HomeTeam" ErrorMessage="Adja meg a csapat nevét"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <asp:Label CssClass="contactlb" ID="AwayTeamName" runat="server" Text="Vendég csapat: "></asp:Label>
                </div>
                <div>
                    <asp:TextBox ID="AwayTeam" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvAwayTeam" ControlToValidate="AwayTeam" ErrorMessage="Adja meg a csapat nevét"></asp:RequiredFieldValidator>
                </div>
                <div>
                    <asp:Label CssClass="contactlb" ID="HomeTeamsheetLb" Text="Hazai csapat kerete:" runat="server"></asp:Label>
                    <asp:DropDownList CssClass="playerddl" ID="HomeTeamPlayers" runat="server" ></asp:DropDownList>
                    <asp:Button CssClass="elevenbutton" ID="AddToHome11" runat="server" Text="Játékos hozzáadása a hazai kezdő 11hez" OnClick="AddToHome11_Click" />
                    <asp:Label CssClass="errorLb" ID="HT11Error" Text="Ez a játékos szerepel már a hazai kezdő 11ben" runat="server"></asp:Label>
                </div>
                <div>
                    <asp:Label CssClass="contactlb" ID="HomeTeam11Lb" Text="Hazai csapat kezdő tizenegye:" runat="server"></asp:Label>
                    <asp:DropDownList CssClass="playerddl" ID="HomeTeamEleven" runat="server" ></asp:DropDownList>
                    <asp:Button CssClass="elevenbutton" ID="RemoveHomeEleven" runat="server" Text="Játékos eltávolítás a hazai kezdőcsapatból" Onclick="RemoveHomeEleven_Click" />
                </div>
                <div>
                    <asp:Label CssClass="contactlb" ID="AwayTeamsheetLb" Text="Vendég csapat kerete:" runat="server"></asp:Label>
                    <asp:DropDownList CssClass="playerddl" ID="AwayTeamPlayers" runat="server" ></asp:DropDownList>
                    <asp:Button CssClass="elevenbutton" ID="AddToAway11" runat="server" Text="Játékos hozzáadása a vendég kezdő 11hez" OnClick="AddToAway11_Click" />
                    <asp:Label CssClass="errorLb" ID="AT11Error" Text="Ez a játékos szerepel már a vendég kezdő 11ben" runat="server" ></asp:Label>
                </div>
                <div>
                    <asp:Label CssClass="contactlb" ID="AwayTeam11Lb" Text="Vendég csapat kezdő tizenegye:" runat="server"></asp:Label>
                    <asp:DropDownList CssClass="playerddl" ID="AwayTeamEleven" runat="server" ></asp:DropDownList>
                    <asp:Button CssClass="elevenbutton" ID="RemoveAwayEleven" runat="server" Text="Játékos eltávolítás a vendég kezdőcsapatból" Onclick="RemoveAwayEleven_Click" />
                </div>
                <div>
                    <asp:Button CssClass="button" ID="StartingElevensBtn" runat="server" Text="Kezdő tizenegyek megadása" OnClick="StartingElevensBtn_Click"/>
                    <asp:Label CssClass="errorLb" ID="SameTeamLb" Text="Ugyanaz a csapat nem játszhat saját maga ellen" runat="server"></asp:Label>
                    <asp:Label CssClass="errorLb" ID="TooFewHomePlayers" runat="server" Text="Túl kevés játékos van az adatbázisban a hazai csapatból."></asp:Label>
                    <asp:Label CssClass="errorLb" ID="TooFewAwayPlayers" runat="server" Text="Túl kevés játékos van az adatbázisban a vendég csapatból."></asp:Label>
                    <asp:Label CssClass="errorLb" ID="TooFewLink" runat="server"><a href="NewData">Kattintson ide, hogy több játékost hozzáadjon az adatbázishoz.</a></asp:Label>
                    <asp:Label CssClass="errorLb" ID="ThereWasAMatch" runat="server" Text="A megadott csapatok már játszottak a mai napon"></asp:Label>
                </div>
                <div>
                     <asp:Button CssClass="button" ID="StartMatchBtn" runat="server" Text="Kezdődik a mérkőzés" onclientclick="kezdes()" OnClick="StartMatchBtn_Click"/>
                </div>
                <div>
                    <asp:Label CssClass="teamsLb" ID="ltCsapatok" runat="server"></asp:Label>
                </div>
                <div>
                    <asp:Label CssClass="teamsLb" ID="ltEredmeny" runat="server"></asp:Label>
                </div>
                    <div>
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamGoal" runat="server" Text="Hazai gól" OnClick="BtnHomeTeamGoal_Click"/>
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamOwnGoal" runat="server" Text="Hazai öngól" OnClick="BtnHomeTeamOwnGoal_Click"/>
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamYellow" runat="server" Text="Hazai sárga lap" OnClick="BtnHomeTeamYellow_Click" />
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamRed" runat="server" Text="Hazai piros lap" OnClick="BtnHomeTeamRed_Click" />
                        <asp:Button CssClass="matchbutton" ID="HomeTeamSub" runat="server" Text="Hazai csere" OnClick="HomeTeamSub_Click" />
                    </div>
                    <div>
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamGoal" runat="server" Text="Vendég gól" OnClick="BtnAwayTeamGoal_Click"/>
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamOwnGoal" runat="server" Text="Vendég öngól" OnClick="BtnAwayTeamOwnGoal_Click"/>
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamYellow" runat="server" Text="Vendég sárga lap" OnClick="BtnAwayTeamYellow_Click" />
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamRed" runat="server" Text="Vendég piros lap" OnClick="BtnAwayTeamRed_Click" />
                        <asp:Button CssClass="matchbutton2" ID="AwayTeamSub" runat="server" Text="Vendég csere" OnClick="AwayTeamSub_Click" />
                    </div>
                    <div>
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamGoodPass" runat="server" Text="Hazai sikeres passz" OnClick="BtnHomeTeamGoodPass_Click" />
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamBadPass" runat="server" Text="Hazai sikertelen passz" OnClick="BtnHomeTeamBadPass_Click" />
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamShot" runat="server" Text="Hazai lövés" OnClick="BtnHomeTeamShot_Click" />
                        <asp:Button CssClass="matchbutton" ID="BtnHomeTeamTackle" runat="server" Text="Hazai szerelés" OnClick="BtnHomeTeamTackle_Click" />
                    </div>
                    <div>
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamGoodPass" runat="server" Text="Vendég sikeres passz" OnClick="BtnAwayTeamGoodPass_Click" />
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamBadPass" runat="server" Text="Vendég sikertelen passz" OnClick="BtnAwayTeamBadPass_Click" />
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamShot" runat="server" Text="Vendég lövés" OnClick="BtnAwayTeamShot_Click" />
                        <asp:Button CssClass="matchbutton2" ID="BtnAwayTeamTackle" runat="server" Text="Vendég szerelés" OnClick="BtnAwayTeamTackle_Click" />
                    </div>
                <div>
                    <asp:Button CssClass="button" ID="BtnSecondHalf" runat="server" Text="2. Félidő kezdődik" onclientclick="ujrakezdes()" OnClick="BtnSecondHalf_Click"/>
                </div>
                <asp:Label CssClass="teamsLb" ID="LTFelido" runat="server"></asp:Label>
                <asp:Label CssClass="teamsLb" ID="LTPerc" ClientIDMode="Static" runat="server"></asp:Label>
                <asp:Button CssClass="button" ID="BtnHalfTime" runat="server" ClientIDMode="Static" Text="Félidő" OnClick="BtnHalfTime_Click" Style="visibility:hidden"/>
                <asp:Button CssClass="button" ID="BtnFullTime" runat="server" ClientIDMode="Static" Text="Mérkőzés vége" OnClick="BtnFullTime_Click" Style="visibility:hidden"/>
                <asp:Button CssClass="postponebutton" ID="BtnPostponed" runat="server" Text="A mérkőzés félbeszakad" OnClick="BtnPostponed_Click" />
                <asp:Label CssClass="teamsLb" ID="LbPostponed" runat="server"></asp:Label>
                <asp:Label  ID="Label1" ClientIDMode="Static" runat="server"></asp:Label>           
                <asp:HiddenField ID="hf" runat="server" ClientIDMode="Static" />

                <div>
                    <asp:Label CssClass="scorelineLb" ID="ltVégeredmény" runat="server"></asp:Label>
                </div>
                <div>
                    <asp:Label CssClass="goalscorersLb" ID="LtGólszerzők" runat="server"></asp:Label>
                </div>
                <div>
                    <asp:GridView ID="DetailedStatsGV" runat="server" CssClass="mydatagrid" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="rows">
                        <Columns>
                        </Columns>
                    </asp:GridView>
                </div>
                <div>
                    <asp:Label CssClass="linktoarticle" ID="ArticleLb" runat="server"><a style="color:black !important" runat="server" href="~/Article">A mérkőzésről generált cikk itt olvasható</a></asp:Label>
                </div>
            </ContentTemplate> 
        </asp:UpdatePanel>
    </div>
</asp:Content>
