using System.Runtime.Serialization;

namespace TimeZoneBot.Models
{
    [DataContract]
    public class TimeZoneModel
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "message ")]
        public string Message { get; set; }

        [DataMember(Name = "countryCode")]
        public string CountryCode { get; set; }

        [DataMember(Name = "zoneName")]
        public string ZoneName { get; set; }

        [DataMember(Name = "abbreviation")]
        public string Abbreviation { get; set; }

        [DataMember(Name = "gmtOffset")]
        public string GmtOffset { get; set; }

        [DataMember(Name = "dst")]
        public string Dst { get; set; }

        [DataMember(Name = "timestamp")]
        public int Timestamp { get; set; }
    }
}