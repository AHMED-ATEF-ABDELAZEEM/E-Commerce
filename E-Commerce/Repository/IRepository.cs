namespace E_Commerce.Repository
{
    public interface IRepository <Type>
    {
        Task AddAsync(Type model);
        Task UpdateAsync(Type model);
        Task DeleteAsync(string Id);
        Task<Type> GetByIdAsync(string Id);
        Task<List<Type>> GetAllAsync();
    }
}
