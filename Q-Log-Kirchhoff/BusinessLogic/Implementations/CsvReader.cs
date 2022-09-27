using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MVC.BusinessLogic.Implementations
{
    public class CsvReader : ICsvReader
    {

        private readonly ILogger<CsvReader> _logger;

        private IFormFile _file;

        public CsvReader(ILogger<CsvReader> logger)
        {
            _logger = logger;
        }

        private bool _isValid;
        public bool IsValid { get => _isValid; }

        private string[] _keys;
        public string[] Keys => _keys;

        private string[][] _values;
        public string[][] Values => _values;

        public char splitChar = ';';

        public void Read(IFormFile file)
        {
            _file = file;
            if (_file == null)
            {
                _isValid = false;
                return;
            }

            try
            {
                var reader = new StreamReader(_file.OpenReadStream());
                if (reader == null)
                {
                    _isValid = false;
                    return;
                }
                var keys = reader.ReadLine().Split(splitChar);
                _keys = keys.ToArray();

                if (keys == null || keys.Length == 0)
                {
                    _isValid = false;
                    return;
                }

                var lineValues = new List<List<string>>();

                var line = "";
                while (!String.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var values = line.Split(splitChar);
                    var actualValues = new List<string>();
                    string tmp = "";
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i].Length >= 1 && values[i][0] == '"')
                        {
                            var entryLength = values[i].Length;
                            // in this case the entry wasnt divided by , and is just put in "" for fun
                            if (values[i][entryLength - 1] == '"')
                            {
                                var chars = values[i].TakeLast(values[i].Length - 1).ToArray();
                                chars = chars.Take(chars.Length - 1).ToArray();
                                tmp = new string(chars);
                                actualValues.Add(tmp);
                                tmp = "";
                            }
                            else
                            {
                                // Dont add the " to the value
                                var chars = values[i].TakeLast(values[i].Length - 1).ToArray();
                                tmp = new string(chars);
                            }
                        }
                        else
                        {
                            if (tmp != "")
                            {

                                if (values[i].Length >= 1 && values[i][values[i].Length - 1] == '"')
                                {
                                    // dont add the " to the value
                                    var chars = values[i].Take(values[i].Length - 1).ToArray();
                                    tmp += ",";
                                    tmp += new string(chars);
                                    actualValues.Add(tmp);
                                    tmp = "";
                                }
                                else
                                {
                                    tmp += values[i];
                                }
                            }
                            else
                            {
                                actualValues.Add(values[i]);
                            }
                        }
                    }
                    if (actualValues.Count != _keys.Length)
                    {
                        _logger.LogWarning("Number of Values does not match number of keys!");
                        _isValid = false;
                        return;
                    }
                    lineValues.Add(actualValues);
                }

                _values = lineValues.Select(l => l.ToArray()).ToArray();


            }
            catch (Exception e)
            {
                _logger.LogWarning("Error while trying to validate csv file. Message: " + e.Message);
                _isValid = false;
                return;
            }
            _isValid = true;
        }

        public bool ContainsKeys(string[] keys)
        {
            foreach (var key in keys)
            {
                bool contained = false;
                foreach (var k in _keys)
                {
                    if (k.ToUpper() == key.ToUpper())
                        contained = true;
                }
                if (!contained)
                    return false;
            }
            return true;
        }
    }
}
