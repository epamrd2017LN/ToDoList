using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using todoclient.Infrastructure;
using todoclient.Models;
using todoclient.Services.BufferStorageServices;
using todoclient.Services.CloudServices;

namespace todoclient.Controllers
{
    /// <summary>
    /// Processes todo requests.
    /// </summary>
    public class ToDosController : ApiController
    {
        private readonly ToDoCloudService todoCloudService = new ToDoCloudService();
        private readonly ToDoBufferStorageService toDoBufferStorageService = new ToDoBufferStorageService();
        private readonly UserCloudService userCloudService = new UserCloudService();
        private readonly UserBufferStorageService userBufferStorageService = new UserBufferStorageService();

        /// <summary>
        /// Returns all todo-items for the current user.
        /// </summary>
        /// <returns>The list of todo-items.</returns>
        public IList<ToDoModel> Get()
        {
            var userId = userCloudService.GetOrCreateUser();
            userBufferStorageService.CreateUser(new UserModel{Id = userId});
            return toDoBufferStorageService.GetItems(userId);
        }

        /// <summary>
        /// Updates the existing todo-item.
        /// </summary>
        /// <param name="todo">The todo-item to update.</param>
        public void Put(ToDoModel todo)
        {
            todo.UserId = userCloudService.GetOrCreateUser();
            todoCloudService.UpdateItem(todo);
        }

        /// <summary>
        /// Deletes the specified todo-item.
        /// </summary>
        /// <param name="id">The todo item identifier.</param>
        public void Delete(int id)
        {
            var todo = toDoBufferStorageService.GetById(id);

            toDoBufferStorageService.DeleteItem(id);

            Synchronizer synchronizer = new Synchronizer();
            Task.Run(() => synchronizer.NotifyCloudAboutDeleteAsync(todo));
        }

        /// <summary>
        /// Creates a new todo-item.
        /// </summary>
        /// <param name="todo">The todo-item to create.</param>
        public void Post(ToDoModel todo)
        {
            todo.UserId = userCloudService.GetOrCreateUser();
            todo.Status = "add";
            todo.ToDoId = toDoBufferStorageService.AddItem(todo);
            
            Synchronizer synchronizer = new Synchronizer();
            Task.Run(() => synchronizer.NotifyCloudAboutCreateAsync(todo));
        }
    }
}
