<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TigerbayFlightApp.Default" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <thead>
                    <tr>
                        <td>
                            Choose your starting point!
                        </td>
                        <td>
                            Choose your destination!
                        </td>
                        <td></td>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlSourceAirport" />
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlDestinationAirport" />
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnCheckFlights" Text="Check Flights!" OnClick="btnCheckFlights_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Repeater runat="server" ID="rptFlightSearchResults" >
                                <HeaderTemplate>
                                    <table>
                                        <thead>
                                            <tr>
                                                Search results
                                            </tr>
                                        </thead>
                                        <tbody>
                                </HeaderTemplate>

                                <ItemTemplate>
                                    <tr>
                                        <td>Outbound origin: <%# Eval("outbound.origincode") %></td>
                                        <td>Outbound destination: <%# Eval("outbound.destinationcode") %></td>
                                    </tr>
                                    <tr>
                                        <td>Flight time: <%# Eval("outbound.departs") %></td>
                                        <td>Flight time: <%# Eval("outbound.arrives") %></td>
                                    </tr>
                                    <tr>
                                        <td>Airline code: <%# Eval("outbound.airlineCode") %></td>
                                        <td>Flight number: <%# Eval("outbound.flightNumber") %></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"><a href="#">Book now!</a></td>
                                    </tr>
                                </ItemTemplate>

                                <FooterTemplate>
                                        </tbody>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:LinkButton runat="server" ID="btnPreviousPage" CommandArgument="" CommandName="" OnCommand="btnPage_Command" Text="Previous Page" Visible="false"></asp:LinkButton>
                            <asp:LinkButton runat="server" ID="btnNextPage" CommandArgument="" CommandName="" OnCommand="btnPage_Command" Text="Next Page" Visible="false"></asp:LinkButton>
                            <asp:Literal runat="server" ID="litFlightSearchResults" />
                            <asp:Literal runat="server" ID="litAlternativeDestinations" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
