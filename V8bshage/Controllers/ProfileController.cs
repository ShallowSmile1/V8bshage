using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using V8bshage.Data;
using V8bshage.Models;


namespace V8bshage.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly AdvertisementContext _adb;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, AdvertisementContext adb)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _adb = adb;
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Advertisement> advList = _adb.Advertisements;
            var user = await _userManager.GetUserAsync(User);

            ProfileViewModel profile = new ProfileViewModel
            {
                User = user,
                Advertisments = advList
            };
            return View(profile);
        }

        [HttpGet]
        public async Task<IActionResult> ChangeProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeProfilePost(User user)
        {
            var updated_user = await _userManager.FindByIdAsync(user.Id);
            if(updated_user == null)
            {
                return NotFound();
            }

            updated_user.Name = user.Name;
            updated_user.LastName = user.LastName;
            updated_user.UserName = user.UserName;
            updated_user.Vk = user.Vk;
            updated_user.Telegram = $"https://t.me/{user.Telegram}";

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    updated_user.Photo = dataStream.ToArray();
                }
            } else
            {
                string storePath = "wwwroot/img/";
                var path = Path.Combine(Directory.GetCurrentDirectory(), storePath, "user.png");

                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // Create a byte array of file stream length
                    byte[] bytes = System.IO.File.ReadAllBytes(path);
                    //Read block of bytes from stream into the byte array
                    fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                    //Close the File Stream
                    fs.Close();

                    updated_user.Photo = bytes;
                }
            }

            await _userManager.UpdateAsync(updated_user);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult RemoveAdv(int? id)
        {
            var adv = _adb.Advertisements.Find(id);
            if (adv == null)
            {
                return NotFound();
            }

            _adb.Remove(adv);
            _adb.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateAdv(int? id)
        {
            var adv = _adb.Advertisements.Find(id);
            if (adv == null)
            {
                return NotFound();
            }

            return View(adv);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdv(Advertisement adv)
        {
            User user = await GetCurrentUserAsync();
            adv.UserId = user.Id;

            if(ModelState.IsValid)
            {

                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        adv.Photo = dataStream.ToArray();
                    }
                }

                _adb.Advertisements.Update(adv);
                await _adb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adv);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
