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
    
    public partial class HC_REQUISITION_STATUS
    {
        public long RID { get; set; }
        public string Title { get; set; }
        public short Sequenceno { get; set; }
        public Nullable<int> Slno { get; set; }
        public bool ConsiderasOpen { get; set; }
        public short ConsiderasOnHold { get; set; }
        public long TID { get; set; }
        public Nullable<short> StatusCode { get; set; }
        public Nullable<bool> CanAddResume { get; set; }
        public bool Status { get; set; }
        public Nullable<long> ModifiedUserID { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> CreatedUserID { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public bool IsAuto { get; set; }
        public Nullable<bool> IsStatusMandatory { get; set; }
        public Nullable<bool> IsReasonMandatory { get; set; }
        public Nullable<short> V3STATUSCODE { get; set; }
    }
}
