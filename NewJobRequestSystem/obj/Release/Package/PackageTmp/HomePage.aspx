<%@ Page Title="" Language="C#" MasterPageFile="~/JOMain.Master" AutoEventWireup="true" CodeBehind="HomePage.aspx.cs" Inherits="NewJobRequestSystem.HomePage" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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

            <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="WebBlue" SelectedIndex="0">
                <Tabs>
                    <telerik:RadTab Text="My Requests" Width="50%"></telerik:RadTab>
                    <telerik:RadTab Text="Department Requests" Width="50%"></telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage1" CssClass="RadMultiPage" runat="server" SelectedIndex="0">

                <telerik:RadPageView ID="RadPageView1" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gridMyRecords" runat="server" CssClass="RadGrid" GridLines="None"
                        AllowPaging="True" PageSize="20" AllowSorting="True" AutoGenerateColumns="False"
                        ShowStatusBar="true" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" Skin="WebBlue"
                        AllowAutomaticUpdates="True" DataSourceID="getRecords">
                        <PagerStyle Mode="NextPrev" Position="Bottom" PageSizeControlType="RadComboBox"></PagerStyle>
                        <MasterTableView DataSourceID="getRecords" DataKeyNames="Jr_Code">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="JONo" HeaderText="Job Order No" DataField="JRF_No" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestedby" HeaderText="Requested by" DataField="FullName_FnameFirst" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestdate" HeaderText="Date Request" DataField="NewDate" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Dept_Desc" HeaderText="Department" DataField="Dept_Desc">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Sect_Desc" HeaderText="Section" DataField="Sect_Desc">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Problem_Desc" HeaderText="Justification" DataField="Problem_Desc">
                                </telerik:GridBoundColumn>

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
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gridDepartmentRecord" runat="server" CssClass="RadGrid" GridLines="None"
                        AllowPaging="True" PageSize="20" AllowSorting="True" AutoGenerateColumns="False"
                        ShowStatusBar="true" AllowAutomaticDeletes="True" AllowAutomaticInserts="True" Skin="WebBlue"
                        AllowAutomaticUpdates="True" DataSourceID="getDepartmentRecords">
                        <PagerStyle Mode="NextPrev" Position="Bottom" PageSizeControlType="RadComboBox"></PagerStyle>
                        <MasterTableView DataSourceID="getDepartmentRecords" DataKeyNames="Jr_Code">
                            <Columns>
                                <telerik:GridBoundColumn UniqueName="JONo" HeaderText="Job Order No" DataField="JRF_No" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestedby" HeaderText="Requested by" DataField="FullName_FnameFirst" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Requestdate" HeaderText="Date Request" DataField="NewDate" AllowFiltering="false">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Dept_Desc" HeaderText="Department" DataField="Dept_Desc">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Sect_Desc" HeaderText="Section" DataField="Sect_Desc">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn UniqueName="Problem_Desc" HeaderText="Justification" DataField="Problem_Desc">
                                </telerik:GridBoundColumn>

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
        
                Case when H.Counters is null then 0 else 1 end Counters
        
                from tblRequest A

		        left join tblIncharge B on A.incharge_id = b.incharge_id

                left join tblAssessment E on a.AssessmentCode = e.AssessmentCode

		        inner join HRIS.dbo.t_EmpMaster C on A.EmpID collate SQL_Latin1_General_CP1_CI_AS = C.EmpID collate SQL_Latin1_General_CP1_CI_AS
        
                inner JOIN HRIS.dbo.t_Department d ON d.DeptCode = c.Department
	
	            inner JOIN HRIS.dbo.t_DepartmentalSection g ON c.Section = g.SectCode and g.DeptCode = c.Department
	
                LEFT JOIN #tblCountAttachment H on A.JR_Code collate SQL_Latin1_General_CP1_CI_AS = H.JR_Code collate SQL_Latin1_General_CP1_CI_AS
	
                where A.EmpID = @EmpID "
        SelectCommandType="Text">
        <SelectParameters>
            <asp:SessionParameter Name="EmpID" SessionField="EmpID" Type="String" />
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
        
                    Case when H.Counters is null then 0 else 1 end Counters
        
                    from tblRequest A

		            left join tblIncharge B on A.incharge_id = b.incharge_id

                    left join tblAssessment E on a.AssessmentCode = e.AssessmentCode

		            inner join HRIS.dbo.t_EmpMaster C on A.EmpID collate SQL_Latin1_General_CP1_CI_AS = C.EmpID collate SQL_Latin1_General_CP1_CI_AS
        
                    inner JOIN HRIS.dbo.t_Department d ON d.DeptCode = c.Department
	
	                inner JOIN HRIS.dbo.t_DepartmentalSection g ON c.Section = g.SectCode and g.DeptCode = c.Department
	
                    LEFT JOIN #tblCountAttachment H on A.JR_Code collate SQL_Latin1_General_CP1_CI_AS = H.JR_Code collate SQL_Latin1_General_CP1_CI_AS

				    WHERE a.DeptCode = @Code and a.SectCode = @SectCode
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
        
                Case when H.Counters is null then 0 else 1 end Counters
        
                from tblRequest A

		        left join tblIncharge B on A.incharge_id = b.incharge_id

                left join tblAssessment E on a.AssessmentCode = e.AssessmentCode

		        inner join HRIS.dbo.t_EmpMaster C on A.EmpID collate SQL_Latin1_General_CP1_CI_AS = C.EmpID collate SQL_Latin1_General_CP1_CI_AS
        
                inner JOIN HRIS.dbo.t_Department d ON d.DeptCode = c.Department
	
	            inner JOIN HRIS.dbo.t_DepartmentalSection g ON c.Section = g.SectCode and g.DeptCode = c.Department
	
                LEFT JOIN #tblCountAttachment H on A.JR_Code collate SQL_Latin1_General_CP1_CI_AS = H.JR_Code collate SQL_Latin1_General_CP1_CI_AS

				WHERE a.DeptCode = @Code
            END"
        SelectCommandType="Text">
        <SelectParameters>
            <asp:SessionParameter Name="Code" SessionField="DeptCode" Type="String" />
            <asp:SessionParameter Name="SectCode" SessionField="SectCode" Type="String" />
        </SelectParameters>

    </asp:SqlDataSource>


</asp:Content>
