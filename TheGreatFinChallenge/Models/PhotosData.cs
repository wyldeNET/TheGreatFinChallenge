using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TheGreatFinChallenge.Models
{
    public class PhotosData
    {
        
        public Users User { get; set; }
        public List<Photos> PhotoList { get; set; }
        public List<Users> UserList { get; set; }

        public PhotosData(Users user, List<Photos> photoList, List<Users> allUsers)
        {
            this.User = user;
            this.PhotoList = photoList;
            this.UserList = allUsers;
        }
    }
}
