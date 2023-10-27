using System;
namespace SimpleEFDbSeeder.Entities
{
    public class Currency : BaseEntity
    {
        public string? CurrencyName { get; set; }

        public string? CurrencyCode { get; set; }

        public string? Symbol { get; set; }

        public string? En { get; set; }

        public string? Es { get; set; }

    }
}

