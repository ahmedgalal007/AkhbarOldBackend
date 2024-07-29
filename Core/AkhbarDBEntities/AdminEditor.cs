// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.AdminEditor
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class AdminEditor
  {
    [Key]
    public int EditorID { get; set; }

    [StringLength(200)]
    [Display(Name = "الاسم بالكامل")]
    public string EditorName { get; set; }

    [StringLength(200)]
    [Display(Name = "الوظيفة")]
    public string Position { get; set; }

    [StringLength(200)]
    [Display(Name = "الايميل")]
    public string Email { get; set; }

    [Display(Name = "الصورة الشخصية")]
    public int? Pic { get; set; }

    [Display(Name = "ترتيب الظهور")]
    public int DisplayOrder { get; set; }

    public int Type { get; set; }
  }
}
