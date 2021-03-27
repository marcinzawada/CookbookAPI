using System.Collections.Generic;
using System.Threading.Tasks;

namespace CookbookAPI.Mappers.Interfaces
{
    public interface IDtoToEntityMapper<TDto, TEntity>
    {
        public Task<TEntity> MapAsync(TDto dto);

        public Task<List<TEntity>> MapManyAsync(List<TDto> dto);


    }
}
