using FoodOrder.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrder.Domain.Users
{
    public class RefreshToken : BaseEntity<Guid>
    {
        public RefreshToken()
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            IsUsed = false;
            IsRevorked = false;
        }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevorked { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public Guid? UserId { get; set; }
        #region Relations
        public User User { get; set; }
        #endregion

    }
}
