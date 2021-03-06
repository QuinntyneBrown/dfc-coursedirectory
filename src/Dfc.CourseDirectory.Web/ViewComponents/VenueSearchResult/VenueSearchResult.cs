﻿using Dfc.CourseDirectory.Web.ViewComponents.VenueSearchResult;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Dfc.CourseDirectory.Web.ViewComponents.VenueSearchResult
{
    public class VenueSearchResult : ViewComponent
    {
        public IViewComponentResult Invoke(VenueSearchResultModel model)
        {
            var actualModel = model ?? new VenueSearchResultModel();

            return View("~/ViewComponents/VenueSearchResult/Default.cshtml", actualModel);
        }
    }
}
