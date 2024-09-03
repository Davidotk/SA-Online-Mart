using Microsoft.AspNetCore.Mvc;
using SAOnlineMart.API.Models; 
using SAOnlineMart.Models;
using SAOnlineMart.MVC.Models; 
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;


namespace SAOnlineMart.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("https://localhost:7178/api/Products");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadAsAsync<IEnumerable<Product>>();
                return View(products);
            }

            return View(new List<Product>());
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View(new Product());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7178/api/Products", product);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7178/api/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                var product = await response.Content.ReadAsAsync<Product>();
                return View(product);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7178/api/Products/{product.ProductID}", product);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7178/api/Products/{id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return NotFound();
        }
    }
}