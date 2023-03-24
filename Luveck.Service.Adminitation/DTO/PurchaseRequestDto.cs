using Luveck.Service.Administration.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Luveck.Service.Administration.DTO
{
    public class PurchaseRequestDto
    {
        public int Id { get; set; }
        public int pharmacyId { get; set; }
        public string userId { get; set; }
        public string NoPurchase { get; set; }
        public bool Reviewed { get; set; }
        public DateTime DateShiped { get; set; }
    }
}
