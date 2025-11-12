using Microsoft.AspNetCore.Mvc;
using TripLog2.Models.DataAccess;
using TripLog2.Models.DomainModels;

namespace TripLog2.Controllers
{
    public class ActivityController : Controller
    {
        private Repository<Activity> data { get; set; }
        public ActivityController(TripLog2Context ctx)
        {
            data = new Repository<Activity>(ctx);
        }
        public IActionResult Index()
        {
            var activities = data.List(new QueryOptions<Activity>
            {
                OrderBy = a => a.Name
            });
            return View(activities);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new Activity());
        }
        [HttpPost]
        public IActionResult Add(Activity activity)
        {
            if(ModelState.IsValid)
            {
                data.Insert(activity);
                data.Save();
                TempData["message"] = $"{activity.Name} added.";
                return RedirectToAction("Index");
            }
            else
            {
                return View(activity);
            }
           
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var activity = data.Get(id);
            data.Delete(activity);
            data.Save();
            TempData["message"] = $"{activity.Name} deleted.";
            return RedirectToAction("Index");
        }

    }
}
