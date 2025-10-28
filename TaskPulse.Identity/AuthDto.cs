using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskPulse.Identity
{
    public record RegisterRequest(string Email, string Password);
    public record LoginRequest(string email, string Password);
    //ответ с сгенерированым токеном
    public record AuthResponse(string Token);
}
