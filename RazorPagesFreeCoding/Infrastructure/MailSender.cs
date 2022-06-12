using MailKit;
using MailKit.Net.Smtp;

using MimeKit;

namespace RazorPagesFreeCoding.Infrastructure
{
	public class MailSender : IMailSender
	{
		public bool SendMail()
		{
			MimeMessage message = new MimeMessage();

			message.From.Add(new MailboxAddress("Tester", "NaN"));

			message.To.Add(MailboxAddress.Parse("NaN"));

			message.Subject ="Добавление продукта";

			message.Body = new TextPart("plain")
			{
				Text = "Продукт добавлен в каталог"
			};

			SmtpClient client = new();

			try
			{
				client.Connect("NaN", 465, true);
				client.Authenticate("NaN", "NaN");
				client.Send(message);
				Console.WriteLine("Email Sent!");
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
				client.Dispose();

			}
		}
	}
}
