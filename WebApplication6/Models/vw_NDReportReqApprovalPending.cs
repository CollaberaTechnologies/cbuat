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
    
    public partial class vw_NDReportReqApprovalPending
    {
        public long RID { get; set; }
        public long ReqID { get; set; }
        public string JobCode { get; set; }
        public string BusinessUnit { get; set; }
        public string Department { get; set; }
        public string Function { get; set; }
        public string Role { get; set; }
        public string Designation { get; set; }
        public string HiringManager { get; set; }
        public string EmploymentType { get; set; }
        public string Location { get; set; }
        public string CreatedDate { get; set; }
        public Nullable<long> Age { get; set; }
        public short SLA { get; set; }
        public string ApprovalPendingAt { get; set; }
        public long Noofdayspendingapproval { get; set; }
        public System.DateTime CDate { get; set; }
    }
}