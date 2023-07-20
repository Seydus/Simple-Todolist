using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using TodoList.Models;
using Dapper;
using TodoList.Helpers;
using Newtonsoft.Json;

namespace TodoList.ViewModels
{
    public class TodoListViewModel
    {
        public TodoListViewModel()
        {
            using (var db = DbHelper.GetConnection())
            {
                this.EditableContainer = new ToDoListContainer();
                this.EditableItem = new TodoListItem();

                this.TodoContainers = db.Query<ToDoListContainer>("SELECT * FROM TodoListContainers ORDER BY Id ASC").ToList();
                this.TodoItems = db.Query<TodoListItem>("SELECT * FROM TodoListItems ORDER BY Id ASC").ToList();
            }
        }

        [JsonProperty("todoContainers")]
        public List<ToDoListContainer> TodoContainers { get; set; }

        [JsonProperty("todoItems")]
        public List<TodoListItem> TodoItems { get; set; }

        public ToDoListContainer EditableContainer { get; set; }

        public TodoListItem EditableItem { get; set; }
    }
}
