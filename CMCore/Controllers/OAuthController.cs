using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OAUtils = Lagersoft.OAuth.Utils;
using System.Web;

namespace CMCore.Controllers
{
	[Produces("application/json")]
	[Route("OAuth/")]
	[EnableCors("AllowSpecificOrigin")]
	public class OAuthControlller : Controller
    {
		[Route("Login")]
		public IActionResult Login(string returnUrl)
		{
			var url = OAUtils.GetLoginUrl(HttpUtility.UrlDecode(returnUrl));
			return Json(url);
		}
		[Route("Logoff")]
		public IActionResult Logoff(string token)
		{
			OAUtils.Logoff(token);
			return Json("ok");
		}
		[Route("GetToken")]
		public IActionResult GetToken(string code)
		{
			var token = OAUtils.GetToken(code);
			return Json(token);
		}

		[Route("Unauthorized")]
		public new IActionResult Unauthorized()
		{
			return Json("Unauthorized");
		}
	}
}