using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KhumaloCraftLibrary
{
    public class ToDoService
    {
        private readonly List<ToDoItem> _toDoItems = new List<ToDoItem>();
        private int _nextId = 1;

        public List<ToDoItem> GetToDoItems()
        {
            return _toDoItems;
        }

        public ToDoItem GetToDoItem(int id)
        {
            return _toDoItems.FirstOrDefault(item => item.Id == id);
        }

        public void AddToDoItem(ToDoItem item)
        {
            item.Id = _nextId++;
            _toDoItems.Add(item);
        }

        public void UpdateToDoItem(ToDoItem item)
        {
            var existingItem = GetToDoItem(item.Id);
            if (existingItem != null)
            {
                existingItem.Title = item.Title;
                existingItem.IsCompleted = item.IsCompleted;
            }
        }

        public void DeleteToDoItem(int id)
        {
            var item = GetToDoItem(id);
            if (item != null)
            {
                _toDoItems.Remove(item);
            }
        }
    }
}
