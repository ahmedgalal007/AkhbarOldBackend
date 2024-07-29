// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.PollsOption
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class PollsOption
  {
    [Key]
    public int OptionID { get; set; }

    public int? PollID { get; set; }

    [StringLength(100)]
    [Required(ErrorMessage = "تأكد من ادخال نص الاختيار")]
    [Display(Name = "نص الاختيار")]
    public string OptionBody { get; set; }

    public int? Votes { get; set; }

    public virtual Polls Poll { get; set; }
  }
}
