using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LenzoGlobalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        string _key = "hard*key--;";

        public TicketController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            try
            {
                _key = _configuration.GetValue<string>("AccessKey").ToString();
            }
            catch (Exception)
            {

            }
        }
        // GET: <TicketController>
        [HttpGet]
        public string Get()
        {
            return "Test is OK on Ticket. local connection: "+ _configuration.GetValue<string>("LocalConnection").ToString(); 
        }
        [HttpPost]
        public string PostTicket([FromBody] TicketDTO model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Ticket ticket = new()
                    {
                        Create = DateTime.Now,
                        ExtraData = model.ExtraData,
                        Id = Guid.NewGuid().ToString(),
                        IdToken = model.IdToken,
                        IsDelete = false,
                        ReferrerPageAddress = model.ReferrerPageAddress,
                        ReferrerWalletAddress= model.ReferrerWalletAddress,
                        Update = DateTime.Now,
                        Answered = false,
                        AnswerId = "",
                        Email = model.Email,
                        Message = model.Message,
                        PhoneNumber = model.PhoneNumber,
                        Readed = false
                    };

                    _context.Tickets.Add(ticket);
                    _context.SaveChanges();
                    return "done";
                }
                catch (Exception ex)
                {

                    return "Exception Message: " + ex.Message;
                }

            }
            return "ModelStateNotValid";
        }



        [HttpGet("ticket/{key}")]
        public IEnumerable<Ticket>? Gettickets(string key)
        {
            if (key == _key)
                return _context.Tickets.ToList().OrderByDescending(i => i.Create);
            else return null;
        }

        [HttpGet("ticket/{id}/{key}")]
        public Ticket? Getticket(string id, string key)
        {
            if (key == _key) return _context.Tickets.FirstOrDefault(i => i.Id == id);
            else return null;
        }

        [HttpGet("ticketT/{id}/{key}")]
        public IEnumerable<Ticket>? GetticketByToken(string id, string key)
        {
            if (key == _key) return _context.Tickets.Where(i => i.IdToken == id).OrderByDescending(i => i.Create);
            else return null;
        }

        [HttpGet("ticketR/{id}/{key}")]
        public IEnumerable<Ticket>? GetticketByReferrer(string id, string key)
        {
            if (key == _key) return _context.Tickets.Where(i => i.ReferrerPageAddress == id).OrderByDescending(i => i.Create);
            else return null;
        }

        // This is new added
        [HttpGet("ticketRW/{id}/{key}")]
        public IEnumerable<Ticket>? GetticketByReferrerW(string id, string key)
        {
            if (key == _key) return _context.Tickets.Where(i => i.ReferrerWalletAddress == id).OrderByDescending(i => i.Create);
            else return null;
        }





        [HttpPut("ticketreaded/{id}/{key}")]
        public async Task<JsonResult> TicketReaded(string id, string key)
        {
            if (key != _key)
                return new JsonResult(new { status = "error", errorMessage = "wrong key" });
            else
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                    return new JsonResult(new { status = "error", errorMessage = "id not found" });
                else
                {
                    ticket.Readed = true;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    await _context.DisposeAsync();
                    return new JsonResult(new { status = "success", id });
                }
            }
        }

        [HttpPut("ticketanswered/{id}/{key}")]
        public async Task<JsonResult> TicketAnswered(string id, string key)
        {
            if (key != _key)
                return new JsonResult(new { status = "error", errorMessage = "wrong key" });
            else
            {
                var ticket = _context.Tickets.FirstOrDefault(t => t.Id == id);
                if (ticket == null)
                    return new JsonResult(new { status = "error", errorMessage = "id not found" });
                else
                {
                    ticket.Answered = true;
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                    await _context.DisposeAsync();
                    return new JsonResult(new { status = "success", id });
                }
            }
        }
    }
}
