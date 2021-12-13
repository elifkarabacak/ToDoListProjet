using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ToDoApp.Models;
using ToDoApp.ViewModels;

namespace ToDoApp.Controllers
{
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _repo;

        public TodoController(ITodoRepository repo)
        {
            _repo = repo;
        }

        [Route("/")]
        [HttpGet]
        public IActionResult Index()
        {
            var allTodoItems = _repo.GetAll();
            var views = allTodoItems
                .Select(r => new TodoItemView(r, Request));
            return Ok(views);
        }

        [Route("/")]
        [HttpPost]
        public IActionResult AddItem(TodoItem item)
        {
            item.Id = Guid.NewGuid().ToString();
            item.Completed = false;

            _repo.AddNew(item);

            return Ok(new TodoItemView(item, Request));
        }

        [Route("/{id}")]
        [HttpGet]
        public IActionResult GetItembyId(string id)
        {
            var item = _repo.Get(id);
            return Ok(new TodoItemView(item, Request));
        }

        [Route("/{id}")]
        [HttpDelete]
        public IActionResult DeleteItem(string id)
        {
                _repo.Delete(id);
                return Ok();
            
        }

        [Route("/{id}")]
        [HttpPatch]
        public IActionResult ChangeItem( TodoItem todo)
        {
           

            _repo.Update(todo);

            var item = _repo.Get(todo.Id);

            return Ok(new TodoItemView(item, Request));
        }
    }
}
