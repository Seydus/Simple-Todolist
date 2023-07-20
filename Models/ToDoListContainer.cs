using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Models
{
    public class ToDoListContainer
    {
        public int Id { get; set; }
        public string? Title { get; set; }
    }
}
