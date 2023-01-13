using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TicketingFrontEnd.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TicketingFrontEnd.Controllers
{
    //Controller for films models
    public class ManageFilms : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static Uri BaseUri = new Uri("https://localhost:7145"); //URI to FilmsCatalogAPI app (currently localhost, in future in Azure)

        public HttpClient FilmsApi = new HttpClient(); //Init of HttpClient of FilmsCatalogAPI app

        public ManageFilms(ILogger<HomeController> logger)
        {
            _logger = logger;
            FilmsApi.BaseAddress = BaseUri;
            FilmsApi.DefaultRequestHeaders.Accept.Clear();
        }

        public async Task<IActionResult> Films() //Get all films in FilmsCatalogAPI
        {

            var message = await FilmsApi.GetAsync("/api/Films");
            if (message.IsSuccessStatusCode)
            {
                var Data = message.Content.ReadAsStringAsync().Result;
                List<Films> films = JsonConvert.DeserializeObject<List<Films>>(Data);
                return View(films);
            }
            else
            {
                
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        }

        public IActionResult Get() //Shows form for new film
        {
            return View("NewFilmForm");
        }

        public async Task<IActionResult> Post(NewFilmModel newfilm) //Method for posting (creating) new film in FilmsCatalogAPI
        {
            var StrContent = new StringContent(JsonConvert.SerializeObject(newfilm), Encoding.UTF8, "Application/JSON"); //Converting newfilm to JSON and creating StringContent
            var message = await FilmsApi.PostAsync("/api/Films", StrContent);
            if (message.IsSuccessStatusCode) 
            {
                var ReturnData = message.Content.ReadAsStringAsync().Result;
                var filmDetails = JsonConvert.DeserializeObject<Films>(ReturnData);
                return View("FilmDetails", filmDetails);
            }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        } 

        public  IActionResult UpdateGet([FromQuery(Name = "FilmID")] string ID)  //Shows form for updating a film record
        {
            ViewBag.FilmID = ID; //Reference to film to be updated
            return View("UpdateFilmForm"); 
        }

        public async Task<IActionResult> UpdatePut([FromQuery(Name = "FilmID")] string ID,UpdateFilmModel updateFilm) //Method for PUT/UpdateFilmSchedule in FilmsCatalogAPI
        {
            var StrContent = new StringContent(JsonConvert.SerializeObject(updateFilm), Encoding.UTF8, "Application/JSON");

            var message = await FilmsApi.PutAsync($"/api/Films/{ID}", StrContent);
            if (message.IsSuccessStatusCode)
            {
                var ReturnData = message.Content.ReadAsStringAsync().Result;
                var filmDetails = JsonConvert.DeserializeObject<Films>(ReturnData);
                return View("FilmDetails", filmDetails);
            }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        }

        public async Task<IActionResult> Delete([FromQuery(Name = "FilmID")] string FilmID) //Removes film record from FilmsCatalogAPI
        {
            var message = await FilmsApi.DeleteAsync($"/Api/Films/{FilmID}");
            if (message.IsSuccessStatusCode) { return RedirectToAction("Films", "ManageFilms"); }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        
        }
        public async Task<IActionResult>Details([FromQuery(Name = "FilmID")] string ID) //Gets details of film record from FilmsCatalogAPI
        {
            var message = await FilmsApi.GetAsync($"/api/Films/{ID}");
            if (message.IsSuccessStatusCode)
            {
                var Data = message.Content.ReadAsStringAsync().Result;
                var filmDetails = JsonConvert.DeserializeObject<Films>(Data);
                return View("FilmDetails",filmDetails);
            }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }

        }
    }
}
