using Core.Entities;
using Core.Interface;
using Microsoft.Data.SqlClient;
using Dapper;

namespace Application;
public class ProductService
{
    private IGenericRepository<Product> _genericRepository;
    private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=GadgetGourmetDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public ProductService(IGenericRepository<Product> genericRepository)
    {
        _genericRepository = genericRepository;
    }
    
    public async Task<IEnumerable<Product>> GetAll()
    {
        return await _genericRepository.GetAll();
    }

    public async Task<Product> GetById(int id)
    {
        return await _genericRepository.GetById(id);
    }

    public async Task Insert(Product Entity)
    {   
       await _genericRepository.Insert(Entity);
    }

    public async Task Update(Product Entity)
    {
        await _genericRepository.Update(Entity);
    }

    public async Task Delete(int id)
    {
        await _genericRepository.Delete(id);
    }

    public async Task<List<Product>> Search(string searchString)
    {
        List<Product> products;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            string query = @"SELECT * FROM [dbo].[product] WHERE [name] LIKE @s;";
            products = conn.Query<Product>(query, new { s = "%" + searchString + "%" }).ToList();
        }
        return products;
    }
}
