using System.ComponentModel.DataAnnotations.Schema;

namespace WeatherReport.Models
{
    [Table("City")]
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }
        public ICollection<Variable> Variables { get; set; }

    }
}
