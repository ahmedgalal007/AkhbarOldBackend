using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
// using Microsoft.Web.Mvc;

namespace AkhbarElyoum.Models
{
    public class Comments
    {
        [Required(ErrorMessage = "Title is required")]
        public int CommentID { get; set; }
        public int? UserID { get; set; }
        public int? NewId { get; set; }
         [Required(ErrorMessage = "يجب ادخال الاسم")]
         [StringLength(100, ErrorMessage = "أقصي عدد للحروف 100 حرف")]
        public string UserName { get; set; }
         [Required(ErrorMessage = "يجب ادخال البريد الالكتروني ")]
         [StringLength(100, ErrorMessage = "أقصي عدد للحروف 100 حرف")]
         [System.ComponentModel.DataAnnotations.EmailAddress(ErrorMessage="من فضلك ادخل بريد الكترونى صحيح")]
        public string Email { get; set; }
         [Required(ErrorMessage = "يجب ادخال العنوان")]
         [StringLength(100, ErrorMessage = "أقصي عدد للحروف 100 حرف")]
        public string Subject { get; set; }
         [Required(ErrorMessage = "يجب ادخال التعليق")]
         [StringLength(8000,ErrorMessage="أقصي عدد للحروف 8000 حرف")]
        public string messagebody { get; set; }
        public string messagedate { get; set; }
        public int? Views { get; set; }
        public int Count { get; set; }
    }
}