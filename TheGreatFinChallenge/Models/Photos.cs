using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatFinChallenge.Models
{
    public class Photos
    {
        [Key]
        public int PhotoID { get; set; }

        public string PhotoName { get; set; }
        public string PhotoLocation { get; set; }
        public int CreatorID { get; set; }
    }
}
