using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Jhipster.Domain;

namespace Jhipster.Domain.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Task> Save(Task task);

        Task<IPage<Task>> FindAll(IPageable pageable);

        Task<Task> FindOne(long id);

        Task Delete(long id);
    }
}
