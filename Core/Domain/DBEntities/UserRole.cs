// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.UserRole
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

namespace Domain.Akhbar.DBEntities
{
  public class UserRole
  {
    public int? RoleId { get; set; }

    public int? AdminUserID { get; set; }

    public int? SectionID { get; set; }

    public int ID { get; set; }

    public int? JournalID { get; set; }

    public virtual AdminUser AdminUser { get; set; }

    public virtual Role Role { get; set; }

    public virtual MainSection section { get; set; }
  }
}
