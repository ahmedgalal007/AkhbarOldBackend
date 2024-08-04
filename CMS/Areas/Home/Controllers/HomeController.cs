using Domain.Akhbar.DBBusiness;
using Domain.Akhbar.Contexts;
using Domain.Akhbar.DBEntities;
using CMS.Areas.FrameWork.Controllers;
using CMS.Areas.UserManagment.Models;
using SimpleHashing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace CMS.Areas.Home.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Home
        public ActionResult Index()
        {

            Session["CurrentModule"] = "الرئيسية";
            Session["CurrentService"] = "";//"تقارير عامة";

            //FormsAuthentication.SetAuthCookie("هبة موسى - الويب", false);
            //Session["UserID"] = 104;
            //Session["UserFullName"] = "هبة موسى - الويب";
            //Session["UserPhoto"] = "";

            ViewBag.UserName = Session["UserFullName"].ToString();

            // if (Session["DeskUser"] != null)
            if (TempData["Action"] != null)
            {
                // if (Session["DeskUser"].ToString() == "JoinDesk")
                if (TempData["Action"].ToString() == "JoinDesk")
                {
                    ViewBag.DeskMessage = "لقد قمت بالانضمام لشيفت الديسك";
                }
                // else if (Session["DeskUser"].ToString() == "LeftDesk")
                else if (TempData["Action"].ToString() == "LeftDesk")
                {
                    ViewBag.DeskMessage = "لقد قمت بمغادرة شيفت الديسك";
                }
            }

            return View();
        }


        [HttpPost]
        public JsonResult KeepSessionAlive()
        {
            return new JsonResult { Data = "Success" };
        }

        //[HttpPost]
        //public JsonResult UserInDesk()
        //{
        //    AkhbarDBContext db = new AkhbarDBContext(new GeneralHellper().AkhbarDbConnection);
        //    TempDeskShiftBusiness tDSB = new TempDeskShiftBusiness(db);
        //    List<TempDeskShift> dsLst = tDSB.Load(null, null);
        //    return new JsonResult { Data = "Success" };
        //}

       
        public ActionResult UserInDesk()
        {
            AkhbarDBContext db = new AkhbarDBContext(new GeneralHellper().AkhbarDbConnection);
            TempDeskShiftBusiness auB = new TempDeskShiftBusiness(db);
            List<TempDeskShift> dsLst = auB.Load(null, null);
            return PartialView("~/Areas/Home/Views/Home/_UserInDesk.cshtml", dsLst);
        }

        public ActionResult Login()
        {
            return View(new AdminUser());
        }

        public ActionResult NewsQuickReport()
        {
            AkhbarDBContext db = new AkhbarDBContext(new GeneralHellper().AkhbarDbConnection);
            var sp = new SP_NewsQuickReport();
            QuickNewsRptData NewsCount = db.Database.ExecuteStoredProcedure(sp).ToList().FirstOrDefault();
            return PartialView("~/Areas/Home/Views/Home/_NewsQuickReport.cshtml", NewsCount);
        }

        [HttpPost]
        public ActionResult Login([Bind(Include = "UserName,Password")] AdminUser user = null)
        {
            AkhbarDBContext db = new AkhbarDBContext(new GeneralHellper().AkhbarDbConnection);
            AdminUserBusiness userB = new AdminUserBusiness(db);

            // Lets first check if the Model is valid or not
            if (ModelState.IsValid)
            {
                // Hash
                SimpleHash hash = new SimpleHash();
               // var currentPass = hash.Compute(user.Password);

                Expression<Func<AdminUser, bool>> filter = e => e.UserName == user.UserName && e.Active == true;
                //Expression<Func<AdminUser, bool>> filter = e => e.UserName == user.UserName && e.Password == user.Password && e.Active == true;
                List<AdminUser> userlst = userB.Load(filter, null);
                if (userlst.Count > 0)
                {
                    foreach (var _user in userlst)
                    {
                        // Verify
                        var result = hash.Verify(user.Password, _user.Password);
                        if (result)
                        {
                            FormsAuthentication.SetAuthCookie(_user.UserName, false);
                            Session["UserID"] = _user.UserID;
                            Session["UserFullName"] = _user.FullName;
                            Session["UserPhoto"] = ConfigurationManager.AppSettings["reltativeUsersImagePath"].ToString() + _user.UserPhoto;
                            //Session["DeskUser"] = "0";

                            _user.LastLive = DateTime.Now;

                            userB.Edit(_user);
                            db.SaveChanges();

                            SaveLoginLog(true);

                            return RedirectToAction("Index", "Home", new { area = "Home" });
                        }
                    }
                }

                ModelState.AddModelError("", "خطأ فى اسم المستخدم او كلمة المرور");
            }
            return View(user);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            LeaveDesk();

            FormsAuthentication.SignOut();
            Session.Abandon();

            SaveLoginLog(false);

            return RedirectToAction("Login", "Home", new { area = "Home" });
        }

        public string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        protected string GetIPAddress2()
        {
            string IPAddress = string.Empty;
            IPAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IPAddress))
            {
                IPAddress = Request.ServerVariables["REMOTE_ADDR"];
            }
            return IPAddress;
        }


        //Should move to base controller
        private void SaveLoginLog(bool LoginFlag)
        {
            try
            {
                string macAddress = GetMACAddress();
                string userMName = HttpContext.User.Identity.Name;
                string ipAddress = Request.UserHostAddress;
                string ipAddress2 = GetIPAddress2();
                string osType = Request.UserAgent;
                string message = "User Machine Name:" + userMName + " - UserIP:" + ipAddress + " - User operating system:" + osType + " - RemoteIP:" + ipAddress2 + " - Mac/Physical-Address:" + macAddress + " - UserId:" + Session["UserID"].ToString() + "  - Name:" + Session["UserFullName"].ToString() + " //  ";
              //  string message = "IP:" + ipAddress + " - UserId:" + Session["UserID"].ToString() + "  - Name:" + Session["UserFullName"].ToString() + " //  ";
                if (LoginFlag)
                    message += "Has Logged in at: ";
                else
                    message += "Has Logged Off at: ";

                message += DateTime.Now.ToLongTimeString();

                string fn = Server.MapPath("~/LogLoginAndOut/") + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + ".txt";
                if (!System.IO.File.Exists(fn))
                {
                    FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(message);
                    sw.WriteLine("----------------------------------------------------------");
                    sw.Close();
                    fs.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(fn, true);
                    sw.WriteLine(message);
                    sw.WriteLine("----------------------------------------------------------");
                    sw.Close();

                }
            }
            catch 
            { }
        }

        #region Repeated Function on Base and AdminUser Controller
        private void LeaveDesk()  //Exists on AdminUser
        {
            if (Session["UserID"] != null)
            {
                int userid = int.Parse(Session["UserID"].ToString());
                AkhbarDBContext db = new AkhbarDBContext(new GeneralHellper().AkhbarDbConnection);
                TempDeskShiftBusiness tempDeskBuss = new TempDeskShiftBusiness(db);
                TempDeskShiftVM vm = new TempDeskShiftVM();
                vm.tempDeskShift = tempDeskBuss.Load(userid);
                if (vm.tempDeskShift != null)
                {
                    vm.lst = tempDeskBuss.Load(null, null).OrderBy(e => e.OrderID).ToList();

                    for (int i = 0; i < vm.lst.Count; i++)
                    {
                        if (vm.tempDeskShift.UserId == vm.lst[i].UserId)
                        {
                            if (vm.lst[i] == vm.lst.Last())
                            {
                                vm.lst[0].WaitingFlag = true;
                                tempDeskBuss.Edit(vm.lst[0]);
                            }
                            else
                            {
                                vm.lst[i + 1].WaitingFlag = true;
                                tempDeskBuss.Edit(vm.lst[i + 1]);
                            }

                            db.SaveChanges();
                        }
                    }

                    tempDeskBuss.Delete(vm.tempDeskShift);
                    db.SaveChanges(); // should use this.savechanges for save log //causes log not writing

                    SaveLeftDeskLog();
                }
            }
        }

        private void SaveLeftDeskLog()  //Exists on Base
        {
            try
            {
                string macAddress = GetMACAddress();
                string userMName = HttpContext.User.Identity.Name;
                string ipAddress = Request.UserHostAddress;
                string ipAddress2 = GetIPAddress2();
                string osType = Request.UserAgent;
                string message = "User Machine Name:" + userMName + " - UserIP:" + ipAddress + " - User operating system:" + osType + " - RemoteIP:" + ipAddress2 + " - Mac/Physical-Address:" + macAddress + " - UserId:" + Session["UserID"].ToString() + "  - Name:" + Session["UserFullName"].ToString() + " //  ";

                // string macAddress = GetMACAddress();
                //string ipAddress = Request.UserHostAddress;
                //string message = "IP:" + ipAddress + " - UserId:" + Session["UserID"].ToString() + "  - Name:" + Session["UserFullName"].ToString() + " //  ";
                message += "Has Left Desk Shift at: ";

                message += DateTime.Now.ToLongTimeString();

                string fn = Server.MapPath("~/LogDeskShifts/") + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + ".txt";
                if (!System.IO.File.Exists(fn))
                {
                    FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(message);
                    sw.WriteLine("----------------------------------------------------------");
                    sw.Close();
                    fs.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(fn, true);
                    sw.WriteLine(message);
                    sw.WriteLine("----------------------------------------------------------");
                    sw.Close();

                }
            }
            catch
            { }
        }
        #endregion

    }
}