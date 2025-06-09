using MaterialQueueProcessAzure.Model;

namespace MaterialQueueProcessAzure.DAL
{
    public interface IRepository
    {
        void AddMaterial(Material material, ILogger<Worker> _logger, string _connString);
    }
}