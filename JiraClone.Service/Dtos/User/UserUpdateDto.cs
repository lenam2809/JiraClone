using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JiraClone.Service.Dtos.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<int> GroupIds { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string NewPassword { get; set; }
        public int Sex { get; set; }
        public int? DonViNoiBoId { get; set; }

    }
    public class UserUpdateInfoDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserUpdateInfoDto()
        {
        }
    }
    public class UserChangePassword
    {
        public int ID { get; set; }
        public string MATKHAUCU { get; set; }
        public string MATKHAUMOI { get; set; }
    }
    public class UserUpdateStatus
    {
        public int Id { get; set; }
        public int Status { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
