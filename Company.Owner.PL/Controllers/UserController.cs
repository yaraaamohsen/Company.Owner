using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Company.Owner.PL.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.Owner.PL.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;

            if (String.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    Name = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                });
            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    Name = U.UserName,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Email = U.Email,
                    Roles = _userManager.GetRolesAsync(U).Result
                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));
            }
           
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByIdAsync(string? id, string ViewName)
        {
            if (id is null) return BadRequest("Id Is null");

            var user = await _userManager.FindByIdAsync(id);

            if (user is null) return NotFound("There is no user matches the ID ");
       
            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                Name = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result
            };

            return View(ViewName, dto);
        }

        [HttpGet]
        public Task<IActionResult> Details(string? id)
        {
            return GetUserByIdAsync(id, "Details");
        }

        [HttpGet]
        public Task<IActionResult> Edit(string? id)
        {
            return GetUserByIdAsync(id, "Edit");
        }

        [HttpGet]
        public Task<IActionResult> Delete(string? id)
        {
            return GetUserByIdAsync(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if(id != model.Id) return BadRequest("Not Selected Id");
                
                var user = await _userManager.FindByIdAsync(id);
                
                if (user is null) return NotFound("There is no user matches the ID ");

                user.UserName = model.Name;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
                           
                var result = await _userManager.UpdateAsync(user);
               
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Not Selected Id");

                var user = await _userManager.FindByIdAsync(id);

                if (user is null) return NotFound("There is no user matches the ID ");

                user.UserName = model.Name;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
