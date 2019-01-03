using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class DashboardController : Controller
    {
        private IHD_DBEntities12 db = new IHD_DBEntities12();
        V5Entities4 v5 = new V5Entities4();
        public void cache()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.AppendCacheExtension("no-store, must-revalidate");
        }
        public ActionResult Getdata()
        {
            long rid = (long)Session["id"];
            long uid = (long)Session["uiid"];
            var value = db.sp_cb_closing_block_dash(uid, rid).FirstOrDefault();
            if (value == null)
            {
                long Submitted = 0;
                long Received = 0;
                long Rejected = 0;
                long Total =0;
                ratio obj = new ratio();
                obj.admin = Submitted;
                obj.Finance = Received;
                obj.sales = Rejected;
                obj.Dash = Total;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                long Submitted = value.PendingCount;
                long Received = value.ApprovedCount;
                long Rejected = value.RejectCount;
                long Total = value.Total;
                ratio obj = new ratio();
                obj.admin = Submitted;
                obj.Finance = Received;
                obj.sales = Rejected;
                obj.Dash = Total;
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
          
          
        }
        public class ratio
        {
            public long admin { get; set; }
            public long Finance { get; set; }
            public long sales { get; set; }
            public long Dash { get; set; }


        }
        public ActionResult Dashboard()
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long rid = (long)Session["id"];
            long uid = (long)Session["uiid"];
            ViewBag.rightsid = rid;

            if (rid == 1)
            {
                ViewBag.countman = db.sp_cb_closing_block_dash(uid, rid);
                ViewBag.count = db.sp_cb_closing_block_dashmanager(uid, rid);
            }
            else
            {
                return View();
            }
            return View();
        }
     
      

        public string Imagename { get; private set; }


        [HttpGet]
        public ActionResult NewBlock()
        {
            if(Session["uiid"] == null)
            {
                
              return RedirectToAction("Login", "Account");

    }
            else
            {
                if (TempData["alert"] != null)
                {
                    ViewBag.alert = TempData["alert"].ToString();
                }
                else
                {
                    ViewBag.alert = "";
                }

                List<SelectListItem> country = v5.HC_CLIENT.Where(x=>x.CountryID==241).Select(c => new SelectListItem
                {
                    Value = c.RID.ToString(),

                    Text = c.Title

                }).Distinct().ToList();

                var countrytip = v5.HC_CLIENT.Where(x => x.CountryID == 244).Select(c => new SelectListItem
                {
                    Value = c.RID.ToString(),

                    Text = c.Title

                }).Distinct().ToList();
                country.InsertRange(0, countrytip);

                List<SelectListItem> req = new List<SelectListItem>();

                req.Add(new SelectListItem
                {
                    Value = 0.ToString(),

                    Text = "Please Select A Client"

                });
                List<SelectListItem> loc = new List<SelectListItem>();
                loc.Add(new SelectListItem
                {
                    Value = 0.ToString(),

                    Text = "Please Select A Client"

                });

                ViewBag.clients = country;
                ViewBag.Requirements = req;
                ViewBag.locations = loc;
                ViewBag.check = "";
                ViewBag.step1 = 1;
                ViewBag.nojobcode = "";
                return View();
            }
            
        }


        // Post: about
        [HttpPost]

        public ActionResult NewBlock(CL_CLOSING_BLOCK model, long client = 0, long Requirement = 0, long location = 0, string Jobcode = null)
        {
            if (Jobcode == null)
            {
                Jobcode = "";
            }

            if (Jobcode != "")
            {
                var jobdata = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).FirstOrDefault();
                if (jobdata == null)
                {
                    ViewBag.nojobcode = "Jobcode does not exists";
                }
                else
                {
                    var id = (long)v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.Client).FirstOrDefault();
                    model.ClientID = id;
                    ViewBag.clientname = v5.HC_CLIENT.Where(x => x.RID == id).Select(x => x.Title).FirstOrDefault();
                    model.Clients = ViewBag.clientname;

                    ViewBag.req = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.ReqTitle).FirstOrDefault();
                    model.req = ViewBag.req;
                    model.Reqid = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.RID).FirstOrDefault();
                    var lb = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.ClientBranchID).FirstOrDefault();

                    ViewBag.lob = v5.HCM_BRANCH.Where(x => x.RID == lb).Select(x => x.Name).FirstOrDefault();
                    model.lob = ViewBag.lob;
                    var s = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.ReqStatus).FirstOrDefault();

                    ViewBag.status = v5.HC_REQUISITION_STATUS.Where(x => x.StatusCode == s).Select(x => x.Title).FirstOrDefault();
                    model.status1 = ViewBag.status;
                 
                    var mappingid = v5.HC_REQUISITION_MAPPING.Where(x => x.ReqID == model.Reqid).Select(x => x.MappingID).FirstOrDefault();
                    ViewBag.reqloc1 = v5.HCM_LOCATIONS.Where(x => x.RID == mappingid).Select(x => x.Title).FirstOrDefault();

                    ViewBag.br = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.BillAmount).FirstOrDefault().ToString();
                    model.br = ViewBag.br;
                    ViewBag.pr1 = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.CTCFromAmount).FirstOrDefault();
                    ViewBag.pr2 = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.CTCToAmount).FirstOrDefault();
                    var pr3 = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.CTCCurrency).FirstOrDefault();
                    ViewBag.pr3 = v5.HCM_CURRENCY.Where(x => x.RID == pr3).Select(x => x.Currency).FirstOrDefault();
                    ViewBag.pr = ViewBag.pr3 + "-" + ViewBag.pr1 + "-" + ViewBag.pr2 + "";
                    model.pr = ViewBag.pr;
                    ViewBag.jobcode = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.ReqNumber).FirstOrDefault();
                    model.jobcode1 = ViewBag.jobcode;
                    ViewBag.jd = v5.HC_REQUISITIONS.Where(x => x.ReqNumber == Jobcode).Select(x => x.PlainJD).FirstOrDefault();
                    model.jd = ViewBag.jd;
                    ViewBag.nojobcode = "";

                }  
            }
            else
            {
                if (client != 0 && Requirement != 0 && location != 0)
                {
                    model.ClientID = client;
                    model.Reqid = Requirement;
                    var mappingid = v5.HC_REQUISITION_MAPPING.Where(x => x.ReqID == model.Reqid).Select(x => x.MappingID).FirstOrDefault();
                    ViewBag.reqloc1 = v5.HCM_LOCATIONS.Where(x => x.RID == mappingid).Select(x => x.Title).FirstOrDefault();
                    ViewBag.clientname = v5.HC_CLIENT.Where(x => x.RID == client).Select(x => x.Title).FirstOrDefault();
                    model.Clients = ViewBag.clientname;
                    ViewBag.req = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.ReqTitle).FirstOrDefault();
                    model.req = ViewBag.req;
                    var lb = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.ClientBranchID).FirstOrDefault();
                    ViewBag.lob = v5.HCM_BRANCH.Where(x => x.RID == lb).Select(x => x.Name).FirstOrDefault();
                    model.lob = ViewBag.lob;
                    var s = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.ReqStatus).FirstOrDefault();
                    ViewBag.status = v5.HC_REQUISITION_STATUS.Where(x => x.RID == s).Select(x => x.Title).FirstOrDefault();
                    model.status1 = ViewBag.status;
                    ViewBag.br = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.BillAmount).FirstOrDefault().ToString();
                    model.br = ViewBag.br;
                    ViewBag.pr1 = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.CTCFromAmount).FirstOrDefault();
                    ViewBag.pr2 = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.CTCToAmount).FirstOrDefault();
                    var pr3 = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.CTCCurrency).FirstOrDefault();
                    ViewBag.pr3 = v5.HCM_CURRENCY.Where(x => x.RID == pr3).Select(x => x.Currency).FirstOrDefault();
                    ViewBag.pr =ViewBag.pr3 + "" + ViewBag.pr1 + "-" + ViewBag.pr2 + "";
                    model.pr = ViewBag.pr;
                    ViewBag.jobcode = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.ReqNumber).FirstOrDefault();
                    model.jobcode1 = ViewBag.jobcode;
                    ViewBag.jd = v5.HC_REQUISITIONS.Where(x => x.RID == Requirement).Select(x => x.PlainJD).FirstOrDefault();
                    model.jd = ViewBag.jd;


                }
                else
                {
                    ViewBag.message = "Please Select All Fields";
                }
            }

            List<SelectListItem> country = v5.HC_CLIENT.Where(x => x.CountryID == 241).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();

            var countrytip = v5.HC_CLIENT.Where(x => x.CountryID == 244).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();
            country.InsertRange(0, countrytip);

            ViewBag.step1 = 1;
            ViewBag.step2 = 1;
            ViewBag.clients = country;
            title(model.client, model.Requirement);
            loc(model.Requirement, model.location);
            ViewBag.check = "";
            ViewBag.alert = "";
            
            return View(model);


        }
        [ValidateInput(false)]
       
        public ActionResult NewBlock2(CL_CLOSING_BLOCK model,int rid=0)
        {
            var CLIENT = model.ClientID;
            if (rid != 0)
            {
                var mappingid = v5.HC_REQUISITION_MAPPING.Where(x => x.ReqID == model.Reqid).Select(x => x.MappingID).FirstOrDefault();
                ViewBag.reqloc1 = v5.HCM_LOCATIONS.Where(x => x.RID == mappingid).Select(x => x.Title).FirstOrDefault();
                model.ClientID = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ClientID).FirstOrDefault();
                model.Reqid = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Reqid).FirstOrDefault();
                ViewBag.clientname = v5.HC_CLIENT.Where(x => x.RID == model.ClientID).Select(x => x.Title).FirstOrDefault();
                model.Clients = ViewBag.clientname;
                ViewBag.req = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.ReqTitle).FirstOrDefault();
                model.req = ViewBag.req;
                var lb = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.ClientBranchID).FirstOrDefault();
                ViewBag.lob = v5.HCM_BRANCH.Where(x => x.RID == lb).Select(x => x.Name).FirstOrDefault();
                model.lob = ViewBag.lob;
                var s = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.ReqStatus).FirstOrDefault();
                ViewBag.status = v5.HC_REQUISITION_STATUS.Where(x => x.RID == s).Select(x => x.Title).FirstOrDefault();
                model.status1 = ViewBag.status;
                ViewBag.br = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.BillAmount).FirstOrDefault().ToString();
                model.br = ViewBag.br;
                ViewBag.pr1 = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.CTCFromAmount).FirstOrDefault();
                ViewBag.pr2 = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.CTCToAmount).FirstOrDefault();
                var pr3 = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.CTCCurrency).FirstOrDefault();
                ViewBag.pr3 = v5.HCM_CURRENCY.Where(x => x.RID == pr3).Select(x => x.Currency).FirstOrDefault();
                ViewBag.pr = ViewBag.pr3 + "" + ViewBag.pr1 + "-" + ViewBag.pr2 + "";
                model.pr = ViewBag.pr;
                ViewBag.jobcode = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.ReqNumber).FirstOrDefault();
                model.jobcode1 = ViewBag.jobcode;
                ViewBag.jd = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.PlainJD).FirstOrDefault();
                model.jd = ViewBag.jd;
                model.CurrentSalary= db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.CurrentSalary).FirstOrDefault();
                model.TeleInterview = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.TeleInterview).FirstOrDefault();
                model.FaceInterview = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.FaceInterview).FirstOrDefault();
                model.FInterview = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.FInterview).FirstOrDefault();
                model.Holidays = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Holidays).FirstOrDefault();
                model.ANumber = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ANumber).FirstOrDefault();
                model.Fname = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Fname).FirstOrDefault();
                model.EmailId = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.EmailId).FirstOrDefault();
                model.PNumber = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.PNumber).FirstOrDefault();
                model.RID = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.RID).FirstOrDefault();
                model.AvailabilityStatus = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.AvailabilityStatus).FirstOrDefault();
                model.AvailJoinDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.AvailJoinDate).FirstOrDefault();
                model.BGVStatus = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.BGVStatus).FirstOrDefault();
                model.Ccompany = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Ccompany).FirstOrDefault();
                model.CLocation = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.CLocation).FirstOrDefault();
                model.ClosedPaye = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ClosedPaye).FirstOrDefault();
                model.comments = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.comments).FirstOrDefault();
                model.ConProjEndDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ConProjEndDate).FirstOrDefault();
                model.CreatedDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.CreatedDate).FirstOrDefault();
                model.DistHoToWorkLoc = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.DistHoToWorkLoc).FirstOrDefault();
                model.DOB = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.DOB).FirstOrDefault();
                model.EmpType = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.EmpType).FirstOrDefault();
                model.ExpectedSalary = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ExpectedSalary).FirstOrDefault();
                model.GPM = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.GPM).FirstOrDefault();
                model.GPPerHour = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.GPPerHour).FirstOrDefault();
                model.Insurance = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Insurance).FirstOrDefault();
                model.interviewDetails = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.interviewDetails).FirstOrDefault();
                model.Interviews = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Interviews).FirstOrDefault();
                model.LoadedPayRate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.LoadedPayRate).FirstOrDefault();
                model.LocPref = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.LocPref).FirstOrDefault();
                model.NationalityID = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.NationalityID).FirstOrDefault();
                model.NoticePeriod = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.NoticePeriod).FirstOrDefault();
                model.Offer = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Offer).FirstOrDefault();
                model.OfferRecieved = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.OfferRecieved).FirstOrDefault();
                model.Payrate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Payrate).FirstOrDefault();
                model.PlannedHolidays = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.PlannedHolidays).FirstOrDefault();
                model.PNumber = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.PNumber).FirstOrDefault();
                model.PurposedBillRate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.PurposedBillRate).FirstOrDefault();
                model.ReasonJobChange = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ReasonJobChange).FirstOrDefault();
                model.Reference1 = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Reference1).FirstOrDefault();
                model.Reference2 = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.Reference2).FirstOrDefault();
                model.ResonForLeaving = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.ResonForLeaving).FirstOrDefault();
                model.RExp = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.RExp).FirstOrDefault();
                model.SameClient = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.SameClient).FirstOrDefault();
                model.TExp = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.TExp).FirstOrDefault();
                model.VisaValidDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.VisaValidDate).FirstOrDefault();
                model.WorkLoc = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.WorkLoc).FirstOrDefault();
                model.WorkVisaStatus = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.WorkVisaStatus).FirstOrDefault();
                ViewBag.step3 = 1;
               

            }
            else
            {
                var id = db.CL_CLOSING_BLOCK.Where(x => x.Fname == model.Fname && x.EmailId == model.EmailId && x.PNumber == model.PNumber).FirstOrDefault();
                if (id == null)
                {
                    
                    ViewBag.step3 = 1;
                    ViewBag.step1 = 1;
                }
                else
                {
                    model.CurrentSalary = db.CL_CLOSING_BLOCK.Where(x => x.RID == rid).Select(x => x.CurrentSalary).FirstOrDefault();
                    model.TeleInterview = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.TeleInterview).FirstOrDefault();
                    model.FaceInterview = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.FaceInterview).FirstOrDefault();
                    model.FInterview = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.FInterview).FirstOrDefault();
                    model.Holidays = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Holidays).FirstOrDefault();
                    model.ANumber = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.ANumber).FirstOrDefault();
                    model.AvailabilityStatus = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.AvailabilityStatus).FirstOrDefault();
                    model.AvailJoinDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.AvailJoinDate).FirstOrDefault();
                    model.BGVStatus = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.BGVStatus).FirstOrDefault();
                    model.Ccompany = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Ccompany).FirstOrDefault();
                    model.CLocation = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.CLocation).FirstOrDefault();
                    model.ClosedPaye = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.ClosedPaye).FirstOrDefault();
                    model.comments = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.comments).FirstOrDefault();
                    model.ConProjEndDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.ConProjEndDate).FirstOrDefault();
                    model.CreatedDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.CreatedDate).FirstOrDefault();
                    model.DistHoToWorkLoc = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.DistHoToWorkLoc).FirstOrDefault();
                    model.DOB = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.DOB).FirstOrDefault();
                    model.EmpType = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.EmpType).FirstOrDefault();
                    model.ExpectedSalary = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.ExpectedSalary).FirstOrDefault();
                    model.GPM = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.GPM).FirstOrDefault();
                    model.GPPerHour = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.GPPerHour).FirstOrDefault();
                    model.Insurance = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Insurance).FirstOrDefault();
                    model.interviewDetails = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.interviewDetails).FirstOrDefault();
                    model.Interviews = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Interviews).FirstOrDefault();
                    model.LoadedPayRate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.LoadedPayRate).FirstOrDefault();
                    model.LocPref = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.LocPref).FirstOrDefault();
                    model.NationalityID = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.NationalityID).FirstOrDefault();
                    model.NoticePeriod = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.NoticePeriod).FirstOrDefault();
                    model.Offer = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Offer).FirstOrDefault();
                    model.OfferRecieved = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.OfferRecieved).FirstOrDefault();
                    model.Payrate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Payrate).FirstOrDefault();
                    model.PlannedHolidays = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.PlannedHolidays).FirstOrDefault();
                    model.PNumber = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.PNumber).FirstOrDefault();
                    model.PurposedBillRate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.PurposedBillRate).FirstOrDefault();
                    model.ReasonJobChange = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.ReasonJobChange).FirstOrDefault();
                    model.Reference1 = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Reference1).FirstOrDefault();
                    model.Reference2 = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.Reference2).FirstOrDefault();
                    model.ResonForLeaving = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.ResonForLeaving).FirstOrDefault();
                    model.RExp = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.RExp).FirstOrDefault();
                    model.SameClient = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.SameClient).FirstOrDefault();
                    model.TExp = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.TExp).FirstOrDefault();
                    model.VisaValidDate = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.VisaValidDate).FirstOrDefault();
                    model.WorkLoc = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.WorkLoc).FirstOrDefault();
                    model.WorkVisaStatus = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID).Select(x => x.WorkVisaStatus).FirstOrDefault();
                   
                    var user = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID && x.ClientID == model.ClientID).FirstOrDefault();
                    if (user == null)
                    {
                        ViewBag.step3 = 1;
                        ViewBag.step1 = 1;

                    }
                    else
                    {
                        var users = db.CL_CLOSING_BLOCK.Where(x => x.RID == id.RID && x.Reqid == model.Reqid).FirstOrDefault();
                        if (users == null)
                        {
                            ViewBag.step3 = 1;
                            ViewBag.step1 = 1;

                        }
                        else
                        {
                            ViewBag.check = "Consultant Already Taged to this Requirement";
                            ViewBag.step1 = 1;
                        }
                    }

                }
            
            }
            List<SelectListItem> nationality = v5.HCM_NATIONALITY.Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();

            ViewBag.nationality = nationality;

            List<SelectListItem> country = v5.HC_CLIENT.Where(x => x.CountryID == 241).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();

            var countrytip = v5.HC_CLIENT.Where(x => x.CountryID == 244).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();
            country.InsertRange(0, countrytip);


            @ViewBag.Client = model.Clients;
            @ViewBag.req = model.req;
            @ViewBag.lob = model.lob;
            @ViewBag.Status = model.status1;
            @ViewBag.br = model.br;
            @ViewBag.pr = model.pr;
            @ViewBag.jobcode = model.jobcode1;
            @ViewBag.jd = model.jd;
            title(model.client,model.Requirement);
            loc(model.Requirement,model.location);
            ViewBag.alert = "";
            ViewBag.clients = country;
        
            ViewBag.step2 = 1;
            return View("NewBlock",model);
        }
        public FileResult DownloadFile(string fileName)
        { 
         cache();
            try
            {
                var filepath = Path.Combine(Server.MapPath("~/uploadimage/"), fileName);
                return File(filepath, GetMimeType(fileName), fileName);
            }
            catch
            {
                return null;
            }
        }
        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        //upload file
        public ActionResult uploadfile()
        {
            if (Session["uiid"] == null)
                return RedirectToAction("Login", "Account");

            

            cache();
            HttpFileCollectionBase files = Request.Files;
            try
            {
                string path = string.Empty;
                string _FileName = string.Empty;
                string actualfilename = string.Empty;

                if (ModelState.IsValid)
                {
                    for (var i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                         _FileName = Path.GetFileName(file.FileName);
                        var date = DateTime.Now.ToString("yyyyMMddhhmm");
                        actualfilename = date + _FileName;
                        //string[] datesplit = date.Split('/');
                        //string dateappend = string.Empty;
                        //foreach(string item in datesplit)
                        //{
                        //    dateappend += "" + item + "";
                        //}

                        using (IHD_DBEntities12 db = new IHD_DBEntities12())
                        {
                             path = "~/uploadimage/" + date + file.FileName;

                            file.SaveAs(Server.MapPath(path));
                        }
                    }
                }
                ViewBag.Message = "File Uploaded Successfully!!";
                IEnumerable<SelectListItem> country = v5.HC_CLIENT.Select(c => new SelectListItem
                {
                    Value = c.RID.ToString(),

                    Text = c.Title

                }).Distinct();

                ViewBag.clients = country;

                return Json(new { ImagePath = path,ImageName= actualfilename });

            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                IEnumerable<SelectListItem> country = v5.HC_CLIENT.Select(c => new SelectListItem
                {
                    Value = c.RID.ToString(),

                    Text = c.Title

                }).Distinct();

                ViewBag.clients = country;
                return Json("File upload failed!!!!!", JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult NewBlock1(CL_CLOSING_BLOCK model, HttpPostedFileBase file)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long uid = (long)Session["uiid"];
            CL_APPROVAL_LOG model1 = new CL_APPROVAL_LOG();
            if (model.RID != 0)
            {

                var updated = db.CL_CLOSING_BLOCK.Where(x => x.RID == model.RID).FirstOrDefault();
                updated.ANumber = model.ANumber;
                updated.AvailabilityStatus = model.AvailabilityStatus;
                updated.AvailJoinDate = model.AvailJoinDate;
                updated.BGVStatus = model.BGVStatus;
                updated.Ccompany = model.Ccompany;
                updated.CLocation = model.CLocation;
                updated.ClosedPaye = model.ClosedPaye;
                updated.comments = model.comments;
                updated.ConProjEndDate = model.ConProjEndDate;
                updated.CreatedDate = DateTime.Now;
                updated.CreatedUser = uid;
                updated.CurrentSalary = model.CurrentSalary;
                updated.DistHoToWorkLoc = model.DistHoToWorkLoc;
                updated.DOB = model.DOB;
                updated.EmailId = model.EmailId;
                updated.EmpType = model.EmpType;
                updated.ExpectedSalary = model.ExpectedSalary;
                updated.Fname = model.Fname;
                updated.GPM = model.GPM;
                updated.GPPerHour = model.GPPerHour;
                updated.Insurance = model.Insurance;
                updated.interviewDetails = model.interviewDetails;
                updated.LoadedPayRate = model.LoadedPayRate;
                updated.LocPref = model.LocPref;
                updated.NationalityID = model.NationalityID;
                updated.NoticePeriod = model.NoticePeriod;
                updated.Offer = model.Offer;
                updated.OfferRecieved = model.OfferRecieved;
                updated.Payrate = model.Payrate;
                updated.PlannedHolidays = model.PlannedHolidays;
                updated.PNumber = model.PNumber;
                updated.PurposedBillRate = model.PurposedBillRate;
                updated.ReasonJobChange = model.ReasonJobChange;
                updated.Reference1 = model.Reference1;
                updated.Reference2 = model.Reference2;
                updated.ResonForLeaving = model.ResonForLeaving;
                updated.RExp = model.RExp;
                updated.SameClient = model.SameClient;
                updated.TExp = model.TExp;
                updated.VisaValidDate = model.VisaValidDate;
                updated.WorkLoc = model.WorkLoc;
                updated.TeleInterview = model.TeleInterview;
                updated.FaceInterview = model.FaceInterview;
                updated.Holidays = model.Holidays;
                updated.FInterview = model.FInterview;
                var data = updated.Imagepath;
                updated.Imagepath = model.Imagepath + "," + data;
                updated.WorkVisaStatus = model.WorkVisaStatus;
                updated.Status = 1;
                ViewBag.alert = "Data Updated sucessfully";

                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();

            }
            else
            {
                var image = model.Imagepath;
                var CLI = model.ClientID;
                var REQ = model.Reqid;
                var name = model.Fname;
                model.Status = 1;
                model.CreatedDate = DateTime.Now;
                model.CreatedUser = uid;
                model1.Status = "Pending";
                db.CL_CLOSING_BLOCK.Add(model);
                ViewBag.alert = "Data Saved Successfully";
                db.Configuration.ValidateOnSaveEnabled = false;
                db.SaveChanges();
            }
                try
                {
                    var client = v5.HC_CLIENT.Where(x => x.RID == model.ClientID).Select(x => x.Title).FirstOrDefault();
                    var reqtitle = v5.HC_REQUISITIONS.Where(x => x.RID == model.Reqid).Select(x => x.ReqTitle).FirstOrDefault();
                    var userid = (long)Session["uiid"];
                    var fromadrress = Session["uid"].ToString()+"@collabera.com";
                    var message = new MailMessage();
                    message.From = new MailAddress(fromadrress);  // replace with valid value
                    message.Subject = "NEW BLOCK E-MAIL";
                    message.Body = "<p><span style='color: rgb(0, 0, 0); '>Hi Team,</span></p><p><span style='color: rgb(0, 0, 0); '><br></span></p><p><span style='color: rgb(0, 0, 0); '>Below Closing Block is Submitted as follows:</span></p><table style=' border: 1px solid black; width: 100 % '><tbody><tr><th style=' border: 1px solid black; text - align:center'>Submitted By</th><th style=' border: 1px solid black; text - align:center'>Created Date</th><th style='border: 1px solid black; text - align:center'>Client Name</th><th style=' border: 1px solid black; text - align:center'>Req Number/Job Code</th><th style=' border: 1px solid black; text - align:center'>Status</th></tr><tr style='background - color: #eee;'><td style=' border: 1px solid black;text-align:center'><br>"+ Session["uid"].ToString().Replace("."," ")+ "</td><td style='border: 1px solid black;text-align:center'>"+DateTime.Now+"</td><td style=' border: 1px solid black;text-align:center'>"+client+"</td><td style=' border: 1px solid black;text-align:center'>"+reqtitle+"</td><td style=' border: 1px solid black;text-align:center'>NEW Submitted</td></tr> </tbody></table> <p class='MsoNormal'><span style='color: rgb(0, 255, 255);'><br></span></p><p class='MsoNormal'><span style='color: rgb(57, 132, 198);'>Thanks &amp; Regards,</span></p><p class='MsoNormal'><span style='color: rgb(57, 132, 198);'><a href='https://www.collabera.com' target='_blank'>COLLABERA</a></span></p><br><br><p></p>";
                    message.IsBodyHtml = true;
                    var smtp = new SmtpClient
                    {
                        Host = "mailbrd.collabera.com",
                        Port = 25,
                        EnableSsl = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,

                    };
                   
                    var tolist = db.cl_mapping.Where(x=>x.RecID==userid).Select(x=>x.ManagerID).ToList();
                    foreach (var item in tolist)
                    {
                        var data = db.CL_USERS.Where(x=>x.RID==item).Select(x=>x.UserName).FirstOrDefault();
                        message.To.Add(data+"@collabera.com");

                    }
                    
                    message.To.Add("shreya.bhattacharya@collabera.com");
                    //message.CC.Add("@collabera.com");
                    //message.CC.Add("@collabera.com");

                    smtp.Send(message);

                    //Sending Email
                    //smtpclient.Send(mail);
                    Response.Write("<B>Email Has been sent successfully.</B>");
                }
                catch (Exception ex)
                {
                    //Catch if any exception occurs
                    Response.Write(ex.Message);
                }
         
        
            //db.CL_APPROVAL_LOG.Add(model1);
            IEnumerable<SelectListItem> country = v5.HC_CLIENT.Where(x => x.CountryID == 241 && x.CountryID == 244).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct();

            ViewBag.clients = country;
            title(model.client, model.Requirement);
            loc(model.Requirement, model.location);
            ViewBag.check = "";
            TempData["alert"] = ViewBag.alert;
            return RedirectToAction("NewBlock");

        }

        //json new block
        public JsonResult title(long id,long reqid=0)
        {
            if (id == 0)
            {
                List<SelectListItem> Requirement = new List<SelectListItem>();

                Requirement.Add(new SelectListItem
                {
                    Value = 0.ToString(),

                    Text = "Please Select A Client"

                });
                ViewBag.Requirements = Requirement;

                return Json(new SelectList(Requirement, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            else
            {
                List<SelectListItem> Requirement = v5.HC_REQUISITIONS.Where(x => x.Client == id).Select(c => new SelectListItem
                {
                    Value = c.RID.ToString()
          ,
                    Text = c.ReqTitle,
                    Selected = c.RID == reqid ? true : false

                }).Distinct().ToList();
                ViewBag.Requirements = Requirement;

                return Json(new SelectList(Requirement, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
           
        }
        public JsonResult loc(long id,long lobid=0)
        {
            if (id==0)
            {
                List<SelectListItem> location = new List<SelectListItem>();

                location.Add(new SelectListItem
                {
                    Value = 0.ToString(),

                    Text = "Please Select A Client"

                });
                ViewBag.locations = location;

                return Json(new SelectList(location, "Value", "Text", JsonRequestBehavior.AllowGet));
            }
            else
            {
                List<SelectListItem> location = v5.HCM_LOCATIONS.Join(v5.HC_REQUISITION_MAPPING.Where(x => x.ReqID == id),
                  d => d.RID,
                  f => f.MappingID,

                  (d, f) => d).Select(c => new SelectListItem
                  {
                      Value = c.RID.ToString()
     ,
                      Text = c.Title,
                      Selected = c.RID == lobid ? true : false

                  }).Distinct().ToList();
                ViewBag.locations = location;

                return Json(new SelectList(location, "Value", "Text", JsonRequestBehavior.AllowGet));
            }          
        }
        public ActionResult Pending(string name=null,string email=null)

        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long uid = (long)Session["uiid"];
            long rid = (long)Session["id"];
            ViewBag.hide = rid;
            if (name==null)
            {
                ViewBag.name = "";

            }
            else
            {
                ViewBag.name = name;
            }
            if (email == null)
            {
                ViewBag.email = "";

            }
            else
            {
                ViewBag.email = email;
            }
            
            ViewBag.VALUE = db.sp_cb_pending_block(uid, rid);
            return View();
        }
        public ActionResult Approved(string name = null, string email = null)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long uid = (long)Session["uiid"];
            long rid = (long)Session["id"];
            ViewBag.hide = rid;
            if (name == null)
            {
                ViewBag.name = "";

            }
            else
            {
                ViewBag.name = name;
            }
            if (email == null)
            {
                ViewBag.email = "";

            }
            else
            {
                ViewBag.email = email;
            }

            ViewBag.VALUE = db.sp_APPROVE_block(uid);
            return View();
        }
        public ActionResult rejected(string name=null, string email = null)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long uid = (long)Session["uiid"];
            long rid = (long)Session["id"];
            ViewBag.hide = rid;
            if (name == null)
            {
                ViewBag.name = "";

            }
            else
            {
                ViewBag.name = name;
            }
            if (email == null)
            {
                ViewBag.email = "";

            }
            else
            {
                ViewBag.email = email;
            }

            ViewBag.VALUE = db.sp_reject_block(uid);
            return View();
        }
        public ActionResult Delete(long clid)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long uid = (long)Session["uiid"];

            ViewBag.VALUE = db.sp_delete_block(uid, clid);
            return null;
        }
        public ActionResult Approve(long slectitem)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            //foreach(var item in slectitem)
            //{
            long uid = (long)Session["uiid"];
            var q = db.CL_CLOSING_BLOCK.Where(x => x.RID == slectitem).First();
            q.Status = 2;
            var user = q.CreatedUser;
            var w = db.sp_Approve(uid, slectitem);
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            try
            {
                var username = db.CL_USERS.Where(x => x.RID == user).Select(x => x.UserName).FirstOrDefault();
                var client = v5.HC_CLIENT.Where(x => x.RID == q.ClientID).Select(x => x.Title).FirstOrDefault();
                var reqtitle = v5.HC_REQUISITIONS.Where(x => x.RID == q.Reqid).Select(x => x.ReqTitle).FirstOrDefault();
                var fromadrress = Session["uid"].ToString() + "@collabera.com";
                var message = new MailMessage();
                message.From = new MailAddress(fromadrress);  // replace with valid value
                message.Subject = "Approve E-MAIL";
                message.Body = "<p><span style='color: rgb(0, 0, 0); '>Hi "+username.Replace("."," ")+",</span></p><p><span style='color: rgb(0, 0, 0); '><br></span></p><p>Below Mentioned Closing block is Approved</p><table style=' border: 1px solid black; width: 100 % '><tbody><tr><th style=' border: 1px solid black; text - align:center'>Submitted By</th><th style=' border: 1px solid black; text - align:center'>Create Date</th><th style=' border: 1px solid black; text - align:center'>Client Name</th><th style=' border: 1px solid black; text - align:center'>Req Number/Job Code</th><th style=' border: 1px solid black; text - align:center'>Status</th></tr><tr style='background - color: #eee;'><td style=' border: 1px solid black;text-align:center'><br>"+ username.Replace(".", " ") + "</td><td style=' border: 1px solid black;text-align:center'>"+q.CreatedDate+"</td><td style=' border: 1px solid black;text-align:center'>"+client+"</td><td style=' border: 1px solid black;text-align:center'>"+reqtitle+"</td><td style=' border: 1px solid black;text-align:center'>Approved</td></tr> </tbody></table> <p class='MsoNormal'><span style='color: rgb(0, 255, 255);'><br></span></p><p class='MsoNormal'><span style='color: rgb(57, 132, 198);'>Thanks &amp; Regards,</span></p><p class='MsoNormal'><span style='color: rgb(57, 132, 198);'><a href='https://www.collabera.com' target='_blank'>COLLABERA</a></span></p><br><br><p></p>";
                message.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Host = "mailbrd.collabera.com",
                    Port = 25,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                };

                //var userid = (long)Session["uiid"];
                var tolist = db.cl_mapping.Where(x => x.RecID == user).Select(x => x.ManagerID).ToList();
            
                foreach (var item in tolist)
                {
                    var data = db.CL_USERS.Where(x => x.RID == item).Select(x => x.UserName).FirstOrDefault();
                    message.CC.Add(data + "@collabera.com");

                }
                message.To.Add(username+ "@collabera.com");
            
                smtp.Send(message);

                //Sending Email
                //smtpclient.Send(mail);
                Response.Write("<B>Email Has been sent successfully.</B>");
            }
            catch (Exception ex)
            {
                //Catch if any exception occurs
                Response.Write(ex.Message);
            }

            return null;
        }
        public ActionResult Reject(long clid,string text)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            cache();
            long uid = (long)Session["uiid"];

            var q = db.CL_CLOSING_BLOCK.Where(x => x.RID == clid).First();
            q.Status = 3;
            var user = q.CreatedUser;
            var w = db.sp_Reject_Test(uid, clid, text) ;
            db.Configuration.ValidateOnSaveEnabled = false;
            db.SaveChanges();
            try
            {
                var username = db.CL_USERS.Where(x => x.RID == user).Select(x => x.UserName).FirstOrDefault();
                var client = v5.HC_CLIENT.Where(x => x.RID == q.ClientID).Select(x => x.Title).FirstOrDefault();
                var reqtitle = v5.HC_REQUISITIONS.Where(x => x.RID == q.Reqid).Select(x => x.ReqTitle).FirstOrDefault();
                var fromadrress = Session["uid"].ToString() + "@collabera.com";
                var message = new MailMessage();
                message.From = new MailAddress(fromadrress);  // replace with valid value
                message.Subject = "Reject  E-MAIL";
                message.Body = "<p><span style='color: rgb(0, 0, 0); '>Hi "+username.Replace("."," ")+",</span></p><p><span style='color: rgb(0, 0, 0); '><br></span></p><p>Below Submitted Closing block is Rejected</p><table style=' border: 1px solid black; width: 100 % '><tbody><tr><th style=' border: 1px solid black; text - align:center'>Submitted By</th><th style=' border: 1px solid black; text - align:center'>Created Date</th><th style=' border: 1px solid black; text - align:center'>Client Name</th><th style=' border: 1px solid black; text - align:center'>Req Number/Job Code</th><th style=' border: 1px solid black; text - align:center'>Status</th><th style=' border: 1px solid black; text - align:center'>Rejected Reason</th></tr><tr style='background - color: #eee;'><td style=' border: 1px solid black;text-align:center'><br>"+username.Replace("."," ")+"</td><td style=' border: 1px solid black;text-align:center'>"+q.CreatedDate+"</td><td style=' border: 1px solid black;text-align:center'>"+client+"</td><td style=' border: 1px solid black;text-align:center'>"+reqtitle+"</td><td style=' border: 1px solid black;text-align:center'>Rejected</td><td style=' border: 1px solid black;text-align:center'>"+text+"</td></tr> </tbody></table> <p class='MsoNormal'><span style='color: rgb(0, 255, 255);'><br></span></p><p class='MsoNormal'><span style='color: rgb(57, 132, 198);'>Thanks &amp; Regards,</span></p><h2><span style='color: rgb(57, 132, 198); font-family: Arial;'>COLLABERA</span></h2><p class='MsoNormal'><br></p><br><br><p></p>";
                message.IsBodyHtml = true;
                var smtp = new SmtpClient
                {
                    Host = "mailbrd.collabera.com",
                    Port = 25,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,

                };

                //var userid = (long)Session["uiid"];
                var tolist = db.cl_mapping.Where(x => x.RecID == user).Select(x => x.ManagerID).ToList();
               
                foreach (var item in tolist)
                {
                    var data = db.CL_USERS.Where(x => x.RID == item).Select(x => x.UserName).FirstOrDefault();
                    message.CC.Add(data + "@collabera.com");

                }
                message.To.Add(username + "@collabera.com");

                smtp.Send(message);

                //Sending Email
                //smtpclient.Send(mail);
                Response.Write("<B>Email Has been sent successfully.</B>");
            }
            catch (Exception ex)
            {
                //Catch if any exception occurs
                Response.Write(ex.Message);
            }


            return null;
        }
       
        public ActionResult Details(long clid)
        {
            if (Session["uiid"] == null)
            {

                return RedirectToAction("Login", "Account");

            }

            ViewBag.Details = db.IHD_cl_details(clid);




            return View();
        }
        public JsonResult client()
        {
            List<SelectListItem> country = v5.HC_CLIENT.Where(x => x.CountryID == 241).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();

            var countrytip = v5.HC_CLIENT.Where(x => x.CountryID == 244).Select(c => new SelectListItem
            {
                Value = c.RID.ToString(),

                Text = c.Title

            }).Distinct().ToList();
            country.InsertRange(0, countrytip);

            return Json(country, JsonRequestBehavior.AllowGet);
        }
        public JsonResult search()
        {
            List<SelectListItem> liclient = db.CL_CLOSING_BLOCK.Select(c => new SelectListItem
            {
                Value = c.Fname
      ,
                Text = c.Fname


            }).Distinct().ToList();
            var countrytip = db.CL_CLOSING_BLOCK.Select(c => new SelectListItem
            {
                Value = c.EmailId
       ,
                Text = c.EmailId


            }).Distinct().ToList();
          
            liclient.InsertRange(0, countrytip);
           


            return Json(liclient, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getstatus(string id)
        {
            ViewBag.status = db.sp_cl_search_details(id);
            return View();
        }
        public ActionResult logout()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.AppendCacheExtension("no-store, must-revalidate");
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

    }
}