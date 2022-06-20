namespace RazorPagesFreeCoding.Infrastructure
{
	public interface IMailSender
	{
		public Task<bool> SendMail(string from, string body, string subject);
	}
}
