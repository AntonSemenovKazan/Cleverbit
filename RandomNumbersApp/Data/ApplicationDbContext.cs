using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RandomNumbersApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomNumbersApp.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Match> Matches { get; set; }

        public DbSet<Participant> Participants { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Match>().HasMany(m => m.Participants).WithOne(p => p.Match).HasForeignKey(p => p.MatchId);
            builder.Entity<Match>().HasOne(m => m.Winner).WithMany().HasForeignKey(m => m.WinnerId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
