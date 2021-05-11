﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NewJobRequestSystem
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HRISEntities : DbContext
    {
        public HRISEntities()
            : base("name=HRISEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<t_City> t_City { get; set; }
        public virtual DbSet<t_CivilStatus> t_CivilStatus { get; set; }
        public virtual DbSet<t_COEBodyTemplate> t_COEBodyTemplate { get; set; }
        public virtual DbSet<t_Country> t_Country { get; set; }
        public virtual DbSet<t_Department> t_Department { get; set; }
        public virtual DbSet<t_DepartmentalSection> t_DepartmentalSection { get; set; }
        public virtual DbSet<t_DependentRelation> t_DependentRelation { get; set; }
        public virtual DbSet<t_EducationalAttainment> t_EducationalAttainment { get; set; }
        public virtual DbSet<t_EmpAddress> t_EmpAddress { get; set; }
        public virtual DbSet<t_EmpDependents> t_EmpDependents { get; set; }
        public virtual DbSet<t_EmpEducationalBackground> t_EmpEducationalBackground { get; set; }
        public virtual DbSet<t_EmpMaster> t_EmpMaster { get; set; }
        public virtual DbSet<t_EmpPrintID> t_EmpPrintID { get; set; }
        public virtual DbSet<t_EmpReClassification> t_EmpReClassification { get; set; }
        public virtual DbSet<t_EmpStatus> t_EmpStatus { get; set; }
        public virtual DbSet<t_EmpTrainingsSeminars> t_EmpTrainingsSeminars { get; set; }
        public virtual DbSet<t_EmpWorkExperience> t_EmpWorkExperience { get; set; }
        public virtual DbSet<t_HeightUnit> t_HeightUnit { get; set; }
        public virtual DbSet<t_Nationality> t_Nationality { get; set; }
        public virtual DbSet<t_PayMode> t_PayMode { get; set; }
        public virtual DbSet<t_Position> t_Position { get; set; }
        public virtual DbSet<t_Province> t_Province { get; set; }
        public virtual DbSet<t_Religion> t_Religion { get; set; }
        public virtual DbSet<t_Sex> t_Sex { get; set; }
        public virtual DbSet<t_ShuttleDestination> t_ShuttleDestination { get; set; }
        public virtual DbSet<t_TempEmpDependents> t_TempEmpDependents { get; set; }
        public virtual DbSet<t_TempEmpWorkExp> t_TempEmpWorkExp { get; set; }
        public virtual DbSet<t_Users> t_Users { get; set; }
        public virtual DbSet<t_UserType> t_UserType { get; set; }
        public virtual DbSet<t_WeightUnit> t_WeightUnit { get; set; }
        public virtual DbSet<tblSystemFile> tblSystemFiles { get; set; }
        public virtual DbSet<tblSystemVersion> tblSystemVersions { get; set; }
        public virtual DbSet<t_COETemplateForPrinting> t_COETemplateForPrinting { get; set; }
        public virtual DbSet<t_COEType> t_COEType { get; set; }
        public virtual DbSet<t_EmployeeFingerData> t_EmployeeFingerData { get; set; }
    
        public virtual ObjectResult<DCS_GetAdministratorByID_Result> DCS_GetAdministratorByID(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DCS_GetAdministratorByID_Result>("DCS_GetAdministratorByID", idParameter);
        }
    
        public virtual ObjectResult<DCS_GetAdministratorByName_Result> DCS_GetAdministratorByName(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DCS_GetAdministratorByName_Result>("DCS_GetAdministratorByName", nameParameter);
        }
    
        public virtual int DCS_GetAffectedDepartments(string uc)
        {
            var ucParameter = uc != null ?
                new ObjectParameter("uc", uc) :
                new ObjectParameter("uc", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DCS_GetAffectedDepartments", ucParameter);
        }
    
        public virtual ObjectResult<DCS_GetAllAdministrators_Result> DCS_GetAllAdministrators()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DCS_GetAllAdministrators_Result>("DCS_GetAllAdministrators");
        }
    
        public virtual ObjectResult<DCS_GetAllApprovers_Users_Result> DCS_GetAllApprovers_Users()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DCS_GetAllApprovers_Users_Result>("DCS_GetAllApprovers_Users");
        }
    
        public virtual ObjectResult<DCS_GetAllApprovers_UsersByName_Result> DCS_GetAllApprovers_UsersByName(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DCS_GetAllApprovers_UsersByName_Result>("DCS_GetAllApprovers_UsersByName", nameParameter);
        }
    
        public virtual ObjectResult<string> DisplayCOE()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("DisplayCOE");
        }
    
        public virtual int EPLS_GetAllEmployeesByDepartment(string deptcode, string leaderempid)
        {
            var deptcodeParameter = deptcode != null ?
                new ObjectParameter("deptcode", deptcode) :
                new ObjectParameter("deptcode", typeof(string));
    
            var leaderempidParameter = leaderempid != null ?
                new ObjectParameter("leaderempid", leaderempid) :
                new ObjectParameter("leaderempid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetAllEmployeesByDepartment", deptcodeParameter, leaderempidParameter);
        }
    
        public virtual int EPLS_GetLeaderBySubordinate(string empid)
        {
            var empidParameter = empid != null ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetLeaderBySubordinate", empidParameter);
        }
    
        public virtual int EPLS_GetManager(string empid)
        {
            var empidParameter = empid != null ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetManager", empidParameter);
        }
    
        public virtual int EPLS_GetSubordinates(string leaderempid)
        {
            var leaderempidParameter = leaderempid != null ?
                new ObjectParameter("leaderempid", leaderempid) :
                new ObjectParameter("leaderempid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetSubordinates", leaderempidParameter);
        }
    
        public virtual int EPLS_GetSubordinatesByEmpID(string empid)
        {
            var empidParameter = empid != null ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetSubordinatesByEmpID", empidParameter);
        }
    
        public virtual int EPLS_GetSubordinatesForPersonalObjective(string leaderempid)
        {
            var leaderempidParameter = leaderempid != null ?
                new ObjectParameter("leaderempid", leaderempid) :
                new ObjectParameter("leaderempid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetSubordinatesForPersonalObjective", leaderempidParameter);
        }
    
        public virtual int EPLS_GetSV_HeadBySubordinate(string empid)
        {
            var empidParameter = empid != null ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetSV_HeadBySubordinate", empidParameter);
        }
    
        public virtual int EPLS_GetSV_Manager()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetSV_Manager");
        }
    
        public virtual int EPLS_GetSV_ManagerForRegistration()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("EPLS_GetSV_ManagerForRegistration");
        }
    
        public virtual int GetAllDepartment()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetAllDepartment");
        }
    
        public virtual int GetAllManager()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetAllManager");
        }
    
        public virtual ObjectResult<GetSelectedEmpforPrintID_Result> GetSelectedEmpforPrintID()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetSelectedEmpforPrintID_Result>("GetSelectedEmpforPrintID");
        }
    
        public virtual int PIS_UpdateMasterData(Nullable<short> batchNo, Nullable<short> serialID, string empID, string lastName, string firstName, string middleName, string addressStreet, string addressCity, string addressProvince, string addressCountry, Nullable<short> addrCity, Nullable<byte> addrProvince, Nullable<byte> addrCountry, string phone, string sex, Nullable<System.DateTime> birthDate, Nullable<System.DateTime> hireDate, string employmentStatus, string nationality, string religion, string civilStatus, Nullable<byte> noOfDependents_orig, string highestEducationalAttainment_orig, string positionHeld, string department, string section, string subSection, string sSSno, string pHno, string pagIbigNo, string tIN, string empPicture, string contactPerson, string contactPhone, string contactAddress, string payMode, string height, string heightUnit, string weight, string weightUnit, string permanentAddress, string spouseName, string spouseOccupation, string fathersName, string fathersOccupation, string mothersMaidenName, string mothersOccupation, Nullable<System.DateTime> endOfWork, string originalSection, string taxExemptCode_orig, string activity, string course, Nullable<bool> isLicensed, string school, string position, byte[] photo, string highestEducationalAttainment, Nullable<bool> iDissued, string initialEntryBy, Nullable<System.DateTime> initialEntryDate, string modifiedBy, Nullable<System.DateTime> lastUpdate, string costCenter, Nullable<bool> apptPaperIssued, string taxExemptCode, Nullable<byte> noOfDependents, string empJapaneseNameLocation, Nullable<byte> agencyID, string emailAddress)
        {
            var batchNoParameter = batchNo.HasValue ?
                new ObjectParameter("BatchNo", batchNo) :
                new ObjectParameter("BatchNo", typeof(short));
    
            var serialIDParameter = serialID.HasValue ?
                new ObjectParameter("SerialID", serialID) :
                new ObjectParameter("SerialID", typeof(short));
    
            var empIDParameter = empID != null ?
                new ObjectParameter("EmpID", empID) :
                new ObjectParameter("EmpID", typeof(string));
    
            var lastNameParameter = lastName != null ?
                new ObjectParameter("LastName", lastName) :
                new ObjectParameter("LastName", typeof(string));
    
            var firstNameParameter = firstName != null ?
                new ObjectParameter("FirstName", firstName) :
                new ObjectParameter("FirstName", typeof(string));
    
            var middleNameParameter = middleName != null ?
                new ObjectParameter("MiddleName", middleName) :
                new ObjectParameter("MiddleName", typeof(string));
    
            var addressStreetParameter = addressStreet != null ?
                new ObjectParameter("AddressStreet", addressStreet) :
                new ObjectParameter("AddressStreet", typeof(string));
    
            var addressCityParameter = addressCity != null ?
                new ObjectParameter("AddressCity", addressCity) :
                new ObjectParameter("AddressCity", typeof(string));
    
            var addressProvinceParameter = addressProvince != null ?
                new ObjectParameter("AddressProvince", addressProvince) :
                new ObjectParameter("AddressProvince", typeof(string));
    
            var addressCountryParameter = addressCountry != null ?
                new ObjectParameter("AddressCountry", addressCountry) :
                new ObjectParameter("AddressCountry", typeof(string));
    
            var addrCityParameter = addrCity.HasValue ?
                new ObjectParameter("AddrCity", addrCity) :
                new ObjectParameter("AddrCity", typeof(short));
    
            var addrProvinceParameter = addrProvince.HasValue ?
                new ObjectParameter("AddrProvince", addrProvince) :
                new ObjectParameter("AddrProvince", typeof(byte));
    
            var addrCountryParameter = addrCountry.HasValue ?
                new ObjectParameter("AddrCountry", addrCountry) :
                new ObjectParameter("AddrCountry", typeof(byte));
    
            var phoneParameter = phone != null ?
                new ObjectParameter("Phone", phone) :
                new ObjectParameter("Phone", typeof(string));
    
            var sexParameter = sex != null ?
                new ObjectParameter("Sex", sex) :
                new ObjectParameter("Sex", typeof(string));
    
            var birthDateParameter = birthDate.HasValue ?
                new ObjectParameter("BirthDate", birthDate) :
                new ObjectParameter("BirthDate", typeof(System.DateTime));
    
            var hireDateParameter = hireDate.HasValue ?
                new ObjectParameter("HireDate", hireDate) :
                new ObjectParameter("HireDate", typeof(System.DateTime));
    
            var employmentStatusParameter = employmentStatus != null ?
                new ObjectParameter("EmploymentStatus", employmentStatus) :
                new ObjectParameter("EmploymentStatus", typeof(string));
    
            var nationalityParameter = nationality != null ?
                new ObjectParameter("Nationality", nationality) :
                new ObjectParameter("Nationality", typeof(string));
    
            var religionParameter = religion != null ?
                new ObjectParameter("Religion", religion) :
                new ObjectParameter("Religion", typeof(string));
    
            var civilStatusParameter = civilStatus != null ?
                new ObjectParameter("CivilStatus", civilStatus) :
                new ObjectParameter("CivilStatus", typeof(string));
    
            var noOfDependents_origParameter = noOfDependents_orig.HasValue ?
                new ObjectParameter("NoOfDependents_orig", noOfDependents_orig) :
                new ObjectParameter("NoOfDependents_orig", typeof(byte));
    
            var highestEducationalAttainment_origParameter = highestEducationalAttainment_orig != null ?
                new ObjectParameter("HighestEducationalAttainment_orig", highestEducationalAttainment_orig) :
                new ObjectParameter("HighestEducationalAttainment_orig", typeof(string));
    
            var positionHeldParameter = positionHeld != null ?
                new ObjectParameter("PositionHeld", positionHeld) :
                new ObjectParameter("PositionHeld", typeof(string));
    
            var departmentParameter = department != null ?
                new ObjectParameter("Department", department) :
                new ObjectParameter("Department", typeof(string));
    
            var sectionParameter = section != null ?
                new ObjectParameter("Section", section) :
                new ObjectParameter("Section", typeof(string));
    
            var subSectionParameter = subSection != null ?
                new ObjectParameter("SubSection", subSection) :
                new ObjectParameter("SubSection", typeof(string));
    
            var sSSnoParameter = sSSno != null ?
                new ObjectParameter("SSSno", sSSno) :
                new ObjectParameter("SSSno", typeof(string));
    
            var pHnoParameter = pHno != null ?
                new ObjectParameter("PHno", pHno) :
                new ObjectParameter("PHno", typeof(string));
    
            var pagIbigNoParameter = pagIbigNo != null ?
                new ObjectParameter("PagIbigNo", pagIbigNo) :
                new ObjectParameter("PagIbigNo", typeof(string));
    
            var tINParameter = tIN != null ?
                new ObjectParameter("TIN", tIN) :
                new ObjectParameter("TIN", typeof(string));
    
            var empPictureParameter = empPicture != null ?
                new ObjectParameter("EmpPicture", empPicture) :
                new ObjectParameter("EmpPicture", typeof(string));
    
            var contactPersonParameter = contactPerson != null ?
                new ObjectParameter("ContactPerson", contactPerson) :
                new ObjectParameter("ContactPerson", typeof(string));
    
            var contactPhoneParameter = contactPhone != null ?
                new ObjectParameter("ContactPhone", contactPhone) :
                new ObjectParameter("ContactPhone", typeof(string));
    
            var contactAddressParameter = contactAddress != null ?
                new ObjectParameter("ContactAddress", contactAddress) :
                new ObjectParameter("ContactAddress", typeof(string));
    
            var payModeParameter = payMode != null ?
                new ObjectParameter("PayMode", payMode) :
                new ObjectParameter("PayMode", typeof(string));
    
            var heightParameter = height != null ?
                new ObjectParameter("Height", height) :
                new ObjectParameter("Height", typeof(string));
    
            var heightUnitParameter = heightUnit != null ?
                new ObjectParameter("HeightUnit", heightUnit) :
                new ObjectParameter("HeightUnit", typeof(string));
    
            var weightParameter = weight != null ?
                new ObjectParameter("Weight", weight) :
                new ObjectParameter("Weight", typeof(string));
    
            var weightUnitParameter = weightUnit != null ?
                new ObjectParameter("WeightUnit", weightUnit) :
                new ObjectParameter("WeightUnit", typeof(string));
    
            var permanentAddressParameter = permanentAddress != null ?
                new ObjectParameter("PermanentAddress", permanentAddress) :
                new ObjectParameter("PermanentAddress", typeof(string));
    
            var spouseNameParameter = spouseName != null ?
                new ObjectParameter("SpouseName", spouseName) :
                new ObjectParameter("SpouseName", typeof(string));
    
            var spouseOccupationParameter = spouseOccupation != null ?
                new ObjectParameter("SpouseOccupation", spouseOccupation) :
                new ObjectParameter("SpouseOccupation", typeof(string));
    
            var fathersNameParameter = fathersName != null ?
                new ObjectParameter("FathersName", fathersName) :
                new ObjectParameter("FathersName", typeof(string));
    
            var fathersOccupationParameter = fathersOccupation != null ?
                new ObjectParameter("FathersOccupation", fathersOccupation) :
                new ObjectParameter("FathersOccupation", typeof(string));
    
            var mothersMaidenNameParameter = mothersMaidenName != null ?
                new ObjectParameter("MothersMaidenName", mothersMaidenName) :
                new ObjectParameter("MothersMaidenName", typeof(string));
    
            var mothersOccupationParameter = mothersOccupation != null ?
                new ObjectParameter("MothersOccupation", mothersOccupation) :
                new ObjectParameter("MothersOccupation", typeof(string));
    
            var endOfWorkParameter = endOfWork.HasValue ?
                new ObjectParameter("EndOfWork", endOfWork) :
                new ObjectParameter("EndOfWork", typeof(System.DateTime));
    
            var originalSectionParameter = originalSection != null ?
                new ObjectParameter("OriginalSection", originalSection) :
                new ObjectParameter("OriginalSection", typeof(string));
    
            var taxExemptCode_origParameter = taxExemptCode_orig != null ?
                new ObjectParameter("TaxExemptCode_orig", taxExemptCode_orig) :
                new ObjectParameter("TaxExemptCode_orig", typeof(string));
    
            var activityParameter = activity != null ?
                new ObjectParameter("Activity", activity) :
                new ObjectParameter("Activity", typeof(string));
    
            var courseParameter = course != null ?
                new ObjectParameter("Course", course) :
                new ObjectParameter("Course", typeof(string));
    
            var isLicensedParameter = isLicensed.HasValue ?
                new ObjectParameter("IsLicensed", isLicensed) :
                new ObjectParameter("IsLicensed", typeof(bool));
    
            var schoolParameter = school != null ?
                new ObjectParameter("School", school) :
                new ObjectParameter("School", typeof(string));
    
            var positionParameter = position != null ?
                new ObjectParameter("Position", position) :
                new ObjectParameter("Position", typeof(string));
    
            var photoParameter = photo != null ?
                new ObjectParameter("Photo", photo) :
                new ObjectParameter("Photo", typeof(byte[]));
    
            var highestEducationalAttainmentParameter = highestEducationalAttainment != null ?
                new ObjectParameter("HighestEducationalAttainment", highestEducationalAttainment) :
                new ObjectParameter("HighestEducationalAttainment", typeof(string));
    
            var iDissuedParameter = iDissued.HasValue ?
                new ObjectParameter("IDissued", iDissued) :
                new ObjectParameter("IDissued", typeof(bool));
    
            var initialEntryByParameter = initialEntryBy != null ?
                new ObjectParameter("InitialEntryBy", initialEntryBy) :
                new ObjectParameter("InitialEntryBy", typeof(string));
    
            var initialEntryDateParameter = initialEntryDate.HasValue ?
                new ObjectParameter("InitialEntryDate", initialEntryDate) :
                new ObjectParameter("InitialEntryDate", typeof(System.DateTime));
    
            var modifiedByParameter = modifiedBy != null ?
                new ObjectParameter("ModifiedBy", modifiedBy) :
                new ObjectParameter("ModifiedBy", typeof(string));
    
            var lastUpdateParameter = lastUpdate.HasValue ?
                new ObjectParameter("LastUpdate", lastUpdate) :
                new ObjectParameter("LastUpdate", typeof(System.DateTime));
    
            var costCenterParameter = costCenter != null ?
                new ObjectParameter("CostCenter", costCenter) :
                new ObjectParameter("CostCenter", typeof(string));
    
            var apptPaperIssuedParameter = apptPaperIssued.HasValue ?
                new ObjectParameter("ApptPaperIssued", apptPaperIssued) :
                new ObjectParameter("ApptPaperIssued", typeof(bool));
    
            var taxExemptCodeParameter = taxExemptCode != null ?
                new ObjectParameter("TaxExemptCode", taxExemptCode) :
                new ObjectParameter("TaxExemptCode", typeof(string));
    
            var noOfDependentsParameter = noOfDependents.HasValue ?
                new ObjectParameter("NoOfDependents", noOfDependents) :
                new ObjectParameter("NoOfDependents", typeof(byte));
    
            var empJapaneseNameLocationParameter = empJapaneseNameLocation != null ?
                new ObjectParameter("EmpJapaneseNameLocation", empJapaneseNameLocation) :
                new ObjectParameter("EmpJapaneseNameLocation", typeof(string));
    
            var agencyIDParameter = agencyID.HasValue ?
                new ObjectParameter("AgencyID", agencyID) :
                new ObjectParameter("AgencyID", typeof(byte));
    
            var emailAddressParameter = emailAddress != null ?
                new ObjectParameter("EmailAddress", emailAddress) :
                new ObjectParameter("EmailAddress", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("PIS_UpdateMasterData", batchNoParameter, serialIDParameter, empIDParameter, lastNameParameter, firstNameParameter, middleNameParameter, addressStreetParameter, addressCityParameter, addressProvinceParameter, addressCountryParameter, addrCityParameter, addrProvinceParameter, addrCountryParameter, phoneParameter, sexParameter, birthDateParameter, hireDateParameter, employmentStatusParameter, nationalityParameter, religionParameter, civilStatusParameter, noOfDependents_origParameter, highestEducationalAttainment_origParameter, positionHeldParameter, departmentParameter, sectionParameter, subSectionParameter, sSSnoParameter, pHnoParameter, pagIbigNoParameter, tINParameter, empPictureParameter, contactPersonParameter, contactPhoneParameter, contactAddressParameter, payModeParameter, heightParameter, heightUnitParameter, weightParameter, weightUnitParameter, permanentAddressParameter, spouseNameParameter, spouseOccupationParameter, fathersNameParameter, fathersOccupationParameter, mothersMaidenNameParameter, mothersOccupationParameter, endOfWorkParameter, originalSectionParameter, taxExemptCode_origParameter, activityParameter, courseParameter, isLicensedParameter, schoolParameter, positionParameter, photoParameter, highestEducationalAttainmentParameter, iDissuedParameter, initialEntryByParameter, initialEntryDateParameter, modifiedByParameter, lastUpdateParameter, costCenterParameter, apptPaperIssuedParameter, taxExemptCodeParameter, noOfDependentsParameter, empJapaneseNameLocationParameter, agencyIDParameter, emailAddressParameter);
        }
    
        public virtual ObjectResult<SKPI_GetAllEmployees_Result> SKPI_GetAllEmployees()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SKPI_GetAllEmployees_Result>("SKPI_GetAllEmployees");
        }
    
        public virtual ObjectResult<SKPI_GetAllEmployeesByDepartment_Result> SKPI_GetAllEmployeesByDepartment(string deptcode)
        {
            var deptcodeParameter = deptcode != null ?
                new ObjectParameter("deptcode", deptcode) :
                new ObjectParameter("deptcode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SKPI_GetAllEmployeesByDepartment_Result>("SKPI_GetAllEmployeesByDepartment", deptcodeParameter);
        }
    
        public virtual ObjectResult<SKPI_GetAllEmployeesByDepartmentandSection_Result> SKPI_GetAllEmployeesByDepartmentandSection(string deptcode, string sectcode)
        {
            var deptcodeParameter = deptcode != null ?
                new ObjectParameter("deptcode", deptcode) :
                new ObjectParameter("deptcode", typeof(string));
    
            var sectcodeParameter = sectcode != null ?
                new ObjectParameter("sectcode", sectcode) :
                new ObjectParameter("sectcode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SKPI_GetAllEmployeesByDepartmentandSection_Result>("SKPI_GetAllEmployeesByDepartmentandSection", deptcodeParameter, sectcodeParameter);
        }
    
        public virtual ObjectResult<SKPI_GetAllEmployeesByEmpID_Result> SKPI_GetAllEmployeesByEmpID(string empid)
        {
            var empidParameter = empid != null ?
                new ObjectParameter("empid", empid) :
                new ObjectParameter("empid", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SKPI_GetAllEmployeesByEmpID_Result>("SKPI_GetAllEmployeesByEmpID", empidParameter);
        }
    
        public virtual ObjectResult<SKPI_GetAllEmployeesByName_Result> SKPI_GetAllEmployeesByName(string name)
        {
            var nameParameter = name != null ?
                new ObjectParameter("name", name) :
                new ObjectParameter("name", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SKPI_GetAllEmployeesByName_Result>("SKPI_GetAllEmployeesByName", nameParameter);
        }
    
        public virtual ObjectResult<SKPI_GetDCApprovers_Users_Result> SKPI_GetDCApprovers_Users()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SKPI_GetDCApprovers_Users_Result>("SKPI_GetDCApprovers_Users");
        }
    
        public virtual int SKPI_GetEmpLoginByID(string userID, string userPass)
        {
            var userIDParameter = userID != null ?
                new ObjectParameter("UserID", userID) :
                new ObjectParameter("UserID", typeof(string));
    
            var userPassParameter = userPass != null ?
                new ObjectParameter("UserPass", userPass) :
                new ObjectParameter("UserPass", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SKPI_GetEmpLoginByID", userIDParameter, userPassParameter);
        }
    
        public virtual int SKPI_GetSection(string deptcode)
        {
            var deptcodeParameter = deptcode != null ?
                new ObjectParameter("deptcode", deptcode) :
                new ObjectParameter("deptcode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SKPI_GetSection", deptcodeParameter);
        }
    
        public virtual int sp_alterdiagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_alterdiagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_creatediagram(string diagramname, Nullable<int> owner_id, Nullable<int> version, byte[] definition)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var versionParameter = version.HasValue ?
                new ObjectParameter("version", version) :
                new ObjectParameter("version", typeof(int));
    
            var definitionParameter = definition != null ?
                new ObjectParameter("definition", definition) :
                new ObjectParameter("definition", typeof(byte[]));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_creatediagram", diagramnameParameter, owner_idParameter, versionParameter, definitionParameter);
        }
    
        public virtual int sp_dropdiagram(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_dropdiagram", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagramdefinition_Result> sp_helpdiagramdefinition(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagramdefinition_Result>("sp_helpdiagramdefinition", diagramnameParameter, owner_idParameter);
        }
    
        public virtual ObjectResult<sp_helpdiagrams_Result> sp_helpdiagrams(string diagramname, Nullable<int> owner_id)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_helpdiagrams_Result>("sp_helpdiagrams", diagramnameParameter, owner_idParameter);
        }
    
        public virtual int sp_renamediagram(string diagramname, Nullable<int> owner_id, string new_diagramname)
        {
            var diagramnameParameter = diagramname != null ?
                new ObjectParameter("diagramname", diagramname) :
                new ObjectParameter("diagramname", typeof(string));
    
            var owner_idParameter = owner_id.HasValue ?
                new ObjectParameter("owner_id", owner_id) :
                new ObjectParameter("owner_id", typeof(int));
    
            var new_diagramnameParameter = new_diagramname != null ?
                new ObjectParameter("new_diagramname", new_diagramname) :
                new ObjectParameter("new_diagramname", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_renamediagram", diagramnameParameter, owner_idParameter, new_diagramnameParameter);
        }
    
        public virtual int sp_upgraddiagrams()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_upgraddiagrams");
        }
    }
}
