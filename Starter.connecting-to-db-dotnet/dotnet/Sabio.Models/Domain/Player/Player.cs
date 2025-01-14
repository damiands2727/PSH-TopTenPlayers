using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain.Player
{
    public class Player
    {
        public int Id { get; set; }
        public string NickName { get; set; }
        public int StatId { get; set; }
        public string StatName { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateCreated { get; set; }
        public int Score { get; set; }
    }
}
