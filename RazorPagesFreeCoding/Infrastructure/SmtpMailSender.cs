using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using RazorPagesFreeCoding.Infrastructure.Configuration;
using Polly;
using RazorPagesFreeCoding.Infrastructure.Policies;

namespace RazorPagesFreeCoding.Infrastructure
{

	public class SmtpMailSender : IMailSender
	{
		private readonly SmtpCredentials _credentials;

		private readonly SmtpClientPolicy _clientPolicy;
		public SmtpMailSender(IOptions<SmtpCredentials> credentials, SmtpClientPolicy clientPolicy)
		{
			_credentials = credentials.Value;
			_clientPolicy = clientPolicy;
		}
		public  async Task<bool>  SendMail(string from, string body, string subject)
		{
	
			MimeMessage message = new MimeMessage();

			message.From.Add(new MailboxAddress(from,  _credentials.userName ));

			message.To.Add(MailboxAddress.Parse(_credentials.to));

			message.Subject = subject;

			message.Body = new TextPart(body);
		
			SmtpClient client = new();

			
			 try
			{
				client.Connect(_credentials.host, 465, true);
				client.Authenticate(_credentials.userName, _credentials.password);
				await  _clientPolicy.AsyncRetryPolicy.ExecuteAsync(()=>client.SendAsync(message));
				return true;	
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
				return false;
			}
			finally
			{
				client.Disconnect(true);
			}
		}
	}
}
