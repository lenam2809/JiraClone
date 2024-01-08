using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Dtos.User
{
    public class UserCreateDto
    {
        public int Id { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public string FullName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; set; }
        public List<int> GroupIds { get; init; }
        public DateTime CreatedDate { get; init; }
        public DateTime ModifiedDate { get; init; }
        public int Status { get; init; }
        public string Address { get; init; }
        public string Avatar { get; init; }
        public int Sex { get; init; }
        public int? DonViNoiBoId { get; init; }
    }

    public class UserAddDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
        public int UnitId { get; set; }
    }
}
