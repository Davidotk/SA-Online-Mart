using Microsoft.AspNetCore.Mvc;
using SAOnlineMart.API.Models; 
using SAOnlineMart.Models;
using SAOnlineMart.MVC.Models; 
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
                var products = await productResponse.Content.ReadAsAsync<IEnumerable<SAOnlineMart.API.Models.Product>>();
                var categories = await categoryResponse.Content.ReadAsAsync<IEnumerable<SAOnlineMart.API.Models.Category>>();

                ViewBag.Categories = categories;

                if (categoryId.HasValue)
                {
                    products = products.Where(p => p.CategoryID == categoryId.Value);
                }

                return View(products);
            }

            return View(new List<SAOnlineMart.API.Models.Product>());
        }

        public IActionResult AddToCart(int productId, int quantity)
        {
            SAOnlineMart.MVC.Models.Cart cart = HttpContext.Session.GetObject<SAOnlineMart.MVC.Models.Cart>("Cart") ?? new SAOnlineMart.MVC.Models.Cart();

            var productResponse = _httpClient.GetAsync($"https://localhost:7178/api/Products/{productId}").Result;
            if (productResponse.IsSuccessStatusCode)
            {
                var product = productResponse.Content.ReadAsAsync<SAOnlineMart.API.Models.Product>().Result;
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
                var product = await productResponse.Content.ReadAsAsync<SAOnlineMart.API.Models.Product>();
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

        public IActionResult Checkout()
        {
            // Get the cart from the session
            SAOnlineMart.MVC.Models.Cart cart = HttpContext.Session.GetObject<SAOnlineMart.MVC.Models.Cart>("Cart") ?? new SAOnlineMart.MVC.Models.Cart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult CompleteCheckout()
        {
            // Get the cart from the session
            SAOnlineMart.MVC.Models.Cart cart = HttpContext.Session.GetObject<SAOnlineMart.MVC.Models.Cart>("Cart");

            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            // Here we would typically create an order in the database

            // Clear the cart after purchase
            cart.Clear();
            HttpContext.Session.SetObject("Cart", cart);

            return RedirectToAction("Index");
        }
    }
}
