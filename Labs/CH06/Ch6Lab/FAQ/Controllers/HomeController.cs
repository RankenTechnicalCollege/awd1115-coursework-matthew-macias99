using System.Diagnostics;
using FAQ.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;    

namespace FAQ.Controllers
{
    public class HomeController : Controller
    {
        private FaqsContext context { get; set; }

        public HomeController(FaqsContext ctx)
        {
            context = ctx;
        }

        [Route("topic/{topic}/category/{category}")]
        [Route("topic/{topic}")]
        [Route("category/{category}")]
        [Route("/")]
        public IActionResult Index(string topic, string category)
        {
            var faqs = context.FAQs.Include(f => f.Category).Include(f => f.Topic).OrderBy(f => f.Question).ToList();

            ViewBag.Topics = context.Topics.OrderBy(t => t.Name).ToList();
            ViewBag.Categories = context.Categories.OrderBy(c => c.Name).ToList();
            ViewBag.SelectedTopic = topic;

            if (!string.IsNullOrEmpty(topic))
            {
                faqs = faqs.Where(f => f.Topic.TopicId == topic).ToList();
            }

            if (!string.IsNullOrEmpty(category))
            {
                faqs = faqs.Where(f => f.Category.CategoryId == category).ToList();
            }
            return View(faqs);
        }

    }
}
