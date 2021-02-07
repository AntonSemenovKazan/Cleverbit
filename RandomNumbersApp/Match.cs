using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RandomNumbersApp.Models;

namespace RandomNumbersApp
{
    public class Match
    {
        public int MatchId { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime Expiration { get; set; }

        public int? WinnerId { get; set; }

        [CanBeNull]
        public Participant Winner { get; set; }

        public List<Participant> Participants { get; set; }
    }
}