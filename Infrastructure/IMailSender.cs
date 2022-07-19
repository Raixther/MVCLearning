namespace MVCLearning.Infrastructure
{
	public interface IMailSender
	{
        void Send(string senderName,
         string to,
         string subject,
         string htmlBody,
         string? senderEmail = null,
         CancellationToken cancellationToken = default
     );
        Task SendAsync(string senderName,
            string to,
            string subject,
            string htmlBody,
            string? senderEmail = null,
            CancellationToken cancellationToken = default
        );
    }
}
