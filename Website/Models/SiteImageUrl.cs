using EPiServer.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseSite.Models
{
    public class SiteImageUrl : ImageUrlAttribute
    {
        /// <summary>
        /// The parameterless constructor will initialize a SiteImageUrl attribute with a default thumbnail
        /// </summary>
        public SiteImageUrl()
            : base("~/Static/gfx/page-type-thumbnail.png")
        {

        }

        public SiteImageUrl(string path)
            : base(path)
        {

        }
    }
}