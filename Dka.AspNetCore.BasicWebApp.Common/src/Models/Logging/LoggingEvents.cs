namespace Dka.AspNetCore.BasicWebApp.Common.Models.Logging
{
    public static class LoggingEvents
    {
        // CRUD operation event IDs.
        public const int CreateItem = 2001000;
        public const int ReadItems = 2001001;
        public const int ReadItem = 2001002;
        public const int UpdateItem = 2001003;
        public const int DeleteItem = 2001004;

        // CRUD operation "bad data" event IDs.
        public const int CreateItemBadData = 4001000;
        public const int ReadItemsBadData = 4001001;
        public const int ReadItemBadData = 4001002;
        public const int UpdateItemBadData = 4001003;
        public const int DeleteItemBadData = 4001004;        
        
        // CRUD operation "not found" event IDs.
        public const int ReadItemsNotFound = 4041001;
        public const int ReadItemNotFound = 4041002;
        public const int UpdateItemNotFound = 4041003;
        public const int DeleteItemNotFound = 4041004;
        
        // CRUD operation "exception" event IDs.
        public const int CreateItemFailed = 5001000;
        public const int ReadItemsFailed = 5001001;
        public const int ReadItemFailed = 5001002;
        public const int UpdateItemFailed = 5001003;
        public const int DeleteItemFailed = 5001004;
        
        // SignIn/SignOut operation event IDs.
        public const int SignInUser = 2001005;
        public const int SignOutUser = 2001006;
        
        // SignIn/SignOut operation "bad data" event IDs.
        public const int SignInUserBadData = 4001005;
        public const int SignOutUserBadData = 4001006;
        
        // SignIn/SignOut operation "not found" event IDs.
        public const int SignInUserNotFound = 4001005;
        public const int SignOutUserNotFound = 4001006;
        
        // SignIn/SignOut operation "exception" event IDs.
        public const int SignInUserFailed = 4001005;
        public const int SignOutUserFailed = 4001006;
    }
}