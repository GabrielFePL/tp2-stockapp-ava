using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Application.Interfaces
{
    internal interface IMFAService
    {
        string GenerateMFACode();
        bool ValidateMFACode(string code, out string storedcode);
    }
}
