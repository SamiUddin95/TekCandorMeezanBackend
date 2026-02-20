using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Service.Models;

namespace TekCandor.Service.Interfaces
{
    public interface ISettingService
    {

        bool UpdateCallbackAmount(UpdateCallbackAmountDTO dto);

    }
}
