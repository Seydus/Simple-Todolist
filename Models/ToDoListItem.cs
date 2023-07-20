using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class TodoListItem
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? ContainerId { get; set; }
        public bool IsDone { get; set; }
    }
}
