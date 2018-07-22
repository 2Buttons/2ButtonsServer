using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace AccountServer.ViewModels.InputParameters
{
    public class GetUserAvatar :UserIdViewModel
    {
      public AvatarSizeType AvatarSizeType { get; set; } = AvatarSizeType.Original;
    }
}
