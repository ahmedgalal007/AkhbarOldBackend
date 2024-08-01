using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class Attachment
    {
        public int FieldID { get; set; }
        public int? NewID { get; set; }
        public string Discription { get; set; }
        public string FileName { get; set; }
    }
}