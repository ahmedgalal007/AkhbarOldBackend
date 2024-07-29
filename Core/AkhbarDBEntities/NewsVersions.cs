// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.NewsVersions
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Akhbar.DBEntities
{
  public class NewsVersions
  {
    public NewsVersions()
    {
      this.VersionTagsLst = (ICollection<VersionTag>) new HashSet<VersionTag>();
      this.TagsLst = (ICollection<Tag>) new HashSet<Tag>();
    }

    [Key]
    public long VersionId { get; set; }

    public int? NewID { get; set; }

    public int SectionID { get; set; }

    public int? CategoryID { get; set; }

    public int ParentID { get; set; }

    public int NewsType { get; set; }

    public byte NewsSource { get; set; }

    [StringLength(200)]
    [Required(ErrorMessage = "تأكد من ادخال العنوان الرئيسي")]
    [Display(Name = "العنوان الرئيسي")]
    public string Title { get; set; }

    [StringLength(200)]
    [Display(Name = "العنوان الفرعي")]
    public string SubTitle { get; set; }

    [Required(ErrorMessage = "تأكد من ادخال محتوي الخبر")]
    [Display(Name = "محتوي الخبر")]
    public string Story { get; set; }

    [StringLength(3000)]
    [Display(Name = "ملخص الخبر")]
    public string Brief { get; set; }

    [StringLength(1000)]
    public string quote { get; set; }

    [StringLength(200)]
    [Required(ErrorMessage = "تأكد من ادخال الكلمات الدالة")]
    [Display(Name = "الكلمات الدالة")]
    public string Keywords { get; set; }

    [Display(Name = "الصورة الاولي")]
    public int PictureID1 { get; set; }

    [Display(Name = "الصورة الثانية")]
    public int PictureID2 { get; set; }

    [StringLength(1000)]
    [Required(ErrorMessage = "تأكد من ادخال وصف الصورة")]
    [Display(Name = "وصف الصورة")]
    public string PictureCaption1 { get; set; }

    [StringLength(1000)]
    [Display(Name = "وصف الصورة")]
    public string PictureCaption2 { get; set; }

    [Display(Name = "تاريخ النشر")]
    public DateTime? PublishDate { get; set; }

    [Display(Name = "تاريخ التخصيص")]
    public DateTime? SDeskDate { get; set; }

    public int EditorID { get; set; }

    [StringLength(150)]
    [Display(Name = "بقلم")]
    public string ByLine { get; set; }

    public DateTime? AddedDate { get; set; }

    public int? AddedBy { get; set; }

    public int Version { get; set; }

    public int? JournalID { get; set; }

    [StringLength(1000)]
    [Display(Name = "ملاحظات")]
    public string Notes { get; set; }

    [StringLength(200)]
    [Display(Name = "عنوان السوشيال")]
    public string SocialTitle { get; set; }

    [Display(Name = "صوره السوشيال")]
    public int? SocialPictureId { get; set; }

    public int? SocialEditorID { get; set; }

    public virtual NewsPictures NewsPicture { get; set; }

    [ForeignKey("NewID")]
    public virtual News News_NewsVersions { get; set; }

    [ForeignKey("AddedBy")]
    public virtual AdminUser AdminUser_NewsVersions { get; set; }

    public virtual ICollection<Tag> TagsLst { get; set; }

    public virtual ICollection<VersionTag> VersionTagsLst { get; set; }
  }
}
