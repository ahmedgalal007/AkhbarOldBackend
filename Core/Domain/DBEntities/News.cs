// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.News
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
    public class News
    {
        public News()
        {
            this.NewsVersionsLst = (ICollection<NewsVersions>)new HashSet<NewsVersions>();
            this.NewsTagsLst = (ICollection<NewsTag>)new HashSet<NewsTag>();
            this.TagsLst = (ICollection<Tag>)new HashSet<Tag>();
            this.NewsGalleryLst = (ICollection<NewsGallery>)new HashSet<NewsGallery>();
            this.GalleryLst = (ICollection<Gallery>)new HashSet<Gallery>();
        }

        [Key]
        public int NewID { get; set; }

        [Required(ErrorMessage = "الرجاء اختيار القسم")]
        public int SectionID { get; set; }

        public int? SectionID1 { get; set; }

        public int? SectionID2 { get; set; }

        public int? SectionID3 { get; set; }

        public int? SectionID4 { get; set; }

        public int? SectionID5 { get; set; }

        public int? SectionID6 { get; set; }

        public int? SectionID7 { get; set; }

        public int? SectionID8 { get; set; }

        public int? SectionID9 { get; set; }

        public int? CategoryID { get; set; }

        public int? ParentID { get; set; }

        public int? NewsType { get; set; }

        public byte? NewsSource { get; set; }

        [StringLength(160, ErrorMessage = "لا يمكن ان يزيد العنوان عن 160 حرف")]
        [Required(ErrorMessage = "الرجاء ادخال عنوان الخبر")]
        [Display(Name = "العنوان الرئيسي")]
        public string Title { get; set; }

        [StringLength(200, ErrorMessage = "لا يمكن ان يزيد العنوان الفرعي عن 200 حرف")]
        [Display(Name = "العنوان الفرعي")]
        public string SubTitle { get; set; }

        [StringLength(80, ErrorMessage = "لا يمكن ان يزيد عنوان محركات البحث عن 80 حرف")]
        [Required(ErrorMessage = "الرجاء ادخال عنوان محركات البحث")]
        [Display(Name = "عنوان محركات البحث")]
        public string SEOTitle { get; set; }

        [Column(TypeName = "ntext")]
        [Required(ErrorMessage = "تأكد من ادخال محتوي الخبر")]
        [Display(Name = "محتوي الخبر")]
        public string Story { get; set; }

        [StringLength(3000, ErrorMessage = "لا يمكن ان يزيد الملخص الفرعي عن 3000 حرف")]
        [Display(Name = "ملخص الخبر")]
        public string Brief { get; set; }

        [StringLength(1000)]
        public string quote { get; set; }

        [StringLength(200, ErrorMessage = "لا يمكن ان تزيد الكلمة الدالة عن 200 حرف")]
        [Display(Name = "الكلمات الدالة")]
        public string Keywords { get; set; }

        [Display(Name = "الصورة الاولى")]
        public int? PictureID1 { get; set; }

        [Display(Name = "الصورة الثانية")]
        public int? PictureID2 { get; set; }

        [StringLength(1000, ErrorMessage = "لا يمكن ان يزيد وصف الصورة عن 1000 حرف")]
        [Required(ErrorMessage = "تأكد من ادخال وصف الصورة")]
        [Display(Name = "وصف الصورة")]
        public string PictureCaption1 { get; set; }

        [StringLength(1000)]
        [Display(Name = "وصف الصورة")]
        public string PictureCaption2 { get; set; }

        public bool HasVideo { get; set; } = false;
        public int? VideoFeedId { get; set; }

        [Display(Name = "تاريخ الإضافة")]
        public DateTime? AddedDate { get; set; }

        [Display(Name = "تاريخ التخصيص")]
        public DateTime? SDeskDate { get; set; }

        [Display(Name = "تاريخ النشر")]
        public DateTime? PublishDate { get; set; }

        public int? EditorID { get; set; }

        [StringLength(150)]
        [Display(Name = "بقلم")]
        public string ByLine { get; set; }

        public byte? Approved { get; set; }

        [Display(Name = "عدد المشاهدات")]
        public int? Views { get; set; }

        [Display(Name = "حالة الخبر")]
        public int? NewStatus { get; set; }

        public int? BackEndNewId { get; set; }

        public int? AssignedTo { get; set; }

        [Display(Name = "اسم مدخل الخبر")]
        public int? Creator { get; set; }

        [StringLength(1000, ErrorMessage = "لا يمكن ان تزيد الملاحظات عن 1000 حرف")]
        [Display(Name = "ملاحظات")]
        public string Notes { get; set; }

        public int? sEditorID { get; set; }

        public int? sDepartDirectorID { get; set; }

        public int? sManagerEditorID { get; set; }

        public int? sDeskID { get; set; }

        public int? sReviewerID { get; set; }

        public int? sUploaderID { get; set; }

        public int? NewsID { get; set; }

        public int? JournalID { get; set; }

        public int? EditAfterRejectFlag { get; set; }

        [StringLength(200)]
        [Display(Name = "عنوان السوشيال")]
        public string SocialTitle { get; set; }

        [Display(Name = "صوره السوشيال")]
        public int? SocialPictureId { get; set; }

        public int? SocialEditorID { get; set; }

        public virtual NewsPictures NewsPicture { get; set; }

        public virtual MainSection MainSection { get; set; }

        [ForeignKey("sDeskID")]
        public virtual AdminUser AdminUser_DeskUser { get; set; }

        [ForeignKey("Creator")]
        public virtual AdminUser AdminUser_AddedUser { get; set; }

        [ForeignKey("sUploaderID")]
        public virtual AdminUser AdminUser_PublisherUser { get; set; }

        public virtual ICollection<NewsVersions> NewsVersionsLst { get; set; }

        public virtual ICollection<Tag> TagsLst { get; set; }

        public virtual ICollection<NewsTag> NewsTagsLst { get; set; }

        public virtual ICollection<Gallery> GalleryLst { get; set; }

        public virtual ICollection<NewsGallery> NewsGalleryLst { get; set; }

        public virtual ICollection<ByLine> ByLineLst { get; set; }

        public virtual ICollection<News_Byline> NewsByLineLst { get; set; }
    }
}
