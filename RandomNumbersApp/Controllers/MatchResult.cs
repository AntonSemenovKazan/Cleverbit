using System;

namespace RandomNumbersApp.Controllers
{
    public class MatchResult
    {
        public DateTimeOffset Expiration { get; set; }

        public string WinnerName { get; set; }

        public int? WinnerNumber { get; set; }
    }
}