using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TicketingFrontEnd.Models;

namespace TicketingFrontEnd.Controllers
{
    //Controller for reservations models
    public class ManageReservations : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static Uri BaseUri = new Uri("https://localhost:7145"); //URI to FilmsCatalogAPI app (currently localhost, in future in Azure)

        public HttpClient FilmsApi = new HttpClient(); //Init of HttpClient of FilmsCatalogAPI app
        public static Guid FilmGUID;

        public ManageReservations(ILogger<HomeController> logger)
        {
            _logger = logger;
            FilmsApi.BaseAddress = BaseUri;
            FilmsApi.DefaultRequestHeaders.Accept.Clear();
        }
        

        public async Task<IActionResult> FilmReservations([FromQuery(Name = "FilmID")] Guid FilmID ) //Gets reservations for given film
        {
            var message = await FilmsApi.GetAsync($"/Api/Reservations/GetReservationsByFilmID/{FilmID}");
            if (message.IsSuccessStatusCode)
            {
                FilmGUID = FilmID; //setting reference to film in interest
                var Data = message.Content.ReadAsStringAsync().Result;
                List<FilmReservations> reservations = JsonConvert.DeserializeObject<List<FilmReservations>>(Data);
                return View(reservations);
            }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        }

        public ActionResult NewReservationGet() { return View("NewResForm"); } //Shows form for new reservation

        public async Task<IActionResult> NewReservationPost(FilmReservations res) //Method for POST/Create new reservation
        {
        res.FilmId = FilmGUID;
        var data = JsonConvert.SerializeObject(res);
        var content = new StringContent(data,Encoding.UTF8,"Application/JSON");

            var message = await FilmsApi.PostAsync("/Api/Reservations", content);
            if (message.IsSuccessStatusCode)
            {
                var messageContent = message.Content.ReadAsStringAsync().Result;
                var Data = JsonConvert.DeserializeObject<FilmReservations>(messageContent);
                return View("ReservationDetails",Data);
            }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        }

        public ActionResult UpdateReservationGet([FromQuery(Name = "ResID")] string ID) //Shows form for update of given reservation
        { ViewBag.ID = ID;ViewBag.FilmID = FilmGUID; return View("UpdateResForm"); }

        public async Task<IActionResult> UpdateReservationPut([FromQuery(Name = "ResID")] Guid ID, FilmReservations res) //Method for PUT/Update of reservation record
        {
            res.FilmId=FilmGUID;
            res.Id = ID;
            var data = JsonConvert.SerializeObject(res);
            var content = new StringContent(data, Encoding.UTF8, "Application/JSON");
            var message =  await FilmsApi.PutAsync($"/Api/Reservations/{ID}", content);
            if (message.IsSuccessStatusCode)
            {
                var messageContent = message.Content.ReadAsStringAsync().Result;
                var Data = JsonConvert.DeserializeObject<FilmReservations>(messageContent);
                return View("ReservationDetails", Data);
            }
            else
            {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        }

        public async Task<IActionResult> ReservationDetails([FromQuery(Name = "ResID")] string ID) //Gets reservation details from FilmsCatalogAPI 
        {
            var message = await FilmsApi.GetAsync($"/Api/Reservations/GetReservationById/{ID}");
            if (message.IsSuccessStatusCode)
            {
                var messageContent= message.Content.ReadAsStringAsync().Result;
                var Data = JsonConvert.DeserializeObject<FilmReservations>(messageContent);
                return View(Data);
            }
            else {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }
        }

        public async Task<IActionResult> DeleteReservation([FromQuery(Name = "ResID")] string ID)  //Removes reservation record with given ID
        {
            var message = await FilmsApi.DeleteAsync($"/Api/Reservations/{ID}");
            if(message.IsSuccessStatusCode)
            {
                
                return RedirectToAction("FilmReservations", "ManageReservations", new {@FilmID=FilmGUID });
            }
            else
            {
                string ErrorCode = message.StatusCode.ToString();
                return RedirectToAction("Error", "ErrorController", new { ErrCode = ErrorCode });
            }

        }





    }
}
