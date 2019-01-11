using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_AllShipping.Services
{
    public class InsertDictionary : SortedDictionary<string, string>
    {
        public string GetString(string name)
        {
            if (ContainsKey(name))
            {
                return this[name];
            }
            return string.Empty;
        }

        public int GetInt(string name, int defaultValue = 0)
        {
            var result = defaultValue;
            if (ContainsKey(name))
            {
                int temp = 0;
                if (int.TryParse(name, out temp))
                {
                    result = temp;
                }
            }
            return result;
        }

        public void SetString(string name, string value)
        {
            if (ContainsKey(name))
            {
                this[name] = value;
            }
            else
            {
                Add(name, value);
            }
        }

        public void SetInt(string name, int value)
        {
            SetString(name, value.ToString());
        }
    }
}
