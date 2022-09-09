using adam_and_eve_api.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Reflection.Emit;

namespace adam_and_eve_api
{
	public class Database : DbContext
	{

		public DbSet<User> users { get; set; }

		public DbSet<UserRecord> records { get; set; }

		public Database(DbContextOptions<Database> opt) : base(opt)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<UserRecord>()
				.HasOne(p => p.user)
				.WithMany(b => b.usersRecords);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseLazyLoadingProxies();		
		}
	}
}
