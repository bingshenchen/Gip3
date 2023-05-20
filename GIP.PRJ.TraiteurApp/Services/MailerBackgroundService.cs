using GIP.PRJ.TraiteurApp.Services.Interfaces;
using System.Threading;

namespace GIP.PRJ.TraiteurApp.Services
{
    public class MailerBackgroundService : BackgroundService
    {
        private readonly IMailerWorkerService _worker;

        public MailerBackgroundService(IMailerWorkerService worker)
        {
            _worker = worker;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _worker.DoWorkAsync();
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
