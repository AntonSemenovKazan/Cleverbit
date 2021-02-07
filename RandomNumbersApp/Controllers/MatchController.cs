using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RandomNumbersApp.Data;

namespace RandomNumbersApp.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MatchController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly MatchService matchService;

        public MatchController(ApplicationDbContext dbContext, MatchService matchService)
        {
            this.dbContext = dbContext;
            this.matchService = matchService;
        }

        [HttpGet]
        public async Task<IEnumerable<MatchResult>> Get(int take = 10)
        {
            await matchService.FinishInProcessMatches(dbContext);
            return await matchService.Get(dbContext, take);
        }

        [Authorize]
        [HttpPost]
        public async Task<ParticipantMatch> Post()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await matchService.PlayNow(dbContext, userId);
        }

        [Authorize]
        [HttpGet]
        public async Task<ParticipantMatch> GetCurrentMatch()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return await matchService.GetCurrentParticipantMatch(dbContext, userId);
        }
    }
}