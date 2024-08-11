
namespace MovieReservation.Model
{
    public class Ticket
    {
        public int TicketId { get; set; }
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string CustomerName { get; set; }
        public int SeatNumber { get; set; }
        public string ShowTime { get; set; }
    }
}
