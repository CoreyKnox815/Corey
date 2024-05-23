

public class Gymnast
{
    public string userName {get; set;}
    public string fname {get; set;}
    public string lname {get; set;}
    public DateTime dob {get; set;}
    public Guid gymnastId {get; set;}

    public Gymnast()
    {

    }
    public Gymnast(string userName, string fname, string lname, DateTime dob)
    {
        this.userName = userName;
        this.fname = fname;
        this.lname = lname;
        this.dob = dob;
        this.gymnastId = Guid.NewGuid();
    }
    public Gymnast(Guid gymnastId, string userName, string fname, string lname, DateTime dob)
    {
        this.userName = userName;
        this.fname = fname;
        this.lname = lname;
        this.dob = dob;
        this.gymnastId = gymnastId;
    }
    
}