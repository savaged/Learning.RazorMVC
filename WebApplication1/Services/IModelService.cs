using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IModelService
    {
        Task<T> Create<T>() where T : IModel, new();
        Task<IEnumerable<T>> Index<T>() where T : IModel;
        Task<T> Show<T>(int id) where T : IModel;
        Task<T> Store<T>(T model) where T : IModel;
        Task<T> Edit<T>(int id) where T : IModel;
        Task Update<T>(T model) where T : IModel;
        Task Destroy<T>(int id) where T : IModel;
    }
}