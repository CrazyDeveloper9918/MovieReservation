using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using MovieReservation.Model;
using System.Net.Sockets;

namespace MovieReservation.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class moviebookingController : ControllerBase
    {
        /// <summary>
        /// Log In user and Book Movie
        /// </summary>

        // Hard-coded list of movies
        private static readonly List<Movie> movies = new List<Movie>
        {
            new Movie { Id = 1, Title = "The Shawshank Redemption", Genre = "Drama", Year = 1994 },
            new Movie { Id = 2, Title = "The Godfather", Genre = "Crime", Year = 1972 },
            new Movie { Id = 3, Title = "The Dark Knight", Genre = "Action", Year = 2008 },
            new Movie { Id = 4, Title = "Pulp Fiction", Genre = "Crime", Year = 1994 },
            new Movie { Id = 5, Title = "Forrest Gump", Genre = "Drama", Year = 1994 }
        };

        #region User Management

        [HttpPost("register")]
        public IActionResult Register()
        {
            var user = new User
            {
                Username = "Jitendra",
                Email = "Jitendra@gmail.com",
                Password = "Test@1234"
            };          

            return Ok(new { message = "User registered successfully", user });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            if (user.Username == "jitendra" && user.Password == "Test@1234")
            {
                return Ok(new { message = "Login successful", username = user.Username });
            }
            else
            {
                return Unauthorized(new { message = "Invalid username or password" });
            }
        }

        // Forgot Password endpoint
        [HttpPost("forgot")]
        public IActionResult ForgotPassword(string email)
        {
            // Check if the provided email matches the hard-coded email
            if (email == "jitendra@123@gmail.com")
            {
                
                return Ok(new { message = "Password reset link has been sent to your email address." });
            }
            else
            {
                return NotFound(new { message = "Email address not found." });
            }
        }

        #endregion Useer Management

        #region Movie Booking

        [HttpGet("all")]
        public IActionResult GetMovies()
        {
            return Ok(movies);
        }

        
        [HttpGet("{movies}/search/{moviename}")]
        public IActionResult SearchMovies(string _movies, string _moviename)
        {
            var matchedMovies = movies
                .Where(m => m.Title.Contains(_moviename, System.StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (matchedMovies.Any())
            {
                return Ok(matchedMovies);
            }
            else
            {
                return NotFound(new { message = "No movies found with the provided name." });
            }
        }

        private static readonly List<Ticket> bookedTickets = new List<Ticket>();

        [HttpPost("ticket")]

        public IActionResult BookTicket([FromBody] BookingRequest bookingRequest)
        {
            var movie = movies.FirstOrDefault(m => m.Id == bookingRequest.MovieId);

            if (movie == null)
            {
                return NotFound(new { message = "Movie not found." });
            }

            var existingTicket = bookedTickets.FirstOrDefault(t => t.MovieId == bookingRequest.MovieId
                && t.ShowTime == bookingRequest.ShowTime
                && t.SeatNumber == bookingRequest.SeatNumber);

            if (existingTicket != null)
            {
                return Conflict(new { message = "The seat is already booked for this showtime." });
            }

            var ticket = new Ticket
            {
                TicketId = bookedTickets.Count + 1,
                MovieId = movie.Id,
                MovieTitle = movie.Title,
                CustomerName = bookingRequest.CustomerName,
                SeatNumber = bookingRequest.SeatNumber,
                ShowTime = bookingRequest.ShowTime
            };

            bookedTickets.Add(ticket);

            return Ok(new { message = "Ticket booked successfully!", ticket });
        }

        [HttpPatch("update/ticket")]
        public IActionResult UpdateTicket([FromBody] BookingRequest updatedTicket)
        {
            var ticket = bookedTickets.FirstOrDefault(t => t.MovieId == updatedTicket.MovieId);

            if (ticket == null)
            {
                return NotFound(new { message = "Movie not found." });
            }

            // Update the ticket details
            ticket.CustomerName = updatedTicket.MovieName;
            ticket.SeatNumber = updatedTicket.SeatNumber;
            ticket.ShowTime = updatedTicket.ShowTime;
            ticket.MovieTitle = updatedTicket.MovieName;


            return Ok(new { message = "Movie updated successfully!", ticket});
        }

        [HttpDelete("delete/moviedelrequest")]
        public IActionResult DeleteTicket([FromBody] Movie movieDeleteRequest)
        {
            var ticket = movies.FirstOrDefault(t => t.Id == movieDeleteRequest.Id);

            if (ticket == null)
            {
                return NotFound(new { message = "Movie not found." });
            }

            movies.Remove(ticket);

            return Ok(new { message = "Movie deleted successfully!" });
        }


        #endregion Movie Booking
    }
}
