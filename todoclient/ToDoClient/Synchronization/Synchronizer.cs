using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using todoclient.Models;
using todoclient.Services.BufferStorageServices;
using todoclient.Services.CloudServices;

namespace todoclient.Infrastructure
{
    public class Synchronizer
    {
        public static readonly string addOperationName = "add";
        public static readonly string deleteOperationName = "delete";
        public static readonly string updateOperationName = "update";
        public static readonly string nonSynchronizedId = "nonsync";

        private const int pushAttemptLimit = 5;

        private readonly ToDoBufferStorageService todoBufferStorageService = new ToDoBufferStorageService();
        private readonly UserBufferStorageService userBufferStorageService = new UserBufferStorageService();
        private readonly ToDoCloudService todoCloudService = new ToDoCloudService();

        public void UpdateBufferStorage()
        {
            IList<UserModel> users = userBufferStorageService.GetAllUsers();

            foreach (UserModel user in users)
            {
                IList<ToDoModel> bufferStorageTodos = todoBufferStorageService.GetItems(user.Id);
                IList<ToDoModel> cloudTodos = todoCloudService.GetItems(user.Id);

                IList<ToDoModel> deletedFromCloudTodos = bufferStorageTodos.Except(cloudTodos).ToList();

                foreach (ToDoModel todo in deletedFromCloudTodos)
                {
                    if (todo.Status == null)
                    {
                        todoBufferStorageService.DeleteItem(todo.ToDoId);
                    }
                }

                IList<ToDoModel> addedToCloudTodos = cloudTodos.Except(bufferStorageTodos).ToList();

                foreach (ToDoModel todo in addedToCloudTodos)
                {
                    todoBufferStorageService.AddItem(todo);
                }
            }

        }

        public void NotifyCloudAboutCreateAsync(ToDoModel todo)
        {
            int pushAttempt = 0;
            HttpResponseMessage response;
            while (pushAttempt < pushAttemptLimit)
            {
                if ((todo.Status == addOperationName))
                {
                    response = todoCloudService.CreateItem(todo);

                    if (response.IsSuccessStatusCode)
                    {
                        todo.Status = null;
                        todoBufferStorageService.UpdateItem(todo);
                        return;
                    }
                }
                else
                {
                    return;
                }              
            }

            todoBufferStorageService.DeleteItem(todo.ToDoId);
        }

        public void NotifyCloudAboutDeleteAsync(ToDoModel todo)
        {
            int pushAttempt = 0;
            HttpResponseMessage response;
            while (pushAttempt < pushAttemptLimit)
            {
                if (todo.Status == deleteOperationName) 
                {
                    response = todoCloudService.DeleteItem(todo.ToDoId);

                    if (response.IsSuccessStatusCode)
                    {
                        todo.Status = null;
                        todoBufferStorageService.UpdateItem(todo);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            todoBufferStorageService.DeleteItem(todo.ToDoId);
        }
    }
}