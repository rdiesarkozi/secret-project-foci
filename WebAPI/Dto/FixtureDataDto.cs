namespace WebAPI.Dto;

public class FixtureDataDto
{
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public DateTime FixtureDate { get; set; }
    public string FixtureId { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public int HomeScore { get; set; }
    public int AwayScore { get; set; } 
}