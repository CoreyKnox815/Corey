    using System.Data.SqlClient;

    public class SqlRepository //: IUserStorageRepo, IComepetitionStorageRepo //by extending the interface, this class agrees to fulfill all the methods of the interface.
    {
        // Fields

        // your connection string has all the details needed to connect to a database
        private string _connectionString;

        //the readonly modifier allows us to set a value in the constructor, then prevents modification.

        public SqlRepository()
        {
            string path = @"C:\Bootcamp\gymproj\Data\Gymnastics.txt";
            this._connectionString = File.ReadAllText(path);
        }

        public void StoreUser(Gymnast gymnast)  // this saves a gymnast to SQL
        {
            //This using statement allows for this reference to be disposed of, and the connection closed after the 
            //method finishes running
            using SqlConnection connection = new SqlConnection(this._connectionString);

            //Once we create our SqlConnection object, we call a method off of it to open the connection to the database.
            connection.Open();

            //We then create a string, for the query or statement we are going to run, that allows us to update it's parameters later.
            string cmdText =
                @"INSERT INTO BootcampDB1.dbo.Users (UserId, UserName, FirstName, LastName, DateOfBirth)
                VALUES
                (@User_Id, @User_Name, @First_Name, @Last_Name, @DateOfBirth)";

            //So we use the parameterized string above to create a SqlCommand object. 
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@User_Id", gymnast.gymnastId);
            cmd.Parameters.AddWithValue("@User_Name", gymnast.userName);
            cmd.Parameters.AddWithValue("@First_Name", gymnast.fname);
            cmd.Parameters.AddWithValue("@Last_Name", gymnast.lname);
            cmd.Parameters.AddWithValue("@DateOfBirth", gymnast.dob);

            //We execute the above INSERT with this Execute non query. Because we are not querying the DB
            //we will execute this as a nonquery. 
            cmd.ExecuteNonQuery();

            //We then close the connection, and after line 62 - our SqlConnection object is disposed of,
            //because we created it with that using statement above. 
            connection.Close();
        }
        public Gymnast GetGymnast(string userName) // this retrieves a gymnast from SQL
        {
            Gymnast gymnast = new Gymnast();

            using SqlConnection connection = new SqlConnection(this._connectionString);
            connection.Open();

            string cmdText = @$"SELECT UserId, UserName, FirstName, LastName, DateOfBirth FROM BootcampDB1.dbo.Users WHERE UserName = @User_Name;";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);
            cmd.Parameters.AddWithValue("@user_Name", userName);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Guid gymastId = new Guid(reader.GetString(0));
                var user_Name = reader.GetString(1);
                var firstName = reader.GetString(2);
                var lastName = reader.GetString(3);
                DateTime dob = reader.GetDateTime(4);


                gymnast = new Gymnast(gymastId, user_Name, firstName, lastName, dob);
                
            }
            
            return gymnast;
        }

        public void UpdateCompetition(Competition competition, Gymnast gymnast)   // this updates a competition by adding a gymnast to the linking table by adding a row with competionId and UserID
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            connection.Open();

            string cmdText =
                @"INSERT INTO BootcampDB1.dbo.GymnastsInCompetition (CompetitionId, UserID)
                VALUES
                ( @Competition_Id, @User_Id)";

            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@Competition_Id", competition.competionId);
            cmd.Parameters.AddWithValue("@User_Id", gymnast.gymnastId);
            
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void CreateCompetition(Competition competition) // this creates competition and stores it in SQL (better name would have been store competition)
        {
            using SqlConnection connection = new SqlConnection(this._connectionString);
            //Once we create our SqlConnection object, we call a method off of it to open the connection to the database.
            connection.Open();

            //We then create a string, for the query or statement we are going to run, that allows us to update it's parameters later.
            string cmdText =
                @$"INSERT INTO BootCampDB1.dbo.Competition (CompetitionId, CompetitionName, StartDate, EndDate, Location)
                VALUES
                (@CompetitionID, @CompetitionName, @StartDate, @EndDate, @Location)";

            //So we use the parameterized string above to create a SqlCommand object. 
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            cmd.Parameters.AddWithValue("@CompetitionId", competition.competionId);
            cmd.Parameters.AddWithValue("@CompetitionName", competition.competitionName);
            cmd.Parameters.AddWithValue("@StartDate", competition.competitionStartDate);
            cmd.Parameters.AddWithValue("@EndDate", competition.competitionEndDate);
            cmd.Parameters.AddWithValue("@Location", competition.location);
  

            //We execute the above INSERT with this Execute non query. Because we are not querying the DB
            //we will execute this as a nonquery. 
            cmd.ExecuteNonQuery();

            //We then close the connection, and after line 62 - our SqlConnection object is disposed of,
            //because we created it with that using statement above. 
            connection.Close();
        }

        public List<Competition> GetAllCompetitions()  // this retrieves all competitions and returns them as a List of Competitions so they can be printed
        {
            // a SQLConnection object is created to connect to the database, and is provided the connection string
            using SqlConnection connection = new SqlConnection(this._connectionString);
            List<Competition> competitions = new();

            connection.Open(); // open the connection to the database

            string cmdText = @"select CompetitionId, CompetitionName, StartDate, EndDate, Location from Competition";

            //Create the command object
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            //We create a SqlDataReader... so that we can read our data from the database.
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) // Read each row returned in query
            {
                Guid CompetitionId = new Guid(reader.GetString(0));
                string competitionName = reader.GetString(1);
                DateTime startDate = reader.GetDateTime(2);
                DateTime endDate = reader.GetDateTime(3);
                string location = reader.GetString(4);

                Competition competition = new(CompetitionId, competitionName, startDate, endDate, location);
                competitions.Add(competition);                                                                              
            }
            connection.Close();

            return competitions;
        }

        public List<Gymnast> GetAllGymnasts() // this retrieves a list of all gymnasts and returns them as List<Gymnast> (List of gymnasts), so they can be printed out
        {
            // a SQLConnection object is created to connect to the database, and is provided the connection string
            using SqlConnection connection = new SqlConnection(this._connectionString);
            List<Gymnast> gymnasts = new();

            connection.Open(); // open the connection to the database

            string cmdText = @"select UserId, UserName, FirstName, LastName, DateOfBirth from Users";

            //Create the command object
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            //We create a SqlDataReader... so that we can read our data from the database.
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) // Read each row returned in query
            {
                Guid userId = new Guid(reader.GetString(0));
                string UserName = reader.GetString(1);
                string FirstName = reader.GetString(2);
                string LastName = reader.GetString(3);
                DateTime dob = reader.GetDateTime(4);

                Gymnast gymnast = new(userId, UserName, FirstName, LastName, dob);
                gymnasts.Add(gymnast);                                                                              
            }
            connection.Close();

            return gymnasts;
        }

        public List<Competition> GetCompetitions(Gymnast gymnast) // this looks similar to the one above because of naming, but This is specifically getting the list of competitions which a gymnast is signed up for
        {
            // a SQLConnection object is created to connect to the database, and is provided the connection string
            using SqlConnection connection = new SqlConnection(this._connectionString);
            List<Competition> competitions = new();

            connection.Open(); // open the connection to the database

            string cmdText = @$"select C.CompetitionId, CompetitionName, StartDate, EndDate, Location
                             FROM Competition as C
                             join GymnastsInCompetition as G on G.CompetitionId = C.CompetitionId

                             WHERE G.UserId = '{gymnast.gymnastId}'";

            //Create the command object
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            //We create a SqlDataReader... so that we can read our data from the database.
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) // Read each row returned in query
            {
                Guid CompetitionId = new Guid(reader.GetString(0));
                string competitionName = reader.GetString(1);
                DateTime startDate = reader.GetDateTime(2);
                DateTime endDate = reader.GetDateTime(3);
                string location = reader.GetString(4);

                Competition competition = new(CompetitionId, competitionName, startDate, endDate, location);
                competitions.Add(competition);                                                                              
            }
            connection.Close();

            return competitions;
        }

        public List<Competition> GetEligibleCompetitions(Gymnast gymnast)  // this does the exact opposite of above, it gets only competitions which a particular Gymnast is NOT enrolled in. So when a gymnast is trying to enroll it will only show ones they are not already enrolled in
        {
            // a SQLConnection object is created to connect to the database, and is provided the connection string
            using SqlConnection connection = new SqlConnection(this._connectionString);
            List<Competition> competitions = new();

            connection.Open(); // open the connection to the database

            string cmdText = @$"select C.CompetitionId, CompetitionName, StartDate, EndDate, Location
                             FROM Competition as C
                             join GymnastsInCompetition as G on G.CompetitionId = C.CompetitionId

                             WHERE G.UserId != '{gymnast.gymnastId}'";

            //Create the command object
            using SqlCommand cmd = new SqlCommand(cmdText, connection);

            //We create a SqlDataReader... so that we can read our data from the database.
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read()) // Read each row returned in query
            {
                Guid CompetitionId = new Guid(reader.GetString(0));
                string competitionName = reader.GetString(1);
                DateTime startDate = reader.GetDateTime(2);
                DateTime endDate = reader.GetDateTime(3);
                string location = reader.GetString(4);

                Competition competition = new(CompetitionId, competitionName, startDate, endDate, location);
                competitions.Add(competition);                                                                              
            }
            connection.Close();

            return competitions;
        }
     }