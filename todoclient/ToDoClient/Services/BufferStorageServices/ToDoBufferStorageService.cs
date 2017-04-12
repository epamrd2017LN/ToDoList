using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ORM;
using todoclient.Infrastructure;
using todoclient.Models;

namespace todoclient.Services.BufferStorageServices
{
    public class ToDoBufferStorageService
    {
        private ToDoContext db = new ToDoContext();

        public void DeleteItem(int id)
        {
            ToDo toDoModel = db.ToDos.FirstOrDefault(todo => todo.ToDoId == id);
            if ((toDoModel != null) && (toDoModel.Status != Synchronizer.nonSynchronizedId))
            {
                db.ToDos.Remove(toDoModel);
                db.SaveChanges();
            }
        }

        public int AddItem(ToDoModel toDoModel)
        {
            var todo = toDoModel.ToOrmToDo();
            db.ToDos.Add(todo);
            db.SaveChanges();
            var id = todo.ToDoId;
            return id;
        }


        public void UpdateItem(ToDoModel toDoModel)
        {
            var todo = toDoModel.ToOrmToDo();
            db.ToDos.Attach(todo);
            db.Entry(todo).State = EntityState.Modified;
            db.SaveChanges();

        }
        public IList<ToDoModel> GetItems(int userId)
        {
            IList<ToDo> ormToDos = db.ToDos
                .Where(todo => todo.UserId == userId && 
                    todo.Status != Synchronizer.deleteOperationName)
                .ToList();
            return ormToDos.Select(todo => todo.ToModelToDo()).ToList();
        }

        public ToDoModel GetById(int id)
        {
            return db.ToDos.FirstOrDefault(t => t.ToDoId == id).ToModelToDo();
        }
    }
}