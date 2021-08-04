using TwenyTwoRT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwenyTwoRT.Controllers.CustomAttribute
{
    public class IsAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            var _userService = (UserService)context
                .HttpContext
                .RequestServices
                .GetService(typeof(UserService));

            if (!_userService.IsAdmin())
            {
                //результат-ответ о том что у тебя нет доступа
                context.Result = new ForbidResult();
            }
            base.OnActionExecuting(context);
        }
    }
}
