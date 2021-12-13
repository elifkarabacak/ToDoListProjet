using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using ToDoApp.Models;

namespace Todo.Test
{
   
    public class TodoRepository
    {
        public readonly ITodoRepository MockTodoRepo;
        public TodoRepository()
        {
            var TodoList = new List<TodoItem> {
                new TodoItem{Id=new Guid().ToString(),Completed=false,Title="test1"},
                 new TodoItem{Id=new Guid().ToString(),Completed=true,Title="test2"},
                  new TodoItem{Id=new Guid().ToString(),Completed=false,Title="test3"},
                   new TodoItem{Id=new Guid().ToString(),Completed=false,Title="test4"}

            };

            var mockTodoItemRepository = new Mock<ITodoRepository>();

            mockTodoItemRepository.Setup(r => r.GetAll()).Returns(TodoList);

            mockTodoItemRepository.Setup(r => r.Get(It.IsAny<string>())).Returns((string i) => TodoList.Single(x => x.Id == i));
     
            mockTodoItemRepository.Setup(r => r.DeleteAll());


            mockTodoItemRepository.Setup(r => r.AddNew(It.IsAny<TodoItem>())).Callback
         ((TodoItem target) =>
         {
             TodoList.Add(target);
         }).Verifiable();

            mockTodoItemRepository.Setup(r => r.Update(It.IsAny<TodoItem>())).Callback
              ((TodoItem target) =>
              {
                  var original = TodoList.Where(i => i.Id == target.Id).Single();
                  if (original == null)
                  {
                      throw new InvalidOperationException();
                  }
                  original.Title = target.Title;
                  original.Completed = target.Completed;
              }).Verifiable();

            mockTodoItemRepository.Setup(r => r.Delete(It.IsAny<string>())).Callback
                ((string
                target) =>
                {
                    var deleted = TodoList.Where(i => i.Id == target).Single();
                    TodoList.Remove(deleted);
                }).Verifiable();

            this.MockTodoRepo = mockTodoItemRepository.Object;
        }


        [Test]
        public void GetAll_Than_Check_Count_Test()
        {
            var expected = this.MockTodoRepo.GetAll().Count;

            Assert.IsNotNull(expected);
            Assert.IsTrue(expected > 0);
        }


    }
}
