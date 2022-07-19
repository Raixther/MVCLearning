using MailKit;
using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit.Text;
using MimeKit;

using MVCLearning.Config;
using MailKit.Security;
using System.Net.Sockets;
using MVCLearning.Exceptions;

namespace MVCLearning.Infrastructure
{
	public class SmtpMailSender:IMailSender,IDisposable,IAsyncDisposable
	{
		private readonly SmtpClient _smtpClient;
		private readonly SmtpConfig _config;

		public SmtpMailSender(IOptions<SmtpConfig> options)
		{
			if (options == null) throw new ArgumentNullException(nameof(options));
			_config = options.Value;
			_smtpClient = new SmtpClient();
		}


        public void Send( string? senderName, string to, string subject, string htmlBody, string? senderEmail, 
       CancellationToken cancellationToken = default)
        {
            if (senderName == null) throw new ArgumentNullException(nameof(senderName));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (htmlBody == null) throw new ArgumentNullException(nameof(htmlBody));

            var fromEmail = senderEmail ?? _config.UserName;
            MimeMessage mimeMessage = CreateMimeMessage(senderName, fromEmail, to, subject, htmlBody);
            try
            {           
                EnsureConnectedAndAuthenticated(cancellationToken);
                _smtpClient.Send(mimeMessage, cancellationToken);
            }
            catch (Exception e) when (
                e is SmtpCommandException or SslHandshakeException or SocketException)
            {
                throw new ConnectionException(e.Message, innerException: e);
            }
        }

        public async Task SendAsync(string senderName, string to, string subject, string htmlBody, string? senderEmail = null,
            CancellationToken cancellationToken = default)
        {
            if (senderName == null) throw new ArgumentNullException(nameof(senderName));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (htmlBody == null) throw new ArgumentNullException(nameof(htmlBody));

            var fromEmail = senderEmail ?? _config.UserName;
            MimeMessage mimeMessage = CreateMimeMessage(senderName, fromEmail, to, subject, htmlBody);

            try
            {
                await EnsureConnectedAndAuthenticatedAsync(cancellationToken);
                await _smtpClient.SendAsync(mimeMessage, cancellationToken);
            }
            catch (Exception e) when (
                e is SmtpCommandException or SslHandshakeException or SocketException)
            {
                throw new ConnectionException(e.Message, innerException: e);
            }
        }

        private static MimeMessage CreateMimeMessage(
     string fromName, string fromEmail, string to, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            var fromAddress = MailboxAddress.Parse(fromEmail);
            fromAddress.Name = fromName;
            message.From.Add(fromAddress);
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlBody };
            return message;
        }

        private void EnsureConnectedAndAuthenticated(CancellationToken cancellationToken)
        {
            if (!_smtpClient.IsConnected)
            {
                _smtpClient.Connect(_config.Host, _config.Port, false, cancellationToken); //_config.SecureSocketOptions
            }
            if (!_smtpClient.IsAuthenticated)
            {
                _smtpClient.Authenticate(_config.UserName, _config.Password, cancellationToken);
            }
        }
        private async Task EnsureConnectedAndAuthenticatedAsync(CancellationToken cancellationToken)
        {
            if (!_smtpClient.IsConnected)
            {
                await _smtpClient.ConnectAsync(_config.Host, _config.Port, false, cancellationToken); //_config.SecureSocketOptions
            }
            if (!_smtpClient.IsAuthenticated)
            {
                await _smtpClient.AuthenticateAsync(_config.UserName, _config.Password, cancellationToken);
            }
        }







        public void Dispose() //ликвидация зависимости
        {
            TaskScheduler.UnobservedTaskException += (sender, args) => Console.WriteLine(args.Exception);
            if (_smtpClient.IsConnected)
            {
                _smtpClient.Disconnect(true);
            }
            _smtpClient.Dispose();
        }
        public async ValueTask DisposeAsync()
        {
            if (_smtpClient.IsConnected)
            {
                //Есть вероятность эксепшена, тогда часть соединений не закроется никогда
                await _smtpClient.DisconnectAsync(true);

            }
            _smtpClient.Dispose();
        }
    }
}
