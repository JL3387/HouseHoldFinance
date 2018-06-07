using HouseHoldFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseHoldFinance.Models
{
    public class HouseholdViewModel
    {
        public int? HHId { get; set; }  
        public string HHName { get; set; }  
        public bool IsJoinHouse { get; set; }
        public ApplicationUser Member { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}