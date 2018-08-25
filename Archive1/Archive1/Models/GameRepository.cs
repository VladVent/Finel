using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database.DataModel;
using Archive1.Models.interfases;
using Microsoft.EntityFrameworkCore;

namespace Archive1.Models
{
    public class GameRepository : IGameRepositore
    {
        private GameDatabase context;
        public GameRepository(GameDatabase gd)
        {
            context = gd;
        }

        public IQueryable<Development> Developments => context.Develop;
        public IQueryable<Game> Games => context.Game;
        public IQueryable<Published> Publisheds => context.Published;
        public IQueryable<Users> User => context.Users;
        public IQueryable<LikeGame> LikeGames => context.LikeGame;
       
        public IQueryable<StatisticsAdmin> statisticsAdmins => context.StatisticsAdmins;

        public void AddGame(Game newGame)
        {
            context.Game.Add(newGame);
            context.SaveChanges();
        }
      public void DeleteGame(int id)
        {
            Game game = context.Game.Include(c => c.GameDev).Where(c => c.Id == id).SingleOrDefault();
            context.Remove(game);
            context.SaveChanges();
        }
        public void AddLikeGame(Users y,Game gae)
        {
            LikeGame likeGame = new LikeGame(y, gae);
            context.LikeGame.Add(likeGame);
            context.SaveChanges();
        }
        public void AddAdmin(string y)
        {
          Users admin = context.Users.Where(r=>r.Name == y).SingleOrDefault();
          context.SaveChanges();
        }
        public void DeleteAdmin(string name)
        {
            Users admin = context.Users.Where(r => r.Name == name).SingleOrDefault();
            context.SaveChanges();
        }
        public void AddStatysAdmin(string name)
        {
            Users admin = context.Users.Where(r => r.UserName == name).SingleOrDefault();
            StatisticsAdmin statisticsAdmin = new StatisticsAdmin(admin.ToString());
           IEnumerable<StatisticsAdmin> statistics = context.StatisticsAdmins;
            if(statistics == null)
            {
                context.StatisticsAdmins.Add(statisticsAdmin);
                context.SaveChanges();
            }
            else
            {
                /*  foreach(var a in statistics)
                  {
                  if (name.ToString() == a.Name.ToString())
                  {
                              //  context.SaveChanges();
                              break;
            }
                  else
                  {
                              context.StatisticsAdmins.Add(statisticsAdmin);
                  }
                    context.SaveChanges();
                  }*/
                var stud = from co in statistics
                           where co.Name == name
                           select co.Name;
                Users admi = context.Users.Where(r => r.UserName == stud.ToString()).SingleOrDefault();
                StatisticsAdmin statistic = new StatisticsAdmin(admi.ToString());
                context.StatisticsAdmins.Add(statistic);
                ;
            }
    }
    }
}
