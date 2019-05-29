using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigKeyLoggerWcf.Molde
{
    public enum LoginResult
    {
        USER_NAME_ERR,
        USER_PWD_ERR,
        USER_NAME_PWD_ERR,
        FAIL_EXCEP,
        SUCCESS
    }

    public enum LoginUser
    {
        ADMIN_USER,
        ORDINARY_USER
    }

    public class LoginInfo
    {
        
    }
}
