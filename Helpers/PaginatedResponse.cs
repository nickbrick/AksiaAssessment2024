using Azure.Core;
using Microsoft.AspNetCore.WebUtilities;
using System.Collections;
using System.Web;

namespace AksiaAssessment2024.Helpers
{
    public record PaginatedResponse<T>
    {
        public IEnumerable<T> Data { get; }
        public string NextPage { get; }
        public string PrevPage { get; }

        public PaginatedResponse(IEnumerable<T> data, HttpRequest request, string pageNumberName, int pageNumber)
        {
            var path = $"{request.Scheme}://{request.Host}{request.Path}";
            Data = data;
            NextPage = data.Any() ? path + request.QueryString.Set(pageNumberName, (pageNumber + 1).ToString()) : "";
            PrevPage = pageNumber > 1 ? path + request.QueryString.Set(pageNumberName, (pageNumber - 1).ToString()) : "";
        }
    }

    public static class QueryStringExtensions
    {
        public static QueryString Set(this QueryString queryString, string key, string value)
        {
            var queryParams = QueryHelpers.ParseQuery(queryString.Value);

            if (queryParams.ContainsKey(key))
                queryParams[key] = value;
            else
                queryParams.Add(key, value);

            return new QueryString(QueryHelpers.AddQueryString("", queryParams));
        }
    }
}

