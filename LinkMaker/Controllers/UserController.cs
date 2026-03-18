using LinkMaker.Common.DTOs;
using LinkMaker.Data;
using LinkMaker.Data.Entities;
using LinkMaker.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LinkMaker.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly LinkMakerDbContext _context; // Keep this only for the SelectLists in Create/Edit

        public UserController(LinkMakerDbContext context, IUserService userService)
        {
            _userService = userService;
            _context = context;
        }

        // GET: UserController
        public async Task<IActionResult> Index()
        {
            // Now uses Redis Cache via the service
            var users = await _userService.GetAll();
            return View(users);
        }

        // GET: UserController/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            // This line triggers your Redis Breakpoint!
            var user = await _userService.GetById(id.Value);

            if (user == null) return NotFound();

            return View(user);
        }

        // GET: UserController/Create
        public IActionResult Create()
        {
            ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserDTO userDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _userService.Create(userDto);
                if (success) return RedirectToAction(nameof(Index));
            }

            ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink", userDto.UrlId);
            return View(userDto);
        }

        // GET: UserController/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userService.GetById(id.Value);
            if (user == null) return NotFound();

            ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink", user.UrlId);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, UserDTO userDto)
        {
            if (id != userDto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _userService.Update(userDto);
                if (success) return RedirectToAction(nameof(Index));
            }

            ViewData["UrlId"] = new SelectList(_context.Urls, "Id", "NewLink", userDto.UrlId);
            return View(userDto);
        }

        // GET: UserController/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var user = await _userService.GetById(id.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: UserController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var success = await _userService.Delete(id);
            if (success) return RedirectToAction(nameof(Index));

            return BadRequest("Could not delete user.");
        }
    }
}