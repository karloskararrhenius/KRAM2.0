﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KRAM1.Models
    {
    public class UserViewModel
        {
        public string UserID { get; set; }
        public string UserName { get; set; }
		public string ProfilePic { get; set; }
		public string UserEmail { get; set; }
        public string PassWord { get; set; }
        public int  Pictures { get; set; }

        public int TotalLikes { get; set; }
    
        }
    }