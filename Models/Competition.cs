public class Competition
{
    public string competitionName {get; set;}
    public DateTime competitionStartDate {get; set;}
    public DateTime competitionEndDate {get; set;}
    public string location {get; set;}
    public List<Gymnast> gymnasts {get; set;}
    
    public Guid competionId {get; set;}

    public Competition()
    {

    }
    public Competition( string competitionName, DateTime competitionStartDate, 
    DateTime competitionEndDate, string location)

    {
        this.competitionName = competitionName;
        this.competitionStartDate = competitionStartDate;
        this.competitionEndDate = competitionEndDate;
        this.location = location;
        this.competionId = Guid.NewGuid();
    }
    public Competition(Guid competionID, string competitionName, DateTime competitionStartDate,  
    DateTime competitionEndDate, string location)
    {
        this.competitionName = competitionName;
        this.competitionStartDate = competitionStartDate;
        this.competitionEndDate = competitionEndDate;
        this.location = location;
        this.competionId = competionID;
    }


    public override string ToString()
    {
        return $"Competition Name: {competitionName}\n" +
               $"Competition Start Date: {competitionStartDate}\n" +
               $"Competition End Date: {competitionEndDate}\n" +
               $"Competition Location: {location}\n" +
               "-----------------------------";
    }
}