using System;
using System.Collections.Generic;
using System.Text;
using CoreApp.Data.Enums;

namespace CoreApp.Data.Interfaces
{
    public interface ISwitchable
    {
        Status Status { set; get; }
    }
}
