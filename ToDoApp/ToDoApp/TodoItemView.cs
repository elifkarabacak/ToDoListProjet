using System;
using Microsoft.AspNetCore.Http;
using ToDoApp.Models;

namespace ToDoApp.ViewModels
{
    public class TodoItemView
    {
        public TodoItemView(TodoItem item, HttpRequest req)
        {
            Title = item.Title;
            Completed = item.Completed ?? false;
            Id =  item.Id;
        }

        public string Title { get; }
        public bool Completed { get; }
        public string Id { get; }
    }
}
