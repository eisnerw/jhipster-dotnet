using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JHipsterNet.Core.Pagination;
using JHipsterNet.Core.Pagination.Extensions;
using Jhipster.Domain;
using Jhipster.Domain.Repositories.Interfaces;
using Jhipster.Infrastructure.Data.Extensions;

namespace Jhipster.Infrastructure.Data.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        public TaskRepository(IUnitOfWork context) : base(context)
        {
        }

        public override async Task<Task> CreateOrUpdateAsync(Task task)
        {
            bool exists = await Exists(x => x.Id == task.Id);

            if (task.Id != 0 && exists)
            {
                Update(task);
            }
            else
            {
                _context.AddOrUpdateGraph(task);
            }
            return task;
        }
    }
}
