using System;
namespace SimpleEFDbSeeder.Entities
{
	public class Country : BaseEntity
	{
        public string? CountryName { get; set; }

        public string? Alpha2 { get; set; }

        public string? Alpha3 { get; set; }

        public string? NumericCode { get; set; }

        public string? Es { get; set; }

        public string? En { get; set; }

    }
}

