using Cozy.Domain.AppCode.Extensions;
using Cozy.Domain.Models.DataContexts;
using Cozy.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cozy.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly CozyDbContext db;

        public UsersController(CozyDbContext db)
        {
            this.db = db;
        }



        [Authorize(Policy = "admin.users.index")]
        public async Task<IActionResult> Index()
        {
            var data = await db.Users.ToListAsync();

            return View(data);
        }

        [Authorize(Policy = "admin.users.details")]
        public async Task<IActionResult> Details(int id) // int roleId = 1
        {
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);

           

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = await (from r in db.Roles
                                   join ur in db.UserRoles
                                   on new { RoleId = r.Id, UserId = user.Id } equals new { ur.RoleId, ur.UserId } into lJoin
                                   from lj in lJoin.DefaultIfEmpty()
                                   select Tuple.Create(r.Id, r.Name, lj != null)).ToListAsync();


            ViewBag.Principals = (from p in Extension.policies
                                  join uc in db.UserClaims on new { ClaimValue = "1", ClaimType = p, UserId = user.Id } equals new { uc.ClaimValue, uc.ClaimType, uc.UserId } into lJoin
                                  from lj in lJoin.DefaultIfEmpty()
                                  select Tuple.Create(p, lj != null)).ToList();


            return View(user);
        }


        [HttpPost]
        [Route("/user-set-role")]
        [Authorize(Policy = "admin.users.setrole")]
        public async Task<IActionResult> SetRole(int userId, int roleId, bool selected)
        {
            #region Check user and role

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }

            if (userId == User.GetCurrentUserId())
            {
                return Json(new
                {
                    error = true,
                    message = "İstifadəçi özünə rol verə bilməz"
                });
            }


            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);

            if (role == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }

            #endregion

            if (selected)
            {
                if (await db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"{user.Name} {user.Surname} adlı istifadəçi {role.Name} adlı roldadır"
                    });
                }
                else
                {
                    db.UserRoles.Add(new CozyUserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"{user.Name} {user.Surname} adlı istifadəçiyə {role.Name} adlı rol verildi"
                    });
                }
            }
            else
            {
                var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (userRole == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"{user.Name} {user.Surname} adlı istifadəçi {role.Name} adlı rolda deyil!"
                    });
                }
                else
                {
                    db.UserRoles.Remove(userRole);

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"{user.Name} {user.Surname} adlı istifadəçi {role.Name} adlı roldan çıxarıldı!"
                    });

                }
            }

        }

        [HttpPost]
        [Route("/user-set-principal")]
        [Authorize(Policy = "admin.users.setprincipal")]
        public async Task<IActionResult> SetPrincipal(int userId, string principalName, bool selected)
        {
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);


            #region Check user and principal

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }

            if (userId == User.GetCurrentUserId())
            {
                return Json(new
                {
                    error = true,
                    message = "İstifadəçi özünə səlahiyyət verə bilməz"
                });
            }

            var hasPrincipal = Extension.policies.Contains(principalName);


            if (!hasPrincipal)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }

            #endregion

            if (selected)
            {
                if (await db.UserClaims.AnyAsync(uc => uc.UserId == userId && uc.ClaimType.Equals(principalName) && uc.ClaimValue.Equals("1")))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"{user.Name} {user.Surname} adlı istifadəçi {principalName} adlı səlahiyyətə malikdir"
                    });
                }
                else
                {
                    db.UserClaims.Add(new CozyUserClaim
                    {
                        UserId = userId,
                        ClaimType = principalName,
                        ClaimValue = "1"
                    });

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"{user.Name} {user.Surname} adlı istifadəçiyə {principalName} adlı səlahiyyət verildi"
                    });
                }

            }
            else
            {
                var userClaim = await db.UserClaims.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClaimType.Equals(principalName) && uc.ClaimValue.Equals("1"));

                if (userClaim == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"{user.Name} {user.Surname} adlı istifadəçi {principalName} adlı səlahiyyətə malik deyil!"
                    });
                }
                else
                {
                    db.UserClaims.Remove(userClaim);

                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"{user.Name} {user.Surname} adlı istifadəçidən {principalName} adlı səlahiyyət qaldırıldı"
                    });
                }
            }
        }



    }
}