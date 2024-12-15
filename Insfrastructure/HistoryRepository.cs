using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Insfrastructure;

public class HistoryRepository : IHistoryRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HistoryRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void TrackPageVisit(string pageName, string pageUrl, string pageTime)
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext?.User.Identity.IsAuthenticated == true)
        {
            var session = httpContext.Session;

            // Get the logged-in user's ID
            string userId = httpContext.User.Identity.Name;

            // Create a session key specific to the user
            string sessionKey = $"PagesVisited_{userId}";

            // Retrieve the serialized JSON string from the session
            string? pagesVisitedJson = session.GetString(sessionKey);

            // Deserialize the JSON string into a List<History>
            List<History> pagesVisited = string.IsNullOrEmpty(pagesVisitedJson) ? new List<History>() : JsonConvert.DeserializeObject<List<History>>(pagesVisitedJson);

            // Add the current page to the list
            pagesVisited.Add(new History { PageName = pageName, PageUrl = pageUrl, PageTime = pageTime });

            // Serialize the updated list back to JSON
            string updatedPagesVisitedJson = JsonConvert.SerializeObject(pagesVisited);

            // Store the updated JSON string in the session
            session.SetString(sessionKey, updatedPagesVisitedJson);
        }
    }
}
