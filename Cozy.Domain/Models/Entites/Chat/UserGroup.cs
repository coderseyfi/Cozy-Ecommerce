using Cozy.Domain.Models.Entities.Membership;

namespace Cozy.Domain.Models.Entites.Chat
{
    public class UserGroup
    {
        public int UserId { get; set; }
        public virtual CozyUser User { get; set; }
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
    }
}
