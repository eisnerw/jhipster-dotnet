using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Jhipster.Domain.Services.Interfaces;
using Jhipster.Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jhipster.Domain.Services
{
    public class TaskService : ITaskService
    {
        protected readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public virtual async Task<Task> Save(Task task)
        {
            await _taskRepository.CreateOrUpdateAsync(task);
            await _taskRepository.SaveChangesAsync();
            return task;
        }

        public virtual async Task<IPage<Task>> FindAll(IPageable pageable)
        {
            var page = await _taskRepository.QueryHelper()
                .GetPageAsync(pageable);
            return page;
        }

        public virtual async Task<Task> FindOne(long id)
        {
            var result = await _taskRepository.QueryHelper()
                .GetOneAsync(task => task.Id == id);
            return result;
        }

        public virtual async Task Delete(long id)
        {
            await _taskRepository.DeleteByIdAsync(id);
            await _taskRepository.SaveChangesAsync();
        }
    }
}
