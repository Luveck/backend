using System;

namespace Luveck.Service.Security.DTO.Response
{
    public class LoginResponseDto
    {
        public string name { get; set; }
        public string lastName { get; set; }
        public string token { get; set; }
        public int intentos { get; set; }
        public DateTime HoraDesbloqueo { get; set; }
        public bool changePass { get; set; }
        public string role { get; set; }
    }
}
