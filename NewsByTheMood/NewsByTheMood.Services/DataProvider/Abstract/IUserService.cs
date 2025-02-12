using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsByTheMood.Services.DataProvider.Abstract
{
    // Interface of users provider service
    public interface IUserService
    {
        public bool IsEmailExists(string email);
        public bool IsUserNameExists(string username);
    }
}
