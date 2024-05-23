public interface IUserStorageRepo // these are not necesarry. Don't bother with them
{
    public void StoreUser(Gymnast gymanst);
    public Gymnast GetGymnast(string usernameToFind);
}

public interface IComepetitionStorageRepo // these are not necesarry. Don't bother with them

{
    public void CreateCompetition(Competition competition);
    public List<Competition> GetCompetitions();
    public void UpdateCompetion(Competition competition, List<Gymnast> gymnasts);
}


