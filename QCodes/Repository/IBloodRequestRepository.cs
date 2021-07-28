using QCodes.DbObjects;
using QCodes.Services;
using System.Threading.Tasks;

namespace QCodes.Repository
{
    public interface IBloodRequestRepository
    {
        Task<BloodRequest> RequestBlood(BloodRequest bloodRequest);
        Task<PaginationService<BloodRequest>> GetAllBloodRequests(BloodRequestsParams bloodRequestsParams);
    }
}