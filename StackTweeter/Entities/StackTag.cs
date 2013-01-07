using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackTweeter.Entities
{
	public class StackTag
	{
		public int Id { get; set; }
		public TweetUser SendOn { get; set; }
		public string Tag { get; set; }
	}
}
