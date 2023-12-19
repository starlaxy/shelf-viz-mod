
namespace shelf_viz_mod.Data.Services
{
    public interface IScopedServiceFactory
    {
        T GetScopedService<T>();
    }

    public class ScopedServiceFactory : IScopedServiceFactory
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ScopedServiceFactory(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public T GetScopedService<T>()
        {
            using var scope = _scopeFactory.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}