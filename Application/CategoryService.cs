using Core.Entities;
using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application;

public class CategoryService
{
    private IGenericRepository<Category> _genericRepository;
    public CategoryService(IGenericRepository<Category> genericRepository)
    {
        _genericRepository = genericRepository;
    }

    public async Task<IEnumerable<Category>> GetAll()
    {
        return await _genericRepository.GetAll();
    }

    public async Task<Category> GetById(int id)
    {
        return await _genericRepository.GetById(id);
    }

    public async Task Insert(Category Entity)
    {
        await _genericRepository.Insert(Entity);
    }

    public async Task Update(Category Entity)
    {
        await _genericRepository.Update(Entity);
    }

    public async Task Delete(int id)
    {
        await _genericRepository.Delete(id);
    }
}
