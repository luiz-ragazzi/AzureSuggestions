using AzureSearchSuggestionsDemo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureSuggestion.Controllers
{
    public class SearchController : Controller
    {
        private SuggestionSearch _suggestSearch = new SuggestionSearch();

        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Suggest(string q, bool fuzzy)
        {
            // Call suggest query and return results
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = _suggestSearch.Suggest(q, fuzzy)
            };

        }
    }
}