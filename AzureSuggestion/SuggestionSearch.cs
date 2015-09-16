
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;

namespace AzureSearchSuggestionsDemo
{
    public class SuggestionSearch
    {
        public class Suggestion
        {

            public string value { get; set; }
            public string OfficeName { get; set; }
            public string PersonName { get; set; }
            public string PersonID { get; set; }
        }

       

        private readonly Uri _serviceUri;
        private HttpClient _httpClient;

        public SuggestionSearch()
        {
            _serviceUri = new Uri("https://" + ConfigurationManager.AppSettings["SearchServiceName"] + ".search.windows.net");
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("api-key", ConfigurationManager.AppSettings["SearchServiceApiKey"]);
        }

        public dynamic Suggest(string searchText, bool fuzzy)
        {
            // Pass the specified suggestion text and return the fields
            Uri uri = new Uri(_serviceUri, "/indexes/offices/docs/suggest?suggesterName=sgOffices&$top=10&$select=PersonName,OfficeName, PersonID&fuzzy=" + fuzzy + "&search=" + Uri.EscapeDataString(searchText));
            HttpResponseMessage response = AzureSearchHelper.SendSearchRequest(_httpClient, HttpMethod.Get, uri);
            AzureSearchHelper.EnsureSuccessfulSearchResponse(response);
            List<Suggestion> suggestionList = new List<Suggestion>();

            foreach (var option in AzureSearchHelper.DeserializeJson<dynamic>(response.Content.ReadAsStringAsync().Result).value)
            {
                Suggestion suggestion = new Suggestion();
                //suggestion.id = (string)option["id"];
                suggestion.value = (string)option["@search.text"];
                suggestion.OfficeName = (string)option["OfficeName"];
                suggestion.PersonName = (string)option["PersonName"];
                suggestion.PersonID = (string)option["PersonID"];
                //suggestion.thumbnail = (string)option["thumbnail"];
                suggestionList.Add(suggestion);
            }

            return suggestionList;
        }
    }
}


