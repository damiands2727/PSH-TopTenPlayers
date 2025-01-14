using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models.Domain.Player;
using Sabio.Models.Requests.Players;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {

        private ILogger _logger;
        private IPlayerService _playerService;
        public PlayerController(
            ILogger<PlayerController> logger,
            IPlayerService playerService
            ) 
        {
            _logger = logger;
            _playerService = playerService;
        }

        [HttpPost]
        public ActionResult AddSingularPlayer(PlayerAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int id = _playerService.Add(model);
                ItemResponse<int> response = new ItemResponse<int>() { Item = id };
                result = StatusCode(201,response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }


            return result;

        }

        [HttpPost("generate-and-add")]
        public async Task<ActionResult> GenerateAndAddPlayers()
        {
            ObjectResult result = null;

            try
            {
                // Generate players from the API
                AddManyPlayersRequest playersRequest = await _playerService.GeneratePlayersFromApi();

                // Add players using the batch insert method
                _playerService.AddMany(playersRequest);

                SuccessResponse response = new SuccessResponse();
                result = StatusCode(201, response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }

            return result;
        }
        [HttpGet("top10")]
        public ActionResult<ItemsResponse<Player>> GetTop10Players()
        {
            ObjectResult result = null;

            try
            {
                List<Player> players = _playerService.GetTop10Players();
                if (players == null || players.Count == 0)
                {
                    ErrorResponse response = new ErrorResponse("No players found.");
                    result = StatusCode(404, response);
                }
                else
                {
                    ItemsResponse<Player> response = new ItemsResponse<Player> { Items = players };
                    result = StatusCode(200, response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }

            return result;
        }
        [HttpDelete]
        public ActionResult<SuccessResponse> Delete()
        {

            int code = 200;
            BaseResponse response = null;

            try
            {
                _playerService.Delete();
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        [HttpGet("downloadCsv")]
        public IActionResult DownloadCsv()
        {
            // Get player data (this could be fetched from a database or in-memory)
            var players = _playerService.GetTop10Players();

            // Create a CSV using a memory stream
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream, Encoding.UTF8);
            var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            // Write records (you can customize this to match your player object)
            csv.WriteRecords(players);

            writer.Flush();
            memoryStream.Position = 0; // Reset stream position to the beginning

            // Generate the filename with date and time
            var fileName = $"TopTenPlayersReport_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.csv";

            // Return the CSV file as a downloadable response
            return File(memoryStream, "text/csv", fileName);
        }
    }
}
