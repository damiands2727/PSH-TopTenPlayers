using Sabio.Models.Domain.Player;
using Sabio.Models.Requests.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IPlayerService
    {
        int Add(PlayerAddRequest model);
        void AddMany(AddManyPlayersRequest model);
        void Delete();
        Task<AddManyPlayersRequest> GeneratePlayersFromApi();
        List<Player> GetTop10Players();
    }
}
