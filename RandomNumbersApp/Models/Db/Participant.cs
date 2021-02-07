using System;
using System.ComponentModel.DataAnnotations;

namespace RandomNumbersApp.Models.Db
{
    public class Participant
    {
        public int ParticipantId { get; set; }

        public int MatchId { get; set; }

        [Required]
        public Match Match { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

        public int Number { get; set; }

        public DateTime CreationTime { get; set; }
    }
}