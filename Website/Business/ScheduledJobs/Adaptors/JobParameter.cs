using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BaseSite.Business.ScheduledJobs.Adaptors
{
    public class JobParameter
    {
        public string Id { get { return Control.ID; } }
        public bool ShowLabel { get { return !string.IsNullOrEmpty(LabelText); } }

        public string LabelText { get; set; }
        public string Description { get; set; }
        public Control Control { get; set; }
    }
}