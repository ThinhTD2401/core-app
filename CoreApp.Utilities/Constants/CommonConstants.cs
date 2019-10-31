using System;

namespace CoreApp.Utilities.Constants
{
    public class CommonConstants
    {
        public const string DefaultFooterId = "DefaultFooterId";
        public const string AccountLocked = "Account is locked";
        public const string AccountLockedMessage = "User account locked out.";
        public const string LoginFailMessage = "Login failure";
        public const string DefaultContactId = "Default";

        //Claim Pricipal addtion
        public const string Email = "Email";
        public const string FullName = "FullName";
        public const string Avatar = "Avatar";
        public const string UserId = "Id";

        //Deault value
        public const string AvatarDefault = "/admin-side/images/img.jpg";
        public const string ProductTag = "Product";
        public const string BlogTag = "Blog";

        //Setting seperating 
        public const string SepRoles = ";";

        public const string CartSession = "CartSession";

        //Authorization
        public class AppRole
        {
            public const string AdminRole = "Admin";
        }
        public class UserClaims
        {
            public const string Roles = "Roles";
        }

        public class PagerDefault
        {
            public const int PageSize = 12;
            public const int PageIndex = 1;
            public const string Keyword = "";
            public const string SortBy = "DateCreated";
            public const string SortOrder = "DESC";
        }
}
}