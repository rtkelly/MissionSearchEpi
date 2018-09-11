using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using MissionSearch;

namespace BaseSite.modules.MissionSearchEpi.UI.Blocks
{
    [ContentType(DisplayName = "Boost Settings", GUID = "5f6add23-346b-4cb0-8618-0143a4a5ac0d", Description = "", AvailableInEditMode = false)]
    public class BoostBlock : BlockData, IBoostSettings
    {

        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual int TitleBoost { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual int ContentBoost { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual int SummaryBoost { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual int DocumentsBoost { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        public virtual int DateBoost { get; set; }
    }


}