using OnlyMembers.Models;

namespace OnlyMembers.Services
{
    public interface IJwtService
    {
        string GenerateToken(Member member);
    }
}
