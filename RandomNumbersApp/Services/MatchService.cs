using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RandomNumbersApp.Data;
using RandomNumbersApp.Models.Db;
using RandomNumbersApp.Models.FrontEnd;

namespace RandomNumbersApp.Services
{
    public class MatchService : IMatchService
    {
        private readonly int durationInSec;

        private readonly Random random = new Random();

        private readonly SemaphoreSlim playSemaphoreSlim = new SemaphoreSlim(1, 1);


        private const int MaxNumber = 100;


        public MatchService(IConfiguration configuration)
        {
            durationInSec = int.Parse(configuration["MatchSettings:DurationInSec"]);
        }

        public async Task FinishInProcessMatches(ApplicationDbContext dbContext)
        {
            var now = DateTime.Now;

            var matches = await dbContext.Matches.Where(m => m.Winner == null && m.Expiration < now)
                .Include(m => m.Participants)
                .ToListAsync();

            if (!matches.Any())
                return;

            foreach (var match in matches)
            {
                var winner = match.Participants.OrderByDescending(p => p.Number).ThenBy(p => p.CreationTime).First();
                match.Winner = winner;
                dbContext.Update(match);
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<MatchResult>> Get(ApplicationDbContext dbContext, int take)
        {
            var dbMatches = await dbContext.Matches
                .Where(m => m.Winner != null)
                .Include(m => m.Winner)
                .ThenInclude(p => p.User)
                .OrderByDescending(m => m.Expiration)
                .Take(take)
                .ToListAsync();

            return dbMatches.Select(m => new MatchResult()
            {
                Expiration = m.Expiration,
                WinnerName = m.Winner?.User.UserName,
                WinnerNumber = m.Winner?.Number
            });
        }

        private async Task<Match> GetCurrentMatch(ApplicationDbContext dbContext)
        {
            var now = DateTime.Now;

            var currentMatches = await dbContext.Matches.Where(m => m.Expiration >= now)
                .Include(m => m.Participants)
                .ThenInclude(p => p.User)
                .OrderByDescending(m => m.Expiration)
                .Take(1)
                .ToListAsync();

            return currentMatches.SingleOrDefault();
        }

        public async Task<ParticipantMatch> GetCurrentParticipantMatch(ApplicationDbContext dbContext, string userId)
        {
            var currentMatch = await GetCurrentMatch(dbContext);
            if (currentMatch == null)
                return null;

            var participant = currentMatch.Participants.SingleOrDefault(p => p.User.Id == userId);

            return new ParticipantMatch()
            {
                Expiration = currentMatch.Expiration,
                Number = participant?.Number
            };
        }

        public async Task<ParticipantMatch> PlayNow(ApplicationDbContext dbContext, string userId)
        {
            await FinishInProcessMatches(dbContext);

            await playSemaphoreSlim.WaitAsync();
            try
            {
                var currentMatch = await GetCurrentMatch(dbContext);

                var participant = currentMatch?.Participants.SingleOrDefault(p => p.User.Id == userId);

                var needToSave = false;
                if (currentMatch == null)
                {
                    var now = DateTime.Now;
                    currentMatch = new Match()
                    {
                        StartTime = now,
                        Expiration = now.AddSeconds(durationInSec)
                    };
                    dbContext.Add(currentMatch);
                    needToSave = true;
                }

                if (participant == null)
                {
                    var user = await dbContext.Users.SingleAsync(u => u.Id == userId);
                    participant = new Participant()
                    {
                        User = user,
                        Match = currentMatch,
                        Number = random.Next(MaxNumber)
                    };
                    dbContext.Add(participant);

                    needToSave = true;
                }

                if (needToSave)
                {
                    await dbContext.SaveChangesAsync();
                }

                return new ParticipantMatch()
                {
                    Expiration = currentMatch.Expiration,
                    Number = participant.Number
                };
            }
            finally
            {
                playSemaphoreSlim.Release();
            }
        }
    }
}