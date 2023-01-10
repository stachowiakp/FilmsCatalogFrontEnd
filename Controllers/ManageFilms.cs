using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TicketingFrontEnd.Models;

namespace TicketingFrontEnd.Controllers
{
    public class ManageFilms : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static Uri BaseUri = new Uri("https://localhost:7145");

        public HttpClient FilmsApi = new HttpClient();

        public ManageFilms(ILogger<HomeController> logger)
        {
            _logger = logger;
            FilmsApi.BaseAddress = BaseUri;
            FilmsApi.DefaultRequestHeaders.Accept.Clear();
        }
        public async Task<IActionResult> Films()
        {

            HttpResponseMessage httpResponse = await FilmsApi.GetAsync("/api/Films");
            if (httpResponse.IsSuccessStatusCode)
            {
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                List<Films> films = JsonConvert.DeserializeObject<List<Films>>(Data);
                return View(films);
            }
            else { return BadRequest(); }

        }

        public IActionResult Get()
        {
            return View("NewFilmForm");
        }
        public async Task<IActionResult> Post(NewFilmModel newfilm)
        {
            var StrContent = new StringContent(JsonConvert.SerializeObject(newfilm), Encoding.UTF8, "Application/JSON");

            var message = await FilmsApi.PostAsync("/api/Films", StrContent);
            if (message.IsSuccessStatusCode) { _logger.Log(LogLevel.Information, $"Message complete: {message.IsSuccessStatusCode}"); return RedirectToAction("Films", "ManageFilms"); }
            else { return BadRequest(); }
        }

        public  IActionResult UpdateGet([FromQuery(Name = "FilmID")] string ID) {
            
            ViewBag.FilmID=ID;

            return View("UpdateFilmForm"); }
        public async Task<IActionResult> UpdatePut([FromQuery(Name = "FilmID")] string ID,UpdateFilmModel updateFilm) {
            var StrContent = new StringContent(JsonConvert.SerializeObject(updateFilm), Encoding.UTF8, "Application/JSON");

            var message = await FilmsApi.PutAsync($"/api/Films/{ID}", StrContent);
            if (message.IsSuccessStatusCode) { _logger.Log(LogLevel.Information, $"Message complete: {message.IsSuccessStatusCode}"); return RedirectToAction("Films", "ManageFilms"); }
            else { return BadRequest(); }
        }
        public async Task<IActionResult> Delete([FromQuery(Name = "FilmID")] string FilmID) {
            var Message = await FilmsApi.DeleteAsync($"/Api/Films/{FilmID}");
            if (Message.IsSuccessStatusCode) { return RedirectToAction("Films", "ManageFilms"); }
            else { return BadRequest(); }
        
        }
        public async Task<IActionResult>Details([FromQuery(Name = "FilmID")] string ID) {
            HttpResponseMessage httpResponse = await FilmsApi.GetAsync($"/api/Films/{ID}");
            if (httpResponse.IsSuccessStatusCode)
            {
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                var filmDetails = JsonConvert.DeserializeObject<Films>(Data);
                return View("FilmDetails",filmDetails);
            }
            else { return BadRequest(); }

        }
    }
}
