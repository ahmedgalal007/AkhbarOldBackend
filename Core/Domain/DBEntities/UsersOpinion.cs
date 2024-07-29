// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.UsersOpinion
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class UsersOpinion
  {
    [Key]
    public int CommentID { get; set; }

    public int NewID { get; set; }

    public int UserID { get; set; }

    [StringLength(100)]
    public string UserName { get; set; }

    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(100)]
    public string Subject { get; set; }

    [StringLength(8000)]
    public string messagebody { get; set; }

    public DateTime? messagedate { get; set; }

    public byte Approved { get; set; }

    public int Views { get; set; }

    public byte? IsDeleted { get; set; }

    [StringLength(100)]
    public string OperationalUser { get; set; }

    [StringLength(20)]
    public string UserIP { get; set; }

    [StringLength(20)]
    public string CloudIP { get; set; }
  }
}
