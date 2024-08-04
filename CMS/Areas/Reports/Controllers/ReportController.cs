using ReportingServiceClientLib;
using CMS.Areas.FrameWork.Controllers;
using CMS.ReportingWebService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CMS.Areas.Reports.Models;
using Domain.Akhbar.Contexts;
using Domain.Akhbar.DBBusiness;
using Domain.Akhbar.DBEntities;
using System.Data.SqlClient;
using System.Security;
using System.ServiceModel.Description;

namespace CMS.Areas.Reports.Controllers
{
    public class ReportController : BaseController
    {
               // GET: NewsViewsReport
        public ActionResult NewsViewsReport(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = "تقرير المشاهدات";
            
            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");

            vm.ReportPath = "/NewsReports/NewsViewsRpt";
            vm.ReportName = "NewsViewsRpt";
            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);
        }

        public ActionResult ProductionReport(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = "تقرير الانتاج (نيوز - ديسك) ";

            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");
          
            vm.ReportPath = "/NewsReports/ProductionRpt";
            vm.ReportName = "ProductionRpt";

            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);
        }

        public ActionResult DeskProductionRpt(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = "تقرير أنتاج الديسك";

            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");

            vm.ReportPath = "/NewsReports/DeskProductionRpt";
            vm.ReportName = "DeskProductionRpt";

            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);
        }


        public ActionResult ProductionUsingByLineRpt(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = " تقرير الانتاج الصحفي ";

            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");

            vm.ReportPath = "/NewsReports/ProductionUsingByLineRpt";
            vm.ReportName = "ProductionUsingByLineRpt";

            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);
        }


        public ActionResult PublishedReport(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = "تقرير اعداد الأخبار المنشورة (مرتب بالقسم) ";

            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");

            vm.ReportPath = "/NewsReports/PublishedRpt";
            vm.ReportName = "PublishedRpt";
            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);

        }

        public ActionResult NonPublishedReport(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = "تقرير اعداد الاخبار الغير معالجة للاقسام";

            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");
           
            vm.ReportPath = "/NewsReports/NonPublishedRpt";
            vm.ReportName = "NonPublishedRpt";
            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);

        }

        public ActionResult SEORpt(ReportsVM vm = null)
        {
            Session["CurrentModule"] = "التقارير";
            Session["CurrentService"] = "تقرير متابعة العناوين والكلمات الدالة";

            FillUsersDDLForSearch(vm);
            ViewBag.UsersID = new SelectList(vm.userLst, "UserID", "FullName");

            FillSectionsDDLForSearch(vm);
            ViewBag.SecID = new SelectList(vm.secLst, "SectionID", "SecTitle");

            vm.ReportPath = "/NewsReports/SEORpt";
            vm.ReportName = "SEORpt";
            return View("~/Areas/Reports/Views/Report/_ReportsViewer.cshtml", vm);

        }

        #region private Methods


        public ActionResult SaveReport(ExportFormat outPutFormat, ReportsVM vm)
        {
            IList<ParameterValue> parameters = new List<ParameterValue>();
            parameters.Add(new ParameterValue { Name = "DateFrom", Value = vm.StartDate.ToString("MM/dd/yyyy") });
            parameters.Add(new ParameterValue { Name = "DateTo", Value = vm.EndDate.ToString("MM/dd/yyyy") });

            if (vm.ReportName == "DeskProductionRpt" ||
                vm.ReportName == "ProductionUsingByLineRpt" )
            {
                parameters.Add(new ParameterValue { Name = "UserID", Value = vm.UserId.ToString() });
            }

            else if (vm.ReportName == "NewsViewsRpt" ||
                vm.ReportName == "SEORpt")
            {
                parameters.Add(new ParameterValue { Name = "SectionID", Value = vm.SectionId.ToString() });
            }

            else
            {
                parameters.Add(new ParameterValue { Name = "SectionID", Value = vm.SectionId.ToString() });
                parameters.Add(new ParameterValue { Name = "UserID", Value = vm.UserId.ToString() });
            }

            byte[] output;
            string extension, mimeType, encoding;
            Warning[] warnings;
            string[] Streamids;

            string DateTimePattern = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            vm.ReportName = vm.ReportName + DateTimePattern;

            ReportExporter.Export(
                "ReportExecutionServiceSoap" /* name of the WCF endpoint from Web.config */,
                new NetworkCredential("report_service", "r1nZ4#`4!16=Vlh"),     //("ttt","123")
                vm.ReportPath,
                parameters.ToArray(),
                outPutFormat,
                out output,
                out extension,
                out mimeType,
                out encoding,
                out warnings,
                out Streamids
            );

            //-------------------------------------------------------------
            // Set HTTP Response Header to show download dialog popup
            //------------------------------------------------------------ -
            string.Format("attachment;filename={0}.{1}", vm.ReportName, extension);
            System.Web.HttpContext.Current.Response.AddHeader("content-disposition", string.Format("inline;filename={0}.{1}", vm.ReportName, extension));
            return new FileContentResult(output, mimeType);
        }

        
        public ActionResult GetReport(string submitButton, ReportsVM vm)
        {
            
            switch (submitButton)
            {
                case "Word":
                    // delegate sending to another controller action
                    return (SaveReport(ExportFormat.Word, vm));
                case "Excel":
                    // call another action to perform the cancellation
                    return (SaveReport(ExportFormat.Excel, vm));
                case "Image":
                    // call another action to perform the cancellation
                    return (SaveReport(ExportFormat.Image, vm));
                case "CSV":
                    // delegate sending to another controller action
                    return (SaveReport(ExportFormat.CSV,vm));
                default:
                    // If they've submitted the form without a submitButton, 
                    // just return the view again.
                    return (SaveReport(ExportFormat.PDF,vm));
            }
        }


        private ReportsVM FillUsersDDLForSearch(ReportsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            AdminUserBusiness adminUB = new AdminUserBusiness(this.DbContext);
            vm.userLst = adminUB.Load(d => d.Type != 1 && d.Active.Value == true, null);
            return vm;
        }

        private ReportsVM FillSectionsDDLForSearch(ReportsVM vm)
        {
            this.DbContext = new AkhbarDBContext("AkhbarDBConnection");
            MainSectionBusiness secB = new MainSectionBusiness(this.DbContext);
            vm.secLst = secB.Load(d => d.Hide == 0 , null);
            return vm;
        }

        #endregion

    }
}