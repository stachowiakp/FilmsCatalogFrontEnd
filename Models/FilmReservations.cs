namespace TicketingFrontEnd.Models
{
    public class FilmReservations
    {
        public Guid Id { get; init; }
        public Guid FilmId { get; init; }

        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }

    }
}
