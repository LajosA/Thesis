<%@ Page Title="Statisztika" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Statistics.aspx.cs" Inherits="Szakdolgozat.Statistics" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:Label CssClass="subtitle" ID="TopListaSt" runat="server" Text="Toplista lekérdezése" ></asp:Label>
    </div>
        <div class="row">
            <asp:Label ID="ToplistaAdattagLb" runat="server" Text="Válassza ki, melyik adattagból szeretne toplistát lekérdezni"></asp:Label>
        </div>
        <div class="row">
            <asp:RadioButtonList id="ToplistaRBL" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="75%" CssClass="list">
                <asp:ListItem Value="Golszam" Text="Gólszám"></asp:ListItem>
                <asp:ListItem Value="Ongolszam" Text="Öngólszám"></asp:ListItem>
                <asp:ListItem Value="KapuraLovesSzam" Text="Kapuralövésszám"></asp:ListItem>
                <asp:ListItem Value="SzerelesSzam" Text="Szerelésszám"></asp:ListItem>
                <asp:ListItem Value="MerkozesSzam" Text="Mérkőzésszám"></asp:ListItem>
                <asp:ListItem Value="PasszPontossag" Text="Passzpontosság"></asp:ListItem>
             </asp:RadioButtonList>
           </div>
           <div class="row">
            <asp:Button CssClass="statsbutton" ID="ToplistaLekerdezesBt" runat="server" Text="Toplista Lekérdezése" OnClick="ToplistaLekerdezesBt_Click"/>
            <asp:Label CssClass="errorLb" ID="EmptyRBL" runat="server" Text="Válassza ki valamelyik statisztikát" Visible="false"></asp:Label>
           </div>
    <hr style="height:2px;border-width:0;color:gray;background-color:gray">
    <div>
        <asp:Label CssClass="subtitle" ID="DetailedStatsSt" runat="server" Text="Részletes statisztikák lekérdezése" ></asp:Label>
    </div>
    <div class="row">
        <div class="col-md-4" >
            <asp:Label ID="PlayerNameLb" runat="server" Text="Adja meg a keresendő játékos nevét, ha játékos statisztikáit keresi"></asp:Label>
            <br />
            <asp:TextBox ID="SearchedPlayerTb" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-4">
            <asp:Label ID="TeamNameLb" runat="server" Text="Adja meg a keresendő játékos csapatának nevét, vagy a keresendő csapat nevét"></asp:Label>
            <br />
            <asp:TextBox ID="SearchedTeamTb" runat="server"></asp:TextBox>
        </div>
        </div>
        <div class="row">
            <asp:CheckBoxList ID="NeededStatsCBL" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" Width="55%" CssClass="list">
                <asp:ListItem Value="Golszam" Text="Gólszám"></asp:ListItem>
                <asp:ListItem Value="Ongolszam" Text="Öngólszám"></asp:ListItem>
                <asp:ListItem Value="KapuraLovesSzam" Text="Kapuralövésszám"></asp:ListItem>
                <asp:ListItem Value="SzerelesSzam" Text="Szerelésszám"></asp:ListItem>
                <asp:ListItem Value="PasszSzam" Text="Passzok"></asp:ListItem>
            </asp:CheckBoxList>
        </div>
    <div class="row">
        <div class="col-md-4">
            <asp:Button CssClass="statsbutton" ID="SummedStatsBt" runat="server" Text="Játékos/csapat összesített statisztikái"  OnClick="SummedStatsBt_Click"/>
        </div>
        <div class="col-md-4">
            <asp:Button CssClass="statsbutton" ID="PlayerMatchStatsBt" runat="server" Text="Játékos meccsekre lebontott statisztikái" OnClick="PlayerMatchStatsBt_Click" />
        </div>
    </div>
        <div>
            <asp:Label CssClass="errorLb"  ID="NotEnoughData" runat="server"></asp:Label>
        </div>
    <hr style="height:2px;border-width:0;color:gray;background-color:gray">
    <div>
        <asp:Label CssClass="datalb" ID="DataLb" runat="server"></asp:Label>
    </div>
    <div>
        <asp:GridView ID="StatisztikaGV" runat="server" CssClass="mydatagrid" HeaderStyle-CssClass="gridheader" RowStyle-CssClass="rows">
            <Columns>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
