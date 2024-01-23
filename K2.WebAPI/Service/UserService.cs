using K2.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace K2.WebAPI.Service
{
    public interface IUserService
    {
        Task<Authorize> Authenticate(string username, string password);
        Task<IEnumerable<Authorize>> GetAll();
    }
    public class UserService : IUserService
    {
        private List<Authorize> _users = new List<Authorize>
        {
            new Authorize { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };
            
        public async Task<Authorize> Authenticate(string username, string password)
        {
            // wrapped in "await Task.Run" to mimic fetching user from a db
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.Username == username && x.Password == password));

            // on auth fail: null is returned because user is not found
            // on auth success: user object is returned
            return user;
        }
        public async Task<IEnumerable<Authorize>> GetAll()
        {
            // wrapped in "await Task.Run" to mimic fetching users from a db
            return await Task.Run(() => _users);
        }
    }
}