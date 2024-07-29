using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Areas.FrameWork.Controllers
{
    public class URLQueryValueProvider : IValueProvider
    {
        public bool ContainsPrefix(string prefix)
        {
            bool? CanEncryptDecrypt = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["EncryptDecryptURLQuery"]);
            if (CanEncryptDecrypt != null && CanEncryptDecrypt == true)
            {
                return HttpContext.Current.Request.QueryString.AllKeys.Contains(prefix);
            }
            else
                return false;

        }

        public ValueProviderResult GetValue(string key)
        {
            

                if (HttpContext.Current.Request.QueryString[key] != null)
                {
                    string value = HttpContext.Current.Request.QueryString[key];
                    value = GeneralHellper.DecryptStringFromBytes(value);
                    ValueProviderResult result = new ValueProviderResult(value, value, CultureInfo.CurrentCulture);
                    return result;
                }
                else
                {
                    return null;
                }
           

        }

    }

    public class URLQueryValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new URLQueryValueProvider();
        }
    }

}