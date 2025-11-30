using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AplicationLayer.DTOs
{
    //Imutable class
    public record RegisterDto(string Email, string Password, string? FullName);
}
