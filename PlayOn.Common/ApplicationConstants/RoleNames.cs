using System.Collections.Frozen;

namespace PlayOn.Common.ApplicationConstants
{
    public static class RoleNames
    {
        public const string Administrator = "Administrator";
        public const string User = "User";

        public static readonly FrozenSet<string> All =
            new[]
            {
                Administrator,
                User
            }
            .ToFrozenSet();
    }
}
