using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Archive1.Models;
using Archive1.Models.interfases;
using database.DataModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.FileProviders;
using System.Reflection.Emit;

namespace Archive1.Controllers
{
    public class GameController : Controller
    {
        private readonly IGameRepositore db;
        private readonly IFileProvider fileProvider;

        private UserManager<Users> userManager;

        //  private IdentityUserRole<string> identityUserRole;
        public GameController(IGameRepositore _db, IFileProvider _fileProvider, UserManager<Users> _us)
        {
            db = _db;
            fileProvider = _fileProvider;
            userManager = _us;

        }
        [Authorize]
        public IActionResult ViewDevelop(int id)
        {
            Game currentGame = db.Games.Include(c => c.GameDev).Where(o => o.Id == id).SingleOrDefault();
            var stud = from co in db.Games
                       where co.GameDev.Id == id
                       select co.GameDev;
            return View("");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGame()
        {
            List<DevModel> compModels = db.Developments
             .Select(c => new DevModel { Id = c.Id, Name = c.Name })
             .ToList();
            List<PubModel> pubmod = db.Publisheds.
                Select(c => new PubModel { Id = c.Id, Name = c.Name })
                .ToList();
            DevView gam = new DevView { GetDevelopments = compModels, GetPublishers = pubmod };
            //if (dev != null && dev > 0)
            //    gam.GetDevelopments = compModels.Where(p => p.Id == dev);
            

            return View(gam);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddGame(string name, string gameGenre, DateTime dateCreate, string gameDescr, int? dev, int? pub)
        {
            List<DevModel> compModels = db.Developments
               .Select(c => new DevModel { Id = c.Id, Name = c.Name })
               .ToList();
            List<PubModel> pubmod = db.Publisheds.
               Select(c => new PubModel { Id = c.Id, Name = c.Name })
               .ToList();
            DevView gam = new DevView { GetDevelopments = compModels, GetPublishers = pubmod };
            
           
            //IEnumerable<Development> gam = db.Developments;
            if (dev != null && dev > 0)
                gam.GetDevelopments = compModels.Where(p => p.Id == dev);
            Development game = db.Developments.Where(c=>c.Id == dev).SingleOrDefault();
            Published published = db.Publisheds.Where(c => c.Id == pub).SingleOrDefault();
            db.AddGame(new Game(name, gameGenre, dateCreate, gameDescr, game, published));
            return View("AddGame", new DevView() { GetDevelopments = compModels, GetPublishers = pubmod, Message = String.Format("Гра \"{0}\" успішно додана!", name) });
        }

        [HttpGet]
        public async Task<IActionResult> LikeGames()
        {

            Users currentUser = await userManager.FindByNameAsync(User.Identity.Name);
            IEnumerable<LikeGame> currentLikeGames = db.LikeGames.Include(c => c.User).Include(c => c.Games).Where(c => c.User.Id == currentUser.Id);

            return View(currentLikeGames);
        }

        [HttpGet]
        public IActionResult ViewGame(string name)
        {
            Game game = db.Games.Where(c => c.Name == name).Include(c => c.GameDev).FirstOrDefault();
            return View(game);
        }
        [HttpGet]
        public async Task<IActionResult> AddLikeGame(Users y, Game gaee)
        {
            Users all = await userManager.FindByNameAsync(User.Identity.Name);
            Game g = db.Games.Include(c => c.GamePub).Where(c => c.Id == gaee.Id).FirstOrDefault();
            db.AddLikeGame(all, g);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        [HttpGet]
        public IActionResult ViewUser()
        {
            IEnumerable<Users> currentUser = db.User; ;
           
            return View(currentUser);
        }
        [HttpGet]
        [Authorize(Roles = "SubAdmin")]
        public async Task<IActionResult> ViewAdmin(string name)
        {
            IEnumerable<Users> role = (await userManager.GetUsersInRoleAsync("Admin"));
            return View(role);

        }
        [HttpGet]
        [Authorize(Roles = "SubAdmin")]
        public async Task<IActionResult> AddAdmin(string name)
        {
            Users admin = db.User.Where(r => r.UserName == name).SingleOrDefault();
            await userManager.RemoveFromRoleAsync(admin, "Users");
            await userManager.AddToRoleAsync(admin, "Admin");
            db.AddStatysAdmin(name);
            return RedirectToRoute(new { controller = "Game", action = "ViewUser", name = name });
        }
        [HttpGet]
        [Authorize(Roles = "SubAdmin")]
        public async Task<IActionResult> DeleteAdmin(string name)
        {
            Users admin = db.User.Where(r => r.Name == name).SingleOrDefault();
            await userManager.RemoveFromRoleAsync(admin, "Admin");
            await userManager.AddToRoleAsync(admin, "Users");
            return RedirectToRoute(new { controller = "Game", action = "ViewUser", name = name });
        }
        [HttpGet]
        [Authorize(Roles = "SubAdmin")]
        public IActionResult ViewStaticAdmin()
        {
            IEnumerable<StatisticsAdmin> currentUser = db.statisticsAdmins;
            ViewData["role"] = currentUser.ToString();
            return View(currentUser);
        }
        [HttpGet]
        public IActionResult Ind(int? sortType)
        {
            if(sortType == 0)
            {
                IEnumerable<Game> game = db.Games.OrderBy(c => c.Name);
                return View(game);
            }
            else
            {
                IEnumerable<Game> game = db.Games.OrderBy(c => c.DateCreate);
                return View(game);
            }
          
        }
    }
}
