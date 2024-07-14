using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeZoneTask.Repositories.Interfaces;

namespace CodeZoneTask.Repositories.Repositories
{
    public class StoreRepository : GenericRepository<Store>, IStoreRepository
    {
        private readonly DbContext _context;

        public StoreRepository(DbContext context) : base(context)
        {
            _context = context;
        }
    }
}
