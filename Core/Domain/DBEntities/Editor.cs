// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Editor
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class Editor
  {
    public int EditorID { get; set; }

    [Required(ErrorMessage = "تاكد من ادخال الاسم")]
    [Display(Name = "اسم الكاتب")]
    [StringLength(100)]
    public string EditorName { get; set; }

    [Display(Name = "ترتيب الظهور")]
    public int DisplayOrder { get; set; }

    public int JournalID { get; set; }

    [StringLength(500)]
    [Display(Name = "الصورة الشخصية")]
    public string picture { get; set; }

    [Display(Name = "القسم")]
    public int? SectionID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال عنوان المقال")]
    [Display(Name = "عنوان المقال")]
    public string ArticleName { get; set; }

    public virtual MainSection Section { get; set; }
  }
}
