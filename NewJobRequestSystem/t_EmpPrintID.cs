//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class t_EmpPrintID
    {
        public Nullable<int> BatchNo { get; set; }
        public string EmpID { get; set; }
        public string EmployeeName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string SSSno { get; set; }
        public string TIN { get; set; }
        public string ContactPerson { get; set; }
        public string ContactAddress { get; set; }
        public string ContactPhone { get; set; }
        public string Barcode { get; set; }
        public byte[] EmpPicture { get; set; }
    
        public virtual t_EmpMaster t_EmpMaster { get; set; }
    }
}
