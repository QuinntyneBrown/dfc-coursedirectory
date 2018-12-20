using System;
using System.Linq;
using Dfc.CourseDirectory.Common;
using Dfc.CourseDirectory.Services;
using Dfc.CourseDirectory.Services.Interfaces;
using Dfc.CourseDirectory.Web.Helpers;
using Dfc.CourseDirectory.Web.RequestModels;
using Dfc.CourseDirectory.Web.ViewComponents.VenueSearchResult;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dfc.CourseDirectory.Models.Models.Venues;
using Dfc.CourseDirectory.Services.Interfaces.ProviderService;
using Dfc.CourseDirectory.Services.Interfaces.VenueService;
using Dfc.CourseDirectory.Services.ProviderService;
using Dfc.CourseDirectory.Services.VenueService;
using Microsoft.AspNetCore.Http;
using Dfc.CourseDirectory.Web.ViewComponents.ProviderSearchResult;

namespace Dfc.CourseDirectory.Web.Controllers
{
    public class VenueSearchController : Controller
    {
        private readonly ILogger<VenueSearchController> _logger;
        private readonly IVenueServiceSettings _venueServiceSettings;
        private readonly IProviderServiceSettings _providerServiceSettings;
        private readonly IVenueService _venueService;
        private readonly IVenueSearchHelper _venueSearchHelper;
        private readonly IProviderSearchHelper _providerSearchHelper;
        private readonly IHttpContextAccessor _contextAccessor;
        private ISession _session => _contextAccessor.HttpContext.Session;
        private readonly IProviderService _providerService;

        public VenueSearchController(
            ILogger<VenueSearchController> logger,
            IOptions<VenueServiceSettings> venueServiceSettings,
            IVenueService venueService,
            IVenueSearchHelper venueSearchHelper,
            IHttpContextAccessor contextAccessor,
            IProviderService providerService,
            IProviderSearchHelper providerSearchHelper,
            IOptions<ProviderServiceSettings> providerServiceSettings)
        {
            Throw.IfNull(logger, nameof(logger));
            Throw.IfNull(venueServiceSettings, nameof(venueServiceSettings));
            Throw.IfNull(venueService, nameof(venueService));
            Throw.IfNull(providerServiceSettings, nameof(providerServiceSettings));
            Throw.IfNull(providerService, nameof(providerService));

            _logger = logger;
            _venueServiceSettings = venueServiceSettings.Value;
            _venueService = venueService;
            _venueSearchHelper = venueSearchHelper;
            _providerSearchHelper = providerSearchHelper;
            _contextAccessor = contextAccessor;
            _providerServiceSettings = providerServiceSettings.Value;
            _providerService = providerService;
        }
        public async Task<IActionResult> Index([FromQuery] VenueSearchRequestModel requestModel)
        {
            _session.SetInt32("UKPRN", Convert.ToInt32(requestModel.SearchTerm));


            VenueSearchResultModel model;
           
            _logger.LogMethodEnter();
            _logger.LogInformationObject("Model", requestModel);

            if (requestModel == null)
            {
                model = new VenueSearchResultModel();
            }
            else
            {
                var criteria = _venueSearchHelper.GetVenueSearchCriteria(
                    requestModel);

                var result = await _venueService.SearchAsync(criteria);

                if (result.IsSuccess && result.HasValue)
                {
                    var items = _venueSearchHelper.GetVenueSearchResultItemModels(result.Value.Value);
                    model = new VenueSearchResultModel(
                        requestModel.SearchTerm,
                        items, null, false);
                }
                else
                {

                    if (result.Value == null)
                    {

                        //If Needed
                        var addDefaultVenueResult = await AddDefaultVenue(requestModel.SearchTerm);
                        //What to do next with result?

                    }




                    model = new VenueSearchResultModel(result.Error);
                }
            }

            _logger.LogMethodExit();
            return ViewComponent(nameof(ViewComponents.VenueSearchResult.VenueSearchResult), model);

        }


        public async Task<bool> AddDefaultVenue(string UKPrn)
        {
            ProviderSearchResultModel providerModel;
            ProviderSearchCriteria providerCriteria = new ProviderSearchCriteria(UKPrn);

            var providerResult = await _providerService.GetProviderByPRNAsync(providerCriteria);

            if (providerResult.IsSuccess && providerResult.HasValue)
            {
                providerModel = new ProviderSearchResultModel(
                    UKPrn,
                    providerResult.Value.Value);
            }
            else
            {
                providerModel = new ProviderSearchResultModel(providerResult.Error);
            }

            foreach (var provider in providerModel.Items)
            {
                if (providerModel.Items.FirstOrDefault() != null)
                {
                    var providerContactType = provider.ProviderContact.Where(s => s.ContactType.Equals("P", StringComparison.InvariantCultureIgnoreCase)) ??
                                              provider.ProviderContact.Where(s => s.ContactType.Equals("L", StringComparison.InvariantCultureIgnoreCase));

                    var venueName = provider.ProviderName;

                    string AddressLine1 = string.Empty;
                    if (!(string.IsNullOrEmpty(providerContactType.FirstOrDefault()?.ContactAddress.PAON.Description)
                        && string.IsNullOrEmpty(providerContactType.FirstOrDefault()?.ContactAddress.StreetDescription)))
                    {
                        AddressLine1 = providerContactType.FirstOrDefault()?.ContactAddress.PAON.Description
                                        + " " + providerContactType.FirstOrDefault()?.ContactAddress.StreetDescription + ", ";
                    }
                    string AddressLine2 = string.Empty;
                    if (providerContactType.FirstOrDefault()?.ContactAddress.Locality != null)
                    {
                        AddressLine2 = providerContactType.FirstOrDefault()?.ContactAddress.Locality.ToString() + ", ";
                    }

                    string AddressLine3 = string.Empty;
                    if (!string.IsNullOrEmpty(providerContactType.FirstOrDefault()?.ContactAddress.Items[0]))
                    {
                        AddressLine3 = providerContactType.FirstOrDefault()?.ContactAddress.Items[0] + ", ";
                    }

                    string PostCode = string.Empty;
                    if (!string.IsNullOrEmpty(providerContactType.FirstOrDefault()?.ContactAddress.PostCode))
                    {
                        PostCode = providerContactType.FirstOrDefault()?.ContactAddress.PostCode;
                    }
                    // Not sure seems to be an object!!!!
                    //Required By VENUE object, what to do??
                    string Town = string.Empty;
                    if (providerContactType.FirstOrDefault()?.ContactAddress.PostTown != null)
                    {
                        Town = providerContactType.FirstOrDefault()?.ContactAddress.PostTown.ToString();
                    }

                    string County = string.Empty;
                    if (providerContactType.FirstOrDefault()?.ContactAddress.Items.FirstOrDefault() != null)
                    {
                        County = providerContactType.FirstOrDefault()?.ContactAddress.Items
                            .FirstOrDefault();
                    }

                    Venue venue = new Venue(
                        null,
                        Convert.ToInt32(UKPrn),
                        venueName,
                        AddressLine1,
                        AddressLine2,
                        AddressLine3,
                       Town,
                        County,
                        PostCode,
                        VenueStatus.Live,
                        "TestUser",
                        DateTime.Now,
                        DateTime.Now
                    );

                    var addedVenue = await _venueService.AddAsync(venue);
                }
            }

            return true;


        }
    }
}