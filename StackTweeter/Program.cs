using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using StackExchange.StacMan;
using StackExchange.StacMan.Questions;
using StackTweeter.Entities;
using Twitterizer;
using Topshelf;

namespace StackTweeter
{
	public class StackTweeterService
	{
		private readonly StackCheckerService _checker;

		public StackTweeterService()
		{
			var tweetService = new TweeterService();
			var tags = new CachedTags();

			Settings settings = null;

			using (var ctx = new StackTweeterContext())
			{
				settings = ctx.Settings.Include(s => s.DefaultUser).AsNoTracking().Single();
			}

			_checker = new StackCheckerService(settings.StackOverflowApiKey, (question) =>
			{
				try
				{
					var body = question.Title;
					var url = string.Format("http://stackoverflow.com/q/{0}/1070291", question.QuestionId);

					//send to our catchall
					//tweetService.SendTweetViaUser(settings.DefaultUser, body,url);

					//send to the tags
					foreach (var user in tags.GetUsersForTags(question.Tags))
					{
						tweetService.SendTweetViaUser(user, body, url);
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.Message);
					Console.WriteLine(ex.StackTrace);
				}
			});
		}

		public void Start()
		{
			_checker.Start();
		}

		public void Stop()
		{
			_checker.Stop();
		}
	}

	public class Program
	{
		public static void Main(string[] args)
		{

			HostFactory.Run(x =>                                
			{
				x.Service<StackTweeterService>(s =>                       
				{
					s.ConstructUsing(name => new StackTweeterService());
					s.WhenStarted(sc => sc.Start());
					s.WhenStopped(sc => sc.Stop());             
				});
				x.RunAsLocalSystem();

				x.StartAutomatically();
				x.SetDescription("Tweeting stack overflow questions");       
				x.SetDisplayName("StackTweeter");
				x.SetServiceName("StackTweeter");                      
			});   
		}
	}
}
