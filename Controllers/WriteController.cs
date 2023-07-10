using LenzoGlobalAPI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LenzoGlobalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InfoController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: <WriteController>
        [HttpGet]
        public string Get()
        {
            return "Test is OK";
        }

        // GET <WriteController>/5
        [HttpGet("{id}")]
        public JsonResult Get(string id)
        {

            Data? val = _context.Datas.SingleOrDefault(d => d.Key == id);
            if (val == null)
            {
                return new JsonResult(new { status = "error", errorMessage = "wrong key" });
            }
            //return new JsonResult(new { status = "success", data = JsonConvert.SerializeObject(val.Value) });
            return new JsonResult(new { status = "success", data = val.Value });
        }

        // POST <WriteController>
        [HttpPost]
        public JsonResult Post([FromBody] DataDTO model)
        {
            if (ModelState.IsValid)
            {
                string existingKey = "";
                Data? eData = _context.Datas.SingleOrDefault(d => d.Key == model.Key);
                if (eData != null)
                {
                    existingKey = eData.Key;
                    eData.Key = existingKey + "expired";
                    eData.Update = DateTime.Now;
                    _context.Datas.Update(eData);
                }
                try
                {
                    Data newData = new Data()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Create = DateTime.Now,
                        Update = DateTime.Now,
                        IsDelete = false,
                        IdToken = model.IdToken,
                        ExtraData = model.ExtraData,
                        ReferrerPageAddress = model.ReferrerPageAddress,
                        ReferrerWalletAddress = model.ReferrerWalletAddress,
                        Key = model.Key,
                        Value = model.Value
                    };

                    _context.Datas.Add(newData);
                    _context.SaveChanges();
                    return new JsonResult(new { status = "success" });

                }
                catch (Exception ex)
                {
                    return new JsonResult(new { status = "error", errorMessage = ex.Message });

                }
            }
            return new JsonResult(new { status = "error", errorMessage = "ModelStateNotValid" });
        }


        // PUT <WriteController>
        [HttpPut]
        public JsonResult Put([FromBody] DataDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Data? eData = _context.Datas.SingleOrDefault(d => d.Key == model.Key);
                    if (eData != null)
                    {
                        eData.IdToken = model.IdToken;
                        eData.ExtraData = model.ExtraData;
                        eData.ReferrerPageAddress = model.ReferrerPageAddress;
                        eData.ReferrerWalletAddress = model.ReferrerWalletAddress;
                        eData.Value = model.Value;
                        eData.Update = DateTime.Now;
                        _context.Datas.Update(eData);
                        _context.SaveChanges();
                        return new JsonResult(new { status = "success" });
                    }
                    else
                    {
                        return new JsonResult(new { status = "error", errorMessage = "Couldn't find a model with the given key." });
                    }


                }
                catch (Exception ex)
                {
                    return new JsonResult(new { status = "error", errorMessage = ex.Message });
                }
            }
            return new JsonResult(new { status = "error", errorMessage = "ModelStateNotValid" });
        }
    }
}
