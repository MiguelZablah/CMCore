﻿using Microsoft.AspNetCore.Cors;
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
		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			var url = OAUtils.GetLoginUrl(HttpUtility.UrlDecode(returnUrl));
			return Ok(url);
		}

		[HttpGet]
		public IActionResult Logoff(string token)
		{
			OAUtils.Logoff(token);
			return Ok("ok");
		}

		[HttpGet]
		public IActionResult GetToken(string code)
		{
			var token = OAUtils.GetToken(code);
			return Ok(token);
		}

		[HttpGet("Unauthorized")]
		public IActionResult Unauthorize()
		{
			return Unauthorized();
		}
	}
}