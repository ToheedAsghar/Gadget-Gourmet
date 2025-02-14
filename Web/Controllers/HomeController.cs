using Core.Entities;
using Core.Interface;
using Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Http;

namespace Gadget_Gourmet.Controllers
{
    public class HomeController : Controller
    {
        public string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GadgetGourmetDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        private readonly HistoryService _historyService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(HistoryService historyService, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _historyService = historyService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            //_Ihistory.TrackPageVisit("Home", Url.Action("Index", "Home"), System.DateTime.Now.ToString());
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                _historyService.TrackPageVisit("Home Page", Url.Action("Index", "Home"), DateTime.Now.ToString("f"));
            }
            return View();
        }
        public IActionResult Privacy()
        {
            //_Ihistory.TrackPageVisit("Privacy", Url.Action("Privacy", "Home"), System.DateTime.Now.ToString());
            return View();
        }
        public IActionResult ContactUs()
        {
            //_Ihistory.TrackPageVisit("ContactUs", Url.Action("ContactUs", "Home"), System.DateTime.Now.ToString());
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(string? email, string? message)
        {
            // msgs -- not implemented
            // direct to our email client
            return RedirectToAction("ContactUs", "Home");
        }
        public IActionResult History()
        {
            // Retrieve the list of visited pages from the session
            var pagesVisited = HttpContext.Session.GetObjectFromJson<List<History>>("PagesVisited") ?? new List<History>();

            return View(pagesVisited);
        }
        public IActionResult Return()
        {
            return View();
        }
		[HttpPost]
        public IActionResult Return(ReturnModel model)
        {
            if (ModelState.IsValid)
            {
                string invoicePathUrl = null;

                if (model.Invoice != null && model.Invoice.Length > 0)
                {

                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Receipts");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Invoice.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);


                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        model.Invoice.CopyTo(stream);
                    }

                    invoicePathUrl = "/Images/Receipts/" + uniqueFileName;
                }

                // Save the return details including the invoicePathUrl to your database or processing system here

                TempData["Message"] = "Return request submitted successfully.";
                TempData["MessageType"] = "success";

                return RedirectToAction("Confirmation");
            }

            // If the model is not valid, return the view with the model to show validation errors
            return View(model);
        }
        public IActionResult Confirmation()
		{
			return View(); 
		}
	}
}



public static class SessionExtensions
{
    // Store an object as a JSON string in the session
    public static void SetObjectAsJson(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    // Retrieve an object from a JSON string stored in the session
    public static T? GetObjectFromJson<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}
