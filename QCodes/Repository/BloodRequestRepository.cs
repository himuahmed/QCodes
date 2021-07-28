using Microsoft.EntityFrameworkCore;
using QCodes.Data;
using QCodes.DbObjects;
using QCodes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QCodes.Repository
{
    public class BloodRequestRepository : IBloodRequestRepository
    {
        private readonly DataContext _dataContext;
        public BloodRequestRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<BloodRequest> RequestBlood(BloodRequest bloodRequest)
        {
            var result = await _dataContext.BloodRequests.AddAsync(bloodRequest);
            var bloodRequestEntity = result.Entity;
            await _dataContext.SaveChangesAsync();
            return bloodRequestEntity;
        }

        public async Task<PaginationService<BloodRequest>> GetAllBloodRequests(BloodRequestsParams bloodRequestsParams)
        {
            IQueryable<BloodRequest> bloodReq = _dataContext.Set<BloodRequest>().Include(p=>p.Person).AsQueryable().OrderByDescending(m => m.createdAt);
            return await PaginationService<BloodRequest>.CreateAsync(bloodReq, bloodRequestsParams.PageNumber, bloodRequestsParams.PageSize);
        }
    }
}
