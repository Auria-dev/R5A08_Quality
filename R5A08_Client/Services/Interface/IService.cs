namespace R5A08_Client.Services.Interface;

public interface IService<TRead, TCreate>
    where TRead : class
    where TCreate : class
{
    Task<IEnumerable<TRead>?> GetAllAsync();
    Task<IEnumerable<TRead>?> SearchByNameAsync(string term);
    Task<bool> AddAsync(TCreate entity);
}