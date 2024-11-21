using Microsoft.EntityFrameworkCore;
using IIMSv1.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace IIMSv1.Data
{
    [PrimaryKey(nameof(Id))]
    public class Log
    {
        public string Id { get; set; }

        public DateTime DateTime { get; set; }

        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public AccountUser User { get; set; }

        public string Activity { get; set; }

        public string TableName { get; set; }

        public string KeyValues { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }
    }
}
