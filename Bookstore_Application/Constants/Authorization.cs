namespace Bookstore_Application.Constants;

public class Authorization
{
    
    public enum Roles{
        Admin,
        User
    }

    public const Roles _defaultRole = Roles.User;
    public const string _defaultEmail = "user@gmail.com";
    public const string _defaultPassword = "Pass@123";
}
