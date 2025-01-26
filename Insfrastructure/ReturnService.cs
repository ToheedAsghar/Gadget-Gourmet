//using Core.Entities;
//using Core.Interface;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Hosting; // Added

//namespace Infrastructure;

//public class ReturnService : IReturnService
//{
//    private readonly IWebHostEnvironment _webHostEnvironment;
//    private readonly ILogger<ReturnService> _logger;

//    public ReturnService(IWebHostEnvironment webHostEnvironment, ILogger<ReturnService> logger)
//    {
//        _webHostEnvironment = webHostEnvironment;
//        _logger = logger;
//    }

//    public async Task<bool> SubmitReturnRequestAsync(ReturnModel returnRequest)
//    {
//        try
//        {
//            string invoicePathUrl = null;

//            if (returnRequest.Invoice != null && returnRequest.Invoice.Length > 0)
//            {
//                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Receipts");

//                // Ensure the directory exists
//                if (!Directory.Exists(uploadsFolder))
//                {
//                    Directory.CreateDirectory(uploadsFolder);
//                }

//                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(returnRequest.Invoice.FileName);
//                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

//                using (var stream = new FileStream(filePath, FileMode.Create))
//                {
//                    await returnRequest.Invoice.CopyToAsync(stream);
//                }

//                invoicePathUrl = "/Images/Receipts/" + uniqueFileName;
//            }

//            // TODO: Save the return details including the invoicePathUrl to your database or processing system here

//            return true;
//        }
//        catch (Exception ex)
//        {
//            // Implement proper logging
//            _logger.LogError(ex, "Error submitting return request.");
//            return false;
//        }
//    }
//}
