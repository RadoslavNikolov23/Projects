using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyExchange
{
    public class Currency
    {
        public Currency(string name, string code, string forOne, string leva, string curs)
        {
            Name = name;
            Code = code;
            ForOne = forOne;
            Leva = leva;
            Curs = curs;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string ForOne { get; set; }
        public string Leva { get; set; }
        public string Curs { get; set; }

        public override string ToString()
        {
            return $"{Name} - {Code} - {ForOne} - {Leva} - {Curs}";
        }

    }
}
