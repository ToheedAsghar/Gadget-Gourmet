#pragma warning disable CS8602

using Application;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Gadget_Gourmet.Controllers
{
	public class UsersController : Controller
	{
		private readonly UserService _userService;
		public UsersController(UserService userService)
		{
            _userService = userService;
		}
		public IActionResult Index()
		{
			return View();
		}

        [HttpGet]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {

			if (user.UserName.Contains("@"))
			{
				user.Email = user.UserName;
				user.UserName = "None";
			}
			
			// dependency injection
			bool retVal = await _userService.Login(user);

			if (retVal)
            {
				user = await _userService.GetUserByEmailOrUsername(user.UserName);
				// return RedirectToAction("Profile", "Users", new { user.UserName, user.Emai });

				return RedirectToAction("Profile", "Users", new
				{
					userName = user.UserName,
					email = user.Email,
					name = user.Name,
					address = user.Address,
					phone = user.Phone,
					gender = user.Gender,
					dateOfBirth = user.DateOfBirth.ToString("yyyy-MM-dd")
				});

			}
            else
            {
                return RedirectToAction("Login", "Users");
            }
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Signup(User user)
		{
			string[] strs;
			string? Data = string.Empty;
			CookieOptions options = new()
			{
				Expires = DateTime.Now.AddDays(7)
			};

			if (HttpContext.Request.Cookies.ContainsKey("flag"))
			{
				Data = HttpContext.Request.Cookies["flag"];
				strs = Data.Split("|");
			}
			else
			{
				Data = user.UserName + "|" + user.Email;
				HttpContext.Response.Cookies.Append("flag", Data, options);
				strs = new string[] { user.UserName??"None", user.Email??"None" };
			}

			// if user doesn't exists before and signup is successfull than it will be redirected to Home, else
			// will be redirected to SignUp page.
			bool retVal = !(await _userService.IdExists(user)) && (await _userService.Signup(user));

			if (retVal)
			{
				return RedirectToAction("Index", "Home", new { values = strs });
			}
			else
			{
				return RedirectToAction("Signup", "Users");
			}
		}

		public IActionResult Cart()
		{
			return View();
		}

		public async Task<IActionResult> Profile(User user)
		{
			string ? Query = user.UserName == "None" ? user.Email : user.UserName;
			user = await _userService.GetUserByEmailOrUsername(Query);
			return View(user);
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile(int id)
		{
			User user = await _userService.GetById(id);
			if (user != null)
			{
				return View(user);
			}
			else
			{
				ModelState.AddModelError(string.Empty, "User Doesn't Exists !");
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> EditProfile(User user)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var existingUser = await _userService.GetById(user.Id);
					if (existingUser == null)
					{
						ViewBag.ErrorMessage = "User not found!!!!";
						return View(user);
					}

					// Update fields
					existingUser.Name = string.IsNullOrEmpty(user.Name) ? existingUser.Name : user.Name;
					existingUser.UserName = string.IsNullOrEmpty(user.UserName) ? existingUser.UserName : user.UserName;
					existingUser.Address = string.IsNullOrEmpty(user.Address) ? existingUser.Address : user.Address;
					existingUser.Email = string.IsNullOrEmpty(user.Email) ? existingUser.Email : user.Email;
					existingUser.Phone = string.IsNullOrEmpty(user.Phone) ? existingUser.Phone : user.Phone;
					existingUser.DateOfBirth = user.DateOfBirth;
					existingUser.Gender = string.IsNullOrEmpty(user.Gender) ? existingUser.Gender : user.Gender;
					existingUser.Password = user.Password;

                    await _userService.Update(existingUser);
					return RedirectToAction("Profile", "Users", new {user=existingUser});
				}
				catch (Exception)
				{
					ViewBag.ErrorMessage = "Something Went Wrong on Our End!";
					return View(user);
				}
			}

			ModelState.AddModelError(string.Empty, "Please Resolve the Errors and Try Again!");
			return View(user);
		}
	}
}
