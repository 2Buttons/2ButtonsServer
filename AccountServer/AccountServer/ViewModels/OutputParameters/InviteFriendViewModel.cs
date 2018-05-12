using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountServer.Models.Vk;
using Newtonsoft.Json;

namespace AccountServer.ViewModels.OutputParameters
{
    public class InviteFriendViewModel
    {
      public int ExternalUserId { get; set; }

      public string FirstName { get; set; }

      public string LastName { get; set; }

      public string SmallPhoto { get; set; }

      internal static InviteFriendViewModel ToViewModel(VkFriendData vkFriend)
      {
        var result = new InviteFriendViewModel()
        {
          ExternalUserId = vkFriend.UserId,
          FirstName = vkFriend.FirstName,
          LastName = vkFriend.LastName,
          SmallPhoto = vkFriend.SmallPhoto
        };
        return result;
      }
  }
}
