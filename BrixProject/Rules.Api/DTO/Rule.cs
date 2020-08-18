

namespace Rules.Api.DTO
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
