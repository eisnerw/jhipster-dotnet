
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Jhipster.Infrastructure.Data;
using Jhipster.Domain;
using Jhipster.Domain.Repositories.Interfaces;
using Jhipster.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Jhipster.Test.Controllers
{
    public class TasksControllerIntTest
    {
        public TasksControllerIntTest()
        {
            _factory = new AppWebApplicationFactory<TestStartup>().WithMockUser();
            _client = _factory.CreateClient();

            _taskRepository = _factory.GetRequiredService<ITaskRepository>();


            InitTest();
        }

        private const string DefaultTitle = "AAAAAAAAAA";
        private const string UpdatedTitle = "BBBBBBBBBB";

        private const string DefaultDescription = "AAAAAAAAAA";
        private const string UpdatedDescription = "BBBBBBBBBB";

        private readonly AppWebApplicationFactory<TestStartup> _factory;
        private readonly HttpClient _client;
        private readonly ITaskRepository _taskRepository;

        private Task _task;


        private Task CreateEntity()
        {
            return new Task
            {
                Title = DefaultTitle,
                Description = DefaultDescription
            };
        }

        private void InitTest()
        {
            _task = CreateEntity();
        }

        [Fact]
        public async Task CreateTask()
        {
            var databaseSizeBeforeCreate = await _taskRepository.CountAsync();

            // Create the Task
            var response = await _client.PostAsync("/api/tasks", TestUtil.ToJsonContent(_task));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the Task in the database
            var taskList = await _taskRepository.GetAllAsync();
            taskList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testTask = taskList.Last();
            testTask.Title.Should().Be(DefaultTitle);
            testTask.Description.Should().Be(DefaultDescription);
        }

        [Fact]
        public async Task CreateTaskWithExistingId()
        {
            var databaseSizeBeforeCreate = await _taskRepository.CountAsync();
            databaseSizeBeforeCreate.Should().Be(0);
            // Create the Task with an existing ID
            _task.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            var response = await _client.PostAsync("/api/tasks", TestUtil.ToJsonContent(_task));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the Task in the database
            var taskList = await _taskRepository.GetAllAsync();
            taskList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task GetAllTasks()
        {
            // Initialize the database
            await _taskRepository.CreateOrUpdateAsync(_task);
            await _taskRepository.SaveChangesAsync();

            // Get all the taskList
            var response = await _client.GetAsync("/api/tasks?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_task.Id);
            json.SelectTokens("$.[*].title").Should().Contain(DefaultTitle);
            json.SelectTokens("$.[*].description").Should().Contain(DefaultDescription);
        }

        [Fact]
        public async Task GetTask()
        {
            // Initialize the database
            await _taskRepository.CreateOrUpdateAsync(_task);
            await _taskRepository.SaveChangesAsync();

            // Get the task
            var response = await _client.GetAsync($"/api/tasks/{_task.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_task.Id);
            json.SelectTokens("$.title").Should().Contain(DefaultTitle);
            json.SelectTokens("$.description").Should().Contain(DefaultDescription);
        }

        [Fact]
        public async Task GetNonExistingTask()
        {
            var response = await _client.GetAsync("/api/tasks/" + long.MaxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateTask()
        {
            // Initialize the database
            await _taskRepository.CreateOrUpdateAsync(_task);
            await _taskRepository.SaveChangesAsync();
            var databaseSizeBeforeUpdate = await _taskRepository.CountAsync();

            // Update the task
            var updatedTask = await _taskRepository.QueryHelper().GetOneAsync(it => it.Id == _task.Id);
            // Disconnect from session so that the updates on updatedTask are not directly saved in db
            //TODO detach
            updatedTask.Title = UpdatedTitle;
            updatedTask.Description = UpdatedDescription;

            var response = await _client.PutAsync($"/api/tasks/{_task.Id}", TestUtil.ToJsonContent(updatedTask));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the Task in the database
            var taskList = await _taskRepository.GetAllAsync();
            taskList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testTask = taskList.Last();
            testTask.Title.Should().Be(UpdatedTitle);
            testTask.Description.Should().Be(UpdatedDescription);
        }

        [Fact]
        public async Task UpdateNonExistingTask()
        {
            var databaseSizeBeforeUpdate = await _taskRepository.CountAsync();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            var response = await _client.PutAsync("/api/tasks/1", TestUtil.ToJsonContent(_task));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the Task in the database
            var taskList = await _taskRepository.GetAllAsync();
            taskList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task DeleteTask()
        {
            // Initialize the database
            await _taskRepository.CreateOrUpdateAsync(_task);
            await _taskRepository.SaveChangesAsync();
            var databaseSizeBeforeDelete = await _taskRepository.CountAsync();

            var response = await _client.DeleteAsync($"/api/tasks/{_task.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the database is empty
            var taskList = await _taskRepository.GetAllAsync();
            taskList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(Task));
            var task1 = new Task
            {
                Id = 1L
            };
            var task2 = new Task
            {
                Id = task1.Id
            };
            task1.Should().Be(task2);
            task2.Id = 2L;
            task1.Should().NotBe(task2);
            task1.Id = 0;
            task1.Should().NotBe(task2);
        }
    }
}
