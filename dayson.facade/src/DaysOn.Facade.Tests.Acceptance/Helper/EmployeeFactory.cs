using System;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;

namespace DaysOn.Facade.Tests.Acceptance.Helper
{
    public static class EmployeeFactory
    {
        public static string CreateEmployee(string companyName, TokenResponse token, out Guid userId, string role = null)
        {
            var firstname = NameHelper.GenerateEmployeeFirstName();
            var lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = NameHelper.CreateEmail($"{firstname} {lastname}", companyName);
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();

            Random random = new Random();

            AddEmployeeCommand cmd = new AddEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = firstname,
                Lastname = lastname,
                JobTitle = jobtitle,
                PhoneNumber = phone,
                PictureUrl = pictureUrl,
                Role = role,
                DateOfBirth = new DateTime(random.Next(1970, 1996), random.Next(1, 11), random.Next(1, 28), 0, 0, 0),
                OthersJson = $"{{'passport':'{GeneratePassport()}'}}"
            };

            userId = CompanyClientWrapper.CreateEmployeeAsync(cmd, token.access_token).Result;
            return employeeUserEmail;
        }

        public static async Task<Tuple<string,Guid>> NewCreateEmployeeAsync(string companyName, TokenResponse token, string role = null)
        {
            var firstname = NameHelper.GenerateEmployeeFirstName();
            var lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = NameHelper.CreateEmail($"{firstname} {lastname}", companyName);
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();

            Random random = new Random();

            AddEmployeeCommand cmd = new AddEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = firstname,
                Lastname = lastname,
                JobTitle = jobtitle,
                PhoneNumber = phone,
                PictureUrl = pictureUrl,
                Role = role,
                DateOfBirth = new DateTime(random.Next(1970, 1996), random.Next(1, 11), random.Next(1, 28), 0, 0, 0),
                OthersJson = $"{{'passport':'{GeneratePassport()}'}}"
            };

            Guid userId = await CompanyClientWrapper.CreateEmployeeAsync(cmd, token.access_token);
            return new Tuple<string, Guid>(employeeUserEmail,userId);
        }

        private static string GeneratePassport()
        {
            return Guid.NewGuid().ToString().Replace("-", "").Substring(7);
        }

        public static async Task<Guid> CreateEmployeeAsync(string companyName, TokenResponse token, string role = null, string email = null)
        {
            var firstname = NameHelper.GenerateEmployeeFirstName();
            var lastname = NameHelper.GenerateEmployeeLastname();
            var employeeUserEmail = email ?? NameHelper.CreateEmail($"{firstname}.{lastname}", companyName);
            var jobtitle = NameHelper.GenerateJobTitle();
            var phone = NameHelper.GeneratePhoneNumber();
            var pictureUrl = NameHelper.GeneratePictureUrl();

            var random = new Random();
            AddEmployeeCommand cmd = new AddEmployeeCommand
            {
                CorrelationId = Guid.NewGuid(),
                EmailAddress = employeeUserEmail,
                Firstname = firstname,
                Lastname = lastname,
                JobTitle = jobtitle,
                PhoneNumber = phone,
                PictureUrl = pictureUrl,
                Role = role,
                DateOfBirth = new DateTime(random.Next(1970, 1996), random.Next(1, 11), random.Next(1, 28), 0, 0, 0),
                OthersJson = $"{{'passport':'{GeneratePassport()}'}}"
            };

            var userId = await CompanyClientWrapper.CreateEmployeeAsync(cmd, token.access_token);
            return userId;
        }
    }
}
