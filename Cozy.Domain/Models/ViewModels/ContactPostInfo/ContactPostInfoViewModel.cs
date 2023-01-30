using Cozy.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.ViewModels.ContactPostInfo
{
    public class ContactPostInfoViewModel
    {
        public ContactPost ContactPosts { get; set; }
        public ContactInfo ContactInfos { get; set; }
    }
}
