using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Players
{
    public class AddManyPlayersRequest
    {
        public List<PlayerAddRequest> Players { get; set; }
    }
}
