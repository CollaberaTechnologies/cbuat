//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication6.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class vw_NDReportCLIDeletion
    {
        public long rid { get; set; }
        public string EmpID { get; set; }
        public string SAPID { get; set; }
        public string Name { get; set; }
        public string DateofBirth { get; set; }
        public string DateofJoining { get; set; }
        public string DateofLeaving { get; set; }
        public string Reason { get; set; }
        public decimal SumAssured { get; set; }
        public decimal TotalCTC { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> JDate { get; set; }
    }
}