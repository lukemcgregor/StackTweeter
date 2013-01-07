using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackTweeter.Entities;
using Twitterizer;

namespace StackTweeter
{
	public class TweeterService
	{
		public void SendTweetViaUser(TweetUser user, string question, string url)
		{
			var tokens = new OAuthTokens();
			tokens.AccessToken = user.AccessToken;
			tokens.AccessTokenSecret = user.AccessTokenSecret;
			tokens.ConsumerKey = user.ConsumerKey;
			tokens.ConsumerSecret = user.ConsumerSecret;

			var response = Twitterizer.TwitterStatus.Update(tokens, String.Format("{0} {1}", question.Substring(0, question.Length > 120 ? 120 : question.Length), url));
		}
	}
}
