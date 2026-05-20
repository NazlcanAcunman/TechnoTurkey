namespace EventTicket.Core.Enums;

public static class Roles
{
    public const string SuperAdmin = "SuperAdmin";
   
    public const string Admin = "Admin";

    public const string Member = "Member";

    public const string Guest = "Guest";

    public static readonly string[] All = { SuperAdmin, Admin, Member, Guest };
}