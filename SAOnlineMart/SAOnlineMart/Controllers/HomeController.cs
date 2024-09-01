using Microsoft.AspNetCore.Mvc;
using SAOnlineMart.API.Models; // This is used for models from your API
using SAOnlineMart.Models;
using SAOnlineMart.MVC.Models; // This is used for your MVC models
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace SAOnlineMart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var productResponse = await _httpClient.GetAsync("https://localhost:7178/api/Products");
            var categoryResponse = await _httpClient.GetAsync("https://localhost:7178/api/Categories");

            if (productResponse.IsSuccessStatusCode && categoryResponse.IsSuccessStatusCode)
            {
                var products = await productResponse.Content.ReadAsAsync<IEnumerable<Product>>();
                var categories = await categoryResponse.Content.ReadAsAsync<IEnumerable<Category>>();

                ViewBag.Categories = categories;

                if (categoryId.HasValue)
                {
                    products = products.Where(p => p.CategoryID == categoryId.Value);
                }

                return View(products);
            }

            return View(new List<Product>());
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            SAOnlineMart.MVC.Models.Cart cart = HttpContext.Session.GetObject<SAOnlineMart.MVC.Models.Cart>("Cart") ?? new SAOnlineMart.MVC.Models.Cart();

            var productResponse = _httpClient.GetAsync($"https://localhost:7178/api/Products/{productId}").Result;
            if (productResponse.IsSuccessStatusCode)
            {
                var product = productResponse.Content.ReadAsAsync<Product>().Result;
                cart.AddItem(product, quantity);
                HttpContext.Session.SetObject("Cart", cart);
            }

            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            SAOnlineMart.MVC.Models.Cart cart = HttpContext.Session.GetObject<SAOnlineMart.MVC.Models.Cart>("Cart");

            if (cart != null)
            {
                cart.RemoveItem(productId);
                HttpContext.Session.SetObject("Cart", cart);
            }

            return RedirectToAction("Cart");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Details(int id)
        {
            var productResponse = await _httpClient.GetAsync($"https://localhost:7178/api/Products/{id}");

            if (productResponse.IsSuccessStatusCode)
            {
                var product = await productResponse.Content.ReadAsAsync<Product>();
                return View(product);
            }

            return NotFound();
        }
        public IActionResult Cart()
        {
            // Get the cart from the session
            SAOnlineMart.MVC.Models.Cart cart = HttpContext.Session.GetObject<SAOnlineMart.MVC.Models.Cart>("Cart") ?? new SAOnlineMart.MVC.Models.Cart();
            return View(cart);
        }


    }
}
