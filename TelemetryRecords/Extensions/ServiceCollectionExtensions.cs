using MongoDB.Driver;
using TelemetryRecords.Common.Constants;
using TelemetryRecords.Models.Config;
using TelemetryRecords.Repositories.AssignmentRepository;
using TelemetryRecords.Repositories.AssignmentRepository.Interfaces;
using TelemetryRecords.Repositories.TelemetryDataRepository;
using TelemetryRecords.Repositories.TelemetryDataRepository.Interfaces;
using TelemetryRecords.Services.AssignmentService;
using TelemetryRecords.Services.AssignmentService.Interfaces;
using TelemetryRecords.Services.TelemetryDataService;
using TelemetryRecords.Services.TelemetryDataService.Interfaces;

namespace TelemetryRecords.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebApi(this IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(
                        new Newtonsoft.Json.Converters.StringEnumConverter()
                    );
                });
            services.AddEndpointsApiExplorer();
            return services;
        }

        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbConfiguration>(configuration.GetSection(TelemetryRecordsConstants.Configuration.MONGODB_CONFIG_SECTION));

            services.AddSingleton<IMongoClient>(sp =>
            {
                MongoDbConfiguration config = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MongoDbConfiguration>>().Value;
                return new MongoClient(config.ConnectionString);
            });

            return services;
        }

        public static IServiceCollection AddMongoDbServices(this IServiceCollection services)
        {
            services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            services.AddScoped<IAssignmentService, AssignmentService>();
            services.AddScoped<ITelemetryDataRepository, TelemetryDataRepository>();
            services.AddScoped<ITelemetryDataService, TelemetryDataService>();
            return services;
        }
    }
}
