using TwenyTwoRT.EfStuff.Model;
using TwenyTwoRT.EfStuff.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace TwenyTwoRT.Models
{
    public class UserService
    {
        private IHttpContextAccessor _contextAccessor;
        private IUserRepository _userRepository;
        public UserService(IUserRepository userRepository,
            IHttpContextAccessor contextAccessor)
        {
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }
        public User GetCurrent()
        {
            var idStr = _contextAccessor.HttpContext.User
                ?.Claims.SingleOrDefault(x => x.Type == "Id")?.Value;
            if (string.IsNullOrEmpty(idStr))
            {
                return null;
            }

            var id = long.Parse(idStr);
            return _userRepository.Get(id);
        }
        public bool IsAdmin()
        {
            var user = GetCurrent();
            return user?.JobType == JobType.Admin;
        }
        public bool IsClient()
        {
            var user = GetCurrent();
            return user.Client != null ? true : false;
        }
        public ClaimsPrincipal GetPrincipal(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Id", user.Id.ToString()));
            claims.Add(new Claim(
                ClaimTypes.AuthenticationMethod,
                Startup.AuthMethod));
            var claimsIdentity = new ClaimsIdentity(claims, Startup.AuthMethod);
            var principal = new ClaimsPrincipal(claimsIdentity);
            return principal;
        }

    }
}
