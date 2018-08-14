using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lagersoft.OAuth;
using OAUtils = Lagersoft.OAuth.Utils;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
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
			var url = OAUtils.GetLoginUrl(Request.Scheme + System.Uri.SchemeDelimiter + Request.Host + returnUrl);
			return Json(url);
		}
		[Route("Logoff")]
		public IActionResult Logoff(string token)
		{
			OAUtils.Logoff(token);
			return Json("ok");
		}
		[Route("OAuth")]
		public IActionResult OAuth(string code)
		{
			var token = OAUtils.GetToken(code);
			if (token != null)
				OAUtils.Authenticate(HttpContext, token);
			return Json(token);
		}

	}
}