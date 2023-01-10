namespace TicketingFrontEnd.Models
{
    public class FilmReservations
    {
        public Guid Id { get; set; }
        public Guid FilmId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        
    }
}
