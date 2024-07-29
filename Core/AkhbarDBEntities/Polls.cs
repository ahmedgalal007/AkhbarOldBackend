// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Polls
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class Polls
  {
    public Polls()
    {
      this.NewsPollLst = (ICollection<NewsPoll>) new HashSet<NewsPoll>();
      this.PollsOptionLst = (ICollection<PollsOption>) new HashSet<PollsOption>();
    }

    [Key]
    public int PollID { get; set; }

    public int SectionID { get; set; }

    [StringLength(300)]
    [Required(ErrorMessage = "تأكد من إدخال نص التصويت")]
    [Display(Name = "نص التصويت")]
    public string PollBody { get; set; }

    [Display(Name = "تاريخ الاضافة")]
    public DateTime? Added_Date { get; set; }

    [Display(Name = "فعال")]
    public byte? Activated { get; set; }

    [Display(Name = "تاريخ التفعيل")]
    public DateTime? DateActivated { get; set; }

    [Display(Name = "عدد التصويتات")]
    public int? TotalVotes { get; set; }

    public int? JournalID { get; set; }

    public virtual ICollection<NewsPoll> NewsPollLst { get; set; }

    public virtual ICollection<PollsOption> PollsOptionLst { get; set; }
  }
}
