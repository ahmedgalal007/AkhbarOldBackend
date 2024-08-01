using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
    public class RSSNewsDetails
	{
        public int? NewsID { get; set; }
        public int? SectionID { get; set; }
        public int? CategoryID { get; set; }
        public int? ParentID { get; set; }
        public int? NewsType { get; set; }
        public int? NewsSource { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Story { get; set; }
        public string Breif { get; set; }
        public string quote { get; set; }
        public string Keywords { get; set; }
        public int? PictureID1 { get; set; }
        public int? PictureID2 { get; set; }
        public string PictureCaption1 { get; set; }
        public string PictureCaption2 { get; set; }
        public string PublishDate { get; set; }
		    public string PublishDateAR { get; set; }
		    public DateTime? PubDate { get; set; }
        public int? EditorID { get; set; }
        public string ByLine { get; set; }
        public int? Approved { get; set; }
        public int? Views { get; set; }
        public int? NewsStatus { get; set; }
        public int? BackEndNewID { get; set; }
        public int? AssignedTo { get; set; }
        public int? Creator { get; set; }
        public string Notes { get; set; }
        public int? sEditorID { get; set; }
        public int? sDepartDirectorID { get; set; }
        public int? sManagerEditorID { get; set; }
        public int? sDeskID { get; set; }
        public int? sReviewerID { get; set; }
        public int? sUploaderID { get; set; }
        public int? JournalID { get; set; }


        /// New Columns
        public string SocialTitle { get; set; }
        public string SocialPictureCaption { get; set; }
        public string SocialPictureName { get; set; }
        /// End New Columns

        /// </summary>

        public string SecTitle { get; set; }
        public int? Weekly { get; set; }
        public string PictureName1 { get; set; }
        public string PictureName2 { get; set; }

        public int? IssueID { get; set; }
        public int? DisplayOrder { get; set; }
        public string EditorName { get; set; }
        public string Desription { get; set; }
        public string TagName { get; set; }
        public int TagId { get; set; }
        public string SectionTitle { get; set; }
        public string IssueDate { get; set; }
    }
}