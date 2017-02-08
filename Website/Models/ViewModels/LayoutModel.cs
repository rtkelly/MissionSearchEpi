using BaseSite.Models.Blocks;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseSite.Models.ViewModels
{
    public class LayoutModel
    {
        public HeaderBlock Header { get; set; }
        public FooterBlock Footer { get; set; }
                
    }
}