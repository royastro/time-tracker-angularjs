using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracker.DataTransferObjects;
using TimeTracker.Model;

namespace TimeTracker.Services
{
    public interface ISecurityService
    {
        dtoUserProfile GetUser(string username);
        dtoUserProfile GetCurrentUser();
        void CreateUser(dtoUserProfile user);
        bool Login(string userName, string password, bool persistCookie = false);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        bool ConfirmAccount(string accountConfirmationToken);
        void CreateAccount(string userName, string password, bool requireConfirmationToken = false);
        string CreateUserAndAccount(string userName, string password, string email, bool requireConfirmationToken = false);
        int GetUserId(string userName);
        void Logout();
        bool IsAuthenticated { get; }
        string CurrentUserName { get; }
    }
}
