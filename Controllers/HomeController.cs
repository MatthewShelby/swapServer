using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json.Nodes;

namespace LenzoGlobalAPI.Controllers
{
    [Route("")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        string _key = "hard*key--;";

        public HomeController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            _key = _configuration.GetValue<string>("AccessKey").ToString();

        }


        #region SWAP
        [HttpGet("health")]
        public ObjectResult health()
        {
            return new ObjectResult(new { status = "success" });
        }


        [HttpGet("fee")]
        public ObjectResult fee()
        {
            return new ObjectResult(new
            {
                status = "success",
                data = new
                {
                    isFeeTaken = _configuration.GetValue<bool>("addFee"),
                    feeRecipientAddress = _configuration.GetValue<string>("feeAddress"),
                    feePercentage = _configuration.GetValue<string>("feePercentage")
                }
            });
        }



        [HttpGet("pairPrice/{chain}/{payToken}/{receiveToken}/{value}/{slippage}")]
        public async Task<ObjectResult> pairPrice(string chain, string payToken, string receiveToken, string value, string slippage)
        {
            var url = "https://bsc.api.0x.org/swap/v1/quote?buyToken=" + receiveToken + "&sellToken=" + payToken + "&sellAmount=" + value + "&slippagePercentage=" + slippage;
            if (_configuration.GetValue<bool>("addFee"))
            {
                url += "&feeRecipient=" + _configuration.GetValue<string>("feeAddress") + "&buyTokenPercentageFee=" + _configuration.GetValue<string>("feePercentage");
            }

            try
            {
                string APIKey = _configuration.GetValue<string>("0xAPIKey").ToString();
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    request.Headers.Add("0x-api-key", APIKey);
                    client.BaseAddress = new Uri(url);
                    var response = await client.SendAsync(request);
                    var data = response.Content.ReadAsStringAsync().Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return new ObjectResult(new { status = "success", data = JsonValue.Parse(data) });
                    }
                    else
                    {
                        return new ObjectResult(new { status = "error", data = JsonValue.Parse(data) });
                    }
                }
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { status = "error", data = ex, message = ex.Message });
            }

        }

        #endregion





        #region InfoRecord
        [EnableCors("_myAllowSpecificOrigins")]
        [HttpPost("record/{title}/{cat}")]
        public async Task<ObjectResult> record(string title, string cat, [FromBody] object data)
        {
            try
            {
                this.Request.Headers.TryGetValue("Host", out var origin);
                string newId = Guid.NewGuid().ToString();
                Record newRecord = new Record()
                {
                    Id = newId,
                    title = title,
                    category = cat,
                    Create = DateTime.Now,
                    Update = DateTime.Now,
                    ExtraData = "SWAP",
                    IsDelete = false,
                    originAddress = origin.ToString(),
                    ReferrerPageAddress = origin.ToString(),
                    IdToken = "SWAP",
                    ReferrerWalletAddress = "",
                    walletAddress = "",
                    details = data.ToString(),
                };
                await _context.Records.AddAsync(newRecord);
                await _context.SaveChangesAsync();
                return new ObjectResult(new { status = "success", id = newId });
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { status = "error", message = ex.Message });
            }
        }


        [HttpGet("allrecord")]
        public async Task<ObjectResult> allREcords()
        {
            try
            {
                List<Record> data = await _context.Records.OrderByDescending(d => d.Create).ToListAsync();

                List<recordDTO> recordList = new List<recordDTO>();
                foreach (var item in data)
                {
                    recordDTO rd = new recordDTO()
                    {
                        details = JsonValue.Parse(item.details),
                        category = item.category,
                        ExtraData = item.ExtraData,
                        _id = item.Id,
                        originAddress = item.originAddress,
                        recordTime = item.Create,
                        title = item.title,
                        walletAddress = item.walletAddress
                    };
                    recordList.Add(rd);
                }
                var records = recordList.ToArray();
                return new ObjectResult(new { status = "success", data = records });

            }
            catch (Exception ex)
            {
                return new ObjectResult(new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }




        [HttpGet("delRecord/{id}")]
        public async Task<ObjectResult> deleteRecord(string id)
        {
            try
            {
                Record rec = await _context.Records.FirstAsync(r => r.Id == id);
                if (rec != null)
                {
                    _context.Records.Remove(rec);
                    await _context.SaveChangesAsync();
                    return new ObjectResult(new { status = "success" });

                }
                return new ObjectResult(new { status = "error", message = "Record not found." });

            }
            catch (Exception ex)
            {
                return new ObjectResult(new { status = "error", message = ex.Message });

            }
        }

        #endregion
    }
}
