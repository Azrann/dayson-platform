using System;
using System.Linq;
using System.Threading.Tasks;
using DaysOn.Facade.Common.Database;

namespace DaysOn.Facade.Common.Dtos.WebUser
{
    public class SearchUsersOffline
    {
        public static async Task<SearchUsersResponse> SearchUsers(SearchUsersRequest request)
        {
            return await Task.FromResult(
                new SearchUsersResponse
                {
                    Users = OfflineDb.Users.Where(u =>
                        (request.TeamIds != null && request.TeamIds.Count > 0 ? request.TeamIds.Contains(u.TeamId) : true) &&
                        (
                            String.IsNullOrEmpty(request.Text) ?
                                true :
                                (
                                    u.FirstName.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                    u.TeamName.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != 1 ||
                                    u.MiddleName.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                    u.LastName.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                    u.Email.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                    u.Phone.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                    u.JobTitle.IndexOf(request.Text, StringComparison.CurrentCultureIgnoreCase) != -1
                                )
                        )
                    ).ToList()
                }
            );
        }
    }
}
