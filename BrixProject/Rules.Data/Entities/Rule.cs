using System;
using System.Collections.Generic;
using System.Text;

namespace Rules.Data.Entities
{
    public class Rule
    {
        public int Id { get; set; }
        public int ProviderId { get; set; }
        public string Parameter { get; set; }
        public string Condition { get; set; }
        public string Value { get; set; }
    }
}
