using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackTweeter.Entities
{
	public class TweetUser
	{
		public int Id { get; set; }
		public string Username { get; set; }
		public string AccessToken { get; set; }
		public string AccessTokenSecret { get; set; }
		public string ConsumerKey { get; set; }
		public string ConsumerSecret { get; set; }
		public ICollection<StackTag> Tags { get; set; }
	}
}
