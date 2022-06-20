namespace RazorPagesFreeCoding.Infrastructure.Configuration
{
	public class SmtpCredentials
	{
		public const string Credentials = "SmtpCredentials";
		public string host{ get; set; }=string.Empty;
		public string password { get; set; } = string.Empty;
		public string userName { get; set; } = string.Empty;
		public string to { get; set; } = string.Empty;
	}
}
