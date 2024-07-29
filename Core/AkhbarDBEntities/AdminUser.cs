// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.AdminUser
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  public class AdminUser
  {
    [Key]
    public int UserID { get; set; }

    [StringLength(400)]
    [Display(Name = "الاسم بالكامل")]
    public string FullName { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال اسم الدخول")]
    [Display(Name = "اسم الدخول")]
    public string UserName { get; set; }

    [StringLength(200)]
    [Required(ErrorMessage = "تأكد من ادخال كلمة السر")]
    [Display(Name = "كلمة السر")]
    public string Password { get; set; }

    [Display(Name = "مدير عام")]
    public int? Type { get; set; }

    public DateTime? LastLive { get; set; }

    [StringLength(50)]
    [Display(Name = "التليفون")]
    public string Telephone { get; set; }

    [StringLength(50)]
    [Display(Name = "الصورة الشخصية")]
    public string UserPhoto { get; set; }

    [StringLength(50)]
    [Display(Name = "الايميل")]
    [EmailAddress(ErrorMessage = "تأكد من صحة إدخال الإيميل")]
    public string UserEmail { get; set; }

    [Display(Name = "تاريخ الميلاد")]
    public DateTime? BirthDay { get; set; }

    [StringLength(50)]
    [Display(Name = "محل الميلاد")]
    public string BirthPlace { get; set; }

    [StringLength(50)]
    [Display(Name = "حساب التويتر")]
    public string UserTwitter { get; set; }

    [StringLength(50)]
    [Display(Name = "حساب الفيس بوك")]
    public string UserFB { get; set; }

    [Display(Name = "الجنسية")]
    public int? Nationality { get; set; }

    [StringLength(1000)]
    [Display(Name = "المؤهل الدراسي")]
    public string EducationalQualification { get; set; }

    [StringLength(50)]
    [Display(Name = "اسم الشهرة")]
    public string NickName { get; set; }

    [Column(TypeName = "ntext")]
    [Display(Name = "اكتب عن نفسك")]
    public string AboutYourSelf { get; set; }

    [Display(Name = "نشط")]
    public bool? Active { get; set; }

    public virtual ICollection<UserRole> UserRoleLst { get; set; }

    public virtual ICollection<AdminLog> AdminLogLst { get; set; }

    public AdminUser()
    {
      this.UserRoleLst = (ICollection<UserRole>) new HashSet<UserRole>();
      this.AdminLogLst = (ICollection<AdminLog>) new HashSet<AdminLog>();
    }
  }
}
