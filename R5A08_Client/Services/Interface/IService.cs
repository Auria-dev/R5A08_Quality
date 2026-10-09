namespace R5A08_Client.Services.Interface;

public interface IService<TRead, TCreate>
    where TRead : class
    where TCreate : class
{
    Task<IEnumerable<TRead>?> GetAllAsync();
    Task<TRead?> GetByIdAsync(int id);
    Task<IEnumerable<TRead>?> SearchByNameAsync(string term);
    Task<bool> AddAsync(TCreate entity);
    Task<bool> UpdateAsync<TUpdate>(int id, TUpdate entity);
    Task<bool> DeleteAsync(int id);
}