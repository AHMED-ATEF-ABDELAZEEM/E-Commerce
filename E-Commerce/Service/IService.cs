namespace E_Commerce.Service
{
    public interface IService <Type,TypeCreate,TypeUpdate>
    {
        Task<List<Type>> GetAllAsync();
        Task<Type> GetByIdAsync (string id);
        Task AddAsync (TypeCreate model);
        Task UpdateAsync (TypeUpdate model);
        Task DeleteAsync (string Id);
    }
}
