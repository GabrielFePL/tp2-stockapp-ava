using AutoMapper.Configuration.Annotations;
using StockApp.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Application.Services
{
    internal class MFAServices : IMFAService
    {
        private readonly ConcurrentDictionary<string, string> _mfaCodes = new ConcurrentDictionary<string, string>();

        public string GenerateMFACode()
        {
            var code = new Random().Next(100000, 999999).ToString();
            _mfaCodes.TryAdd(code, code);
            return code;
        }

        public bool ValidateMFACode(string code, out string storedcode)
        {
            storedcode = string.Empty;
            foreach (var item in _mfaCodes)
            {
                if (item.Value == code)
                {
                    storedcode = item.Key;
                    return true;
                }
            }
            return false;
        }

        public void StoreCode(string key, string code)
        {
            _mfaCodes[key] = code;
        }

        public void RemoveCode(string key)
        {
            _mfaCodes.TryRemove(key, out _);
        }
    }
}
