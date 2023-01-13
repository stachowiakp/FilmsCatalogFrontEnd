using Microsoft.AspNetCore.Mvc;
using System.Net;
using TicketingFrontEnd.Models;

namespace TicketingFrontEnd.Controllers
{
    public class ErrorController : Controller
    {
        public  IActionResult Error([FromQuery(Name = "ErrCode")]string ErrCode )
        {
            CustomError error = new CustomError();
            error.ErrorCode = int.Parse(ErrCode);
            if (error.ErrorCode >= 400 || error.ErrorCode < 500)
            {
                error.ErrorName = "Client error";
                error.Message = "You probably used wrong data. Please, try again.";
            }
            if(error.ErrorCode >= 500)
            {
                error.ErrorName = "Server error";
                error.Message = "Something went wrong with your request due to software bug. Please, inform us about it.";
            }
            return View("Error", error);
            
        }

    }
}
