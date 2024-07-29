using Domain.Akhbar.DBContext;
using CMS.Areas.FrameWork.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;



namespace CMS.Areas.FrameWork.Controllers
{
     
    [SystemAuthorization]  
    [CommonViewBagsActionFilter]
    [HandleError]
    //[AuthorizeDomain]
    public class BaseController : Controller
    {
        public enum EditorType { Editor = 1, Photographer = 2, Infographer = 3, Videographer = 4 }

        private List<DbContext> lstDisposableContexts = new List<DbContext>();

        public BaseController()
        {

        }

        #region MyVariables

        private DbContext _DbContext;
        public DbContext DbContext
        {
            get { return _DbContext; }
            set
            {
                _DbContext = value;
                lstDisposableContexts.Add(_DbContext);
            }
        }
      
        public int? LoginUserID
        {
            get { return Session["UserID"] != null ? (int?)Convert.ToInt32(Session["UserID"]) : null; }
        }

        public string LoginUserFullName
        {
            get { return Session["UserFullName"].ToString() ; }
        }

        int _pageSize = 10;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        int _NewsLimit = 200;
        public int NewsLimit
        {
            get { return _NewsLimit; }
            set { _NewsLimit = value; }
        }


        #endregion

        #region NewsImgData
        int _newsImgSocialWidth = 800;
        public int NewsImgSocialWidth
        {
            get { return _newsImgSocialWidth; }
            set { _newsImgSocialWidth = value; }
        }
        int _newsImgSocialHeight = 400;
        public int NewsImgSocialHeight
        {
            get { return _newsImgSocialHeight; }
            set { _newsImgSocialHeight = value; }
        }


        int _newsImgLargeWidth = 600;
        public int NewsImgLargeWidth
        {
            get { return _newsImgLargeWidth; }
            set { _newsImgLargeWidth = value; }
        }
        int _newsImgLargeHeight = 400;
        public int NewsImgLargeHeight
        {
            get { return _newsImgLargeHeight; }
            set { _newsImgLargeHeight = value; }
        }


        int _newsImgMediumWidth = 400;
        public int NewsImgMediumWidth
        {
            get { return _newsImgMediumWidth; }
            set { _newsImgMediumWidth = value; }
        }
        int _newsImgMediumHeight = 300;
        public int NewsImgMediumHeight
        {
            get { return _newsImgMediumHeight; }
            set { _newsImgMediumHeight = value; }
        }


        int _newsImgSmallWidth = 200;
        public int NewsImgSmallWidth
        {
            get { return _newsImgSmallWidth; }
            set { _newsImgSmallWidth = value; }
        }
        int _newsImgSmallHeight = 170;
        public int NewsImgSmallHeight
        {
            get { return _newsImgSmallHeight; }
            set { _newsImgSmallHeight = value; }
        }


        string _OrImgPath = ConfigurationManager.AppSettings["OriginalImagesPath"].ToString();//"/images/Original/";
        public string OrImgPath
        {
            get { return _OrImgPath; }
            set { _OrImgPath = value; }
        }

        string _relativeOrImgPath = ConfigurationManager.AppSettings["reltativeOriginalImagesPath"].ToString();//"/images/Original/";
        public string RelativeOrImgPath
        {
            get { return _relativeOrImgPath; }
            set { _relativeOrImgPath = value; }
        }



        string _largeImgPath = ConfigurationManager.AppSettings["LargeImagesPath"].ToString();//"/images/Large/";
        public string LargeImgPath
        {
            get { return _largeImgPath; }
            set { _largeImgPath = value; }
        }

        string _relativeLargeImgPath = ConfigurationManager.AppSettings["reltativeLargeImagesPath"].ToString();//"/images/Large/";
        public string RelativeLargeImgPath
        {
            get { return _relativeLargeImgPath; }
            set { _relativeLargeImgPath = value; }
        }

        string _mediumImgPath = ConfigurationManager.AppSettings["MediumImagesPath"].ToString();//"/images/Medium/";
        public string MediumImgPath
        {
            get { return _mediumImgPath; }
            set { _mediumImgPath = value; }
        }

        string _relativeMediumImgPath = ConfigurationManager.AppSettings["reltativeMediumImagesPath"].ToString();//"/images/Large/";
        public string RelativeMediumImgPath
        {
            get { return _relativeMediumImgPath; }
            set { _relativeMediumImgPath = value; }
        }

        string _smallImgPath = ConfigurationManager.AppSettings["SmallImagesPath"].ToString();//"/images/small/";
        public string SmallImgPath
        {
            get { return _smallImgPath; }
            set { _smallImgPath = value; }
        }

        string _relativeSmallImgPath = ConfigurationManager.AppSettings["reltativeSmallImagesPath"].ToString();//"/images/Large/";
        public string RelativeSmallImgPath
        {
            get { return _relativeSmallImgPath; }
            set { _relativeSmallImgPath = value; }
        }

        string _socialImgPath = ConfigurationManager.AppSettings["SocialImagesPath"].ToString();//"/images/Social/";
        public string SocialImgPath
        {
            get { return _socialImgPath; }
            set { _socialImgPath = value; }
        }

        string _relativeSocialImgPath = ConfigurationManager.AppSettings["reltativeSocialImagesPath"].ToString();//"/images/Large/";
        public string RelativeSocialImgPath
        {
            get { return _relativeSocialImgPath; }
            set { _relativeSocialImgPath = value; }
        }

        string _galleryImgPath = ConfigurationManager.AppSettings["GalleryImagePath"].ToString();
        public string GalleryImgPath
        {
            get { return _galleryImgPath; }
            set { _galleryImgPath = value; }
        }

        string _relativeGalleryImgPath = ConfigurationManager.AppSettings["reltativeGalleryImagePath"].ToString();//"/images/Large/";
        public string RelativeGalleryImgPath
        {
            get { return _relativeGalleryImgPath; }
            set { _relativeGalleryImgPath = value; }
        }

        string _usersImgPath = ConfigurationManager.AppSettings["UsersImagesPath"].ToString();
        public string UsersImgPath
        {
            get { return _usersImgPath; }
            set { _usersImgPath = value; }
        }

        string _relativeUsersImgPath = ConfigurationManager.AppSettings["reltativeUsersImagePath"].ToString();//"/images/Large/";
        public string RelativeUsersImgPath
        {
            get { return _relativeUsersImgPath; }
            set { _relativeUsersImgPath = value; }
        }
        string _embededImagePath = ConfigurationManager.AppSettings["EmbededImagesPath"].ToString();//"/images/Social/";
        public string EmbededImagePath
        {
            get { return _embededImagePath; }
            set { _embededImagePath = value; }
        }
        string _reltativeEmbededImagesPath = ConfigurationManager.AppSettings["reltativeEmbededImagesPath"].ToString();//"/images/Large/";
        public string ReltativeEmbededImagesPath
        {
            get { return _reltativeEmbededImagesPath; }
            set { _reltativeEmbededImagesPath = value; }
        }

        string _profileImgPath = ConfigurationManager.AppSettings["ProfileImagePath"].ToString();//"/images/ProfilesImages/";
        public string ProfileImgPath
        {
            get { return _profileImgPath; }
            set { _profileImgPath = value; }
        }

        string _relativeProfileImgPath = ConfigurationManager.AppSettings["reltativeProfileImagePath"].ToString();//"/images/ProfilesImages/";
        public string RelativeProfileImgPath
        {
            get { return _relativeProfileImgPath; }
            set { _relativeProfileImgPath = value; }
        }

        string _waterMarkImagePath = ConfigurationManager.AppSettings["WaterMarkImagePath"].ToString();
        public string WaterMarkImagePath
        {
            get { return _waterMarkImagePath; }
            set { _waterMarkImagePath = value; }
        }


        #endregion

        #region GetMessage
        public alertMessageVM GetMessage(string messageText)
        {
            alertMessageVM alert = new alertMessageVM();
            alert.Message = "";

            if (messageText != null || messageText != "")
            {

                switch (messageText)
                {
                    case "Delete":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم الحذف بنجاح";
                            break;
                        }
                    case "Edit":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم التعديل بنجاح";
                            break;
                        }
                    case "Create":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تمت الاضافة بنجاح";
                            break;
                        }
                    case "Sorted":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم إعادة الترتيب بنجاح";
                            break;
                        }
                    case "Error":
                        {
                            alert.alertType = alertMessageVM.AlertType.Error;
                            alert.Message = "لم يستطيع البرنامج اتمام العمليه الرجاء اعادة المحاولة";
                            break;
                        }
                    case "NewsPass":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم تمرير الخبر بنجاح";
                            break;
                        }
                    case "Publish":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم نشر الخبر بنجاح";
                            break;
                        }
                    case "Restore":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم استعاده الخبر بنجاح";
                            break;
                        }
                    case "KeyWords":
                        {
                            alert.alertType = alertMessageVM.AlertType.Error;
                            alert.Message = "الرجاء التأكد من ادخال الكلمات الدالة";
                            break;
                        }
                    case "Images":
                        {
                            alert.alertType = alertMessageVM.AlertType.Error;
                            alert.Message = "الرجاء التأكد من نوع وحجم ملف الصورة";
                            break;
                        }
                    case "Rejected":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم رفض الخبر بنجاح";
                            break;
                        }
                    case "Waiting":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم تأجيل الخبر بنجاح";
                            break;
                        }
                    case "DeskAdded":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "تم حفظ التخصيص بنجاح";
                            break;
                        }
                    case "NotPermitted":
                         {
                            alert.alertType = alertMessageVM.AlertType.Error;
                            alert.Message = "ليس مسموحا لك بالتعديل في أخبار منشورة";
                            break;
                        }
                    case "JoinDesk":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "لقد قمت بالانضمام لشيفت الديسك";
                            break;
                        }
                    case "LeftDesk":
                        {
                            alert.alertType = alertMessageVM.AlertType.Success;
                            alert.Message = "لقد قمت بمغادرة شيفت الديسك";
                            break;
                        }
                    case "Exists":
                        {
                            alert.alertType = alertMessageVM.AlertType.Warning;
                            alert.Message = "هذا الخبر موجود بالفعل";
                            break;
                        }
                    case "ForbiddenEdit":
                        {
                            alert.alertType = alertMessageVM.AlertType.Warning;
                            alert.Message = "لا يمكن التعديل علي الخبر بعد مرور 24 ساعه";
                            break;
                        }
                    default:
                        break;
                }
            }
            return alert;
        }
        #endregion

        #region logOperation

        protected int SaveChanges()
        {
            if (LoginUserID != null)
            {       
                DbContext.ChangeTracker.DetectChanges();
                var addedEntities = DbContext.ChangeTracker.Entries()
                           .Where(t => t.State == EntityState.Added)
                           .Select(t => t.Entity)
                           .ToArray();
                if (addedEntities.Count() > 0)
                {
                    int id= DbContext.SaveChanges();
                    LogOperate(addedEntities, "Added");
                    return id;
                }

                var modifiedEntities = DbContext.ChangeTracker.Entries()
                           .Where(t => t.State == EntityState.Modified)
                           .Select(t => t.Entity)
                           .ToArray();
                if (modifiedEntities.Count() > 0)
                {
                    int id = DbContext.SaveChanges();
                    LogOperate(modifiedEntities, "Modified");
                    return id;
                }

                    var deletedEntities = DbContext.ChangeTracker.Entries()
                         .Where(t => t.State == EntityState.Deleted)
                         .Select(t => t.Entity)
                         .ToArray();
                if (deletedEntities.Count() > 0)
                {
                    LogOperate(deletedEntities, "Deleted");
                    return DbContext.SaveChanges();
                }
            }
            return 0;
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

        private void LogOperate(object[] myEntities, string Action)
        {
            var manager = ((IObjectContextAdapter)DbContext).ObjectContext.ObjectStateManager;
            foreach (var change in myEntities)
            {
                var myType = change.GetType();
                var entityName = myType.Name;
                var myObjectState = manager.GetObjectStateEntry(change);
                string macAddress = GetMACAddress();
                string userMName = HttpContext.User.Identity.Name;
                string ipAddress = Request.UserHostAddress;
                string ipAddress2 = GetIPAddress2();
                string osType = Request.UserAgent;
                string message = "User Machine Name:" + userMName + " - UserIP:" + ipAddress + " - User operating system:" + osType +" - RemoteIP:" + ipAddress2 + " - Mac/Physical-Address:" + macAddress + " - UserId:" + LoginUserID + "  - Name:" + LoginUserFullName + " // The Entity Name:" + entityName + " ";

                if (Action != "Added")
                {
                    string keyColName = myObjectState.EntityKey.EntityKeyValues[0].Key.ToString(); 
                    string keyColVal = myObjectState.EntityKey.EntityKeyValues[0].Value.ToString();

                    message += " - With:" + keyColName + "= " + keyColVal + "  ";
                }

                message += Action + " //" + Environment.NewLine + "";

                if (Action != "Deleted")
                {
                    foreach (var prop in myType.GetProperties().Where(p => !(p.GetGetMethod().IsVirtual)).ToArray())
                    {
                        string propName = prop.Name;

                        var propVal = prop.GetValue(change, null);
                        var currentValue = myObjectState.CurrentValues[propName].ToString();
                        //var originalValue = myObjectState.OriginalValues[propName].ToString();

                        //if (currentValue != originalValue)
                        message += propName + " : " + currentValue + "  " + Environment.NewLine;

                    }
                }
                Log(message);
            }
        }

        private void Log(string message)
        {
            try
            {

                string fn = Server.MapPath("~/LogUserActivities/") + DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString() + ".txt";
                if (!System.IO.File.Exists(fn))
                {
                    FileStream fs = new FileStream(fn, FileMode.Create, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(DateTime.Now.ToLongTimeString() +"  " +message);
                    sw.WriteLine("----------------------------------------------------------");
                    sw.Close();
                    fs.Close();
                }
                else
                {
                    StreamWriter sw = new StreamWriter(fn, true);
                    sw.WriteLine(DateTime.Now.ToLongTimeString() +"  "+ message);
                    sw.WriteLine("----------------------------------------------------------");
                    sw.Close();

                }
            }
            catch (Exception e)
            { }
        }

        #endregion

        #region ErrorHLog

        public object MailManager { get; private set; }

        [HandleError]
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            this.ErrorLog(filterContext.Exception);
        }

        [HandleError]
        private void ErrorLog(Exception e)
        {

            using (StreamWriter sw = System.IO.File.AppendText(System.Web.HttpContext.Current.Server.MapPath("~/Errors/log-" + String.Format("{0:yyyy-MM-dd}", DateTime.Today) + ".txt")))
            {
                sw.WriteLine("------  Error Happen With UserID: " + LoginUserID + " - UserName: " + LoginUserFullName + " - At: " + Convert.ToString(DateTime.Now) + " ---------");
                if (e != null)
                {
                    sw.WriteLine("Message: " + e.Message);
                    sw.WriteLine("Stack Trace: ");
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine("");
                }
                sw.Close();
            }
        }

        //private void SendRequest(string _msg)
        //{
        //    SmtpClient SmtpClientObj = new SmtpClient();
        //    SmtpClientObj.Host = "SmtpServer";
        //    SmtpClientObj.Port = 200;//"SmtpPort";
        //    SmtpClientObj.EnableSsl = true;
        //    SmtpClientObj.UseDefaultCredentials = false;
        //    SmtpClientObj.Credentials = new System.Net.NetworkCredential("Username", "Password");

        //    StringBuilder strbldrMsg = new StringBuilder();

        //    strbldrMsg.Append("نـص الرسـالة : " + _msg + "<br/>");
        //    strbldrMsg.Append(" الغـرض من الرسـالة : " + "خطأ حدث بالموقع " + "<br/>");

        //    MailManager.Send(SmtpClientObj, "akhbaralyoumAdminMail", "مـوقع أخبار اليوم - خطأ جـديد", strbldrMsg.ToString(), "ToMail");
        //}

        public ActionResult Error()
        {
            return PartialView();
        }

        #endregion

        #region DeskLog
        public void SaveDeskLog(bool joinDeskShiftFlag)
        {
            try
            {
                string macAddress = GetMACAddress();
                string userMName = HttpContext.User.Identity.Name;
                string ipAddress = Request.UserHostAddress;
                string ipAddress2 = GetIPAddress2();
                string osType = Request.UserAgent;
                string message = "User Machine Name:" + userMName + " - UserIP:" + ipAddress + " - User operating system:" + osType +" - RemoteIP:" + ipAddress2 + " - Mac/Physical-Address:" + macAddress +" - UserId:" + Session["UserID"].ToString() + "  - Name:" + Session["UserFullName"].ToString() + " //  ";
                if (joinDeskShiftFlag)
                    message += "Has Join Desk Shift at: ";
                else
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

        #region MyMethods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool IsValidMainImg(object value)
        {
            int MaxContentLength = 1024 * 1024 * 3;
            string[] AllowedFileExtensions = new string[] { ".jpg", ".jpeg", ".pjpeg", ".gif", ".png", ".x-png", ".bmp" };

            var file = value as HttpPostedFileBase;

            if (file == null || file.FileName=="")
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()))
            {
                //ErrorMessage = "الرجاء رفع ملف من النوع التالى: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                // ErrorMessage = "حجم الملف كبير جدا : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }

        public bool IsValidDoc(object value)
        {
            int MaxContentLength = 1024 * 1024 * 3;
            string[] AllowedFileExtensions = new string[] { ".pdf" , ".doc", ".docx", ".gif" };

            var file = value as HttpPostedFileBase;

            if (file == null || file.FileName == "")
                return false;
            else if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.')).ToLower()))
            {
                //ErrorMessage = "الرجاء رفع ملف من النوع التالى: " + string.Join(", ", AllowedFileExtensions);
                return false;
            }
            else if (file.ContentLength > MaxContentLength)
            {
                // ErrorMessage = "حجم الملف كبير جدا : " + (MaxContentLength / 1024).ToString() + "MB";
                return false;
            }
            else
                return true;
        }


        public static void SaveImage(ref Bitmap imgSource, Size imgSize, string ImagesPath, bool stretch)
        {
            int width = imgSize.Width > imgSource.Width ? imgSource.Width : imgSize.Width;
            int height = width == imgSource.Width ? imgSource.Height : (int)(width * imgSource.Height / imgSource.Width);
            if (stretch)
            {
                width = imgSize.Width;
                height = imgSize.Height;
            }

            Bitmap _OutputImage = new Bitmap(width, height);
            Graphics _Graphics = Graphics.FromImage(_OutputImage);
            _Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            _Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            _Graphics.Clear(Color.White);
            _Graphics.DrawImage(imgSource, 0, 0, width, height);


            _OutputImage.Save(ImagesPath, System.Drawing.Imaging.ImageFormat.Jpeg);

            _OutputImage.Dispose();
            _Graphics.Dispose();


        }

        public void AddWaterMark(ref Bitmap image, string picName)
        {
            Bitmap watermarkImage = new Bitmap(WaterMarkImagePath + "1.png");

            using (watermarkImage)
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x =  (image.Width / 2 - watermarkImage.Width / 2);
                int y =  (image.Height / 2 - watermarkImage.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width + 1, watermarkImage.Height)));
               // imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(image.Width, image.Height)));
                //image.Save(@"C:\Heba\CMS27012018\CMS\App_Data\Temp\" + picName, ImageFormat.Jpeg);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                for (int i = lstDisposableContexts.Count - 1; i >= 0; i--)
                {
                    lstDisposableContexts[i].Dispose();
                    lstDisposableContexts[i] = null;
                    lstDisposableContexts.RemoveAt(i);


                }

            }
            base.Dispose(disposing);
        }

        public static string GetPureURL(string URL)
        {
            string s;

            s = (URL.Replace(" ", "-").Replace("\"", "").Replace(":", "").Replace("?", "").Replace("/", "").Replace("%", "").Replace("|", ""));
            if (s.Length > 55)
            {
                s = s.Substring(0, 55);
            }
            return s;
        }

        public static string GetPureKeyWord(string str)
        {
           string myString = (str.Replace("\"", "").Replace(":", "").Replace("?", "").Replace("/", "").Replace("%", "").Replace("|", ""));
            if (myString.Length > 50)
            {
                myString = myString.Trim().Substring(0, 50);
            }
           
          //  string myString = Regex.Replace(str, @"[^\u0600-\u06FF]+", " ");
            return myString;
        }

        #endregion
    } 
}