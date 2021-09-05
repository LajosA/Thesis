<%@ Page Title="Adatbevitel" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewData.aspx.cs" Inherits="Szakdolgozat.NewData" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label CssClass="title" ID="ContactTitleLb" runat="server" Text="Új játékos felvétele az adatbázisba"></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="contactlb" ID="PlayerNameLb" runat="server" Text="Játékos neve: "></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="PlayerName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvPlayerName" ControlToValidate="PlayerName" ErrorMessage="Adjon meg valamit"></asp:RequiredFieldValidator>
    </div>
    <div>
        <asp:Label CssClass="contactlb" ID="TeamNameLb" runat="server" Text="Játékos csapatának neve: "></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="TeamName" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvTeamName" ControlToValidate="TeamName" ErrorMessage="Adjon meg valamit"></asp:RequiredFieldValidator>
    </div>
    <div>
        <asp:Label CssClass="contactlb" ID="KitNumberLb" runat="server" Text="Játékos mezszáma: "></asp:Label>
    </div>
    <div>
        <asp:TextBox ID="KitNumber" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator CssClass="errorLb" runat="server" ID="rfvKitNumber" ControlToValidate="KitNumber" ErrorMessage="Adjon meg valamit"></asp:RequiredFieldValidator>
        <asp:RangeValidator CssClass="errorLb" runat="server" Type="Integer" MinimumValue="1" MaximumValue="99" ControlToValidate="KitNumber" ErrorMessage="Adja meg a játékos tényleges mezszámát" />
    </div>
    <div>
        <asp:Button CssClass="button" ID="AddPlayer" runat="server" Text="Játékos hozzáadása az adatbázishoz" OnClick="AddPlayer_Click"/>
    </div>
    <div>
        <asp:Label CssClass="errorLb" ID="ltHiba" runat="server"></asp:Label>
    </div>
    <div>
        <asp:Label CssClass="succesLb" ID="lbSuccess" runat="server" Text="Játékos sikeresen hozzáadva az adatbázishoz"></asp:Label>
    </div>
</asp:Content>
