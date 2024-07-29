// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.GalleryType
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("GalleryType")]
  public class GalleryType
  {
    public GalleryType() => this.GalleryLst = (ICollection<Gallery>) new HashSet<Gallery>();

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GalleryTypeId { get; set; }

    [Key]
    [StringLength(50)]
    [Required(ErrorMessage = "تأكد من إدخال نوع الألبوم ")]
    [Display(Name = "نوع الالبوم")]
    public virtual string GalleryTypeName { get; set; }

    public virtual ICollection<Gallery> GalleryLst { get; set; }
  }
}
