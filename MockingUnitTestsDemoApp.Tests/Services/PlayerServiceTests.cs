using FluentAssertions;
using MockingUnitTestsDemoApp.Impl.Models;
using MockingUnitTestsDemoApp.Impl.Repositories.Interfaces;
using MockingUnitTestsDemoApp.Impl.Services;
using NSubstitute;

namespace MockingUnitTestsDemoApp.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly PlayerService _subject;
        private readonly IPlayerRepository _mockPlayerRepository;
        private readonly ITeamRepository _mockTeamRepository;
        private readonly ILeagueRepository _mockLeagueRepository;                       

        public PlayerServiceTests()
        {
            _mockPlayerRepository = Substitute.For<IPlayerRepository>();
            _mockTeamRepository = Substitute.For<ITeamRepository>();
            _mockLeagueRepository = Substitute.For<ILeagueRepository>();            
            _subject = new PlayerService(_mockPlayerRepository, _mockTeamRepository, _mockLeagueRepository);
        }

        private List<Player> GetFakePlayers()
        {
            var players = new List<Player>
            {
                new Player { ID = 1, FirstName = "Vinicius", LastName = "Júnior", DateOfBirth = DateTime.Parse("07/12/2000"), TeamID = 1 },
                new Player { ID = 2, FirstName = "Richarlisson", LastName = "Andrade", DateOfBirth = DateTime.Parse("05/10/1997"), TeamID = 1 },
                new Player { ID = 3, FirstName = "Karim", LastName = "Benzema", DateOfBirth = DateTime.Parse("12/19/1987"), TeamID = 2 },
                new Player { ID = 4, FirstName = "Kylian", LastName = "Mbappé", DateOfBirth = DateTime.Parse("12/20/1998"), TeamID = 2 },
                new Player { ID = 5, FirstName = "Kai", LastName = "Havertz", DateOfBirth = DateTime.Parse("06/11/1999"), TeamID = 3 },
                new Player { ID = 6, FirstName = "Thomas", LastName = "Müller", DateOfBirth = DateTime.Parse("09/13/1989"), TeamID = 3 }
            };

            return players;
        }

        private List<Team> GetFakeTeams()
        {
            var teams = new List<Team>
            {
                new Team { ID = 1, Name = "Brasil", FoundingDate = DateTime.Parse("01/01/1914"), LeagueID = 1 },
                new Team { ID = 2, Name = "França", FoundingDate = DateTime.Parse("01/01/1904"), LeagueID = 2 },
                new Team { ID = 3, Name = "Alemanha", FoundingDate = DateTime.Parse("01/01/1908"), LeagueID = 2 }
            };

            return teams;
        }

        private List<League> GetFakeLeagues()
        {
            return new List<League>
            {
                new League {ID = 1, Name = "Copa América", FoundingDate = DateTime.Parse("01/01/1916")},
                new League {ID = 2, Name = "Eurocopa", FoundingDate = DateTime.Parse("01/01/1958")}
            };
        }

        [Theory]
        [InlineData(1)]
        public void GetForLeague_ValidId_ReturnPlayers(int id)
        {
            // Arrange
            _mockLeagueRepository.IsValid(Arg.Any<int>()).Returns(true);
            _mockTeamRepository.GetForLeague(Arg.Any<int>()).Returns(GetFakeTeams());
            _mockPlayerRepository.GetForTeam(Arg.Any<int>()).Returns(GetFakePlayers());

            // Act
            var players = _subject.GetForLeague(id);

            // Assert
            players.Should().NotBeEmpty();
        }

        [Theory]
        [InlineData(1)]
        public void GetForLeague_InvalidId_ReturnEmpty(int id)
        {
            // Arrange
            _mockLeagueRepository.IsValid(Arg.Any<int>()).Returns(false);

            // Act
            var players = _subject.GetForLeague(id);

            // Assert
            players.Should().BeEmpty();
        }
    }
}