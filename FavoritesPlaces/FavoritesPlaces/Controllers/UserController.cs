using TwenyTwoRT.EfStuff;
using TwenyTwoRT.EfStuff.Model;
using TwenyTwoRT.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using AutoMapper;
using TwenyTwoRT.EfStuff.Repository.IRepository;

namespace TwenyTwoRT.Controllers
{
    public class UserController : Controller
    {
        private SpaceDbContext _dbContext;
        private UserService _userService;
        private IUserRepository _userRepository;
        private IMapper _mapper;
        public UserController(SpaceDbContext dbContext, UserService userService, IMapper mapper, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _userService = userService;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Registration()
        {
            var model = new RegistrationViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var isUserUniq = _userRepository.Get(model.Login) == null;
            if (isUserUniq) 
            {
                var user = _mapper.Map<User>(model);
                _userRepository.Save(user);


                await HttpContext.SignInAsync(
                    _userService.GetPrincipal(user));

                return RedirectToAction("Profile", "User");
            }
            return View(model);
        }
       
        public IActionResult Profile()
        {
            
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new RegistrationViewModel();
            var returnUrl = Request.Query["ReturnUrl"];
            model.ReturnUrl = returnUrl;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(RegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userRepository.Get(model.Login);
            if (user == null || user.Password != model.Password)
            {
                return View(model);
            }

            await HttpContext.SignInAsync(
                _userService.GetPrincipal(user));

            if (!string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        [HttpPost]
        public IActionResult Socials(SocialsPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if ((model.Password == GlobalConst.TELEGRAMGROUPPASS)
                && (model.Link.ToLower() == nameof(GlobalConst.TELEGRAMGROUPLINK).ToLower()))
            {
                return Redirect(GlobalConst.TELEGRAMGROUPLINK);
            }
            else if ((model.Password == GlobalConst.YOUTUBETEACHERPASS)
                && (model.Link.ToLower() == nameof(GlobalConst.YOUTUBETEACHERLINK).ToLower()))
            {
                return Redirect(GlobalConst.YOUTUBETEACHERLINK);
            }
            else
            {
                return RedirectToAction("SocialsWrongPass", "User");
            }
        }
    }
}
