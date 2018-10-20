<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="TigerbayFlightApp.Default" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="Static/css/bootstrap-grid.min.css" rel="stylesheet" />
    <link href="Static/css/bootstrap-reboot.min.css" rel="stylesheet" />
    <link href="Static/css/bootstrap.min.css" rel="stylesheet" />
    <script src="Static/js/bootstrap.bundle.min.js"></script>
    <script src="Static/js/bootstrap.min.js"></script>
    <style>
        .top-buffer { margin-top:20px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="row">
                <img src="Static/images/logo.jpg" class="mx-auto img-responsive"/>
            </div>
            <div class="row">
                <div class="col text-center">
                    <h2>Tigerbay Flight Search</h2>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm">
                    Choose your starting location
                </div>
                <div class="col-sm">
                    Choose your destination
                </div>
                <div class="col-sm">
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col-sm">
                    <asp:DropDownList runat="server" ID="ddlSourceAirport" CssClass="form-control"/>
                </div>
                <div class="col-sm">
                    <asp:DropDownList runat="server" ID="ddlDestinationAirport" CssClass="form-control"/>
                </div>
                <div class="col-sm text-center">
                    <asp:LinkButton runat="server" ID="btnCheckFlights" Text="Check Flights!" OnClick="btnCheckFlights_Click" CssClass="btn btn-primary"/>
                </div>
            </div>
        </div>
        <asp:Repeater runat="server" ID="rptFlightSearchResults" >
            <HeaderTemplate>
                <div class="container top-buffer">
            </HeaderTemplate>

            <ItemTemplate>
                <table class="table">
                    <tr>
                        <td>
                            <strong>Outbound origin:</strong>
                        </td>
                        <td>
                            <%# Eval("outbound.origincode") %>
                        </td>
                        <td>
                            <strong>Outbound destination:</strong>
                        </td>
                        <td>
                            <%# Eval("outbound.destinationcode") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Flight departure time:</strong>
                        </td>
                        <td>
                            <%# Eval("outbound.departs") %>
                        </td>
                        <td>
                            <strong>Flight arrival time:</strong>
                        </td>
                        <td>
                            <%# Eval("outbound.arrives") %>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Airline code:</strong>
                        </td>
                        <td>
                            <%# Eval("outbound.airlineCode") %>
                        </td>
                        <td>
                            <strong>Flight number:</strong>
                        </td>
                        <td>
                            <%# Eval("outbound.flightNumber") %>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3"></td>
                        <td><a href="#" class="btn btn-secondary">Book now!</a></td>
                    </tr>
                </table>
            </ItemTemplate>

            <FooterTemplate>
                </div>
            </FooterTemplate>
        </asp:Repeater>
        <div class="container">
            <div class="row">
                <div class="col-lg text-center">
                    <asp:LinkButton runat="server" ID="btnPreviousPage" CommandArgument="" CommandName="" OnCommand="btnPage_Command" Text="Previous Page" Visible="false" CssClass="btn btn-primary"></asp:LinkButton>
                </div>
                <div class="col-lg text-center">
                    <asp:LinkButton runat="server" ID="btnNextPage" CommandArgument="" CommandName="" OnCommand="btnPage_Command" Text="Next Page" Visible="false" CssClass="btn btn-primary"></asp:LinkButton>
                </div>
            </div>
            <div class="row top-buffer">
                <div class="col">
                    <div class='alert alert-danger'>
                        <asp:Literal runat="server" ID="litFlightSearchResults" />
                        <asp:ListView runat="server" ID="lstAlternativeDestinations" >
                            <ItemTemplate>
                                <li>
                                    <%# Container.DataItem %>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
