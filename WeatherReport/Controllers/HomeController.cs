using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WeatherReport.Models;

namespace WeatherReport.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, MyDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet("{variableName}/{startTimestamp:datetime}/{endTimestamp:datetime}/{cityName?}")]
        public IActionResult GetVariableData(string variableName, DateTime startTimestamp, DateTime endTimestamp, string cityName)
        {
            var query = _dbContext.Variables.Include(x=> x.City)
                .Where(v => v.Name == variableName && v.Timestamp >= startTimestamp && v.Timestamp <= endTimestamp && v.City.CityName == cityName)
                .Select(v => new ViewVariable
                {
                    Name = v.Name,
                    Unit = v.Unit,
                    Value = v.Value,
                    Timestamp = v.Timestamp,
                    CityName = v.City.CityName
                });

            return View(query.ToList());
        }

        [Route("CityData")]
        public ViewResult CityData()
        {
            var january2023Start = new DateTime(2023, 1, 1);
            var january2023End = new DateTime(2023, 1, 31, 23, 59, 59);

            var hottestCity =  _dbContext.Cities
                    .Select(c => new
                    {
                        name = c.CityName,
                        TotalDaysAbove30Celsius = c.Variables
                            .Count(v => v.Name == "Temperature" && Convert.ToUInt32(v.Value) > 30 && v.Timestamp >= january2023Start && v.Timestamp <= january2023End)
                    })
                    .OrderByDescending(c => c.TotalDaysAbove30Celsius)
                    .FirstOrDefault();

                var viewModel = new HottestCityViewModel
                {
                    Title = "Hottest City",
                    CityName = hottestCity.name,
                    TotalDaysAbove30Celsius = hottestCity.TotalDaysAbove30Celsius
                };      

            return View(viewModel);
        }

        public class ViewVariable
        {
            public string Name { get; set; }
            public string Unit { get; set; }
            public string Value { get; set; }
            public string CityName { get; set; }
            public DateTimeOffset Timestamp { get; set; }
        }
        public class HottestCityViewModel
        {
            public string Title { get; set; }
            public string CityName { get; set; }
            public int TotalDaysAbove30Celsius { get; set; }
        }

    }
}