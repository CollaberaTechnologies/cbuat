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
    
    public partial class vw_GetClientData
    {
        public long RID { get; set; }
        public string Name { get; set; }
        public short StageID { get; set; }
        public string StageName { get; set; }
        public short StatusID { get; set; }
        public string StatusName { get; set; }
        public bool Status { get; set; }
        public string Address { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string Notes { get; set; }
        public string OpportunityTitle { get; set; }
        public string GroupName { get; set; }
        public string AccountManager { get; set; }
        public short IsLead { get; set; }
        public long IndustryTypeID { get; set; }
        public string IndustryType { get; set; }
        public string City { get; set; }
        public System.DateTime NextMeeting { get; set; }
        public System.DateTime ClosureDate { get; set; }
        public string ContactName { get; set; }
        public string ContactEmailID { get; set; }
        public string ContactMobileNo { get; set; }
        public int TotalRequirements { get; set; }
        public int OpenRequirements { get; set; }
        public int ClosedRequirements { get; set; }
        public bool IsDataSync { get; set; }
    }
}
