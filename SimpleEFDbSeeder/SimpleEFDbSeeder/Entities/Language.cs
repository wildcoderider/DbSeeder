using System;
namespace SimpleEFDbSeeder.Entities
{
    public class Language : BaseEntity
    {
        public string? LanguageName { get; set; }

        public string? IsoCode { get; set; }

        public string? En { get; set; }

        public string? Es { get; set; }

    }
}

