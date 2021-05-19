﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TheGreatFinChallenge.Models
{
    // Activities model
    public class Activities
    {
        [Key]
        public int pk_ActivityID { get; set; }

        public int fk_UserID { get; set; }
        public string ActivityType { get; set; }
        public int TotalCalories { get; set; }
        public decimal Distance { get; set; }
        //public decimal DistanceRecalculated { get; set; }
        public TimeSpan TTime { get; set; }

        public DateTime StartTime { get; set; }
        public string Gear { get; set; }
    }
}
