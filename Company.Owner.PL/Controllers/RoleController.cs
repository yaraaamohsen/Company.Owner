using System.Data;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Company.Owner.PL.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace Company.Owner.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(model.Name);
                
                if(role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name
                    };
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        TempData["toastr-success"] = "Role Created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;

            if (String.IsNullOrEmpty(SearchInput))
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name
                });
            }
            else
            {
                roles = _roleManager.Roles.Select(R => new RoleToReturnDto()
                {
                    Id = R.Id,
                    Name = R.Name
                }).Where(R => R.Name.ToLower().Contains(SearchInput.ToLower()));
            }

            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleByIdAsync(string? id, string ViewName)
        {
            if (id is null) return BadRequest("Id Is null");

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null) return NotFound("There is no Role matches the ID ");

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name 
            };

            return View(ViewName, dto);
        }

        [HttpGet]
        public Task<IActionResult> Details(string? id)
        {
            return GetRoleByIdAsync(id, "Details");
        }

        [HttpGet]
        public Task<IActionResult> Edit(string? id)
        {
            return GetRoleByIdAsync(id, "Edit");
        }

        [HttpGet]
        public Task<IActionResult> Delete(string? id)
        {
            return GetRoleByIdAsync(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Not Selected Id");

                var role = await _roleManager.FindByIdAsync(id);

                if (role is null) return NotFound("There is no user matches the ID ");

                var roleResult = await _roleManager.FindByNameAsync(model.Name);

                if (roleResult is null)
                {
                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        TempData["toastr-success"] = "Role Updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }

                var AddOrRemoveUsersStatus = TempData["AddOrRemoveUsersStatus"]?.ToString();
                //var AddOrRemoveUsersStatus = Request.Query["status"].FirstOrDefault();
                if (AddOrRemoveUsersStatus == "true")
                {
                    TempData["toastr-success"] = "Role Updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("","Invalid Operation");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Not Selected Id");

                var role = await _roleManager.FindByIdAsync(id);

                if (role is null) return NotFound("There is no user matches the ID ");

                var roleResult = await _roleManager.FindByNameAsync(model.Name);

           
                role.Name = model.Name;

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["toastr-success"] = "Role Deleted successfully!";
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Invalid Operation");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null) return NotFound();

            ViewData["RoleId"] = roleId;

            var UsersInRole = new List<UserInRoleDto>();

            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleDto() 
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };

                userInRole.IsSelected = await _userManager.IsInRoleAsync(user, role.Name) ? true : false;

                UsersInRole.Add(userInRole);
            }
            return View(UsersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId, List<UserInRoleDto> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if(role is null) return NotFound("Role Not Found");

            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if(appUser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (! user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                TempData["AddOrRemoveUsersStatus"] = "true";
                TempData["toastr-success"] = "Role Members Updated successfully!";
                return RedirectToAction(nameof(Edit), new {id = roleId});
            }
            return View(users);
        }
    }
}
 