using Microsoft.AspNetCore.Identity;

namespace CybergymHRS.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
