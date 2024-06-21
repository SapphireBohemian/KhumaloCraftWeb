using KhumaloCraftLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KhumaloCraftWeb.Controllers
{
    public class ToDoController : Controller
    {
        private readonly ToDoService _toDoService;

        public ToDoController(ToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        public IActionResult Index()
        {
            var items = _toDoService.GetToDoItems();
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ToDoItem item)
        {
            if (ModelState.IsValid)
            {
                _toDoService.AddToDoItem(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            var item = _toDoService.GetToDoItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ToDoItem item)
        {
            if (ModelState.IsValid)
            {
                _toDoService.UpdateToDoItem(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        public IActionResult Delete(int id)
        {
            var item = _toDoService.GetToDoItem(id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _toDoService.DeleteToDoItem(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
