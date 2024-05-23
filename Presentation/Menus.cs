

public class Menus
{
    static SqlRepository repo = new();

    public static void MainMenu()
    {

        bool isValid = false;
        do
        {
            Console.WriteLine("Please select an option");
            Console.WriteLine("[1] Create New User");
            Console.WriteLine("[2] Gymnast Login");
            Console.WriteLine("[3] Admin login");
            Console.WriteLine("[0] Exit");
            try
            {
                int input = int.Parse(Console.ReadLine());
                Gymnast gymnast = new();
                isValid = true;
                switch (input)
                {
                    case 1:
                        gymnast = CreateUser();
                        GymnastOptionsMenu(gymnast);
                        break;
                    case 2:
                        gymnast = UserLogin();
                        GymnastOptionsMenu(gymnast);
                        break;
                    case 3:
                        AdminLogin();
                        AdminMenu();
                        break;
                    case 0:
                        break;
                    default:
                        Console.WriteLine("Invalid, please select one of the available options.");
                        isValid = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Entry.");
                Console.WriteLine(ex.Message);
                isValid = false;
            }
        }
        while (!isValid);
    }
    public static Gymnast CreateUser()
    {
        string userName;
        string fname;
        string lname;
        DateTime dob;
        bool isValid;

        Console.WriteLine("Please enter a username");
        userName = Console.ReadLine();
        Console.WriteLine("Please enter Gymnast's first name");
        fname = Console.ReadLine();
        Console.WriteLine("Please enter Gymnast's last name");
        lname = Console.ReadLine();
        Console.WriteLine("Please enter Gymnast's date of birth ex: 01/01/1900");
        dob = DateTime.Parse(Console.ReadLine());
        Gymnast gymnast = new(userName, fname, lname, dob);
        repo.StoreUser(gymnast);  // this needs to moved to the controller layer, and we call controller from here instead.
        return gymnast;

    }
    public static Gymnast UserLogin()
    {
        string userName;
        Console.WriteLine("Please enter a username");
        userName = Console.ReadLine();
        Gymnast gymnast = new();
        gymnast = repo.GetGymnast(userName);
        Console.WriteLine($"Welcome {gymnast.fname} {gymnast.lname}!");
        Console.ReadLine();
        return gymnast;

    }
    public static void AdminLogin()
    {
        string pw;
        bool pwValid = false;
        do
        {
            Console.WriteLine("Please enter the Admin Password: ");
            pw = Console.ReadLine();
            if (pw == "1234")
            {
                pwValid = true;
            }
            else
            {
                pwValid = false;
                Console.WriteLine("Invalid password. Please try again.");
            }
        }
        while (!pwValid);

    }
    public static void AdminMenu()
    {
        int selection;
        Console.WriteLine("Please select what you would like to do: ");
        Console.WriteLine("[1] Create new competition");
        Console.WriteLine("[2] Add gymnast to a competition");
        Console.WriteLine("[0] Exit");
        selection = int.Parse(Console.ReadLine());

        switch (selection)
        {
            case 1:
                CreateCompetionMenu();
                break;
            case 2:
                AddGymnastToCompetition();
                break;
            case 0:
                System.Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid Selection");
                break;
        }
    }

    public static void CreateCompetionMenu()
    {
        string competitionName;
        DateTime competitionStartDate;
        DateTime competitionEndDate;
        string location;


        Console.WriteLine("Please enter the name of the competition: ");
        competitionName = Console.ReadLine();
        Console.WriteLine("Please enter the start date of the competition: ");
        competitionStartDate = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Please enter the end date of the competition: ");
        competitionEndDate = DateTime.Parse(Console.ReadLine());
        Console.WriteLine("Please enter the location: ");
        location = Console.ReadLine();

        Competition createdCompetition = new(competitionName, competitionStartDate, competitionEndDate, location);
        repo.CreateCompetition(createdCompetition);
        Console.WriteLine($"Competition ID: {createdCompetition.competionId} was saved!");
    }

    public static void AddGymnastToCompetition()
    {
        int compIndex = 0;
        int gymnastIndex = 0;

        Console.WriteLine("Please select which competition you'd like to add a gymnast to:");
        List<Competition> competitions = repo.GetAllCompetitions();
        foreach (Competition competition in competitions)
        {
            compIndex++; // add 1 to the index, so it starts with 1(instead of 0 as defined above). Eeach time the loop goes through it will increment
            Console.WriteLine($"[{compIndex}] {competition.competitionName}"); // right now just printing the index and the competitionName. You can add more.
        }
        int compSelection = int.Parse(Console.ReadLine()) - 1; // this takes the selection, but decreases the number by 1. Since when you select 1 you're really looking for the first item in a List which starts at 0

        List<Gymnast> gymnasts = repo.GetAllGymnasts();

        Console.WriteLine($"Please select which gymnast you'd like to add to {competitions[compSelection].competitionName}");
        foreach (Gymnast gymnast in gymnasts)
        {
            gymnastIndex++;
            Console.WriteLine($"[{gymnastIndex}] {gymnast.fname} {gymnast.lname}"); // right now just printing the index and the first and last name. You can add more.
        }

        int gymnastSelection = int.Parse(Console.ReadLine()) - 1;

        repo.UpdateCompetition(competitions[compSelection], gymnasts[gymnastSelection]);

        Console.WriteLine($"{gymnasts[gymnastSelection].fname} {gymnasts[gymnastSelection].lname} was added to Competiion {competitions[compSelection].competitionName}");

    }

    public static void GymnastOptionsMenu(Gymnast gymnast)
    {
        bool validSelection = false;
        bool stayActive = true;
        int selection = 0;
        Console.WriteLine($"Welcome {gymnast.fname}!");

        do
        {
            Console.WriteLine($"What would you like to do?");
            Console.WriteLine("[1] View my competitions");
            Console.WriteLine("[2] Sign up for a competiton");
            Console.WriteLine("[0] Exit");
            try
            {
                selection = int.Parse(Console.ReadLine());
                switch (selection)
                {
                    case 1:
                        ViewGymnastCompetitions(gymnast);
                        break;
                    case 2:
                        SignUpForCompetition(gymnast);
                        break;
                    case 0:
                        System.Environment.Exit(0);
                        stayActive = false;
                        break;
                    default:
                        Console.WriteLine("Invalid entry.");
                        validSelection = false;
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                validSelection = false;
            }
        }
        while (!validSelection && stayActive);
    }

    public static void ViewGymnastCompetitions(Gymnast gymnast)
    {
        List<Competition> userCompetitions = new();
        userCompetitions = repo.GetCompetitions(gymnast);

        foreach(Competition competion in userCompetitions)
        {
            Console.WriteLine(competion.ToString());
        }
        Console.ReadLine();

    }

    public static void SignUpForCompetition(Gymnast gymnast)
    {
        int compIndex = 0;

        Console.WriteLine("Please select which competition you'd like to sign up for:");
        List<Competition> competitions = repo.GetEligibleCompetitions(gymnast);
        foreach (Competition competition in competitions)
        {
            compIndex++; // add 1 to the index, so it starts with 1(instead of 0 as defined above). Eeach time the loop goes through it will increment
            Console.WriteLine($"[{compIndex}] {competition.competitionName}"); // right now just printing the index and the competitionName. You can add more.
        }
        int compSelection = int.Parse(Console.ReadLine()) - 1;
        repo.UpdateCompetition(competitions[compSelection], gymnast);
    }

    

}