using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using StackTweeter.Entities;

namespace StackTweeter
{
	public class CachedTags
	{
		private Dictionary<string, TweetUser> _tagMap = new Dictionary<string, TweetUser>();
		public CachedTags()
		{
			using (var ctx = new StackTweeterContext())
			{
				// Count how many posts the blog has 
				//var postCount = ctx.Entry(a)
				//					  .Collection(b => b.Tags)
				//					  .Query()
				//					  .Count();

				foreach (var user in ctx.TweetUsers.Include(t => t.Tags).AsNoTracking())
				{
					foreach (var tag in user.Tags)
					{
						_tagMap.Add(tag.Tag, user);
					}
				}
			}
		}

		public IEnumerable<TweetUser> GetUsersForTags(IEnumerable<String> tags)
		{
			var users = new List<TweetUser>();
			foreach (var tag in tags)
			{
				if(_tagMap.ContainsKey(tag))
				{
					users.Add(_tagMap[tag]);
				}
			}

			return users.Distinct();
		}
	}
}
