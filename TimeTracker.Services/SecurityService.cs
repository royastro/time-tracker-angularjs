using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using AutoMapper;
using TimeTracker.DataAccess;
using TimeTracker.DataTransferObjects;
using TimeTracker.Model;
using WebMatrix.WebData;

namespace TimeTracker.Services
{
    public class SecurityService : ISecurityService
    {
        private IUnitOfWork unitOfWork;
        private EntityMapper mapper = new EntityMapper();

        public SecurityService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            mapper.SetupEntityToDataTransferObjectMappings();
            mapper.SetupDataTranferObjectMappingsToEntity();
        }

        public SecurityService()
        {   
        }

        public dtoUserProfile GetUser(string username)
        {
            var userProfile = unitOfWork.GetUserProfileRepository().Get(u => u.UserName == username).SingleOrDefault();
            var result = Mapper.Map<UserProfile, dtoUserProfile>(userProfile);
            return result;
        }

        public dtoUserProfile GetCurrentUser()
        {
            return GetUser(CurrentUserName);
        }

        public void CreateUser(dtoUserProfile user)
        {
            dtoUserProfile dbUser = GetUser(user.UserName);
            if (dbUser != null)
                throw new Exception("User with that username already exists.");

            var newUser = Mapper.Map<dtoUserProfile, UserProfile>(user);

            unitOfWork.GetUserProfileRepository().Insert(newUser);
            unitOfWork.Save();
        }

        public static void Register()
        {
            if(!WebMatrix.WebData.WebSecurity.Initialized)
                WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection("TimeTracker", "UserProfile", "User_Id", "UserName", autoCreateTables: true);
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebMatrix.WebData.WebSecurity.Login(userName, password, persistCookie);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return WebMatrix.WebData.WebSecurity.ChangePassword(userName, oldPassword, newPassword);
        }

        public bool ConfirmAccount(string accountConfirmationToken)
        {
            return WebMatrix.WebData.WebSecurity.ConfirmAccount(accountConfirmationToken);
        }

        public void CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            WebMatrix.WebData.WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        public string CreateUserAndAccount(string userName, string password, string email, bool requireConfirmationToken = false)
        {
            return WebMatrix.WebData.WebSecurity.CreateUserAndAccount(userName, password, new { Email = email }, requireConfirmationToken);
        }

        public int GetUserId(string userName)
        {
            return WebMatrix.WebData.WebSecurity.GetUserId(userName);
        }

        public void Logout()
        {
            WebMatrix.WebData.WebSecurity.Logout();
        }

        public bool IsAuthenticated
        {
            get
            {
                return WebMatrix.WebData.WebSecurity.IsAuthenticated;
            }
        }

        public string CurrentUserName
        {
            get
            {
                return WebMatrix.WebData.WebSecurity.CurrentUserName;
            }
        }
    }
}
