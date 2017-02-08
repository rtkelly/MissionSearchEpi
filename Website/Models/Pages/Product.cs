using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using MissionSearch;
using MissionSearchEpi;
using MissionSearch.Attributes;

namespace BaseSite.Models.Pages
{
    [ContentType(DisplayName = "Product", GUID = "7ec4229b-3755-4285-af98-d010175fb298", Description = "")]
    public class Product : SearchableBasePage
    {
        [CultureSpecific]
        [Display(Name = "Name", Order = 1)]
        public virtual String PageTitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Summary", Order = 2)]
        public virtual String Summary { get; set; }
                
        [Display(Name = "Price", Order = 4)]
        [SearchIndex("price")]
        public virtual double Price { get; set; }

               
    }
}