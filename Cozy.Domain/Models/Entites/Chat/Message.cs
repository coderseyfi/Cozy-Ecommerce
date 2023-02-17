using Cozy.Domain.AppCode.Infrastructure;
using Cozy.Domain.Models.Entities.Membership;

namespace Cozy.Domain.Models.Entites.Chat
{
    public class Message : BaseEntity
    {
        public int FromId { get; set; }
        public virtual CozyUser From { get; set; }
        public int? ToId { get; set; }
        public virtual CozyUser To { get; set; }
        public int? GroupId { get; set; }
        public virtual Group Group { get; set; }
        public string Text { get; set; }

    }
}
