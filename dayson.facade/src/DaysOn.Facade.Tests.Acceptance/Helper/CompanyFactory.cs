using System;
using System.Threading;
using System.Threading.Tasks;
using DaysOn.CompanyBc.Models.Commands;
using DaysOn.Facade.Common;

namespace DaysOn.Facade.Tests.Acceptance.Helper
{
    public static class CompanyFactory
    {
        public static string CreateCompany(out TokenResponse tokenAdmin)
        {
            var companyName = NameHelper.GenerateCompanyName();
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                FirstName = ownerName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                OwnerId = Guid.NewGuid()
            };
            Guid companyId = CompanyClientWrapper.CreateCompanyAsync(command).Result;

            Thread.Sleep(100);
            tokenAdmin = AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString()).Result;

            if (tokenAdmin == null)
            {
                Thread.Sleep(50);
                tokenAdmin = AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString()).Result;
            }
            return companyName;
        }

        public static async Task<TokenResponse> CreateCompanyAsync(string companyName)
        {
            var ownerName = NameHelper.GenerateEmployeeName();
            var middleName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                FirstName = ownerName,
                MiddleName = middleName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                OwnerId = Guid.NewGuid()
            };

            await CompanyClientWrapper.CreateCompanyAsync(command);

            Thread.Sleep(100);
            var tokenAdmin = await AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString());

            if (tokenAdmin == null)
            {
                Thread.Sleep(50);
                tokenAdmin = await AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString());
            }
            return tokenAdmin;
        }

        public static async Task<TokenResponse> CreateCompanyAsync(string companyName, Guid companyId)
        {
            var ownerName = NameHelper.GenerateEmployeeName();
            var employeeEmail = NameHelper.CreateEmail(ownerName, companyName);
            CreateCompanyCommand command = new CreateCompanyCommand()
            {
                CompanyName = companyName,
                AdminEmailAddress = employeeEmail,
                FirstName = ownerName,
                LastName = "Bolson",
                PhoneNumber = "REPLACE_WITH_YOUR_VALUE",
                OwnerId = Guid.NewGuid(),
                CompanyId = companyId
            };
            await CompanyClientWrapper.CreateCompanyAsync(command);

            Thread.Sleep(100);
            var tokenAdmin = await AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString());

            if (tokenAdmin == null)
            {
                Thread.Sleep(50);
                tokenAdmin = await AuthClientWrapper.GetTokenAsync(employeeEmail, command.OwnerId.GetHashCode().ToString());
            }
            return tokenAdmin;
        }
    }
}
