using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherReport.Models
{
    [Table("Variable")]
    public class Variable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public string Value { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        
        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public  virtual City City { get; set; }
    }
}
