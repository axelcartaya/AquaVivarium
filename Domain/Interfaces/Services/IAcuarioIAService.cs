using Domain.Models;
using Domain.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Services
{
    public interface IAcuarioIAService
    {
        Task<string> GenerarAnalisisIAAsync(AcuarioTransferDto acuario);
    }
}
