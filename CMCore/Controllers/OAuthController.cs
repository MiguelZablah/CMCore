using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OAUtils = Lagersoft.OAuth.Utils;
using System.Web;

namespace CMCore.Controllers
{
	[Produces("application/json")]
	[Route("OAuth/")]
	[EnableCors("AllowSpecificOrigin")]
	public class OAuthController : Controller
	{
		[HttpGet("Login")]
		public IActionResult Login(string returnUrl)
		{
			var url = OAUtils.GetLoginUrl(HttpUtility.UrlDecode(returnUrl));
			return Ok(url);
		}

		[HttpGet("Logoff")]
		public IActionResult Logoff(string token)
		{
			OAUtils.Logoff(token);
			return Ok("ok");
		}

		[HttpGet("GetToken")]
		public IActionResult GetToken(string code)
		{
			var token = OAUtils.GetToken(code);
			return Ok(token);
		}

		[HttpGet("Unauthorized")]
		// ReSharper disable once IdentifierTypo
		public IActionResult Unauthorize()
		{
			return Unauthorized();
		}
	}
}