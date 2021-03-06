﻿using Dfc.CourseDirectory.Common;
using Dfc.CourseDirectory.Web.ViewComponents.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dfc.CourseDirectory.Web.ViewComponents.PostCodeSearchResult
{
    public class PostCodeSearchResultModel : ValueObject<PostCodeSearchResultModel>, IViewComponentModel
    {
        public bool HasErrors => Errors.Count() > 0;
        public IEnumerable<string> Errors { get; }
        public string PostCode { get; set; }
        public string Id { get; set; }

        public string VenueName { get; set; }

        public IEnumerable<PostCodeSearchResultItemModel> Items { get; set; }

        public PostCodeSearchResultModel()
        {
            Errors = new string[] { };
            Items = new PostCodeSearchResultItemModel[] { };
        }

        public PostCodeSearchResultModel(string error)
        {
            Errors = new string[] { error };
            Items = new PostCodeSearchResultItemModel[] { };
        }

        public PostCodeSearchResultModel(
            string postCode,
            string id,
            string venuename,
            IEnumerable<PostCodeSearchResultItemModel> items)
        {
            Throw.IfNullOrWhiteSpace(postCode, nameof(postCode));
            Throw.IfNull(items, nameof(items));

            Errors = new string[] { };
            PostCode = postCode;
            Id = id;
            VenueName = venuename;
            Items = items;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return HasErrors;
            yield return Errors;
            yield return PostCode;
            yield return Id;
            yield return VenueName;
            yield return Items;
        }
    }
}