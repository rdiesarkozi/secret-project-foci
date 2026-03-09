using WebAPI.Interfaces;
using WebAPI.Models.Group;

namespace WebAPI.Services;

public class InvitationService : IInvitationService
{
    public Task<GroupInvitation> CreateInvitationAsync(int groupId, string email, string invitedByUserId)
    {
        throw new NotImplementedException();
    }

    public Task<GroupInvitation?> GetInvitationByTokenAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GroupInvitation>> GetPendingInvitationsForEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GroupInvitation>> GetPendingInvitationsForGroupAsync(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<GroupMember?> AcceptInvitationAsync(string token, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeclineInvitationAsync(string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CancelInvitationAsync(int invitationId)
    {
        throw new NotImplementedException();
    }
}