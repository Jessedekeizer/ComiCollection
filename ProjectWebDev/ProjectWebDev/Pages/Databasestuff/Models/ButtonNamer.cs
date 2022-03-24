
namespace ProjectWebDev.Pages.Databasestuff.Models;

public class ButtonNamer
{
    public string Button4Href(string userrol)
    {
        switch (userrol)
        {
            case "o":
                return  "/Overzichten/AccountScreenUser";
            case "a":
                return "/Overzichten/AccountScreenUser";
            case "u":
                return "/Overzichten/AccountScreenUser";
            default:
                return "/Login/LoginScreen";
        }
        return null;
    }
    
    public string Button4Name(string userrol)
    {
        switch (userrol)
        {
            case "o":
                return "Mijn Account";
            case "a":
                return "Mijn Account";
            case "u":
                return "Mijn Account";
            default:
                return "Login";
            
        }
        return null;
    }
    public string Button3Href(string userrol)
    {
        switch (userrol)
        {
            case "o":
                return  "/Overzichten/EditBooks";
            case "a":
                return "/Overzichten/EditBooks";
            
            case "u":
                return "/Overzichten/MyCollection";
            default:
                return "/Overzichten/MyCollection";
            
        }
        return null;
    }
    
    public string Button3Name(string userrol)
    {
        switch (userrol)
        {
            case "o":
                return "Bewerk Boeken";
            case "a":
                return "Bewerk Boeken";
            case "u":
                return "My Collection";
            default:
                return "My Collection";
        }
        return null;
    }
}