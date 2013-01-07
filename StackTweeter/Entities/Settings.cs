using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackTweeter.Entities
{
	public class Settings
	{
		public int Id { get; set; }
		public string StackOverflowApiKey { get; set; }
		public TweetUser DefaultUser { get; set; }
	}
}
