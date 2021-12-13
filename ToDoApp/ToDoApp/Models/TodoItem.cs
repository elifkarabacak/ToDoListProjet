using System;
namespace ToDoApp.Models
{
    public class TodoItem
    {
        
              public string Id { get; set; }
        public string Title { get; set; }
        public bool? Completed { get; set; }

        public bool ShouldSerializeId() => false;
        public bool ShouldDeserializeId() => true;
 
    }
}
