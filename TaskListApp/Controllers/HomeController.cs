using System;
using System.Web.Mvc;
using TaskListApp.Models;

namespace TaskListApp.Controllers
{
    public class HomeController : Controller, IDisposable
    {
        private TaskRepository taskRepository = new TaskRepository();
        private bool disposed = false;
        //
        // GET: /MyTask/

        public ActionResult Index()
        {
            return View(taskRepository.GetAllTasks());
        }

        //
        // GET: /MyTask/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MyTask/Create

        [HttpPost]
        public ActionResult Create(MyTask task)
        {
            try
            {
                taskRepository.CreateTask(task);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult About()
        {
            return View();
        }

        # region IDisposable

        new protected void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        new protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.taskRepository.Dispose();
                }
            }

            this.disposed = true;
        }

        # endregion

    }
}