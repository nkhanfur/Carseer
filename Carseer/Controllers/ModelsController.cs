using Carseer.Api.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Mime;

namespace Carseer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class ModelsController : ControllerBase
    {
        private readonly string _carMakePath;
        private readonly IVehicleService _vehicleService;
        private readonly IWebHostEnvironment _hostingEnvirounment;
        private record struct CarMakeData(int MakeId, string MakeName);

        public ModelsController(IVehicleService vehicleService, IWebHostEnvironment hostingEnvironment)
        {
            _vehicleService = vehicleService;
            _hostingEnvirounment = hostingEnvironment;
            _carMakePath = Path.Combine(_hostingEnvirounment.ContentRootPath, @"files\CarMake.csv");
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int modelyear, string make)
        {
            if (modelyear <= 0 || string.IsNullOrEmpty(make))
            {
                return BadRequest("INVALID_QUERY");
            }

            // Read car make csv file
            List<CarMakeData> carMakeData = new();
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                DetectDelimiter = true,
                DetectDelimiterValues = new string[] { "\t", "," },
                HasHeaderRecord = false,
            };

            using (var reader = new StreamReader(_carMakePath))
            {
                var csv = new CsvReader(reader, configuration);
                while (csv.Read())
                {
                    if (!int.TryParse(csv.GetField<string>(0), out _))
                    {
                        continue;
                    }

                    carMakeData.Add(new CarMakeData
                    {
                        MakeId = csv.GetField<int>(0),
                        MakeName = csv.GetField<string>(1)!
                    });
                }
            }

            var carMakeDataDic = carMakeData.ToDictionary(c => c.MakeName, c => c);
            var carMake = carMakeDataDic.Single(a => a.Key.Equals(make, StringComparison.OrdinalIgnoreCase)).Value;
            var models = await _vehicleService.GetModelsForMakeIdYear(carMake.MakeId, modelyear);
            if (models.Count == 0)
            {
                return BadRequest("NO_RESULT");
            }

            return Ok(new { Models = models });
        }
    }
}