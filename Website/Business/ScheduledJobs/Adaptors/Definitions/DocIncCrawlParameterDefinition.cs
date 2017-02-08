using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiServer.Web.WebControls;

namespace BaseSite.Business.ScheduledJobs.Adaptors.Definitions
{
    public class DocIncCrawlParameterDefinition : IParameterDefinitions
    {

        public IEnumerable<JobParameter> GetParameterControls()
        {
             return new List<JobParameter>
                 {
                   AddTextBoxMimeTypes(),
                   //AddCalendarLastCrawl(),
                   AddTextBoxCrawlStart()
                   
                 };
        }
               
               

        private static JobParameter AddTextBoxMimeTypes()
        {
            return new JobParameter
            {
                LabelText = "Document Types",
                Description = "Comma seperated list of extensions to crawl",
                Control = new TextBox { ID = "TextBoxMimeTypes", TextMode = TextBoxMode.MultiLine }
            };
        }

        private static JobParameter AddTextBoxCrawlStart()
        {
            return new JobParameter
            {
                LabelText = "Last Crawl Date",
                Description = "Incremental crawl start date",
                Control = new TextBox { ID = "TextBoxCrawlStart" }
            };
        }
        
        /*
        private static JobParameter AddCalendarLastCrawl()
        {
            return new JobParameter
            {
                LabelText = "Crawl Start",
                Description = "",
                Control = new System.Web.UI.WebControls.Calendar { ID = "CalendarLastCrawl" }
            };
        }
        */

       
       

        public void SetValue(System.Web.UI.Control control, object value)
        {
            try
            {
                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = (bool)value;
                }
                else if (control is TextBox)
                {
                    ((TextBox)control).Text = (string)value;
                }
                else if (control is DropDownList)
                {
                    ((DropDownList)control).SelectedValue = (string)value;
                }
                else if (control is InputPageReference)
                {
                    ((InputPageReference)control).PageLink = (PageReference)value;
                }

                else if (control is System.Web.UI.WebControls.Calendar)
                {
                    ((System.Web.UI.WebControls.Calendar)control).SelectedDate = (DateTime)value;
                }
            }

            catch
            {

            }
                
        }

        public object GetValue(System.Web.UI.Control control)
        {
            if (control is CheckBox)
            {
                return ((CheckBox)control).Checked;
            }
            if (control is TextBox)
            {
                return ((TextBox)control).Text;
            }
            if (control is DropDownList)
            {
                return ((DropDownList)control).SelectedValue;
            }
            if (control is InputPageReference)
            {
                return ((InputPageReference)control).PageLink;
            }
           
            if (control is System.Web.UI.WebControls.Calendar)
            {
                return ((System.Web.UI.WebControls.Calendar)control).SelectedDate;
            }
           
            return null;
        }
    }
}