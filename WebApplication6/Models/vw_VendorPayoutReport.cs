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
    
    public partial class vw_VendorPayoutReport
    {
        public long ReqResID { get; set; }
        public long ReqID { get; set; }
        public string EmpID { get; set; }
        public string CandidateName { get; set; }
        public string Designation { get; set; }
        public string Grade { get; set; }
        public System.DateTime JoinedDate { get; set; }
        public string RequisitionTitle { get; set; }
        public string JobCode { get; set; }
        public long ClientID { get; set; }
        public Nullable<long> AssignedAnchorID { get; set; }
        public Nullable<long> GradeID { get; set; }
        public System.DateTime ApplicantCreatedDate { get; set; }
        public string Client { get; set; }
        public string Location { get; set; }
        public string BU { get; set; }
        public string SBU { get; set; }
        public string JoiningLocation { get; set; }
        public string Team { get; set; }
        public string TicketNo { get; set; }
        public System.DateTime RefferedDate { get; set; }
        public string Vendor { get; set; }
        public string OfferedCTC { get; set; }
        public decimal BillableCTC { get; set; }
        public decimal BillableAmount { get; set; }
    }
}