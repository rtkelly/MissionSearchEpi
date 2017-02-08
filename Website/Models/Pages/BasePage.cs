﻿using EPiServer;
using EPiServer.Core;
using EPiServer.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EPiServer.BaseLibrary.Scheduling;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.Web;
using Newtonsoft.Json;
using MissionSearchEpi;
using MissionSearch;
using System.ComponentModel.DataAnnotations;
using MissionSearchEpi.Util;
using BaseSite.Models.Pages;
using MissionSearch.Attributes;

namespace BaseSite.Models
{
    public class BasePage : PageData
    {      

        [Ignore]
        public IContentRepository _repository { get; set; }

        [Ignore]
        public IContentRepository Repository
        {
            get
            {
                if (_repository == null)
                    _repository = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<IContentRepository>();
                return _repository;
            }
        }

        [SearchIndex("categories")]
        public List<string> Categories
        {
            get
            {
                return EpiHelper.GetCategories(Category);
            }
        }

        [SearchIndex("animal")]
        public List<string> Animal
        {
            get
            {
                return EpiHelper.GetCategories(Category, 4);
            }
        }

        
       
    }
}