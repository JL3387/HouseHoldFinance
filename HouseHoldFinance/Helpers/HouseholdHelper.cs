using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace HouseHoldFinance.Helpers
{
    public static class HouseholdHelper
    {
        public static int? GetHouseholdId(this IIdentity user)
        {
            var claimsIdentity = (ClaimsIdentity)user;
            var HouseHoldClaim = claimsIdentity.Claims
                .FirstOrDefault(c => c.Type == "HouseholdId");
            if (HouseHoldClaim != null)
            {
                return Int32.TryParse(HouseHoldClaim.Value, out var tempVal) ? tempVal : (int?)null;
            }
            else
                return null;
        }
       
        public static bool IsInHousehold(this IIdentity user)
        {
            var cUser = (ClaimsIdentity)user;
            var hid = cUser.Claims.FirstOrDefault(c => c.Type == "HouseholdId");
            return (hid != null && !string.IsNullOrWhiteSpace(hid.Value));
        }
    }
}