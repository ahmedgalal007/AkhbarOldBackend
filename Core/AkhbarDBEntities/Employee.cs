// Decompiled with JetBrains decompiler
// Type: Akhbar.DBEntities.Employee
// Assembly: AkhbarDBEntities, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0975156E-6BB7-495E-B9F6-BE9BA8B2A173
// Assembly location: E:\Dot Net Projects\_Akhbar\Backend\CMSWebGate\CMS\bin\AkhbarDBEntities.dll

using System.ComponentModel.DataAnnotations;

namespace Akhbar.DBEntities
{
  public class Employee
  {
    public int EmployeeID { get; set; }

    [StringLength(50)]
    [Display(Name = "الاسم بالكامل")]
    public string EmplyeeName { get; set; }

    public int? displayorder { get; set; }
  }
}
