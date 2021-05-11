<%@ Page Title="Home" Language="C#" MasterPageFile="~/JOMain.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="NewJobRequestSystem.HomePage" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="row">

        <div class="col-md-12 col-xs-12">

            <div class="title centermessage">
                <h3><b>JOB ORDER RECORDS</b></h3>
            </div>

            <div class="separator"></div>

        </div>

    </div>

    <div class="x_panel" runat="server" id="divRecords">

        <div class="x_content">

            <div class="row">
                <div class="col-lg-8">
                    <table style="width: 100%; border: none">
                        <tr>
                            <td style="width: 100px;">
                                <label runat="server">Select Year:</label>
                            </td>
                            <td style="width: 200px">
                                <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlSummaryYear"
                                    Width="150px" Font-Size="18px" Skin="WebBlue" AutoPostBack="true">
                                </telerik:RadDropDownList>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <div class="col-lg-4" style="text-align: right">
                    
                </div>

            </div>

            <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="WebBlue" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="My Requests" Width="50%"></telerik:RadTab>
                    <telerik:RadTab Text="Department Requests" Width="50%"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage1" CssClass="RadMultiPage" runat="server" SelectedIndex="0">

                <telerik:RadPageView ID="RadPageView1" runat="server">
                    <div class="row" style="margin-top: 15px" runat="server">

                        <div class="col-lg-12">

                            <fieldset style="border: none; margin-top: 5px">
                                <legend>Legend</legend>
                                <table style="border: none; margin-top: -15px">
                                    <tr>

                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #44bb19;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>Completed</h5>
                                        </td>


                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #da3e14;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>Denied / Cancelled</h5>
                                        </td>


                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #e68b33;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Checking</h5>
                                        </td>


                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #33E0FF;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Assessment</h5>
                                        </td>


                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #FFE000;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Noting</h5>
                                        </td>

                                    </tr>
                                    <tr>

                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #D305F7;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Approval</h5>
                                        </td>

                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #778899;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For PDE/DE Acceptance</h5>
                                        </td>

                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #b35900;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>Attachment Approval</h5>
                                        </td>

                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #008B8B;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>On-Going</h5>
                                        </td>

                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #05F7CF;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Employee Acceptance</h5>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>

                        </div>
                    </div>

                    <telerik:RadGrid RenderMode="Lightweight" ID="gridMyRecords" runat="server" CssClass="RadGrid" GridLines="None"
                        AllowPaging="True" PageSize="20" AllowSorting="True" AutoGenerateColumns="False" EnableLinqExpressions="false"
                        ShowStatusBar="true" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" Skin="WebBlue"
                        AllowAutomaticUpdates="True" DataSourceID="getRecords" OnItemDataBound="gridMyRecords_ItemDataBound">
                        <PagerStyle Mode="NextPrev" Position="Bottom" PageSizeControlType="RadComboBox"></PagerStyle>
                        <MasterTableView DataSourceID="getRecords" DataKeyNames="Jr_Code" AllowFilteringByColumn="true">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="JONo" HeaderText="Job Order No" DataField="JRF_No" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestedby" HeaderText="Requested by" DataField="FullName_FnameFirst" 
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestdate" HeaderText="Date Request" DataField="NewDate" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Dept_Desc" HeaderText="Department" DataField="Dept_Desc"
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Sect_Desc" HeaderText="Section" DataField="Sect_Desc"
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Problem_Desc" HeaderText="Justification" DataField="Problem_Desc"
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" AllowFiltering="false"
                                    ItemStyle-CssClass="ItemsCenter" Display="false">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="txtStatus" Text='<%# Eval("JOStatus")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn UniqueName="action" FilterControlWidth="100%" AutoPostBackOnFilter="true" AllowFiltering="false">
                                    <HeaderStyle Width="6%" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkviewdetails" runat="server"
                                            CommandArgument='<%#Eval("Jr_Code")%>' OnClick="lnkviewdetails_Click">
                                    <img src="images/view.png" alt="view" style="height: 25px;"/></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>

                        </MasterTableView>

                    </telerik:RadGrid>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <div class="row" style="margin-top: 15px" runat="server">

                        <div class="col-lg-12">

                            <fieldset style="border: none; margin-top: 5px">
                                <legend>Legend</legend>
                                <table style="border: none; margin-top: -15px">
                                    <tr>

                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #44bb19;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>Completed</h5>
                                        </td>


                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #da3e14;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>Denied / Cancelled</h5>
                                        </td>


                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #e68b33;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Checking</h5>
                                        </td>


                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #33E0FF;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Assessment</h5>
                                        </td>


                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #FFE000;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Noting</h5>
                                        </td>

                                    </tr>
                                    <tr>

                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #D305F7;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Approval</h5>
                                        </td>

                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #778899;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For PDE/DE Acceptance</h5>
                                        </td>

                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #b35900;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>Attachment Approval</h5>
                                        </td>

                                        <td style="padding-left: 10px">
                                            <div style="width: 30px; height: 30px; background-color: #008B8B;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>On-Going</h5>
                                        </td>

                                        <td>
                                            <div style="width: 30px; height: 30px; background-color: #05F7CF;">
                                            </div>
                                        </td>
                                        <td style="padding-left: 10px; width: 150px">
                                            <h5>For Employee Acceptance</h5>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>

                        </div>
                    </div>

                    <telerik:RadGrid RenderMode="Lightweight" ID="gridDepartmentRecord" runat="server" CssClass="RadGrid" GridLines="None"
                        AllowPaging="True" PageSize="20" AllowSorting="True" AutoGenerateColumns="False" EnableLinqExpressions="false"
                        ShowStatusBar="true" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" Skin="WebBlue"
                        AllowAutomaticUpdates="True" DataSourceID="getDepartmentRecords" OnItemDataBound="gridDepartmentRecord_ItemDataBound">
                        <PagerStyle Mode="NextPrev" Position="Bottom" PageSizeControlType="RadComboBox"></PagerStyle>
                        <MasterTableView DataSourceID="getDepartmentRecords" DataKeyNames="Jr_Code" AllowFilteringByColumn="true">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="JONo" HeaderText="Job Order No" DataField="JRF_No" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestedby" HeaderText="Requested by" DataField="FullName_FnameFirst" 
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestdate" HeaderText="Date Request" DataField="NewDate" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Dept_Desc" HeaderText="Department" DataField="Dept_Desc"
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Sect_Desc" HeaderText="Section" DataField="Sect_Desc"
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Problem_Desc" HeaderText="Justification" DataField="Problem_Desc" 
                                    FilterControlWidth="100%" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" ShowFilterIcon="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridTemplateColumn FilterCheckListEnableLoadOnDemand="true" AllowFiltering="false"
                                    ItemStyle-CssClass="ItemsCenter" Display="true">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="txtStatus" Text='<%# Eval("JOStatus")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn UniqueName="action" FilterControlWidth="100%" AutoPostBackOnFilter="true">
                                    <HeaderStyle Width="6%" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkviewdetails" runat="server"
                                            CommandArgument='<%#Eval("Jr_Code")%>' OnClick="lnkviewdetails_Click">
                                    <img src="images/view.png" alt="view" style="height: 25px;"/></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>

                        </MasterTableView>

                    </telerik:RadGrid>
                </telerik:RadPageView>
            </telerik:RadMultiPage>

        </div>
    </div>

    <asp:SqlDataSource ID="getRecords" runat="server"
        SelectCommand="        
                
                IF OBJECT_ID('TEMPDB.dbo.#tblCountAttachment') IS NOT NULL DROP TABLE #tblCountAttachment
        
                CREATE TABLE #tblCountAttachment
	            
                (JR_Code nvarchar(350) ,Counters int)
	            
                INSERT INTO #tblCountAttachment
	
	            SELECT JR_Code , Count(*) Counters from tblAttachment where RTRIM(LTRIM(typeofattachment)) = 'Admin' group by jr_code

	            Select 
                A.*,
        
                FORMAT (A.Request_Date, 'dd/MMM/yyyy') NewDate,
        
                b.Incharge,
        
                d.DeptName as Dept_Desc, 
	    
                g.SectName as Sect_Desc
                ,(RTRIM(c.FirstName) + ' ' + RTRIM(c.LastName))
	            As FullName_FnameFirst,
		        
                e.Assessment,        
        
                Case when H.Counters is null then 0 else 1 end Counters,

                case when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 0 and
						  A.Is_Assessed = 0 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Checking' 
					
					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 0 and
						  A.Is_Noted = 0 and
						  --A.Is_AttachmentApproved = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Assessment'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Noting'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 0 and
						  --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Approval'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
                          --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'Pending'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 0
					then 'On-Going'


                    when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and 
                          --A.Is_AttachmentApproved = 0 and
						  A.IsCompleted = 0
					then 'Attachment Approval'
        
					when A.IsRejected = 1 or A.IsCancelled = 1
					then 'Denied_Cancelled'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
						  --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy = 'N/A' or A.FinalAcceptanceBy is null)
					then 'For Employee Acceptance'					

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
						  --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy != 'N/A' or A.FinalAcceptanceBy is not null)
					then 'Completed'
					
					end JOStatus
        
                from tblRequest A

		        left join tblIncharge B on A.incharge_id = b.incharge_id

                left join tblAssessment E on a.AssessmentCode = e.AssessmentCode

		        inner join HRIS.dbo.t_EmpMaster C on A.EmpID collate SQL_Latin1_General_CP1_CI_AS = C.EmpID collate SQL_Latin1_General_CP1_CI_AS
        
                inner JOIN HRIS.dbo.t_Department d ON d.DeptCode = c.Department
	
	            inner JOIN HRIS.dbo.t_DepartmentalSection g ON c.Section = g.SectCode and g.DeptCode = c.Department
	
                LEFT JOIN #tblCountAttachment H on A.JR_Code collate SQL_Latin1_General_CP1_CI_AS = H.JR_Code collate SQL_Latin1_General_CP1_CI_AS
	
                where A.EmpID = @EmpID  and Year(A.Request_Date) = @Year       
        
                order by case when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy != 'N/A' or A.FinalAcceptanceBy is not null)
					then 1 else 2 end"
        SelectCommandType="Text">
        <SelectParameters>
            <asp:SessionParameter Name="EmpID" SessionField="EmpID" Type="String" />
            <asp:ControlParameter Name="Year" PropertyName="SelectedValue" ControlID="ddlSummaryYear"></asp:ControlParameter>
        </SelectParameters>

    </asp:SqlDataSource>

    <asp:SqlDataSource ID="getDepartmentRecords" runat="server"
        SelectCommand="        
                
                IF OBJECT_ID('TEMPDB.dbo.#tblCountAttachment') IS NOT NULL DROP TABLE #tblCountAttachment
        
                CREATE TABLE #tblCountAttachment
	            
                (JR_Code nvarchar(350) ,Counters int)
	            
                INSERT INTO #tblCountAttachment
	
	            SELECT JR_Code , Count(*) Counters from tblAttachment where RTRIM(LTRIM(typeofattachment)) = 'Admin' group by jr_code

                IF 
                
                (@Code = 'P' AND @SectCode = '08')  OR (@Code = 'P' AND @SectCode = '17') OR

                (@Code = 'V' AND @SectCode = '00') OR (@Code = 'V' AND @SectCode = '01') OR

                (@Code = 'V' AND @SectCode = '02') OR (@Code = 'S' AND @SectCode = '02') OR

                (@Code = 'S' AND @SectCode = '01')

                BEGIN 

	                Select 
                    A.*,
        
                    FORMAT (A.Request_Date, 'dd/MMM/yyyy') NewDate,
        
                    b.Incharge,
        
                    d.DeptName as Dept_Desc, 
	    
                    g.SectName as Sect_Desc
                    ,(RTRIM(c.FirstName) + ' ' + RTRIM(c.LastName))
	                As FullName_FnameFirst,
		        
                    e.Assessment,        
        
                    Case when H.Counters is null then 0 else 1 end Counters,
                    
                    case when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 0 and
						  A.Is_Assessed = 0 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Checking' 
					
					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 0 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Assessment'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Noting'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Approval'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
                          --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'Pending'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 0
					then 'On-Going'

                    when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and 
                          --A.Is_AttachmentApproved = 0 and
						  A.IsCompleted = 0
					then 'Attachment Approval'


					when A.IsRejected = 1 or A.IsCancelled = 1
					then 'Denied_Cancelled'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy = 'N/A' or A.FinalAcceptanceBy is null)
					then 'For Employee Acceptance'					

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy != 'N/A' or A.FinalAcceptanceBy is not null)
					then 'Completed'
					
					end JOStatus
        
                    from tblRequest A

		            left join tblIncharge B on A.incharge_id = b.incharge_id

                    left join tblAssessment E on a.AssessmentCode = e.AssessmentCode

		            inner join HRIS.dbo.t_EmpMaster C on A.EmpID collate SQL_Latin1_General_CP1_CI_AS = C.EmpID collate SQL_Latin1_General_CP1_CI_AS
        
                    inner JOIN HRIS.dbo.t_Department d ON d.DeptCode = c.Department
	
	                inner JOIN HRIS.dbo.t_DepartmentalSection g ON c.Section = g.SectCode and g.DeptCode = c.Department
	
                    LEFT JOIN #tblCountAttachment H on A.JR_Code collate SQL_Latin1_General_CP1_CI_AS = H.JR_Code collate SQL_Latin1_General_CP1_CI_AS

				    WHERE a.DeptCode = @Code and a.SectCode = @SectCode and A.EmpID !=@EmpID and Year(A.Request_Date) = @Year
                    
                    order by case when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy != 'N/A' or A.FinalAcceptanceBy is not null)
					then 1 else 2 end
              END
              ELSE
              BEGIN

	            Select 
                A.*,
        
                FORMAT (A.Request_Date, 'dd/MMM/yyyy') NewDate,
        
                b.Incharge,
        
                d.DeptName as Dept_Desc, 
	    
                g.SectName as Sect_Desc
                ,(RTRIM(c.FirstName) + ' ' + RTRIM(c.LastName))
	            As FullName_FnameFirst,
		        
                e.Assessment,        
        
                Case when H.Counters is null then 0 else 1 end Counters,

                case when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 0 and
						  A.Is_Assessed = 0 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
                          --A.Is_AttachmentApproved = 0 and
						  A.IsCompleted = 0
					then 'For Checking' 
					
					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 0 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
                          --A.Is_AttachmentApproved = 0 and
						  A.IsCompleted = 0
					then 'For Assessment'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 0 and
						  A.Is_Approved = 0 and
                          --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Noting'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
                          --A.Is_AttachmentApproved = 0 and
						  A.Is_Approved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'For Approval'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
                          --A.Is_AttachmentApproved = 0 and
						  A.Is_JobAccepted = 0 and
						  A.IsCompleted = 0
					then 'Pending'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 0
					then 'On-Going'


                    when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and 
                          --A.Is_AttachmentApproved = 0 and
						  A.IsCompleted = 0
					then 'Attachment Approval'

					when A.IsRejected = 1 or A.IsCancelled = 1
					then 'Denied_Cancelled'

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.Is_JobAccepted = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy = 'N/A' or A.FinalAcceptanceBy is null)
					then 'For Employee Acceptance'					

					when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy != 'N/A' or A.FinalAcceptanceBy is not null)
					then 'Completed'
					
					end JOStatus
        
                from tblRequest A

		        left join tblIncharge B on A.incharge_id = b.incharge_id

                left join tblAssessment E on a.AssessmentCode = e.AssessmentCode

		        inner join HRIS.dbo.t_EmpMaster C on A.EmpID collate SQL_Latin1_General_CP1_CI_AS = C.EmpID collate SQL_Latin1_General_CP1_CI_AS
        
                inner JOIN HRIS.dbo.t_Department d ON d.DeptCode = c.Department
	
	            inner JOIN HRIS.dbo.t_DepartmentalSection g ON c.Section = g.SectCode and g.DeptCode = c.Department
	
                LEFT JOIN #tblCountAttachment H on A.JR_Code collate SQL_Latin1_General_CP1_CI_AS = H.JR_Code collate SQL_Latin1_General_CP1_CI_AS

				WHERE a.DeptCode = @Code and A.EmpID !=@EmpID and Year(A.Request_Date) = @Year

                order by case when A.IsRejected = 0 and A.IsCancelled = 0 and 
						  A.Is_Submitted = 1 and
						  A.Is_Checked = 1 and
						  A.Is_Assessed = 1 and
						  A.Is_Noted = 1 and
						  A.Is_Approved = 1 and
						  A.Is_JobAccepted = 1 and
                          --A.Is_AttachmentApproved = 1 and
						  A.IsCompleted = 1 and (A.FinalAcceptanceBy != 'N/A' or A.FinalAcceptanceBy is not null)
					then 1 else 2 end
            END"
        SelectCommandType="Text">
        <SelectParameters>
            <asp:SessionParameter Name="Code" SessionField="DeptCode" Type="String" />
            <asp:SessionParameter Name="SectCode" SessionField="SectCode" Type="String" />
            <asp:SessionParameter Name="EmpID" SessionField="EmpID" Type="String" />
            <asp:ControlParameter Name="Year" PropertyName="SelectedValue" ControlID="ddlSummaryYear"></asp:ControlParameter>
        </SelectParameters>

    </asp:SqlDataSource>


</asp:Content>
