using System.Collections.Generic;
using System.Threading.Tasks;
using RandomNumbersApp.Data;
using RandomNumbersApp.Models.FrontEnd;

namespace RandomNumbersApp.Services
{
    public interface IMatchService
    {
        Task FinishInProcessMatches(ApplicationDbContext dbContext);

        Task<IEnumerable<MatchResult>> Get(ApplicationDbContext dbContext, int take);

        Task<ParticipantMatch> GetCurrentParticipantMatch(ApplicationDbContext dbContext, string userId);

        Task<ParticipantMatch> PlayNow(ApplicationDbContext dbContext, string userId);
    }
}