using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TicketingFrontEnd.Models
{
    public class NewFilmModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ScreeningDate { get; set; }
    }
}
