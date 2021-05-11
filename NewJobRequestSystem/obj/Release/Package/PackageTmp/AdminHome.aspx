<%@ Page Title="" Language="C#" MasterPageFile="~/JOMain.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="NewJobRequestSystem.AdminHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- page content -->
    <div role="main">
        <div class="row">

            <div class="row top_tiles">
                
                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="newJO">0</h1>
                        </div>
                        <h4 style="padding: 5px;">New Job Order for Checking</h4>
                        <a href="ForChecking.aspx" runat="server" id="A3" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>
                
                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="joForAssessment">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Job Order for Assessment</h4>
                        <a href="Assessment.aspx" runat="server" id="A4" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>
                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="joDenied">0</h1>

                        </div>
                        <h4 style="padding: 5px;">Job Order Denied / Cancelled</h4>
                        <a href="Denied.aspx" runat="server" id="A1" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>

                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="joForNoting">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Job Order for Noting</h4>
                        <a href="ForNoting.aspx" runat="server" id="A2" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>
            </div>
            <div class="row top_tiles">

                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="joForApproval">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Job Order For Approval</h4>
                        <a href="ForApproval.aspx" runat="server" id="A7" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>

                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="joWaitingLists">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Job Order Waiting List</h4>
                        <a href="PendingJobOrder.aspx" runat="server" id="A9" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>

                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="joInProgress">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Job Order in Progress</h4>
                        <a href="OnGoingJobOrder.aspx" runat="server" id="A5" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>

                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="completeRequests">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Completed Requests</h4>
                        <a href="Completed.aspx" runat="server" id="A6" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>

                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12">
                    <div class="tile-stats">
                        <div class="icon">
                            <img src="images/servicerequest.png" style="width: 60px;" />
                        </div>
                        <div>
                            <h1 class="count" runat="server" id="allJOCount">0</h1>
                        </div>
                        <h4 style="padding: 5px;">Summary of Requests</h4>
                        <a href="#" runat="server" id="A8" style="padding-left: 5px; color: cadetblue;">Click here to view..</a>
                    </div>
                </div>
                <div class="animated flipInY col-lg-3 col-md-3 col-sm-6 col-xs-12"></div>

            </div>

        </div>
    </div>
    <!-- /page content -->
</asp:Content>
