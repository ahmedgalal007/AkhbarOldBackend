using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Akhbar.DBEntities
{
  [Table("AdminLog")]
  public class AdminLog
  {
    [Key]
    public int LogID { get; set; }

    public int? LogUserID { get; set; }

    public DateTime? LogDate { get; set; }

    [StringLength(200)]
    [Display(Name = "مسار ملف اللوج")]
    public string LogURL { get; set; }

    [StringLength(50)]
    public string LogRemoteAddress { get; set; }

    [StringLength(50)]
    public string LogServerName { get; set; }

    [StringLength(200)]
    public string LogQueryString { get; set; }

    public virtual AdminUser AdminUser { get; set; }
  }
}
