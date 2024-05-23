public interface IUserStorageRepo
{
    public void StoreUser(Gymnast gymanst);
    public Gymnast GetGymnast(string usernameToFind);
}

public interface IComepetitionStorageRepo
{
    public void CreateCompetition(Competition competition);
    public List<Competition> GetCompetitions();
    public void UpdateCompetion(Competition competition, List<Gymnast> gymnasts);
}


