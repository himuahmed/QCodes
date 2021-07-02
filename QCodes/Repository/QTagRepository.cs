using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QCodes.Data;
using QCodes.DbObjects;

namespace QCodes.Repository
{
    public class QTagRepository : IQTagRepository
    {
        private readonly DataContext _dataContext;

        public QTagRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<QTag>> GetAllTags()
        {
            return await _dataContext.QTags.ToListAsync();
        }

        public async Task<QTag> GetTagById(string tagId)
        {
            return await _dataContext.QTags.FirstOrDefaultAsync(qt => qt.TagId == tagId);
        }
    }
}
