using WebAPI.Models.Group;

namespace WebAPI.Interfaces;

public interface IInvitationService
{
    Task<GroupInvitation> CreateInvitationAsync(int groupId, string email, string invitedByUserId);
    Task<GroupInvitation?> GetInvitationByTokenAsync(string token);
    Task<IEnumerable<GroupInvitation>> GetPendingInvitationsForEmailAsync(string email);
    Task<IEnumerable<GroupInvitation>> GetPendingInvitationsForGroupAsync(int groupId);
    Task<GroupMember?> AcceptInvitationAsync(string token, string userId);
    Task<bool> DeclineInvitationAsync(string token);
    Task<bool> CancelInvitationAsync(int invitationId);

}