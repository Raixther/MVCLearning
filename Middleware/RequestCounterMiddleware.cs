using System.Collections.Concurrent;
namespace MVCLearning.Middleware
{
	public class RequestCounterMiddleware
	{
		private readonly RequestDelegate _next;

		public static ConcurrentDictionary<PathString, int> requests = new();

		public RequestCounterMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			requests.AddOrUpdate(context.Request.Path, 1, (key, oldValue) => oldValue + 1);

			 await	_next(context);
		}
	}
}
