using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace TicketingFrontEnd.Models
{
    public class UpdateFilmModel
    {
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ScreeningDate { get; set; }
        
    
    }
}
