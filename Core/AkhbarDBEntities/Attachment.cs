// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Attachment
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class Attachment
  {
    [Key]
    public int FileId { get; set; }

    public int? NewID { get; set; }

    [StringLength(500)]
    [Display(Name = "وصف الملف")]
    public string Discription { get; set; }

    [StringLength(500)]
    [Display(Name = "اسم الملف")]
    public string FileName { get; set; }
  }
}
