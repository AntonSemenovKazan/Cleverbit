using System;

namespace RandomNumbersApp.Models.FrontEnd
{
    public class ParticipantMatch
    {
        public int? Number { get; set; }

        public DateTimeOffset Expiration { get; set; }
    }
}