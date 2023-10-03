﻿using System.Collections.Generic;

namespace Luveck.Service.Security.DTO.Response
{
    public class UserListResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool state { get; set; }
        public string role { get; set; }
        public string RoleId { get; set; }
        public IList<string> Roles { get; set; }
    }
}
