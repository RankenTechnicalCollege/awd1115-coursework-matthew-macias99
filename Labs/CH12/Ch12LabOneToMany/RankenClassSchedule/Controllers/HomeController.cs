using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RankenClassSchedule.Models;
using RankenClassSchedule.Models.Data;
using RankenClassSchedule.Models.DomainModels;

namespace RankenClassSchedule.Controllers
{
    public class HomeController : Controller
    {
        private Repository<Class> classes { get; set; }
        private Repository<Day> days { get; set; }

        public HomeController(ClassScheduleContext context)
        {
            classes = new Repository<Class>(context);
            days = new Repository<Day>(context);
        }
        
        public IActionResult Index(int id)
        {
            var dayOptions = new QueryOptions<Day>
            {
                OrderBy = d => d.DayId
            };
            var classOptions = new QueryOptions<Class>
            {
                Includes = "Teacher, Day"
            };

            if (id == 0)
            {
                classOptions.OrderBy = c => c.DayId;
                classOptions.ThenOrderBy = c => c.MilitaryTime;
            }
            else
            {
                classOptions.Where = c => c.DayId == id;
                classOptions.OrderBy = c => c.MilitaryTime;
            }

            var dayList = days.List(dayOptions);
            var classList = classes.List(classOptions);

            ViewBag.Id = id;
            ViewBag.Days = dayList;


            return View(classList);
        }
    }
}
