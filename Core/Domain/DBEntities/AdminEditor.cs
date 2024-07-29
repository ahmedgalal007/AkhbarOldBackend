using System.ComponentModel.DataAnnotations;

namespace Domain.Akhbar.DBEntities
{
  public class AdminEditor
  {
    [Key]
    public int EditorID { get; set; }

    [StringLength(200)]
    [Display(Name = "الاسم بالكامل")]
    public string EditorName { get; set; }

    [StringLength(200)]
    [Display(Name = "الوظيفة")]
    public string Position { get; set; }

    [StringLength(200)]
    [Display(Name = "الايميل")]
    public string Email { get; set; }

    [Display(Name = "الصورة الشخصية")]
    public int? Pic { get; set; }

    [Display(Name = "ترتيب الظهور")]
    public int DisplayOrder { get; set; }

    public int Type { get; set; }
  }
}
