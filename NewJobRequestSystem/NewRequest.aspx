<%@ Page Title="New Request" Language="C#" MasterPageFile="~/JOMain.Master" AutoEventWireup="true" CodeBehind="NewRequest.aspx.cs" Inherits="NewJobRequestSystem.NewRequest"
    MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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

    <div class="col-md-12 col-xs-12">
        <div class="title centermessage">
            <h3><b>JOB ORDER APPLICATION FORM</b></h3>
        </div>
        <div class="separator"></div>
    </div>

    <div class="x_panel" runat="server" id="divApplication">
        <%--<div class="x_title">
            <h2 style="margin-top: 8px">Job Request Application</h2>

            <div class="clearfix"></div>
        </div>--%>
        <div class="x_content">

            <div class="row">

                <div class="col-lg-4">
                    <label class="docconlabels">Date Filed:</label>

                    <asp:TextBox ID="tbdateRequest" runat="server" ReadOnly="true" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                </div>

                <div class="col-lg-8">
                    
                    <fieldset>
                        <legend><b>In-Charge</b></legend>
                        <telerik:RadRadioButtonList runat="server" ID="rdoInCharge" DataBindings-DataTextField="incharge" DataBindings-DataValueField="incharge_id"
                            Font-Size="Large" AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="rdoInCharge_SelectedIndexChanged">
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
                            Font-Size="Large" AutoPostBack="true" Direction="Horizontal">
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

                                <asp:TextBox ID="tbItemName" runat="server" ReadOnly="false" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="SingleLine"></asp:TextBox>

                            </div>

                            <div class="col-lg-4">

                                <label class="docconlabels">Quantity:</label>

                                <asp:TextBox ID="tbItemQuantity" runat="server" ReadOnly="false" placeholder="Enter Text Here..." CssClass="form-control" Height="38px" TextMode="Number"></asp:TextBox>

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

                                        <%--<asp:TextBox runat="server" ID="tbCustomer" Width="100%" ReadOnly="false" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>--%>

                                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="cboCustomer"
                                            MarkFirstMatch="true" EnableLoadOnDemand="false" Width="100%" Height="400px"
                                            HighlightTemplatedItems="true" Skin="Bootstrap" Font-Size="14px" 
                                            DataTextField="Cust_Acronym"
                                            DataValueField="Cust_Acronym"
                                            OnClientItemsRequested="UpdateItemCountField"
                                            OnItemsRequested="cboCustomer_ItemsRequested"
                                            EmptyMessage="Select" AutoPostBack="true"
                                             OnSelectedIndexChanged="cboCustomer_SelectedIndexChanged">
                                            <HeaderTemplate>
                                                <ul>
                                                    <li class="col1">Code</li>
                                                    <li class="col2_2">Customer Name</li>
                                                </ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ul>
                                                    <li class="col1">
                                                        <%# DataBinder.Eval(Container.DataItem, "Cust_Acronym") %></li>
                                                    <li class="col2">
                                                        <%# DataBinder.Eval(Container.DataItem, "Cust_Name") %></li>

                                                </ul>
                                            </ItemTemplate>
                                            <%--<FooterTemplate>
                                                A total of
                                                <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                items
                                            </FooterTemplate>--%>
                                        </telerik:RadComboBox>

                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-12">

                                        <label class="docconlabels">Partcode:</label>

                                        <asp:TextBox runat="server" ID="tbPartCode" Width="100%" ReadOnly="false" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    
                                    <div class="col-lg-12">

                                        <label class="docconlabels">Partname:</label>

                                        <asp:TextBox runat="server" ID="tbPartName" Width="100%" ReadOnly="false" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>

                                <br />
                                <div class="row">
                                    <div class="col-lg-8">

                                        <label class="docconlabels">Type Of Jig:</label>

                                        <%--<asp:TextBox runat="server" ID="tbTypeOfJig" Width="100%" CssClass="form-control" placeholder="Enter Text Here..."></asp:TextBox>--%>

                                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="cboTypeOfJig"
                                            MarkFirstMatch="true" EnableLoadOnDemand="true" Width="100%" Height="400px"
                                            HighlightTemplatedItems="true" Skin="Bootstrap" Font-Size="14px" 
                                            DataTextField="JigType" OnItemsRequested="cboTypeOfJig_ItemsRequested"
                                            DataValueField="JigType"
                                            OnClientItemsRequested="UpdateItemCountField"
                                            EmptyMessage="Select" AutoPostBack="true"
                                            Filter="Contains" OnSelectedIndexChanged="cboTypeOfJig_SelectedIndexChanged">
                                            <%--<HeaderTemplate>
                                                <ul>
                                                    <li class="col1">Code</li>
                                                    <li class="col2_2">Customer Name</li>
                                                </ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <ul>
                                                    <li class="col1">
                                                        <%# DataBinder.Eval(Container.DataItem, "Cust_Acronym") %></li>
                                                    <li class="col2">
                                                        <%# DataBinder.Eval(Container.DataItem, "Cust_Name") %></li>

                                                </ul>
                                            </ItemTemplate>--%>
                                            <%--<FooterTemplate>
                                                A total of
                                                <asp:Literal runat="server" ID="RadComboItemsCount" />
                                                items
                                            </FooterTemplate>--%>
                                        </telerik:RadComboBox>

                                    </div>
                                    <div class="col-lg-4">

                                        <label class="docconlabels">Quantity:</label>

                                        <asp:TextBox runat="server" ID="tbQuantity_Jigs" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

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

                                        <asp:TextBox runat="server" ID="qtyhrtxt" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">

                                    <div class="col-lg-8">

                                        <label class="docconlabels">Monthly Requirement: <i style="color:red">*</i></label>

                                        <asp:TextBox runat="server" ID="mreqtxt" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>
                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-8">

                                        <label class="docconlabels">Machine Capacity: <i style="color:red">*</i></label>

                                        <asp:TextBox runat="server" ID="mcaptxt" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

                                    </div>

                                </div>
                                <br />
                                <div class="row">
                                    <div class="col-lg-8">

                                        <label class="docconlabels">Next Forecast: <i style="color:red">*</i></label>

                                        <asp:TextBox runat="server" ID="tbNextForecast" Width="100%" CssClass="form-control" TextMode="Number" placeholder="Enter Text Here..."></asp:TextBox>

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

                    <asp:TextBox runat="server" ID="tbjustification" CssClass="form-control" Width="100%" TextMode="MultiLine" Height="150px" placeholder="Enter Text Here..."></asp:TextBox>

                </div>

            </div>


            <br />


            <div class="row">

                <div class="col-lg-12">

                    <fieldset>
                        <legend><b>Supporting Details / Attachment</b>&nbsp;&nbsp;<small>Documents(docx ,doc ,ppt ,pptx ,xls ,xlxs); Pictures(png, jpeg ,jpg)</small></legend>

                        <div class="row">

                            <div class="col-lg-12">

                                <table style="width: 100%; border: none">
                                    <tr>
                                        <td style="width: 100px"><b>Select File:</b></td>
                                        <td style="width: 200px">
                                            <telerik:RadAsyncUpload ID="uploadFile" Skin="WebBlue"
                                                Style="padding-top: 8px" MultipleFileSelection="Disabled" runat="server" Width="100%">
                                            </telerik:RadAsyncUpload>
                                        </td>
                                        <td style="width: 100px">
                                            <asp:LinkButton ID="LinkButton1" runat="server" Style="font-size: 10px;"
                                                class="btn btn-primary btn-flat" OnClientClick="return confirm('Do you want to proceed?')" OnClick="UploadMultipleFileExisting">
                                                <i class="glyphicon glyphicon-upload"></i> Upload</asp:LinkButton></td>
                                        <td></td>
                                    </tr>
                                </table>

                            </div>

                        </div>

                        <br />

                        <telerik:RadGrid RenderMode="Lightweight" ID="gridAttachment" GridLines="None" EnableLinqExpressions="false"
                            runat="server" AllowAutomaticDeletes="True" AllowAutomaticUpdates="true" AllowAutomaticInserts="true"
                            PageSize="10" PagerStyle-Mode="Advanced" AllowSorting="true" OnItemDeleted="RadGrid1_ItemDeleted"
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

                                    <%--<telerik:GridButtonColumn ConfirmText="Delete File?" ConfirmDialogType="RadWindow"
                                        ConfirmTitle="Delete" HeaderText="Delete" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center"
                                        CommandName="Delete" Text="Delete" UniqueName="DeleteColumn" ItemStyle-HorizontalAlign="Center">
                                    </telerik:GridButtonColumn>--%>

                                    <telerik:GridTemplateColumn HeaderStyle-Width="100px" ItemStyle-Width="100px" UniqueName="DeleteColumn" ItemStyle-HorizontalAlign="Center" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDelete" runat="server" class="aligncenter" Text="Delete" Style="color: blue;"
                                                CommandArgument='<%# Eval("id")%>' OnClientClick="return confirm('Do you want to delete the file?')" OnClick="lnkDelete_Click" />
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

                <div class="col-lg-6" runat="server" visible="false">

                    <%--<telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="RadcboApprover"
                        MarkFirstMatch="true" EnableLoadOnDemand="true" Width="100%" Height="400px"
                        HighlightTemplatedItems="true" Skin="Bootstrap" Font-Size="17px" DataTextField="FullName_LnameFirst"
                        OnDataBound="RadcboApprover_DataBound" DataSourceID="SQLDSGetAllApprovers" DataValueField="EmpID"
                        OnClientItemsRequested="UpdateItemCountField"
                        AutoPostBack="true" EmptyMessage="Select Approver"
                        Filter="Contains">
                        <HeaderTemplate>
                            <ul>
                                <li class="col1">Employee ID</li>
                                <li class="col2_2">Employee Name</li>
                            </ul>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <ul>
                                <li class="col1">
                                    <%# DataBinder.Eval(Container.DataItem, "EmpID") %></li>
                                <li class="col2">
                                    <%# DataBinder.Eval(Container.DataItem, "FullName_LnameFirst") %></li>

                            </ul>
                        </ItemTemplate>
                        <FooterTemplate>
                            A total of
                            <asp:Literal runat="server" ID="RadComboItemsCount" />
                            items
                        </FooterTemplate>
                    </telerik:RadComboBox>--%>
                </div>

                <div class="col-lg-12 aligncenter">

                    <telerik:RadButton ID="btnSubmit" runat="server"
                        Text="Submit Request" SingleClick="true"
                        Primary="true" RenderMode="Lightweight" Enabled="true"
                        Style="width: 150px; height: 38px;"
                        SingleClickText="Sending Request..." OnClick="btnSubmit_Click">

                        <ConfirmSettings ConfirmText="By clicking Yes/OK will submit the request. Do you want to proceed?" Title="Confirm Action" />
                    </telerik:RadButton>

                </div>

            </div>
            <%--DeleteCommand="delete from tblAttachment where id=@id)" DeleteCommandType="Text"--%>
            <asp:SqlDataSource ID="SQLDSGetAttachment" runat="server"
                SelectCommand="Select * from tblAttachment where RTRIM(LTRIM(JR_Code)) = @JR_Code" SelectCommandType="Text">

                <SelectParameters>
                    <asp:SessionParameter SessionField="RCode" Name="JR_Code" ConvertEmptyStringToNull="true" />
                </SelectParameters>
                <%--<DeleteParameters>
                    <asp:Parameter Name="id" Type="Int32"></asp:Parameter>
                </DeleteParameters>--%>

            </asp:SqlDataSource>

        </div>
    </div>

    <div class="card" runat="server" id="divSuccess" visible="false">

        <div class="card-body">

            <div class="row">

                <div class="col-12 aligncenter">

                    <img src="../images/thumb-up-smiley.png" style="width: 250px; height: 250px;" />

                    <br />

                    <h2><b>Request Successfully Sent</b></h2>

                </div>

            </div>

        </div>

        <div class="box-footer">

            <div class="row">

                <div class="col-lg-5"></div>

                <div class="col-lg-2">
                    <telerik:RadButton ID="btnGoToMain" runat="server"
                        Text="Go To Main" SingleClick="true"
                        Width="250px" Height="40px" Primary="true" RenderMode="Lightweight"
                        SingleClickText="Loading..." OnClick="btnGoToMain_Click">
                    </telerik:RadButton>

                </div>

                <div class="col-lg-5"></div>

            </div>

            <br />

            <br />

        </div>

    </div>

</asp:Content>
