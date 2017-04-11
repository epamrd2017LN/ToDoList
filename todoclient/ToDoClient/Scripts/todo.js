var tasksManager = function() {

    // appends a row to the tasks list.
    // @parentSelector: selector to append a row to.
    // @item: task object to append.
    var appendItem = function (parentSelector, item) {
        var i = $("<li class='ui-state-default' data-id='" + item.ToDoId + "'></li>")
        i.append("<div class='checkbox'><label><input type='checkbox' class='completed' value='' /><span class='name'>" + item.Name + "</span></label></div>")

        $(parentSelector).append(i);
    }

    var appendItemDone = function (parentSelector, item) {
        var i = $("<li data-id='" + item.ToDoId + "'></li>")
        i.append(item.Name + "<button class='btn btn-default btn-xs pull-right remove-item'><span class='glyphicon glyphicon-remove'></span></button>")

        $(parentSelector).append(i);
    }

    // adds all tasks as rows (deletes all rows before).
    // @parentSelector: selector to append a row to.
    // @tasks: array of tasks to append.
    var displayTasks = function(parentSelector, parenSelectorDone, tasks) {
        $(parentSelector).empty();
        $(parenSelectorDone).empty();
        $.each(tasks, function (i, item) {
            if (!item.IsCompleted) {
                appendItem(parentSelector, item);
            }
            else {
                appendItemDone(parenSelectorDone, item);
            }
        });
    };

    // starts loading tasks from server.
    // @returns a promise.
    var loadTasks = function() {
        return $.getJSON("/api/todos");
    };

    // starts creating a task on the server.
    // @isCompleted: indicates if new task should be completed.
    // @name: name of new task.
    // @return a promise.
    var createTask = function(name) {
        return $.post("/api/todos",
        {
            Name: name
        });
    };

    // starts updating a task on the server.
    // @id: id of the task to update.
    // @isCompleted: indicates if the task should be completed.
    // @name: name of the task.
    // @return a promise.
    var updateTask = function(id, isCompleted, name) {
        return $.ajax(
        {
            url: "/api/todos",
            type: "PUT",
            contentType: 'application/json',
            data: JSON.stringify({
                ToDoId: id,
                IsCompleted: isCompleted,
                Name: name
            })
        });
    };

    // starts deleting a task on the server.
    // @taskId: id of the task to delete.
    // @return a promise.
    var deleteTask = function (taskId) {
        return $.ajax({
            url: "/api/todos/" + taskId,
            type: 'DELETE'
        });
    };

    // returns public interface of task manager.
    return {
        loadTasks: loadTasks,
        displayTasks: displayTasks,
        createTask: createTask,
        deleteTask: deleteTask,
        updateTask: updateTask
    };
}();

$(function () {
    $("#tasks").sortable();
    $("#tasks").disableSelection();

    // load all tasks on startup
    tasksManager.loadTasks()
        .done(function(tasks) {
            tasksManager.displayTasks("#tasks", "#tasks-done", tasks);

            countTodos();
        });

    // add new todo
    $('#addNewToDo').click(function () {

        var name = $('#newToDoName')[0].value;

        if (name != '') {
            tasksManager.createTask(name)
                .then(tasksManager.loadTasks)
                .done(function (tasks) {
                    tasksManager.displayTasks("#tasks", "tasks-done", tasks);

                    countTodos();
                });

            $('#newToDoName').val('');
        }
    
    });

    // mark task as done
    $('#tasks').on('change', '.completed', function () {

        if ($(this).prop('checked')) {

            var li = $(this).parent().parent().parent();
            var taskId = li.attr("data-id");
            var isCompleted = li.find('.completed')[0].checked;
            var name = li.find('.name').text();

            tasksManager.updateTask(taskId, isCompleted, name)
                .then(tasksManager.loadTasks)
                .done(function (tasks) {
                    tasksManager.displayTasks("#tasks", "#tasks-done", tasks);

                    countTodos();
                });
        }
    });

    // remove done task
    $('#tasks-done').on('click', '.remove-item', function () {
        var taskId = $(this).parent().attr("data-id");
        tasksManager.deleteTask(taskId)
            .then(tasksManager.loadTasks)
            .done(function(tasks) {
                tasksManager.displayTasks("#tasks", "#tasks-done", tasks);

                countTodos()
            });
    });

    // count todos and done todoes
    function countTodos() {
        var count = $("#tasks li").length;
        $('.count-todos').html(count);
        var countDone = $("#tasks-done li").length;
        $('.count-todos-done').html(countDone);
    }

});