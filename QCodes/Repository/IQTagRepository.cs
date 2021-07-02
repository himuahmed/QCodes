using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QCodes.DbObjects;

namespace QCodes.Repository
{
    public interface IQTagRepository
    {
        Task<List<QTag>> GetAllTags();
        Task<QTag> GetTagById(string tagId);
    }
}