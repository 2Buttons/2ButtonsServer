using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CommonLibraries;

namespace AuthorizationServer.ViewModels.OutputParameters.User
{
    public class UserRegistrationViewModel
  {
      public string Login { get; set; }
      public DateTime BirthDate { get; set; } = DateTime.Now.Date;
      public SexType SexType { get; set; }
      public string Phone { get; set; }
      public string Email { get; set; }
      public string City { get; set; }
  }
}
