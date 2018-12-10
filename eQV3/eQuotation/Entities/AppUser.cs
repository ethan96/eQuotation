using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace eQuotation.Entities
{
    public class AppUser : IdentityUser<string, AppUserLogin,AppUserRole, AppUserClaim>
    {
        public AppUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<AppUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }


        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Company { get; set; }

        public string Department { get; set; }

        public string Location { get; set; }

        public string Position { get; set; }

        public bool Disabled { get; set; }


        //custom
        //public int ID { get; set; }
        [StringLength(10)]
        public string Groupkey { get; set; }
        //?
        public string PurchaseGroup { get; set; }
        //?
        public string MRPController { get; set; }
        //.
        public string Gender { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string CellPhoneNum { get; set; }
        public string JobTittle { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string CompanyPhoneNum { get; set; }
        public string CompanyFax { get; set; }
        //[DataType(DataType.EmailAddress)]
        //public string CompanyMail { get; set; }

    }
}