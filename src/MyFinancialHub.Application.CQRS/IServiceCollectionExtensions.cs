using MyFinancialHub.Application.CQRS.Handlers.Dispatchers;

namespace MyFinancialHub.Application.CQRS
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            return services
                .AddScoped<IDispatcher, Dispatcher>()
                .AddHandlers();
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            //TODO: check interface
            var handlers = services.Where(t => t.ServiceType?.FullName?.EndsWith("Handler") ?? false);

            //TODO: check how to register handlers using the assembly scanning from the root project calling it

            /*
            var assembly = typeof(IServiceCollectionExtensions).Assembly;

            // Command Handlers
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            // Query Handlers
            services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
            */
            return services;

        }
    }
}
