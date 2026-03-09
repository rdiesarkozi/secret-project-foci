namespace WebAPI.Dto;

public class GroupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string JoinCode { get; set; }
    public string Visibility { get; set; }
    public DateTime CreatedAtUtc { get; set; }
}