using System.Collections.Generic;

namespace MVC.Models.ConfigurationViewModels
{
    public class UserViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Id { get; set; }

        public List<string> Groups { get; set; }

        public string Group { get; set; }
    }
}
