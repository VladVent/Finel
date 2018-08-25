using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using database.DataModel;
using Microsoft.AspNetCore.Identity;
namespace Archive1.Models.interfases
{
    public interface IGameRepositore
    {
        IQueryable<Development> Developments { get; }
        IQueryable<Game> Games { get; }
        IQueryable<Published> Publisheds { get; }
        IQueryable<Users> User { get; }
        IQueryable<LikeGame> LikeGames { get; }
        IQueryable<StatisticsAdmin> statisticsAdmins { get; }

        void AddGame(Game newGame);
        void DeleteGame(int id);
        void AddLikeGame(Users y, Game gae);
        void AddAdmin(string y );
        void DeleteAdmin(string name);
        void AddStatysAdmin(string name);
    }
}
