using Domain.Akhbar.DBEntities;
using System.Data.Entity;
using BaseBusiness;

namespace Domain.Akhbar.DBBusiness
{
  public class VideoFeedBusiness : BaseBusiness<VideoFeed>
  {
    public VideoFeedBusiness()
    {
    }

    public VideoFeedBusiness(DbContext _DbContext)
      : base(_DbContext)
    {
    }
  }
}
