using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatFinChallenge.Models
{
    public class RegisterData
    {
        public List<Groups> Groups { get; set; }

        public RegisterData(List<Groups> groups)
        {
            this.Groups = groups;
        }
    }
}
