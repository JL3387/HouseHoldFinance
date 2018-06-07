using HouseHoldFinance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseHoldFinance.Helpers
{
    public class MemberHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Exception RemoveUserFromHousehold(string userId, int HouseholdId)
        {
            try
            {
                var HHId = db.Households.Find(HouseholdId);
                var usr = db.Users.Find(userId);
                HHId.Members.Remove(usr);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}