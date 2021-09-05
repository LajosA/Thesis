<%@ Page Title="Description" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Szakdolgozat._Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label CssClass="startertitle" ID="HelpTitle" runat="server" Text="Multifunkciós sportrendezvény adminisztráló rendszer"></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="subtitle" ID="NewPlayerSubtitle" runat="server"><a style="color:black !important" href="NewData">Itt új játékost adhat az adatbázishoz</a></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="starter" ID="NewPlayerLb" runat="server" Text="Ezen az oldalon a felhasználónak lehetősége nyílik arra, hogy új adatokat vigyen be az adatbázisba. A felhasználó új játékosok adatait viheti be a programba, későbbi felhasználásra. "></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="subtitle" ID="MatchSubtitle" runat="server"><a style="color:black !important" href="Match">Itt levezethet egy mérkőzést</a></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="starter" ID="MatchLb" runat="server" Text="A felhasználó ezen az oldalon tudja levezetni egy mérkőzés történéseit, valós időben. Lehetősége nyílik arra is, hogy kiválassza, milyen részletességgel szeretné leírni a mérkőzést, és milyen módon szeretné a későbbiekben felhasználni a leírtakat."></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="subtitle" ID="StatsSubtitle" runat="server"><a style="color:black !important" href="Statistics">Itt lekérheti a statisztikákat</a></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="starter" ID="StatsLb" runat="server" Text="A Statisztikák oldalon a felhasználó az előzőekben leírt meccsekből készített statisztikákat kérheti le, vizsgálhatja. Két jól elkülönített részen lehet különböző indíttatásból, különböző típusú statisztikákat lekérni. Bármilyen típusú statisztikát kér le a felhasználó, a statisztika egy táblázatban az oldal alján fog megjelenni. "></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="subtitle" ID="ArticleSubtitle" runat="server"><a style="color:black !important" href="Article">Itt elolvashatja és módosíthatja az automatikusan generált cikkeket</a></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="starter" ID="ArticleLb" runat="server" Text="A felhasználó ezen az oldalon rákereshet az automatikusan generált cikkekre a levezetett mérkőzésekről. A felhasználó továbbá tudja szerkeszteni, és elmenti az automatikusan generált cikkeket is."></asp:Label>
    </div>
</asp:Content>
