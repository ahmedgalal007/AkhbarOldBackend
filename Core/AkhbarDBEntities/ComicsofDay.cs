// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.ComicsofDay
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  [Table("ComicsofDay")]
  public class ComicsofDay
  {
    [Key]
    public int PictureID { get; set; }

    [StringLength(200)]
    [Display(Name = "عنوان الصورة")]
    public string PictureTitle { get; set; }

    public DateTime Date_Added { get; set; }
  }
}
