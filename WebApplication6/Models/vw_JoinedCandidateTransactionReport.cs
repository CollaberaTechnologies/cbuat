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
    
    public partial class vw_JoinedCandidateTransactionReport
    {
        public long ResID { get; set; }
        public long ReqResID { get; set; }
        public long ReqID { get; set; }
        public string CandidateNo { get; set; }
        public long ClientID { get; set; }
        public string Client { get; set; }
        public string BU { get; set; }
        public string Location { get; set; }
        public string JoiningLocation { get; set; }
        public System.DateTime ApplicantCreatedDate { get; set; }
        public Nullable<long> AssignedAnchorID { get; set; }
        public Nullable<long> GradeID { get; set; }
        public string RequisitionTitle { get; set; }
        public string JobCode { get; set; }
        public string CandidateName { get; set; }
        public string CurrentStage { get; set; }
        public string CurrentCTC { get; set; }
        public string OfferedCTC { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string CurrentEmployer { get; set; }
        public string CurrentPosition { get; set; }
        public decimal TotalExp { get; set; }
        public string CandidateSource { get; set; }
        public string SourceRefID { get; set; }
        public System.DateTime OfferMadeDate { get; set; }
        public System.DateTime PlannedJoiningDate { get; set; }
        public System.DateTime JoinedDate { get; set; }
        public string ReportingTo { get; set; }
        public string ReportingDesignation { get; set; }
        public string RecruitmentMangaer { get; set; }
        public string Recruiter { get; set; }
        public string JoinedMonth { get; set; }
    }
}
