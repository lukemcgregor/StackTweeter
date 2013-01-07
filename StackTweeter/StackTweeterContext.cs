using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackTweeter.Entities;

namespace StackTweeter
{
	public class StackTweeterContext:DbContext
	{
		public StackTweeterContext() : base("StaticVoid.StackTweeter") { }

		public DbSet<TweetUser> TweetUsers { get; set; }
		public DbSet<StackTag> StackTags { get; set; }
		public DbSet<Settings> Settings { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<StackTag>().HasRequired(t => t.SendOn).WithMany(u=>u.Tags);
		}
	}
}
