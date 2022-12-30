using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using TicketingFrontEnd.Models;

namespace TicketingFrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static Uri BaseUri = new Uri("https://localhost:7145");

        public HttpClient FilmsApi = new HttpClient();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            FilmsApi.BaseAddress = BaseUri;
            FilmsApi.DefaultRequestHeaders.Accept.Clear();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Films()
        {
            
            HttpResponseMessage httpResponse = await FilmsApi.GetAsync("/api/Films");
            if(httpResponse.IsSuccessStatusCode)
            {
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                List<Films> films =JsonConvert.DeserializeObject<List<Films>>(Data);
                return View(films);
            }
            else { return BadRequest(); }
            
        }

        public async Task<IActionResult> FilmReservations([FromQuery(Name = "Title")] string Title)
        {
            HttpResponseMessage httpResponse = await FilmsApi.GetAsync($"/Api/Reservations/GetFilmReservations/{Title}");
            if (httpResponse.IsSuccessStatusCode)
            {
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                List<FilmReservations> reservations = JsonConvert.DeserializeObject<List<FilmReservations>>(Data);
                return View(reservations);
            }
            else { return BadRequest(); }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}