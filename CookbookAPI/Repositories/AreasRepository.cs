using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookbookAPI.Data;
using CookbookAPI.Entities;
using CookbookAPI.Repositories.Interfaces;

namespace CookbookAPI.Repositories
{
    public class AreasRepository : BaseRepository<Area, CookbookDbContext>, IAreasRepository<Area>
    {
        public AreasRepository(CookbookDbContext context) : base(context)
        {
        }
    }
}
