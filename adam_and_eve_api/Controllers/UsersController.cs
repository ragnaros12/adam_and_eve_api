using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using adam_and_eve_api.models;
using MailKit;
using MimeKit;
using System.Text.Json.Serialization;
using System.Text.Json;
using NuGet.Common;
using System.ComponentModel;
using Token = adam_and_eve_api.models.Token;

namespace adam_and_eve_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string alph = "abcdefghijknopqrstuvwxyzABCDEFGHIJKNOPQRSTUVWXYZ";
        private readonly Database _context;


        public class LoginForm
        {
            public string login { get; set; }
            public string password { get; set; }
        }

        public UsersController(Database context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(LoginForm loginform)
        {
            if(string.IsNullOrWhiteSpace(loginform.login) || string.IsNullOrWhiteSpace(loginform.password)
                || loginform.login.Length < 4 || loginform.password.Length < 4)
            {
                return BadRequest("data not valid");
            }
            else
            {
                var user = new User(loginform.login, loginform.password);
				await _context.users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }

        [HttpGet("users")]
        public IActionResult getUsers()
        {
            return Ok(_context.users.ToList());
        }


        [HttpPost("login")]
        public async Task<IActionResult> login(LoginForm loginForm)
        {
            var users = _context.users.Where(u => u.login == loginForm.login && u.password == loginForm.password);
            if(users.Count() != 0)
            {
                var user = users.First();
                user.token = string.Join("", Enumerable.Range(0, 20).Select(u => alph[Random.Shared.Next(0, alph.Length)]));

                await _context.SaveChangesAsync();
                return Ok(new Token(user.token));

            }
            return Unauthorized();
        }


        [HttpGet("info")]
        public async Task<IActionResult> info(string token)
        {
			var users = _context.users.Where(u => u.token == token);
            if(users.Count() != 0)
            {
                var user = users.First();
                return Ok(user);
            }
            return Unauthorized();
		}

        [HttpPost("next")]
        public async Task<IActionResult> next(string token, bool? isLike = null)
        {
			var users = _context.users.Where(u => u.token == token);
			if (users.Count() != 0)
			{
				var user = users.First();
                if(user.usersRecords.Count == 0)
                {
                    user.usersRecords = new List<UserRecord>();
                    var us = findUser(user, _context.users.ToList());
                    if(us == null)
                    {
                        return BadRequest("user not found");
                    }
                    user.usersRecords.Add(new() { user = us, value = false});
                    await _context.SaveChangesAsync();
                    return Ok(us);
                }
                else
                {
                    if (isLike != null)
                    {
                        user.usersRecords.Last().value = (bool)isLike;
                        await _context.SaveChangesAsync();
                        var us = findUser(user, _context.users.ToList());
                        if (us == null)
                        {
                            return BadRequest("user not found");
                        }
                        user.usersRecords.Add(new() { user = us, value = false });
                        await _context.SaveChangesAsync();
                        return Ok(us);
                    }
                    else
                    {
                        return Ok(user.usersRecords.Last().user);
                    }
				}
			}
			else
			{
				return Unauthorized();
			}
		}

        private User? findUser(User user, List<User> users) 
        {
			foreach (User userN in users)
			{
				if (user.usersRecords.Where(u => u.user.Id == userN.Id).Count() == 0 && user.Id != userN.Id)
				{
					if (user.gender == userN.interestedIn && user.interestedIn == userN.gender && user.purpose
						== userN.purpose)
					{
                        return userN;
					}
				}
			}
            return null;
		}

        [HttpPost]
        public IActionResult isVerify(string token)
        {
			var users = _context.users.Where(u => u.token == token);
			if (users.Count() != 0)
			{
				var user = users.First();
                return user.isVerified ? Ok() : BadRequest();
			}
			else
			{
				return Unauthorized();
			}
		}
        [HttpPost]
        public IActionResult sendVerify(string token)
        {
			var users = _context.users.Where(u => u.token == token);
			if (users.Count() != 0)
			{
                var user = users.First();
                return Ok();
			}
			else
			{
				return Unauthorized();
			}
		}
		[HttpPost]
		public IActionResult Verify(string token, string data)
		{
			var users = _context.users.Where(u => u.token == token);
			if (users.Count() != 0)
			{
				var user = users.First();
				if(data == "1234")
                {
                    user.isVerified = true;
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
			}
			else
			{
				return Unauthorized();
			}
		}

		[HttpPost("records")]
        public async Task<IActionResult> rec(string token)
        {
			return Ok(_context.users.Where(u => u.token == token).First().usersRecords);

		}


	}
}
