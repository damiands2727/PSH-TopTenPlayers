using Sabio.Models.Domain.RandomUserAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Domain
{
    public class RandomUserApiResponse
    {
        public List<RandomUserResult> Results { get; set; }
    }
}
