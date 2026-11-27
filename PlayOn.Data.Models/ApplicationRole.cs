using Microsoft.AspNetCore.Identity;

namespace PlayOn.Data.Models
{
    /// <summary>
    /// Represents an application role (e.g. Administrator, Member) used by ASP.NET Core Identity.
    /// </summary>
    public class ApplicationRole : IdentityRole<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class.
        /// </summary>
        public ApplicationRole() : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRole"/> class with the specified role name.
        /// </summary>
        /// <param name="roleName">The name of the role.</param>
        public ApplicationRole(string roleName) : base(roleName)
        {

        }
    }
}
