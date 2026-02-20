using System;
using System.Collections.Generic;
using System.Text;
using TekCandor.Repository.Interfaces;
using TekCandor.Service.Interfaces;
using TekCandor.Service.Models;

namespace TekCandor.Service.Implementations
{
    public class SettingService: ISettingService
    {

        private readonly ISettingRepository _repository;

        public SettingService(ISettingRepository repository)
        {
            _repository = repository;
        }

        public bool UpdateCallbackAmount(UpdateCallbackAmountDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.CallbackAmount))
                return false;

            return _repository.UpdateCallbackAmount(dto.CallbackAmount);
        }

    }
}
