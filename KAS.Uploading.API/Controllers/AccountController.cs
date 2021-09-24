using IdentityModel;
using KAS.Uploading.BusinessFunctions.IServices;
using KAS.Uploading.Models.Entities;
using KAS.Uploading.Models.ViewModels;
using KAS.Server.Share;
using KAS.Server.Share.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace KAS.Uploading.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller//APIGenericController
    {
        private UserManager<ApplicationUser> _userManager { get; }
        private readonly IEFCommonService<ApplicationUser, Guid> _accountService;

        public AccountController(UserManager<ApplicationUser> userManager/*, IEFCommonService<ApplicationUser, Guid> accountService*/)
        {
            _userManager = userManager;
            //_accountService = accountService;
        }

        [HttpGet("registerFake")]
        public async Task<IActionResult> RegisterWithFakeData()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync("https://randomuser.me/api/?results=100&nat=gb,us&inc=gender,name,email,picture");

                var results = JsonConvert.DeserializeObject<JObject>(response).Value<JArray>("results");

                string UpFirst(string input)
                {
                    return char.ToUpper(input[0]) + input.Substring(1);
                }

                foreach (var randUser in results)
                {
                    var gender = UpFirst(randUser.Value<string>("gender"));
                    var first = UpFirst(randUser.SelectToken("name.first").Value<string>());
                    var last = UpFirst(randUser.SelectToken("name.last").Value<string>());
                    var email = randUser.Value<string>("email");
                    var picture = randUser.SelectToken("picture.large").Value<string>();

                    var model = new RegisterViewModel()
                    {
                        Email = email,
                        Firstname = first,
                        Lastname = last,
                        Gender = gender,
                        Password = "abc!123"
                    };

                    await Register(model);
                }
            }

            return this.Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                    return this.ErrorResult(ErrorCode.REGISTER_REQUIRED_EMAIL);

                if (string.IsNullOrEmpty(model.Firstname))
                    return this.ErrorResult(ErrorCode.REGISTER_REQUIRED_FIRST_NAME);

                if (string.IsNullOrEmpty(model.Lastname))
                    return this.ErrorResult(ErrorCode.REGISTER_REQUIRED_LAST_NAME);

                var user = new ApplicationUser() { Email = model.Email, UserName = model.Email };

                user.Claims.Add(new IdentityUserClaim<Guid>()
                {
                    ClaimType = JwtClaimTypes.GivenName,
                    ClaimValue = model.Firstname
                });
                user.Claims.Add(new IdentityUserClaim<Guid>()
                {
                    ClaimType = JwtClaimTypes.FamilyName,
                    ClaimValue = model.Lastname
                });
                user.Claims.Add(new IdentityUserClaim<Guid>()
                {
                    ClaimType = JwtClaimTypes.Gender,
                    ClaimValue = model.Gender.ToString()
                });
                user.Claims.Add(new IdentityUserClaim<Guid>()
                {
                    ClaimType = JwtClaimTypes.BirthDate,
                    ClaimValue = model.BirthDate.ToString("yyyy-MM-dd")
                });

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //await HttpContext.RequestServices.GetRequiredService<UserRepository>()
                    //    .CreateAsync(user.Id, model.Firstname + " " + model.Lastname, model.Gender, model.Image);
                    return this.OkResult();
                }
                else
                {
                    if (result.Errors.Any(x => x.Code == "DuplicateUserName"))
                    {
                        return this.ErrorResult(ErrorCode.REGISTER_DUPLICATE_USER_NAME);
                    }
                    return this.ErrorResult(ErrorCode.BAD_REQUEST);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("error account = " + e);
                return new OkObjectResult(null);
            }
        }
    }
}