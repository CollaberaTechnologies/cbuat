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
    
    public partial class vw_NDReportRequirementCostofHire
    {
        public long RID { get; set; }
        public string JobCode { get; set; }
        public string BusinessUnit { get; set; }
        public string Department { get; set; }
        public string Role { get; set; }
        public string Designation { get; set; }
        public string RecruiterName { get; set; }
        public string HiringManager { get; set; }
        public string EmploymentType { get; set; }
        public decimal ServiceFee { get; set; }
        public decimal RelocationCost { get; set; }
        public decimal NoticePeriodReimbursement { get; set; }
        public decimal JoiningBonus { get; set; }
        public long TravelReimbursement { get; set; }
        public decimal OtherExpense { get; set; }
        public decimal TotalExpense { get; set; }
        public Nullable<System.DateTime> ADate { get; set; }
    }
}
