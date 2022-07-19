using Microsoft.Extensions.Options;

using MVCLearning.Config;
using MVCLearning.DomainEvents;
using MVCLearning.Exceptions;
using MVCLearning.Infrastructure;
using Polly;

namespace MVCLearning.EventsConsumers
{
    public class ProductAddedEventHandler : BackgroundService
    {
        private readonly ILogger<ProductAddedEventHandler> _logger;
        private CancellationToken _stoppingToken;
        private readonly IMailSender _mailSender;
        private readonly SmtpConfig _smtpConfig;
        public ProductAddedEventHandler(
            ILogger<ProductAddedEventHandler> logger,
            IMailSender mailSender, IOptions<SmtpConfig> options
            )
        {          
            _logger = logger;
            _mailSender = mailSender;
            _smtpConfig = options.Value;
            DomainEventsManager.Register<ProductAdded>(ev => { _ = SendEmailNotification(ev); });
        }

        private async Task SendEmailNotification(ProductAdded ev)
{

            Task SendAsync(CancellationToken cancellationToken)
            {
                return _mailSender.SendAsync(
                    "Почтальен Печкин",
                    _smtpConfig.To,
                    $"В каталог добавили новый товар",
                    $"Добавлен товар: {ev.Product.Name}",
                    cancellationToken: cancellationToken
                );
            }

            var policy = Policy
                .Handle<ConnectionException>() //ретраим только ошибки подключения
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(retryAttempt, 2)),
                    (exception, retryAttempt) =>
                    {
                        _logger.LogWarning(
                            exception, "There was an error while sending email. Retrying: {Attempt}", retryAttempt);
                    });
            var result = await policy.ExecuteAndCaptureAsync(SendAsync, _stoppingToken);
            if (result.Outcome == OutcomeType.Failure)
                _logger.LogError(result.FinalException, "There was an error while sending email");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _stoppingToken = stoppingToken;
            return Task.CompletedTask;
        }
    }
}
