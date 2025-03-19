
using MediaEncoder.Domain;
using MediaEncoder.Infrastructure;

namespace MediaEncoder.WebAPI
{
    public class EncoderBackgroundService : BackgroundService
    {

        private readonly MediaEncoderDbContext _mediaEncoderDbContext;
        private readonly IMediaEncoderRepository _mediaEncoderRepository;
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediaEncoder _mediaEncoder;

        public EncoderBackgroundService( IServiceScopeFactory serviceScopeFactory)
        {
            var sp = serviceScopeFactory.CreateScope();
            _mediaEncoderDbContext = sp.ServiceProvider.GetRequiredService<MediaEncoderDbContext>();
            _mediaEncoderRepository = sp.ServiceProvider.GetRequiredService<IMediaEncoderRepository>();
            _logger = sp.ServiceProvider.GetRequiredService<ILogger>();
            _mediaEncoder = sp.ServiceProvider.GetRequiredService<IMediaEncoder>();
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override void Dispose()
        {
            base.Dispose();
            _serviceScopeFactory.CreateScope().Dispose();
          
        }




        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }














    }
}
