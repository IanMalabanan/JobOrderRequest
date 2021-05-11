<%@ Page Title="Assessment" Language="C#" MasterPageFile="~/JOMain.Master" AutoEventWireup="true" CodeBehind="UserForAssessment.aspx.cs" Inherits="NewJobRequestSystem.UserForAssessment" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .newfont {
            font-variant: small-caps;
            font-weight: bold;
            letter-spacing: 2.1px;
        }


        /** Columns */
        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul,
        .rcbHovered ul,
        .rcbDisabled ul {
            margin: 0;
            padding: 0;
            width: 100%;
            display: inline-block;
            list-style-type: none;
        }

        .exampleRadComboBox.RadComboBoxDropDown .rcbHeader {
            padding: 5px 27px 4px 7px;
        }

        .rcbScroll {
            overflow: scroll !important;
            overflow-x: hidden !important;
        }

        .col2 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 70%;
            line-height: 14px;
            float: left;
        }

        .col2_2 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 35%;
            line-height: 14px;
            float: left;
        }

        .col3 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 35%;
            line-height: 14px;
            float: left;
        }

        .col1 {
            margin: 0;
            padding: 0 5px 0 0;
            width: 30%;
            line-height: 14px;
            float: left;
        }
    </style>

    <telerik:RadScriptBlock runat="server">
        <script type="text/javascript">
            function UpdateItemCountField(sender, args) {
                //Set the footer text.
                sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
            }
        </script>
    </telerik:RadScriptBlock>

    <script>
        function OnClientLoadHandler(sender) {
            sender.get_inputDomElement().readOnly = "readonly";
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadNotification RenderMode="Lightweight" ID="RadNotification1" runat="server" Position="Center"
        Animation="Fade" EnableRoundedCorners="true" EnableShadow="true" Style="z-index: 100000" AutoCloseDelay="3000">
    </telerik:RadNotification>

    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow RenderMode="Lightweight" ID="RejectRequest" runat="server" Title="Reason of Rejection"
                Width="500px" Height="300px" VisibleStatusbar="false" VisibleTitlebar="true" ReloadOnShow="true"
                ShowContentDuringLoad="false" Behaviors="Close" Modal="true" Skin="WebBlue" IconUrl="~/images/sohbiicon.ico">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <div class="x_panel" runat="server" id="divDetails">
        <div class="x_title">
            <h2 style="margin-top: 8px">Records For Assessment - Details</h2>
            <ul class="nav navbar-right panel_toolbox">
                <li>
                    <a class="btn" style="margin-left: 80px; color: black" runat="server" onserverclick="ClosePage">
                        <i class="fa fa-close"></i>
                    </a>
                </li>
            </ul>
            <div class="clearfix"></div>
        </div>
        <div class="x_content">

            <div class="row" runat="server" id="rowHold" visible="false">

                <div class="col-lg-12">

                    <div class="alert alert-warning" role="alert">

                        <asp:Label runat="server" ID="lblHoldRemarks" Font-Size="Medium"></asp:Label>
                    </div>

                    <br />

                </div>

            </div>

            <div class="row">

                <div class="col-lg-4">
                    <label class="docconlabels">Date Filed:</label>

                    <asp:TextBox ID="tbdateRequest" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>


                <div class="col-lg-8">
                    <fieldset>
                        <legend><b>In-Charge</b></legend>
                        <telerik:RadRadioButtonList runat="server" ID="rdoInCharge" DataBindings-DataTextField="incharge" DataBindings-DataValueField="incharge_id"
                            Font-Size="Large" AutoPostBack="true" Direction="Horizontal" Enabled="false">
                        </telerik:RadRadioButtonList>
                    </fieldset>
                </div>

            </div>

            <br />

            <div class="row">

                <div class="col-lg-4">

                    <label class="docconlabels">Name of Requestor:</label>

                    <asp:TextBox ID="tbReqName" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-lg-4">

                    <label class="docconlabels">Department:</label>

                    <asp:TextBox ID="tbDepartment" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-lg-4">

                    <label class="docconlabels">Section:</label>

                    <asp:TextBox ID="tbSection" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

            </div>

            <br />
            <div class="row">
                <div class="col-lg-12">
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

                <div class="col-lg-12">

                    <div class="row">

                        <div class="col-lg-6">
                            <fieldset>
                                <legend><b>Process Jigs / Inspection Jigs</b></legend>

                                <div class="row">

                                    <div class="col-lg-12">

                                        <label class="docconlabels">Customer:</label>

                                        <asp:TextBox runat="server" ID="tbCustomer" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">

                                        <label class="docconlabels">Partcode:</label>

                                        <asp:TextBox runat="server" ID="tbPartCode" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">

                                        <label class="docconlabels">Partname:</label>

                                        <asp:TextBox runat="server" ID="tbPartName" Width="100%" ReadOnly="true" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>

                                <br />
                                <div class="row">
                                    <div class="col-lg-8">

                                        <label class="docconlabels">Type Of Jig:</label>

                                        <asp:TextBox runat="server" ID="tbTypeOfJig" ReadOnly="true" Width="100%" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                    <div class="col-lg-4">

                                        <label class="docconlabels">Quantity:</label>

                                        <asp:TextBox runat="server" ID="tbQuantity_Jigs" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>

                            </fieldset>
                        </div>
                        <div class="col-lg-6">
                            <fieldset>
                                <legend><b>For Jigs Request</b></legend>

                                <div class="row">
                                    <div class="col-lg-8">

                                        <label class="docconlabels">Quantity produce per hour:</label>

                                        <asp:TextBox runat="server" ID="qtyhrtxt" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">

                                    <div class="col-lg-8">

                                        <label class="docconlabels">Monthly Requirement:</label>

                                        <asp:TextBox runat="server" ID="mreqtxt" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-8">

                                        <label class="docconlabels">Machine Capacity:</label>

                                        <asp:TextBox runat="server" ID="mcaptxt" ReadOnly="true" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-8">

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

                <div class="col-lg-12">

                    <label class="docconlabels">Cause Analysis / Justification:</label>

                    <asp:TextBox runat="server" ID="tbjustification" ReadOnly="true" CssClass="form-control" Width="100%" TextMode="MultiLine" Height="150px" placeholder="Enter Text Here..."></asp:TextBox>

                </div>

            </div>

            <br />

            <div class="row">

                <div class="col-lg-12">

                    <fieldset>
                        <legend><b>Supporting Details / Attachment</b>&nbsp;&nbsp;<small>Documents(docx ,doc ,ppt ,pptx ,xls ,xlxs); Pictures(png, jpeg ,jpg)</small></legend>

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
                    </fieldset>

                </div>
            </div>

            <br />

            <div class="row">
                <div class="col-lg-4">
                    <label class="docconlabels">Assessment:</label>

                    <asp:TextBox ID="tbAssessment" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>
                <div class="col-lg-6" runat="server" visible="false" id="colAssessRemarks">
                    <label class="docconlabels">Remarks:</label>

                    <asp:TextBox ID="tbAssessmentRemarks" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

            </div>

            <div class="col-container">
                <div class="row">
                    <div class="separator"></div>
                </div>

                <div class="row" style="padding: 5px;">

                    <div class="col-lg-12">

                        <table style="border:none;width: 100%">
                            <tr>
                                <td colspan="2" style="border:1px solid black; background-color: whitesmoke; text-align: center;">To be Signed by Requesting Department</td>
                                <td colspan="2" style="border:1px solid black; background-color: whitesmoke; text-align: center;">To be Signed by PDE / DE</td>
                                <td rowspan="2" style="border:1px solid black; text-align: center;">Approved By:</td>
                                <td rowspan="2" style="border:1px solid black;  text-align: center;">Received By:</td>
                            </tr>
                            <tr>
                                <td style="border:1px solid black; text-align: center; width: 16%">Requested By:
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">Checked By:
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">Assessed By:
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">Noted By:
                                </td>
                                <%--<td style="border: none; text-align: center; width: 16%">Approved By:</td>
                                <td style="border: none; text-align: center; width: 16%">Received By:</td>--%>
                            </tr>
                            <tr>
                                <td style="border:1px solid black;text-align: center; width: 16%">
                                    <label style="color: #337ab7" runat="server" id="lblDateSigned_Preparer"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="lblPreparerRemarks"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="Preparer"></label>
                                    <p><i>(Department Requestor)</i></p>
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">
                                    <label style="color: #337ab7" runat="server" id="lblDateSigned_Checker"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="lblCheckerRemarks"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="Checker"></label>
                                    <p><i>(Dept. Supervisor)</i></p>
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">
                                    <label style="color: #337ab7" runat="server" id="lblDateSigned_Assesstor"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="lblAssesstorRemarks"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="Assesstor"></label>
                                    <p><i>(PDE / DE Leader)</i></p>
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">
                                    <label style="color: #337ab7" runat="server" id="lblDateSigned_Noter"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="lblNoterRemarks"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="Noter"></label>
                                    <p><i>(Dept. Supervisor)</i></p>
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">
                                    <label style="color: #337ab7" runat="server" id="lblDateSigned_Approver"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="lblApproverRemarks"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="Approver"></label>
                                    <p><i>(Manager)</i></p>
                                </td>
                                <td style="border:1px solid black; text-align: center; width: 16%">
                                    <label style="color: #337ab7" runat="server" id="lblDateSigned_Receiver"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="lblReceiverRemarks"></label>
                                    <br />
                                    <label style="color: #337ab7" runat="server" id="Receiver"></label>
                                    <p><i>(Requestor)</i></p>
                                </td>
                            </tr>
                            <tr style="border:none;">
                                <td colspan="6" style="border-color:transparent;">
                                    <label style="color: #337ab7; font-style: italic" runat="server" id="Label1">
                                        <b>Note:</b> <i style="font-weight: 100">Request should be from Leader /GL/Head/SV of the department .For D.E Inspection Jig must be assessed by PDE first to check the Existence of JIG before proceed to D.E Signatories. For Requestor Don’t by pass the Concerned Signatories.</i>
                                    </label>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>

            <asp:SqlDataSource ID="SQLDSGetAttachment" runat="server"
               SelectCommand="Select * from tblAttachment where RTRIM(LTRIM(JR_Code)) = @JR_Code and RTRIM(LTRIM(typeofattachment)) = 'User'" SelectCommandType="Text">

                <SelectParameters>
                    <%--<asp:SessionParameter SessionField="RCode" Name="JR_Code" ConvertEmptyStringToNull="true" />--%>
                    <asp:QueryStringParameter Name="JR_Code" QueryStringField="RCode" ConvertEmptyStringToNull="true" />
                </SelectParameters>

            </asp:SqlDataSource>

        </div>
    </div>

</asp:Content>
