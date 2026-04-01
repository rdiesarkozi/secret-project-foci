namespace WebAPI.Data.Enums;

public enum GroupVisibility
{
    /// <summary>
    /// The group is hidden and can only be joined explicitly.
    /// </summary>
    Private = 0,
    
    /// <summary>
    /// The group is visible to all users.
    /// </summary>
    Public = 1
}