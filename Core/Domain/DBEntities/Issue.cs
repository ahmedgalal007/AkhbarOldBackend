// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Issue
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class Issue
  {
    public int IssueID { get; set; }

    [StringLength(200)]
    [Display(Name = "عنوان المقال")]
    public string IssueTitle { get; set; }

    [Required(ErrorMessage = "تأكد من ادخال تاريخ الاضافة")]
    [Display(Name = "التاريخ")]
    public DateTime? IssueDate { get; set; }

    [Display(Name = "حالة المقال")]
    public int IssueStatus { get; set; }

    public int JournalID { get; set; }
  }
}
