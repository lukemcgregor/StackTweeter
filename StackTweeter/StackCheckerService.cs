using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StackExchange.StacMan;
using StackExchange.StacMan.Questions;

namespace StackTweeter
{
	public class StackCheckerService
	{
		private const int _interval = 1;
		private DateTime _lastQuery;
		private readonly CancellationTokenSource _cts = new CancellationTokenSource();
		private readonly string _apiKey;
		private readonly Action<Question> _whatToDoWithQuestions;

		public StackCheckerService(string apiKey, Action<Question> whatToDoWithQuestions)
		{
			_apiKey = apiKey;
			var now = DateTime.Now;
			_lastQuery = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
			_lastQuery = _lastQuery.AddMinutes(now.Minute + (_interval - (now.Minute % _interval)));
			_whatToDoWithQuestions = whatToDoWithQuestions;
		}

		public void Start()
		{
			Task.Factory.StartNew(FindNewQuestions, _cts.Token);
		}

		public void Stop()
		{
			_cts.Cancel();
		}

		public async void FindNewQuestions()
		{
			var client = new StacManClient(FilterBehavior.Strict, key: _apiKey);

			var delay = _lastQuery.Subtract(DateTime.Now).Add(new TimeSpan(0, _interval, 0));

			await Task.Delay(delay, _cts.Token);

			while (!_cts.Token.IsCancellationRequested)
			{
				var x = await client.Questions.GetAll(
					"stackoverflow",
					sort: AllSort.Creation,
					fromdate: _lastQuery.ToUniversalTime(),
					todate: _lastQuery.ToUniversalTime().AddMinutes(_interval));

				foreach (var question in x.Data.Items)
				{
					Console.WriteLine("{0} - {1},[{2}]", question.QuestionId, question.Title, String.Join("][", question.Tags));
					_whatToDoWithQuestions.Invoke(question);
				}

				_lastQuery = _lastQuery.AddMinutes(_interval);

				delay = _lastQuery.Subtract(DateTime.Now).Add(new TimeSpan(0, _interval, 0)); ;

				await Task.Delay(delay, _cts.Token);
			}
		}
	}
}
