namespace MovieReservation.Model
{
    public class BookingRequest
    {
        public int MovieId { get; set; }
        public string CustomerName { get; set; }
        public int SeatNumber { get; set; }
        public string ShowTime { get; set; }
        public string MovieName { get; set; }

    }
}
