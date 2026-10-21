using System;
using System.Collections.Generic;

namespace DaysOn.CompanyBc.Models.Dtos
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string JobTitle { get; set; }
        public string Role { get; set; }
        public string PictureUrl { get; set; }
        public string OthersJson { get; set; }

        public IdNameDto Company { get; set; }
        
        public IdNameDto MyTeam { get; set; }

        public virtual List<IdNameDto> ManagerOf { get; set; }
        public virtual List<IdNameDto> TemporalManagerOf { get; set; }
    }
}
