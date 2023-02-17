using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cozy.Domain.Models.ViewModels
{
    /*
     {
            friendId: 2,
            friendName: "Demo2",
            lastMessage: "bye bye",
            date: "2022-11-12 12:44",
        }
     */
    public class MessageViewModel
    {
        public int FriendId { get; set; }
        public string FriendName { get; set; }
        public string LastMessage { get; set; }
        public string Date { get; set; }
    }
}
