using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheGreatFinChallenge.Models;


namespace TheGreatFinChallenge.Models.Data
{
    public class TGFCContext : DbContext
    {

        // Initialize the Sjette constructor.
        public TGFCContext(DbContextOptions<TGFCContext> options) : base(options) { }

        // Initialize the Sjette-User constructor.
        public DbSet<TheGreatFinChallenge.Models.Users> Users { get; set; }

        // Initialize the Sjette-Activities constructor.
        public DbSet<TheGreatFinChallenge.Models.Activities> Activities{ get; set; }

        // Initialize the Sjette-Groups constructor.
        public DbSet<TheGreatFinChallenge.Models.Groups> Groups { get; set; }

        // Initialize the Sjette-GroupMembership constructor.
        public DbSet<TheGreatFinChallenge.Models.GroupMembership> GroupMembership { get; set; }

        // Initialize the Sjette-GroupMembership constructor.
        public DbSet<TheGreatFinChallenge.Models.Photos> Photos { get; set; }


        // Initialize the Sjette-MutualUsers constructor.
        public DbSet<TheGreatFinChallenge.Models.MutualUsers> MutualUsers { get; set; }
    }
}
