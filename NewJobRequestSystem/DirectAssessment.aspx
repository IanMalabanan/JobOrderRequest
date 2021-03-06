<%@ Page Title="Asessment" Language="C#" MasterPageFile="~/Signatories.Master" AutoEventWireup="true" CodeBehind="DirectAssessment.aspx.cs" Inherits="NewJobRequestSystem.DirectAssessment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .orderText {
            font: normal 12px Arial,Verdana;
            margin-top: 6px;
        }

        .RadWindow .rwTitleWrapper {
            box-sizing: content-box;
        }
    </style>


    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <script type="text/javascript">

            function ShowRejectForm(empid, code, action) {
                window.radopen("RejectRequest.aspx?EmpID=" + empid + "&RCode=" + code + "&Action=" + action, "RejectRequest");
                return false;
            }

            function ShowHoldForm(empid, code, action) {
                window.radopen("HoldRequest.aspx?EmpID=" + empid + "&RCode=" + code + "&Action=" + action, "HoldRequest");
                return false;
            }

        </script>
    </telerik:RadCodeBlock>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="card" runat="server" id="divApplication">
        <div class="card-body">
            <div class="row" runat="server" id="rowHold" visible="false">

                <div class="col-lg-12">

                    <div class="alert alert-warning" role="alert">

                        <asp:Label runat="server" ID="lblHoldRemarks" Font-Size="Medium"></asp:Label>
                    </div>

                </div>

            </div>


            <div class="row">

                <div class="col-4">

                    <label class="docconlabels">Date Filed:</label>

                    <asp:TextBox ID="tbdateRequest" runat="server" ReadOnly="true" placeholder="Enter Text Here..."
                        CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-8">
                    <fieldset>
                        <legend>

                            <b class="pull-left">In-Charge</b>

                            <telerik:RadCheckBox runat="server" CssClass="pull-right" ID="chkEditInCharged" Text="Change In-Charge?"
                                AutoPostBack="true" OnCheckedChanged="chkEditInCharged_CheckedChanged" />

                        </legend>

                        <telerik:RadRadioButtonList runat="server" ID="rdoInCharge" DataBindings-DataTextField="incharge"
                            DataBindings-DataValueField="incharge_id" Font-Size="Large" AutoPostBack="true" Direction="Horizontal"
                            Enabled="false" OnSelectedIndexChanged="rdoInCharge_SelectedIndexChanged">
                        </telerik:RadRadioButtonList>

                        <telerik:RadButton ID="btnUpdateAssestor" runat="server"
                            Text="Change" SingleClick="true" Visible="false"
                            Primary="true" RenderMode="Lightweight" Enabled="true"
                            Style="width: 150px; height: 38px; margin-top: 30px"
                            SingleClickText="Processing..." OnClick="ChangeIncharge">

                            <ConfirmSettings ConfirmText="By clicking Yes/OK will submit the request. Do you want to proceed?" UseRadConfirm="true" Title="Confirm Action" />
                        </telerik:RadButton>

                    </fieldset>
                </div>

            </div>

            <br />

            <div class="row">

                <div class="col-4">

                    <label class="docconlabels">Name of Requestor:</label>

                    <asp:TextBox ID="tbReqName" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-4">

                    <label class="docconlabels">Department:</label>

                    <asp:TextBox ID="tbDepartment" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-4">

                    <label class="docconlabels">Section:</label>

                    <asp:TextBox ID="tbSection" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

            </div>

            <br />
            <div class="row">
                <div class="col-12">
                    <fieldset>
                        <legend><b>Request Type</b></legend>
                        <telerik:RadCheckBoxList runat="server" ID="rdoReqType" DataBindings-DataTextField="Request_Type" DataBindings-DataValueField="RequestTypeCode"
                            Font-Size="Large" AutoPostBack="true" Direction="Horizontal" Enabled="false">
                        </telerik:RadCheckBoxList>
                    </fieldset>
                </div>
            </div>
            <br />



            <div class="row" runat="server" id="rowFacilities">
                <div class="col-lg-12">
                    <fieldset>
                        <legend><b>Facilities / Maintenance / Automation / Fabrication</b></legend>

                        <div class="row">

                            <div class="col-lg-4">

                                <label class="docconlabels">Item Name:</label>

                                <asp:TextBox ID="tbItemName" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                            </div>

                            <div class="col-lg-4">

                                <label class="docconlabels">Quantity:</label>

                                <asp:TextBox ID="tbItemQuantity" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="Number"></asp:TextBox>

                            </div>

                        </div>
                    </fieldset>

                    <br />
                </div>
            </div>

            <div class="row" runat="server" id="rowJigs" visible="false">

                <div class="col-12">

                    <div class="row">
                        <div class="col-6">
                            <fieldset>
                                <legend><b>Process Jigs / Inspection Jigs</b></legend>

                                <div class="row">

                                    <div class="col-12">

                                        <label class="docconlabels">Customer:</label>

                                        <asp:TextBox runat="server" ID="tbCustomer" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-12">

                                        <label class="docconlabels">Partcode:</label>

                                        <asp:TextBox runat="server" ID="tbPartCode" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-12">

                                        <label class="docconlabels">Partname:</label>

                                        <asp:TextBox runat="server" ID="tbPartName" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>



                                </div>

                                <br />
                                <div class="row">

                                    <div class="col-8">

                                        <label class="docconlabels">Type Of Jig:</label>

                                        <asp:TextBox runat="server" ID="tbTypeOfJig" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                    <div class="col-4">

                                        <label class="docconlabels">Quantity:</label>

                                        <asp:TextBox runat="server" ID="tbQuantity_Jigs" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>

                            </fieldset>
                        </div>
                        <div class="col-6">

                            <fieldset>

                                <legend><b>For Jigs Request</b></legend>

                                <div class="row">
                                    <div class="col-8">

                                        <label class="docconlabels">Quantity produce per hour:</label>

                                        <asp:TextBox runat="server" ID="qtyhrtxt" Width="100%" ReadOnly="true" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>

                                <br />

                                <div class="row">

                                    <div class="col-8">

                                        <label class="docconlabels">Monthly Requirement:</label>

                                        <asp:TextBox runat="server" ID="mreqtxt" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>

                                <br />

                                <div class="row">
                                    <div class="col-8">

                                        <label class="docconlabels">Machine Capacity:</label>

                                        <asp:TextBox runat="server" ID="mcaptxt" Width="100%" ReadOnly="true" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>

                                <br />

                                <div class="row">
                                    <div class="col-8">

                                        <label class="docconlabels">Next Forecast:</label>

                                        <asp:TextBox runat="server" ID="tbNextForecast" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>

                            </fieldset>

                        </div>

                    </div>

                    <br />

                </div>

            </div>
            <div class="row">

                <div class="col-12">

                    <label class="docconlabels">Cause Analysis / Justification:</label>

                    <asp:TextBox runat="server" ID="tbjustification" ReadOnly="true" CssClass="form-control" Width="100%" TextMode="MultiLine" Height="150px" placeholder="Enter Text Here..."></asp:TextBox>

                </div>

            </div>

            <br />

            <div class="row">

                <div class="col-12">

                    <fieldset>
                        <legend><b>Supporting Details / Attachment</b>&nbsp;&nbsp;<small>Documents(docx ,doc ,ppt ,pptx ,xls ,xlxs); Pictures(png, jpeg ,jpg)</small></legend>

                        <%--<telerik:RadGrid RenderMode="Lightweight" ID="gridAttachment" GridLines="None" EnableLinqExpressions="false"
                            runat="server" AllowAutomaticDeletes="True" AllowAutomaticUpdates="true" AllowAutomaticInserts="true"
                            PageSize="10" PagerStyle-Mode="Advanced" AllowSorting="true"
                            AllowPaging="false" AutoGenerateColumns="False" DataSourceID="SQLDSGetAttachment" Skin="WebBlue">

                            <MasterTableView CommandItemDisplay="None" DataKeyNames="id" AllowFilteringByColumn="false"
                                DataSourceID="SQLDSGetAttachment" HorizontalAlign="NotSet" AutoGenerateColumns="False">

                                <Columns>

                                    <telerik:GridBoundColumn DataField="id" HeaderText="id" Display="false"
                                        AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                        AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                        SortExpression="id" CurrentFilterFunction="Contains" UniqueName="id" />

                                    <telerik:GridBoundColumn DataField="attachmentname" HeaderText="File Name"
                                        AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                        AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                        SortExpression="attachmentname" CurrentFilterFunction="Contains" UniqueName="attachmentname" />

                                    <telerik:GridBoundColumn DataField="contenttype" HeaderText="File Type"
                                        AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                        AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                        SortExpression="contenttype" CurrentFilterFunction="Contains" UniqueName="contenttype" />

                                    <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" UniqueName="DownloadColumn" ItemStyle-HorizontalAlign="Center" HeaderText="Download" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDownloadAttachment" runat="server" class="aligncenter" Text="Download" Style="color: blue;"
                                                CommandArgument='<%# Eval("id")%>' OnClientClick="return confirm('Do you want to download the file?')" OnClick="DownloadFile" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                </Columns>

                            </MasterTableView>

                            <ClientSettings AllowKeyboardNavigation="true"></ClientSettings>

                        </telerik:RadGrid>--%>

                        <div class="row">
                            <div class="col-lg-12">

                                <%--<telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="WebBlue" SelectedIndex="0">
                                    <Tabs>
                                        <telerik:RadTab Text="User" Width="50%"></telerik:RadTab>
                                        <telerik:RadTab Text="Incharge" Width="50%"></telerik:RadTab>
                                    </Tabs>
                                </telerik:RadTabStrip>
                                <telerik:RadMultiPage ID="RadMultiPage1" CssClass="RadMultiPage" runat="server" SelectedIndex="0">

                                    <telerik:RadPageView ID="RadPageView1" runat="server">--%>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gridAttachment" GridLines="None" EnableLinqExpressions="false"
                                    runat="server" AllowAutomaticDeletes="True" AllowAutomaticUpdates="true" AllowAutomaticInserts="true"
                                    PageSize="10" PagerStyle-Mode="Advanced" AllowSorting="true"
                                    AllowPaging="false" AutoGenerateColumns="False" DataSourceID="SQLDSGetAttachment" Skin="WebBlue">

                                    <MasterTableView CommandItemDisplay="None" DataKeyNames="id" AllowFilteringByColumn="false"
                                        DataSourceID="SQLDSGetAttachment" HorizontalAlign="NotSet" AutoGenerateColumns="False">

                                        <Columns>

                                            <telerik:GridBoundColumn DataField="id" HeaderText="id" Display="false"
                                                AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                                AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                                SortExpression="id" CurrentFilterFunction="Contains" UniqueName="id" />

                                            <telerik:GridBoundColumn DataField="attachmentname" HeaderText="File Name"
                                                AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                                AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                                SortExpression="attachmentname" CurrentFilterFunction="Contains" UniqueName="attachmentname" />

                                            <telerik:GridBoundColumn DataField="contenttype" HeaderText="File Type"
                                                AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                                AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                                SortExpression="contenttype" CurrentFilterFunction="Contains" UniqueName="contenttype" />

                                            <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" UniqueName="DownloadColumn" ItemStyle-HorizontalAlign="Center" HeaderText="Download" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownloadAttachment" runat="server" class="aligncenter" Text="Download" Style="color: blue;"
                                                        CommandArgument='<%# Eval("id")%>' OnClientClick="return confirm('Do you want to download the file?')" OnClick="DownloadFile" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                        </Columns>

                                    </MasterTableView>

                                    <ClientSettings AllowKeyboardNavigation="true"></ClientSettings>

                                </telerik:RadGrid>
                                <%--</telerik:RadPageView>

                                    <telerik:RadPageView ID="RadPageView2" runat="server">

                                        <telerik:RadGrid RenderMode="Lightweight" ID="gridAttachment2" GridLines="None" EnableLinqExpressions="false"
                                            runat="server" AllowAutomaticDeletes="True" AllowAutomaticUpdates="true" AllowAutomaticInserts="true"
                                            PageSize="10" PagerStyle-Mode="Advanced" AllowSorting="true"
                                            AllowPaging="false" AutoGenerateColumns="False" DataSourceID="SQLDSGetAttachment2" Skin="WebBlue">

                                            <MasterTableView CommandItemDisplay="None" DataKeyNames="id" AllowFilteringByColumn="false"
                                                DataSourceID="SQLDSGetAttachment2" HorizontalAlign="NotSet" AutoGenerateColumns="False">

                                                <Columns>

                                                    <telerik:GridBoundColumn DataField="id" HeaderText="id" Display="false"
                                                        AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                                        SortExpression="id" CurrentFilterFunction="Contains" UniqueName="id" />

                                                    <telerik:GridBoundColumn DataField="attachmentname" HeaderText="File Name"
                                                        AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                                        SortExpression="attachmentname" CurrentFilterFunction="Contains" UniqueName="attachmentname" />

                                                    <telerik:GridBoundColumn DataField="contenttype" HeaderText="File Type"
                                                        AllowFiltering="true" FilterControlWidth="100%" ShowFilterIcon="false"
                                                        AutoPostBackOnFilter="true" FilterCheckListEnableLoadOnDemand="true"
                                                        SortExpression="contenttype" CurrentFilterFunction="Contains" UniqueName="contenttype" />

                                                    <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" UniqueName="DownloadColumn" ItemStyle-HorizontalAlign="Center" HeaderText="Download" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDownloadAttachment" runat="server" class="aligncenter" Text="Download" Style="color: blue;"
                                                                CommandArgument='<%# Eval("id")%>' OnClientClick="return confirm('Do you want to download the file?')" OnClick="DownloadFile" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>

                                                </Columns>
                                            </MasterTableView>

                                            <ClientSettings AllowKeyboardNavigation="true"></ClientSettings>

                                        </telerik:RadGrid>
                                    </telerik:RadPageView>
                                </telerik:RadMultiPage>--%>
                            </div>
                        </div>

                    </fieldset>

                </div>

            </div>

            <br />

            <div class="row">
                <div class="col-4">
                    <label class="docconlabels">Assessment:</label>

                    <asp:DropDownList ID="ddlAssessment" runat="server" CssClass="form-control" AutoPostBack="true" Height="39px" DataTextField="Assessment" DataValueField="AssessmentCode" OnSelectedIndexChanged="ddlAssessment_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <div class="col-3">
                    <label class="docconlabels">Is Jig Exists:</label>

                    <asp:DropDownList ID="ddlCheckJig" runat="server" CssClass="form-control" AutoPostBack="true" Height="39px">
                        <asp:ListItem Text="Select" Value="" />
                        <asp:ListItem Text="Yes" Value="true" />
                        <asp:ListItem Text="No" Value="false" />
                    </asp:DropDownList>
                </div>                

            </div>

            <div class="row">
                <div class="col-10" runat="server" id="colAssessRemarks">
                    <label class="docconlabels">Remarks:</label>

                    <asp:TextBox ID="tbAssessmentRemarks" runat="server" ReadOnly="false" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-2">
                    <telerik:RadButton ID="btnSubmit" runat="server"
                        Text="Assess Job Order" SingleClick="true"
                        Primary="true" RenderMode="Lightweight" Enabled="true"
                        Style="width: 150px; height: 38px; margin-top: 30px"
                        SingleClickText="Processing..." OnClick="Approve">

                        <ConfirmSettings ConfirmText="By clicking Yes/OK will submit the request. Do you want to proceed?" UseRadConfirm="true" Title="Confirm Action" />
                    </telerik:RadButton>
                </div>
            </div>


        </div>

    </div>



    <asp:SqlDataSource ID="SQLDSGetAttachment" runat="server"
        SelectCommand="Select * from tblAttachment where RTRIM(LTRIM(JR_Code)) = @JR_Code and RTRIM(LTRIM(typeofattachment)) = 'User'" SelectCommandType="Text">

        <SelectParameters>
            <asp:SessionParameter SessionField="RCode" Name="JR_Code" ConvertEmptyStringToNull="true" />
        </SelectParameters>

    </asp:SqlDataSource>

    <%--<asp:SqlDataSource ID="SQLDSGetAttachment2" runat="server"
        SelectCommand="Select * from tblAttachment where RTRIM(LTRIM(JR_Code)) = @JR_Code and RTRIM(LTRIM(typeofattachment)) = 'Admin'" SelectCommandType="Text">

        <SelectParameters>
            <asp:SessionParameter SessionField="RCode" Name="JR_Code" ConvertEmptyStringToNull="true" />
        </SelectParameters>


    </asp:SqlDataSource>--%>

    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RejectRequest" runat="server" Title="Reason of Rejection"
                Width="500px" Height="300px" VisibleStatusbar="false" VisibleTitlebar="true" ReloadOnShow="true"
                ShowContentDuringLoad="false" Behaviors="Close" Modal="true" Skin="WebBlue" IconUrl="~/images/sohbiicon.ico">
            </telerik:RadWindow>
            <telerik:RadWindow RenderMode="Lightweight" ID="HoldRequest" runat="server" Title="Hold Remarks"
                Width="500px" Height="300px" VisibleStatusbar="false" VisibleTitlebar="true" ReloadOnShow="true"
                ShowContentDuringLoad="false" Behaviors="Close" Modal="true" Skin="WebBlue" IconUrl="~/images/sohbiicon.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <div class="card" runat="server" id="divSuccess" visible="false">

        <div class="card-body">

            <div class="row">

                <div class="col-12 aligncenter">

                    <img src="../images/thumb-up-smiley.png" style="width: 250px; height: 250px;" />

                    <br />

                    <h2><b>OK</b></h2>

                </div>

            </div>

        </div>

    </div>

    <div class="card" runat="server" id="divRejected" visible="false">

        <div class="card-body">

            <div class="row">

                <div class="col-12 aligncenter">

                    <img src="../images/rejected.png" style="width: 650px; height: 250px;" />

                    <br />

                    <br />

                    <h2><b>Request Has Been Rejected</b></h2>

                </div>

            </div>

        </div>

    </div>

    <div class="card" runat="server" id="divCancelled" visible="false">

        <div class="card-body">

            <div class="row">

                <div class="col-12 aligncenter">

                    <img src="../images/cancelled.png" style="width: 650px; height: 250px;" />

                    <br />

                    <br />

                    <h2><b>Request Has Been Cancelled</b></h2>

                </div>

            </div>

        </div>

    </div>


    <div class="card" runat="server" id="divAssess" visible="true">

        <div class="card-body">

            <div class="row">

                <div class="col-12 aligncenter">

                    <h2><b>You are not authorize to assess this request</b></h2>

                </div>

            </div>

        </div>

    </div>


</asp:Content>
