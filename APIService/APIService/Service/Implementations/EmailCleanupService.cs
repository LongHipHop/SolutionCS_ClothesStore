using APIService.Repository.Interface;

namespace APIService.Service.Implementations
{
    public class EmailCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailCleanupService> _logger;

        public EmailCleanupService(IServiceProvider serviceProvider, ILogger<EmailCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var accountRepository = scope.ServiceProvider.GetRequiredService<IAccountRepostiory>();

                var expiredAccounts = await accountRepository.GetWaitingAccountsOlderThanAsync(24);
                foreach (var acc in expiredAccounts)
                {
                    await accountRepository.DeleteAsync(acc);
                    _logger.LogInformation($"Deleted expired waiting account: {acc.Email}");
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
