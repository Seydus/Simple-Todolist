using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TodoList.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using TodoList.ViewModels;
using TodoList.Helpers;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TodoList.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TodoListViewModel viewModel = new TodoListViewModel();
            viewModel.EditableContainer.Id = viewModel.TodoContainers.Select(c => c.Id).FirstOrDefault();
            return View("Index", viewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, string title)
        {
            if (ModelState.IsValid)
            {
                using (var db = DbHelper.GetConnection())
                {
                    string sql = "UPDATE TodoListItems SET Title = @Title WHERE Id = @Id";
                    var parameters = new { Title = title, Id = id };
                    db.Execute(sql, parameters);
                }

                return Json(new { success = true, message = "Item Title Edited successfully" });
            }

            return Json(new { success = false, message = "Failed to Edit Item Title" });
        }

        [HttpPost]
        public IActionResult EditContainer([FromForm] int id, [FromForm] string title)
        {
            if (ModelState.IsValid)
            {
                using (var db = DbHelper.GetConnection())
                {
                    string sql = "UPDATE TodoListContainers SET Title = @Title WHERE Id = @Id";
                    var parameters = new { Title = title, Id = id };
                    db.Execute(sql, parameters);
                }

                return Json(new { success = true, message = "Container Title Edited successfully", title = title });
            }

            return Json(new { success = false, message = "Failed to Edit Item Title: " + title, title = title });
        }

        [HttpPost]
        public IActionResult DeleteContainer(int id)
        {
            using (var db = DbHelper.GetConnection())
            {
                string sql = "DELETE FROM TodoListContainers WHERE Id = @Id";
                var parameters = new { Id = id };
                db.Execute(sql, parameters);
            }

            return Json(new { success = true, message = "Container data deleted successfully" });
        }

        [HttpPost]
        public IActionResult DeleteItem(int id)
        {
            using (var db = DbHelper.GetConnection())
            {
                string sql = "DELETE FROM TodoListItems WHERE Id = @Id";
                var parameters = new { Id = id };
                db.Execute(sql, parameters);
            }

            return Json(new { success = true, message = "Item data deleted successfully" });
        }

        [HttpGet]
        [Route("/Home/{action}/{id}")]
        public IActionResult GetItemData(int id)
        {
            var viewModel = new TodoListViewModel();
            viewModel.EditableContainer.Id = id;

            if (Request.Headers["Accept"].Contains("application/json"))
            {
                // Return JSON data
                return Json(new { success = true, message = "Data Successfully Loaded", data = viewModel.TodoItems, containerId = id });
            }
            else
            {
                // Return a view
                return View("Index", viewModel);
            }
        }

        [HttpPost]
        public IActionResult ToggleIsDone(int id)
        {
            using (var db = DbHelper.GetConnection())
            {
                TodoListItem item = db.Get<TodoListItem>(id);
                if (item != null)
                {
                    item.IsDone = !item.IsDone;
                    db.Update<TodoListItem>(item);
                    return Json(new { success = true, message = "Item toggled successfully", isDone = item.IsDone });
                }
                else
                {
                    return Json(new { success = false, message = "Item not found" });
                }
            }
        }

        [HttpGet]
        public IActionResult GetContainerData()
        {
            var viewModel = new TodoListViewModel();
            return Json(new { success = true, message = "Data Successfully Loaded", data = viewModel.TodoContainers });
        }

        [HttpPost]
        public IActionResult CreateContainer([FromBody] TodoListViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                using (var db = DbHelper.GetConnection())
                {
                    string sql = "INSERT INTO TodoListContainers (Title) VALUES (@Title)";
                    var parameters = new { Title = viewModel.EditableContainer.Title };
                    db.Execute(sql, parameters);
                }

                return Json(new { success = true, message = "Container Data inserted successfully" });
            }
            else
            {
                var validationErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );

                return Json(new { success = false, message = "Error inserting Container data", errors = validationErrors });
            }
        }

        [HttpPost]
        public IActionResult CreateItem([FromBody] TodoListViewModel viewModel)
        {
            if(ModelState.IsValid)
            {
                using (var db = DbHelper.GetConnection())
                {
                    string sql = "INSERT INTO TodoListItems (Title, ContainerId, IsDone) VALUES (@Title, @ContainerId, @IsDone)";
                    var parameters = new { Title = viewModel.EditableItem.Title, ContainerId = viewModel.EditableItem.ContainerId, IsDone = viewModel.EditableItem.IsDone };
                    db.Execute(sql, parameters);
                }

                return Json(new { success = true, message = "Item Data inserted successfully" });
            }
            else
            {
                var validationErrors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                );

                return Json(new { success = false, message = "Error inserting Item data", errors = validationErrors });
            }
        }
    }
}