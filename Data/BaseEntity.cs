using Microsoft.EntityFrameworkCore;
using System.Text.Json.Nodes;

namespace LenzoGlobalAPI
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime Create { get; set; }
        public DateTime Update { get; set; }
        public bool IsDelete { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string ReferrerPageAddress { get; set; }
        public string IdToken { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string? ExtraData { get; set; }
        public string? ReferrerWalletAddress { get; set; }

    }

    public class Data : BaseEntity
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Key { get; set; }
        public string Value { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    }
    public class Ticket : BaseEntity
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string Message { get; set; }
        public bool Readed { get; set; }
        public bool Answered { get; set; }
        public string? AnswerId { get; set; }
    }

    public class DataDTO
    {

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Key { get; set; }
        public string Value { get; set; }
        public string ReferrerPageAddress { get; set; }
        public string IdToken { get; set; }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public string? ExtraData { get; set; }
        public string? ReferrerWalletAddress { get; set; }


    }

    public class Record : BaseEntity {
        public string title { get; set; }
        public string category { get; set; }
        public string? walletAddress { get; set; }
        public string? originAddress { get; set; }
        public string details { get; set; }
        public string? ExtraData { get; set; }

    }
    public class recordDTO{
        public string? _id { get; set; }
        public string? title { get; set; }
        public string? category { get; set; }
        public string? walletAddress { get; set; }
        public string? originAddress { get; set; }
        public DateTime? recordTime { get; set; }
        public object? details { get; set; }
        public string? ExtraData { get; set; }

    }

    public class TicketDTO 
    {
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public bool Readed { get; set; }
        public bool Answered { get; set; }
        public string? AnswerId { get; set; }
        public string ReferrerPageAddress { get; set; }
        public string IdToken { get; set; }
        public string? ExtraData { get; set; }
        public string? ReferrerWalletAddress { get; set; }

    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Data> Datas { get; set; } = null!;
        public DbSet<Ticket> Tickets { get; set; } = null!;
        public DbSet<Record> Records { get; set; } = null!;


    }
}
