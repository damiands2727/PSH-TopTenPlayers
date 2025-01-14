using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Players
{
    public class PlayerAddRequest
    {
        public int StatId { get; set; }
        public string NickName { get; set; }
        public string ProfileImage { get; set; }
        public int Score { get; set; }
    }
}
