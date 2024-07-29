// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Trend
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class Trend
  {
    public long ID { get; set; }

    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من ادخال الكلمة الدالة")]
    [Display(Name = "الكلمة الدالة")]
    public string Name { get; set; }

    public long TagId { get; set; }

    public DateTime CreationDate { set; get; }
  }
}
