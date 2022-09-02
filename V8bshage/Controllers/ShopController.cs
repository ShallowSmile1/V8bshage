using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using V8bshage.Data;
using V8bshage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace V8bshage.Controllers
{
    public class ShopController : Controller
    {
        private readonly AdvertisementContext _adb;

        private readonly UserManager<User> _userManager;

        public ShopController(AdvertisementContext adb, UserManager<User> userManager)
        {
            _adb = adb;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            IEnumerable<Advertisement> advList = _adb.Advertisements;
            return View(advList);
        }

        public IActionResult Create()
        {
            return View();
        }

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Advertisement adv)
        {
            if(ModelState.IsValid)
            {
                var user = await GetCurrentUserAsync();

                Advertisement new_adv = new Advertisement
                {
                    Title = adv.Title,
                    Description = adv.Description,
                    Price = adv.Price,
                    UserId = user.Id
                };

                if (Request.Form.Files.Count > 0)
                {
                    IFormFile file = Request.Form.Files.FirstOrDefault();
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        new_adv.Photo = dataStream.ToArray();
                    }
                } else
                {
                    string storePath = "wwwroot/img/";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), storePath, "default_adv.png");

                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        // Create a byte array of file stream length
                        byte[] bytes = System.IO.File.ReadAllBytes(path);
                        //Read block of bytes from stream into the byte array
                        fs.Read(bytes, 0, System.Convert.ToInt32(fs.Length));
                        //Close the File Stream
                        fs.Close();

                        new_adv.Photo = bytes;
                    }

                }

                _adb.Advertisements.Add(new_adv);
                await _adb.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(adv);
        }

        [HttpGet]
        public async Task<IActionResult> Info(int id)
        {
            var adv = _adb.Advertisements.Find(id);
            var user = await GetCurrentUserAsync();

            AdvInfoViewModel model = new AdvInfoViewModel
            {
                Adv = adv,
                User = user
            };

            return View(model);
        }
    }
}
