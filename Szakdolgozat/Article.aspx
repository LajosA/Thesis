<%@ Page Title="Generált cikkek" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Article.aspx.cs" Inherits="Szakdolgozat.Article" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label CssClass="title" ID="ArticleTitle" runat="server" Text="Cikkírás"></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="subtitle" ID="subTitle" runat="server" Text="Válassza ki, melyik mérkőzés cikkét szeretné lekérni"></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="contactlb" ID="HomeTeamLb" runat="server" Text="Hazai csapat: "></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="HomeTeamName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvHTName" ControlToValidate="HomeTeamName" ErrorMessage="Adjon meg valamit"></asp:RequiredFieldValidator>
    </div>
    <div>
        <asp:Label CssClass="contactlb" ID="AwayTeamLb" runat="server" Text="Vendég csapat: "></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="AwayTeamName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvATName" ControlToValidate="AwayTeamName" ErrorMessage="Adjon meg valamit"></asp:RequiredFieldValidator>
    </div>
    <div>
        <asp:Label CssClass="contactlb" ID="DayLb" runat="server" Text="Mérkőzés időpontja: "></asp:Label>
    </div>
    <div>
        <asp:Calendar CssClass="calendar" ID="Calendar" runat="server">
            <SelectedDayStyle CssClass="calendarSelector"/>
            <SelectorStyle CssClass="calendarSelector" />
        </asp:Calendar>
        <asp:Label CssClass="errorLb2" runat="server" ID="lbCalendarError" Text="Válassza ki a keresett mérkőzés napját"></asp:Label>
    </div>
    <div>
        <asp:Button CssClass="button" ID="BtSearchForArticle" runat="server" Text="Cikk keresése" OnClick="BtSearchForArticle_Click"/>
    </div>
    <div>
        <asp:Label CssClass="errorLb2" ID="LbNotExist" runat="server" Text="Megadott napon nem készült cikk ilyen mérkőzésről"></asp:Label>
    </div>
    <div>
        <asp:TextBox CssClass="article" ID="TbArticle" runat="server" TextMode="MultiLine" Rows="15"></asp:TextBox>
    </div>
    <div>
        <asp:Button CssClass="button" ID="BtEditArticle" runat="server" Text="Cikk szerkesztése" OnClick="BtEditArticle_Click"/>
        <asp:Button CssClass="button" ID="BtUpdateArticle" runat="server" Text="Cikk frissítése" OnClick="BtUpdateArticle_Click"/>
    </div>
    <div>
        <asp:Label CssClass="succesLb" ID="lbSuccess" runat="server" Text="Cikk sikeresen frissítve!"></asp:Label>
    </div>
</asp:Content>
