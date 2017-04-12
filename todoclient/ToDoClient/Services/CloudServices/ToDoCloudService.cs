using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ORM;
using todoclient.Infrastructure;
using todoclient.Models;

namespace todoclient.Services.CloudServices
{
    /// <summary>
    /// Works with ToDo backend.
    /// </summary>
    public class ToDoCloudService
    {

        /// <summary>
        /// The service URL.
        /// </summary>
        private readonly string serviceApiUrl = ConfigurationManager.AppSettings["ToDoServiceUrl"];

        /// <summary>
        /// The url for getting all todos.
        /// </summary>
        private const string GetAllUrl = "ToDos?userId={0}";

        /// <summary>
        /// The url for updating a todo.
        /// </summary>
        private const string UpdateUrl = "ToDos";

        /// <summary>
        /// The url for a todo's creation.
        /// </summary>
        private const string CreateUrl = "ToDos";

        /// <summary>
        /// The url for a todo's deletion.
        /// </summary>
        private const string DeleteUrl = "ToDos/{0}";

        private readonly HttpClient httpClient;
        private readonly ToDoContext db;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public ToDoCloudService()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            db = new ToDoContext();
        }

        /// <summary>
        /// Gets all todos for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of todos.</returns>
        public IList<ToDoModel> GetItems(int userId)
        {
            var dataAsString = httpClient.GetStringAsync(string.Format(serviceApiUrl + GetAllUrl, userId)).Result;
            return JsonConvert.DeserializeObject<IList<ToDoModel>>(dataAsString);
        }

        /// <summary>
        /// Creates a todo. UserId is taken from the model.
        /// </summary>
        /// <param name="item">The todo to create.</param>
        public HttpResponseMessage CreateItem(ToDoModel item)
        {
            return httpClient.PostAsJsonAsync(serviceApiUrl + CreateUrl, item).Result;
        }

        /// <summary>
        /// Updates a todo.
        /// </summary>
        /// <param name="item">The todo to update.</param>
        public void UpdateItem(ToDoModel item)
        {
            httpClient.PutAsJsonAsync(serviceApiUrl + UpdateUrl, item)
                .Result.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Deletes a todo.
        /// </summary>
        /// <param name="id">The todo Id to delete.</param>
        public HttpResponseMessage DeleteItem(int id)
        {
            return httpClient.DeleteAsync(string.Format(serviceApiUrl + DeleteUrl, id))
                .Result;
        }
    }
}