using LinkMaker.Data;
using LinkMaker.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkMaker.Controllers
{
    public class LinkController : Controller
    {
        private readonly WebDbContext _context;
        public LinkController(WebDbContext context)
        {
            _context = context;
        }
        // GET: LinkController/Index
        public ActionResult Index()
        {
            return View(_context.Links.ToList());
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Link link)
        {
            if (link != null)
            {
                _context.Links.Add(link);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(link);
        }

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public ActionResult AccessLink()
        {
            return View(_context.Links.ToList());
        }
        public IActionResult Go(string code)
        {
            var link = _context.Links.FirstOrDefault(l => l.newUrl == code);

            if (link == null)
            {
                return NotFound();
            }

            return Redirect(link.curUrl);
        }
    }
}
