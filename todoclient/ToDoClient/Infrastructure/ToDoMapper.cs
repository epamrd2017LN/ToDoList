using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ORM;
using todoclient.Models;

namespace todoclient.Infrastructure
{
    public static class ToDoMapper
    {
        public static ToDoModel ToModelToDo(this ToDo toDo)
        {
            if (toDo == null)
            {
                return null;
            }
            ToDoModel toDoItemViewModel = new ToDoModel();
            toDoItemViewModel.UserId = toDo.UserId;
            toDoItemViewModel.ToDoId = toDo.ToDoId;
            toDoItemViewModel.Name = toDo.Name;
            toDoItemViewModel.IsCompleted = toDo.IsCompleted;
            toDoItemViewModel.Status = toDo.Status;
            return toDoItemViewModel;
        }

        public static ToDo ToOrmToDo(this ToDoModel toDoItemViewModel)
        {
            if (toDoItemViewModel == null)
            {
                return null;
            }
            ToDo toDo = new ToDo();
            toDo.UserId = toDoItemViewModel.UserId;
            toDo.ToDoId = toDoItemViewModel.ToDoId;
            toDo.Name = toDoItemViewModel.Name;
            toDo.IsCompleted = toDoItemViewModel.IsCompleted;
            toDo.Status = toDoItemViewModel.Status;
            return toDo;
        }
    }
}