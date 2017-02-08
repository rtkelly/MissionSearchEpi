using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace BaseSite.Business.ScheduledJobs.Adaptors
{
    public interface IParameterDefinitions
    {
        IEnumerable<JobParameter> GetParameterControls();
        void SetValue(Control control, object value);
        object GetValue(Control control);
    }
}