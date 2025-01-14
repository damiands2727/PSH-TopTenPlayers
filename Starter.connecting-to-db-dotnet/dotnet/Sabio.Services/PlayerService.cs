using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Sabio.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Services.Interfaces;
using Sabio.Models.Requests.Players;
using System.Numerics;
using Newtonsoft.Json;
using System.Net.Http;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Player;
using static System.Net.Mime.MediaTypeNames;
using static System.Formats.Asn1.AsnWriter;

namespace Sabio.Services
{
    public class PlayerService:IPlayerService
    {
        IDataProvider _data = null;
        public PlayerService(IDataProvider data)
        {
            _data = data;
        }


        public List<Player> GetTop10Players()
        {
            List<Player> players = null;

            string procName = "[dbo].[Stats_Top10Players]";
            _data.ExecuteCmd(procName, inputParamMapper: null,

                singleRecordMapper: delegate

                (IDataReader reader, short set)
            {
                int startingIndex = 0;
                Player player = MapSinglePlayer(reader, ref startingIndex);
           

                if (players == null)
                {
                    players = new List<Player>();
                }

                players.Add(player);
            });

            return players;
        }
       

        public int Add(PlayerAddRequest model)
        {
            int id = 0;
            string procName = "[dbo].[Players_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;
                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });

            return id;
        }

        public void Delete()
        {
            string procName = "[dbo].[Players_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper:null, returnParameters: null);
        }

        public void AddMany(AddManyPlayersRequest model)
        {
            string procName = "[dbo].[Players_InsertMany]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                SqlParameter playersParam = new SqlParameter("@Players", SqlDbType.Structured);
                playersParam.TypeName = "dbo.PlayerTableType"; 
                playersParam.Value = ConvertToPlayerDataTable(model.Players);
                col.Add(playersParam);
            });
        }


        public async Task<AddManyPlayersRequest> GeneratePlayersFromApi()
        {
            int addRandomNumber = new Random().Next(0, 11);

            string apiUrl = $"https://randomuser.me/api/?results={addRandomNumber}&inc=name,picture";
            using HttpClient client = new HttpClient();

            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();
                string jsonResponse = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonConvert.DeserializeObject<RandomUserApiResponse>(jsonResponse);

                List<PlayerAddRequest> players = apiResponse.Results.Select(user => new PlayerAddRequest
                {
                    NickName = $"{user.Name.Title} {user.Name.First} {user.Name.Last}",
                    ProfileImage = user.Picture.Large,
                    StatId = new Random().Next(1, 3), 
                    Score = new Random().Next(1, 101) 
                }).ToList();

                return new AddManyPlayersRequest { Players = players };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while generating players from API", ex);
            }
        }




        private DataTable ConvertToPlayerDataTable(List<PlayerAddRequest> players)
        {
            DataTable table = new DataTable();
            table.Columns.Add("StatId", typeof(int));
            table.Columns.Add("NickName", typeof(string));
            table.Columns.Add("ProfileImage", typeof(string));
            table.Columns.Add("Score", typeof(int));

            foreach (PlayerAddRequest player in players)
            {
                DataRow row = table.NewRow();
                row["StatId"] = player.StatId;
                row["NickName"] = player.NickName;
                row["ProfileImage"] = player.ProfileImage;
                row["Score"] = player.Score;
                table.Rows.Add(row);
            }

            return table;
        }


        private static void AddCommonParams(PlayerAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@StatId", model.StatId);
            col.AddWithValue("@NickName", model.NickName);
            col.AddWithValue("@ProfileImage", model.ProfileImage);
            col.AddWithValue("@Score", model.Score);
        }
        private static Player MapSinglePlayer(IDataReader reader, ref int startingIndex)
        {
            Player player = new Player();
            player.Id = reader.GetSafeInt32(startingIndex++);
            player.NickName = reader.GetSafeString(startingIndex++);
            player.StatId = reader.GetSafeInt32(startingIndex++);
            player.StatName = reader.GetSafeString(startingIndex++);
            player.ProfileImage = reader.GetSafeString(startingIndex++);
            player.DateCreated = reader.GetSafeDateTime(startingIndex++);
            player.Score = reader.GetSafeInt32(startingIndex++);

            return player;
        }
    }
}
