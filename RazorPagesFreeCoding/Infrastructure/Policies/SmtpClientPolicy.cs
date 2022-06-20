using Polly;
using Serilog;


namespace RazorPagesFreeCoding.Infrastructure.Policies
{
	public class SmtpClientPolicy
	{
		public AsyncPolicy<string> AsyncRetryPolicy{ get; }

		public SmtpClientPolicy()
		{
			AsyncRetryPolicy = Policy.HandleResult<string>(res => res is null).RetryAsync(3);
		}
	}
}
