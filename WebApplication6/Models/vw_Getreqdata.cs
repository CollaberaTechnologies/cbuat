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
    
    public partial class vw_Getreqdata
    {
        public long RID { get; set; }
        public long ContactID { get; set; }
        public string BU { get; set; }
        public long BUID { get; set; }
        public string SBU { get; set; }
        public long SBUID { get; set; }
        public string DepartmentID { get; set; }
        public string Department { get; set; }
        public string ProjectID { get; set; }
        public long Project { get; set; }
        public string ReqTitle { get; set; }
        public string JobTitle { get; set; }
        public string ReqNumber { get; set; }
        public Nullable<int> NoOfOpenings { get; set; }
        public Nullable<short> ReqStatus { get; set; }
        public string RequisitionStatus { get; set; }
        public string JobDesc { get; set; }
        public string CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedUser { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string Contacts { get; set; }
        public long DesignationID { get; set; }
        public string Designation { get; set; }
        public decimal MinExperience { get; set; }
        public decimal MaxExperience { get; set; }
        public long ReportingMgrID { get; set; }
        public string ReportingManager { get; set; }
        public Nullable<System.DateTime> ReceivedDate { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string InvAssessmentSheet { get; set; }
        public long GradeID { get; set; }
        public string Grade { get; set; }
        public string WorkFlowTemplate { get; set; }
        public long WFTemplateID { get; set; }
        public long RequesterID { get; set; }
        public string Requester { get; set; }
        public long BaseLocationID { get; set; }
        public string BaseLocation { get; set; }
        public string Location { get; set; }
        public string ApprovalSummary { get; set; }
        public string EmploymentType { get; set; }
        public short EmpType { get; set; }
        public short ReqType { get; set; }
        public string RequirementType { get; set; }
        public long PriorityID { get; set; }
        public string Priority { get; set; }
        public string Notes { get; set; }
        public string EmpID { get; set; }
        public string Name { get; set; }
        public string Functions { get; set; }
        public string SubFunctions { get; set; }
        public string Education { get; set; }
        public string Educations { get; set; }
        public string ExpRange { get; set; }
        public string Skills { get; set; }
        public string requisitionSkills { get; set; }
        public long TID { get; set; }
        public bool Status { get; set; }
        public Nullable<int> NoOfResumeTagged { get; set; }
        public Nullable<int> Interview { get; set; }
        public Nullable<int> Offer { get; set; }
        public Nullable<int> Joined { get; set; }
        public Nullable<int> Shortlist { get; set; }
        public Nullable<int> Screening { get; set; }
        public Nullable<int> Applicants { get; set; }
        public long CreatedUserID { get; set; }
        public string Keyword { get; set; }
        public string AnchorName { get; set; }
        public Nullable<long> AnchorID { get; set; }
        public string ApprovalPendingAt { get; set; }
        public Nullable<int> Ageing { get; set; }
        public int TurnAroundTime { get; set; }
        public int sla { get; set; }
        public int tat { get; set; }
        public string ontimeclosed { get; set; }
        public int onholddays { get; set; }
        public string Client { get; set; }
        public short SourcingStatusID { get; set; }
        public string ClientContactID { get; set; }
    }
}