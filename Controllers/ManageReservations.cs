using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using TicketingFrontEnd.Models;

namespace TicketingFrontEnd.Controllers
{
    public class ManageReservations : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static Uri BaseUri = new Uri("https://localhost:7145");

        public HttpClient FilmsApi = new HttpClient();
        public static Guid FilmGUID;

        public ManageReservations(ILogger<HomeController> logger)
        {
            _logger = logger;
            FilmsApi.BaseAddress = BaseUri;
            FilmsApi.DefaultRequestHeaders.Accept.Clear();
        }
        

        public async Task<IActionResult> FilmReservations([FromQuery(Name = "FilmID")] Guid FilmID )
        {
            HttpResponseMessage httpResponse = await FilmsApi.GetAsync($"/Api/Reservations/GetReservationsByFilmID/{FilmID}");
            if (httpResponse.IsSuccessStatusCode)
            {
                FilmGUID = FilmID;
                var Data = httpResponse.Content.ReadAsStringAsync().Result;
                List<FilmReservations> reservations = JsonConvert.DeserializeObject<List<FilmReservations>>(Data);
                return View(reservations);
            }
            else { return BadRequest(); }
        }

        public ActionResult NewReservationGet() { return View("NewResForm"); }
        public async Task<IActionResult> NewReservationPost(FilmReservations res) {
            res.FilmId = FilmGUID;
        var data = JsonConvert.SerializeObject(res);
        var content = new StringContent(data,Encoding.UTF8,"Application/JSON");

            var message = await FilmsApi.PostAsync("/Api/Reservations", content);
            if (message.IsSuccessStatusCode)
            {
                return RedirectToAction("Films","ManageFilms");
            }
            else { return BadRequest(); }
        }

        public ActionResult UpdateReservationGet([FromQuery(Name = "ResID")] string ID) { ViewBag.ID = ID; return View("UpdateResForm"); }

        public async Task<IActionResult> UpdateReservationPut([FromQuery(Name = "ResID")] Guid ID, FilmReservations res) {
            res.FilmId=FilmGUID;
            res.Id = ID;
            var data = JsonConvert.SerializeObject(res);
            var content = new StringContent(data, Encoding.UTF8, "Application/JSON");
            var message =  await FilmsApi.PutAsync($"/Api/Reservations/{ID}", content);
            if (message.IsSuccessStatusCode)
            {
                return RedirectToAction("Films","ManageFilms");
            }
            else
            {
                return BadRequest();
            }
        }

        public async Task<IActionResult> ReservationDetails([FromQuery(Name = "ResID")] string ID) {
            var message = await FilmsApi.GetAsync($"/Api/Reservations/GetReservationById/{ID}");
            if (message.IsSuccessStatusCode)
            {
                var messageContent= message.Content.ReadAsStringAsync().Result;
                var Data = JsonConvert.DeserializeObject<FilmReservations>(messageContent);
                return View(Data);
            }
            else { return BadRequest(); }
        }

        public async Task<IActionResult> DeleteReservation([FromQuery(Name = "ResID")] string ID) {
            var message = await FilmsApi.DeleteAsync($"/Api/Reservations/{ID}");
            if(message.IsSuccessStatusCode)
            {
                return RedirectToAction("Films", "ManageFilms");
            }
            else
            {
                return BadRequest();
            }

        }





    }
}
