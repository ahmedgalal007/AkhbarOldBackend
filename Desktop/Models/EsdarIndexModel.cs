using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AkhbarElyoum.Models
{
	public class EsdarIndexModel
	{
        public List<NewsDetails> Slider { get; set; }
				public List<NewsDetails> News { get; set; }
	}
}