using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Dtos.User
{
    public class UserGridDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? DeletedUserId { get; set; }
        public bool IsCheck { get; set; }
        public List<int> LstGroupIds { get; set; }
        public UserGridDto()
        {
            LstGroupIds = new List<int>();
        }
    }
}
