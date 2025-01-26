using Microsoft.AspNetCore.Http;

namespace Core.Entities
{
	public class ReturnModel
	{
			public string Name { get; set; }
			public string Email { get; set; }
			public string PhoneNumber { get; set; }
			public string Reason { get; set; }

			public string InvoicePathUrl {get; set;}
			public IFormFile Invoice { get; set; }
	}
}