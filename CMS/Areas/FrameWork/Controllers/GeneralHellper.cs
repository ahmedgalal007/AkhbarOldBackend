using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using Domain.Akhbar.DBBusiness;
using Domain.Akhbar.DBEntities;
using Domain.Akhbar.DBContext;

namespace CMS.Areas.FrameWork.Controllers
{
    public class GeneralHellper
    {
        public string AkhbarDbConnection
        {
            get
            {
                return "AkhbarDBConnection";
            }
        }
        public List<VUserPermissions> UserPermissions(AkhbarDBContext db)
        {
            HttpSessionState session = HttpContext.Current.Session;
            int PermissionTimeOut = 0;
            if (int.TryParse(ConfigurationManager.AppSettings["UserPermissionTimeOut"], out PermissionTimeOut) == false)
            {
                PermissionTimeOut = 525600;//Max Value equal 1 year 525600 --  1440 means 24 hours
            }

            if (session["UserID"] != null)
            {

                    AdminUser _AdminUser = new AdminUser { UserID = (int)session["UserID"] };
                    AdminUserBusiness _AdminUserBusiness = new AdminUserBusiness(db);

                if (session["UserPermissions"] != null)
                {
                    List<VUserPermissions> lstSF = (List<VUserPermissions>)session["UserPermissions"];
                    //if current user not equal last login user
                    if (lstSF[0].UserID != _AdminUser.UserID)
                    {
                        lstSF = _AdminUserBusiness.LoadUserPermissions(_AdminUser);
                        session.AddWithTimeout("UserPermissions", lstSF, TimeSpan.FromMinutes(PermissionTimeOut));
                    }
                    else
                    {
                        if (session.GetWithTimeout("UserPermissions") == null)
                        {
                            lstSF = _AdminUserBusiness.LoadUserPermissions(_AdminUser);
                            session.AddWithTimeout("UserPermissions", lstSF, TimeSpan.FromMinutes(PermissionTimeOut));
                        }
                    }
                }
                else
                {
                    if (session.GetWithTimeout("UserPermissions") == null)
                    {
                        List<VUserPermissions> lstSF = _AdminUserBusiness.LoadUserPermissions(_AdminUser);
                        session.AddWithTimeout("UserPermissions", lstSF, TimeSpan.FromMinutes(PermissionTimeOut));
                    }
                }

                return (List<VUserPermissions>)session.GetWithTimeout("UserPermissions");
            }
            else
                return new List<VUserPermissions>();
        }


        /// <summary>
        /// this function will add query string to be assigned to all links or form submit
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="QueryRouteValue"></param>
        public static void AddURLQueryString(Controller controller, RouteValueDictionary QueryRouteValue)
        {

            RouteValueDictionary dic = (RouteValueDictionary)controller.TempData["AddURLQueryString"] ?? new RouteValueDictionary();
            if (QueryRouteValue != null)
            {
                foreach (var item in QueryRouteValue)
                {
                    dic[item.Key] = item.Value;

                }
            }
            controller.TempData["AddURLQueryString"] = dic;
        }

        /// <summary>
        /// this function will remove query string from all links or form submit
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="key"></param>
        public static void RemoveURLQueryString(Controller controller, string[] key)
        {
            controller.TempData["RemoveURLQuery"] = key;
        }

        /// <summary>
        /// get the oreginal request query string without modifications
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        //public static RouteValueDictionary GetRequestURLQueryString(Controller controller)
        //{
        //    RouteValueDictionary TemRouteValues = new RouteValueDictionary();
        //    ////get the current query string e.g. ?BucketID=17371&amp;compareTo=123
        //    var qs = controller.HttpContext.Request.QueryString;
        //    //add query string parameters to the route value dictionary
        //    foreach (string param in qs)
        //        if (!string.IsNullOrEmpty(qs[param]) && param != "X-Requested-With" && param != "_")
        //        {

        //            TemRouteValues[param] = qs[param];                   
        //        }

        //    return TemRouteValues;
        //}

        /// <summary>
        /// this methode generate URL query string after getting the oreginal request string 
        /// and adding additional quey and remove query passed from the controller or the view
        /// and passing the new URL query string to all custom link and form methods
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static RouteValueDictionary GetSystemRouteValues(Controller controller)
        {

            RouteValueDictionary routeValues = new RouteValueDictionary();
            RouteValueDictionary RequestURLQueryString = new RouteValueDictionary();
            ////get the current query string e.g. ?BucketID=17371&amp;compareTo=123
            var qs = controller.HttpContext.Request.QueryString;
            //add query string parameters to the route value dictionary
            foreach (string param in qs)
            {
                if (!string.IsNullOrEmpty(qs[param]) && param != "X-Requested-With" && param != "_")
                {

                    RequestURLQueryString[param] = qs[param];
                }
            }

            if (RequestURLQueryString != null && RequestURLQueryString.Count > 0)
            {
                foreach (var item in RequestURLQueryString)
                {
                    routeValues[item.Key] = GeneralHellper.DecryptStringFromBytes(item.Value.ToString());
                }
            }

            routeValues["page"] = routeValues["page"] ?? "1";

            //////////////////////////AdditionalRouts/////////////////////////////////////////////
            RouteValueDictionary SearchrouteValues = (RouteValueDictionary)controller.TempData["AddURLQueryString"];
            if (SearchrouteValues != null)
            {
                foreach (var item in SearchrouteValues)
                {
                    routeValues[item.Key] = item.Value;
                }
            }
       
            if (controller.TempData["RemoveURLQuery"] != null)
            {
                foreach (var key in (string[])controller.TempData["RemoveURLQuery"])
                {
                    if (routeValues.Keys.Contains(key))
                        routeValues.Remove(key);
                }

            }
            return routeValues;
        }

        public static string GetSystemRouteValuesAsString(Controller controller,bool Decripted = false)
        {
            RouteValueDictionary routeValues = GetSystemRouteValues(controller);
            string x = String.Join("&", from item in routeValues.Keys
                                    select item + "=" + (Decripted == false ? EncryptStringToBytes(routeValues[item].ToString()) : routeValues[item]));
            
            return x;
        }

        /// <summary>
        /// encrypt using Rijndael algorithm
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string EncryptStringToBytes(string plainText)
        {
            byte[] encrypted = null;

            byte[] Key = System.Text.Encoding.ASCII.GetBytes("d25&KW");//32 byte key
            byte[] IV = System.Text.Encoding.ASCII.GetBytes("G&EK#");//16 byte key;

            bool? CanEncryptDecrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EncryptDecryptURLQuery"]);

            // Check arguments.
            if (plainText != null && plainText.Length > 0 && CanEncryptDecrypt != null && CanEncryptDecrypt == true)
            {
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");

                // Create an RijndaelManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = Key;
                    rijAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {

                                //Write all data to the stream.
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }


                // Return the encrypted bytes from the memory stream.
                return Convert.ToBase64String(encrypted);
            }
            else
                return plainText;


        }

        /// <summary>
        /// decrypt using Rijndael algorithm
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="Key"></param>
        /// <param name="IV"></param>
        /// <returns></returns>
        public static string DecryptStringFromBytes(string cipherBase64String)
        {

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            byte[] Key = System.Text.Encoding.ASCII.GetBytes("d25&KW");//32 byte key
            byte[] IV = System.Text.Encoding.ASCII.GetBytes("G&EK#");//16 byte key;

            bool? CanEncryptDecrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EncryptDecryptURLQuery"]);

            // Check arguments.
            if (cipherBase64String != null && cipherBase64String.Length > 0 && CanEncryptDecrypt != null && CanEncryptDecrypt == true)
            {
                if (Key == null || Key.Length <= 0)
                    throw new ArgumentNullException("Key");
                if (IV == null || IV.Length <= 0)
                    throw new ArgumentNullException("IV");

                byte[] cipherText = Convert.FromBase64String(cipherBase64String);

                // Create an RijndaelManaged object
                // with the specified key and IV.
                using (RijndaelManaged rijAlg = new RijndaelManaged())
                {
                    rijAlg.Key = Key;
                    rijAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {

                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                }

                return plaintext;
            }
            else
                return cipherBase64String;
        }


       
    }
}