using Application;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Gadget_Gourmet.Controllers
{
	public class AdminController : Controller
	{
		private readonly ProductService _productService;
		private readonly UserService _userService;
		private readonly CategoryService _categoryService;

        public AdminController(ProductService productService, UserService userService, CategoryService categoryService)
		{
            _productService = productService;
            _userService = userService;
            _categoryService = categoryService;
		}

		/* All CRUD Operations for Admin will be Shown in Index */
		public IActionResult Index(string Message)
		{
			return View();
		}

        #region Product CRUD Operations

        #region Add Product
        public async Task<IActionResult> AddProduct(int? id)
		{
			Product prod;
			if (id.HasValue && id.Value > 0)
			{
				prod = await _productService.GetById(id.Value) ?? new Product();
			}
			else
			{
				prod = new Product();
			}
			return View(prod);
		}
		[HttpPost]
		public async Task<IActionResult> AddProduct(Product prod)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, "Resolve the Errors and Try Again!");
				return View();
			}

            await _productService.Insert(prod);

			string msg = string.Empty;
			return RedirectToAction("Index", "Admin", new { message = msg });
		}

        #endregion

        #region Remove Product
        [HttpGet]
		public IActionResult RemoveProduct()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RemoveProduct(int id)
		{
			Product check = await _productService.GetById(id);
			if (check != null)
			{
				try
				{
                    await _productService.Delete(id);
					return RedirectToAction("Index", "Admin", new { msg = "Product Removed Successfully!" });
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = "An error occurred while trying to remove the product.";
					return View();
				}
			}
			else
			{
				ViewBag.ErrorMessage = "Product not found!";
				return View();
			}
		}

        #endregion

        #region Update Product

        [HttpGet]
		public IActionResult UpdateProduct()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            Product check = await _productService.GetById(id);
            if (check != null)
            {
                try
                {
					return RedirectToAction("UpdateProductDetails", "Admin", new { Id=id });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Something Went Wrong on Our End!";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Product not found!";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProductDetails(int Id)
        {
            Product product = await _productService.GetById(Id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductDetails(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingProduct = await _productService.GetById(product.Id);
                    if (existingProduct == null)
                    {
                        ViewBag.ErrorMessage = "Product not found!";
                        return View(product);
                    }

                    // Update the fields you want to allow modification, but ensure the Id remains intact
                    existingProduct.Name = product.Name;
                    existingProduct.Description = product.Description;
                    existingProduct.Weight = product.Weight;
                    existingProduct.Price = product.Price;
                    existingProduct.Color = product.Color;
                    existingProduct.Manufacturer = product.Manufacturer;
                    existingProduct.Category = product.Category;
                    existingProduct.Quantity = product.Quantity;

                    await _productService.Update(existingProduct);
                    return RedirectToAction("Index", "Admin", new { msg = "Product Updated Successfully!" });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Something Went Wrong on Our End!";
                    return View(product);
                }
            }

            ModelState.AddModelError(string.Empty, "Please Resolve the Errors and Try Again!");
            return View(product);
        }
        #endregion

        #region  View All Products
        public async Task<IActionResult> AllProducts()
		{
			List<Product> products = (await _productService.GetAll()).ToList();
			return View(products);
		}
        #endregion

        #endregion

        #region User CRUD Operations

        #region Add User
        [HttpGet]
		public async Task<IActionResult> AddUser(int? id)
		{
			User user;
			if (id.HasValue && id.Value > 0)
			{
				user = await _userService.GetById(id.Value) ?? new User();
			}
			else
			{
				user = new User();
				user.UserName = string.Empty;
				user.Name = string.Empty;
				user.Email = string.Empty;
				user.Phone = string.Empty;
				user.Address = string.Empty;
				user.DateOfBirth = DateTime.MinValue;
				user.Password = string.Empty;
			}
			return View(user);
		}

		[HttpPost]
		public async Task<IActionResult> AddUser(User user)
		{
			if (!ModelState.IsValid)
			{
				ModelState.AddModelError(string.Empty, "Resolve the Errors and Try Again!");
				return View();
			}

            await _userService.Insert(user);

			string msg = "User Added Successfully!";
			return RedirectToAction("Index", "Admin", new { message = msg });
		}

        #endregion

        #region Remove User
        [HttpGet]
		public IActionResult RemoveUser()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> RemoveUser(int id)
		{
			User check = await _userService.GetById(id);
			if (check != null)
			{
				try
				{
					await _userService.Delete(id);
					return RedirectToAction("Index", "Admin", new { msg = "User Removed Successfully!" });
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = "An error occurred while trying to remove the User";
					return View();
				}
			}
			else
			{
				ViewBag.ErrorMessage = "User not found!";
				return View();
			}
		}
        #endregion

        #region Update User
        [HttpGet]
		public IActionResult UpdateUser()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateUser(int id)
		{
			User check = await _userService.GetById(id);
			if (check != null)
			{
				try
				{
					return RedirectToAction("UpdateUserDetails", "Admin", new { Id = id });
				}
				catch (Exception ex)
				{
					ViewBag.ErrorMessage = "Something Went Wrong on Our End!";
					return View();
				}
			}
			else
			{
				ViewBag.ErrorMessage = "User not found!";
				return View();
			}
		}

		[HttpGet]
		public async Task<IActionResult> UpdateUserDetails(int Id)
		{
			User user = await _userService.GetById(Id);

			return View(user);
		}

        [HttpPost]
        public async Task<IActionResult> UpdateUserDetails(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _userService.GetById(user.Id);
                    if (existingUser == null)
                    {
                        ViewBag.ErrorMessage = "User not found!";
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
                    return RedirectToAction("Index", "Admin", new { msg = "User Updated Successfully!" });
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

        #endregion

        #region View All Users
        public async Task<IActionResult> AllUsers()
		{
			List<User> users = (await _userService.GetAll()).ToList();
			return View(users);
		}
        #endregion

        #endregion

        #region Category CRUD Operations

        #region Add Category
        [HttpGet]
        public async Task<IActionResult> AddCategory(int? id)
        {
            Category category;
            if (id.HasValue && id.Value > 0)
            {
				category = await _categoryService.GetById(id.Value) ?? new Category();
            }
            else
            {
                category = new Category();
                category.Name = String.Empty;
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Resolve the Errors and Try Again!");
                return View();
            }

            await _categoryService.Insert(category);

            string msg = "Category Added Successfully!";
            return RedirectToAction("Index", "Admin", new { message = msg });
        }
		#endregion

		#region Remove Category
		[HttpGet]
        public IActionResult RemoveCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCategory(int id)
        {
            Category check = await _categoryService.GetById(id);
            if (check != null)
            {
                try
                {
                    await _categoryService.Delete(id);
                    return RedirectToAction("Index", "Admin", new { msg = "Category Removed Successfully!" });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred while trying to remove the Category";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Category not found!";
                return View();
            }
        }
		#endregion

		#region Update Category

		[HttpGet]
        public IActionResult UpdateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            Category check = await _categoryService.GetById(id);
            if (check != null)
            {
                try
                {
                    return RedirectToAction("UpdateCategoryDetails", "Admin", new { Id = id });
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Something Went Wrong on Our End!";
                    return View();
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Category not found!";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategoryDetails(int Id)
        {
            Category category = await _categoryService.GetById(Id);
            if (category != null)
            {
                return View(category);
            }
            else
            {
                return RedirectToAction("UpdateCategory", "Admin");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategoryDetails(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingCategory = await _categoryService.GetById(category.Id);
                    if (existingCategory == null)
                    {
                        ViewBag.ErrorMessage = "Category not found!";
                        return View(category);
                    }

					existingCategory.Name = string.IsNullOrEmpty(category.Name) ? category.Name : category.Name;

                    await _categoryService.Update(existingCategory);
                    return RedirectToAction("Index", "Admin", new { msg = "Category Updated Successfully!" });
                }
                catch (Exception)
                {
                    ViewBag.ErrorMessage = "Something Went Wrong on Our End!";
                    return View(category);
                }
            }

            ModelState.AddModelError(string.Empty, "Please Resolve the Errors and Try Again!");
            return View(category);
        }

        #endregion

        #region View All Categories
        public async Task<IActionResult> AllCategories()
        {
            List<Category> categories  = (await _categoryService.GetAll()).ToList();
            return View(categories);
        }
        #endregion

        #endregion
    }
}
