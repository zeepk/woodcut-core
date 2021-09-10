﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet5_webapp.Data;
using dotnet5_webapp.Models;
using dotnet5_webapp.Services;
using Microsoft.Extensions.Configuration;
using dotnet5_webapp.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.IdentityModel.JsonWebTokens;

namespace dotnet5_webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService UserService;
        private readonly DataContext _context;

        public UsersController(IUserService userService, DataContext context)
        {
            UserService = userService;
            _context = context;
        }

        // GET: api/Users
        // returns all users and their records

        //[HttpGet]
        //public ActionResult<User> GetUser()
        //{
        //    User newUser = new User()
        //    {
        //        DateCreated = DateTime.Now,
        //        Username = "username",
        //        DisplayName = "displayname"
        //    };
        //    return Ok(newUser);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/zee+pk
        // returns specific user and their records
        [HttpGet("{username}")]
        public async Task<ActionResult<UserSearchResponse>> GetUser(string username)
        {
            var response = await UserService.SearchForPlayer(username);
            return Ok(response);
        }

        // PUT: api/Users/updateall
        // add a new record to each user
        [HttpPost("updateall")]
        public async Task<IActionResult> UpdateAllUsers()
        {
            var updatedUsers = await UserService.AddNewStatRecordForAllUsers();
            return Ok(updatedUsers);
        }
        
        // PUT: api/Users/updateall
        // add a new record to each user
        [HttpPost("updateactivities")]
        public async Task<IActionResult> UpdateActivitiesForAllUsers()
        {
            var updatedUsers = await UserService.AddNewActivitiesForAllUsers();
            return Ok(updatedUsers);
        }

        // PUT: api/Users/update/zee+pk
        // add a new record to the specified user
        [HttpPost("update/{username}")]
        public async Task<ActionResult<Player>> UpdateUser(string username)
        {
            var users = _context.User.Include(u => u.StatRecords);
            var user = await users.FirstOrDefaultAsync(user => user.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            await UserService.CreateStatRecord(user);
            await _context.SaveChangesAsync();


            return Ok(user);

        }

        // PUT: api/Users/5
        // autogenerated
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            _context.Entry(player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(player);

        }

        // POST: api/Users
        // autogenerated
        [HttpPost]
        public async Task<ActionResult<Player>> PostUser(Player player)
        {
            _context.User.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = player.Id }, player);
        }

        // DELETE: api/Users/5
        // autogenerated
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> DeleteUser(int id)
        // {
        //     var user = await _context.User.FindAsync(id);
        //     if (user == null)
        //     {
        //         return NotFound();
        //     }
        //
        //     _context.User.Remove(user);
        //     await _context.SaveChangesAsync();
        //
        //     return NoContent();
        // }
        
        [HttpGet("playercount")]
        public async Task<ActionResult<int>> GetPlayerCount()
        {
            var response = await UserService.CurrentPlayerCount();
            return Ok(response);
        }    
        
        // GET: api/Users/vos
        // get current vos (voice of seren)
        [HttpGet("vos")]
        public async Task<ActionResult<ResponseWrapper<(string, string)>>> CurrentVos()
        {
            var vos = await UserService.CurrentVos();
            return Ok(vos);
        }
        
        // GET: api/Users/gains/zee+pk
        // returns specific user and their records
        [HttpGet("gains/{username}")]
        public async Task<ActionResult<ResponseWrapper<CurrentGainForUserServiceResponse>>> GetGainsForUser(string username)
        {
            var response = await UserService.CurrentGainForUser(username);
            return Ok(response);
        }

        [HttpGet("details/{username}")]
        public async Task<ActionResult<ResponseWrapper<PlayerDetailsServiceResponse>>> GetPlayerDetails(string username)
        {
            var response = await UserService.GetPlayerDetails(username);
            return Ok(response);
        }        
        
        [HttpGet("metrics/{username}")]
        public async Task<ActionResult<ResponseWrapper<PlayerMetricsServiceResponse>>> GetPlayerMetrics(string username)
        {
            var response = await UserService.GetPlayerMetrics(username);
            return Ok(response);
        }  
        
        [HttpGet("quests/{username}")]
        public async Task<ActionResult<ResponseWrapper<PlayerQuestsServiceResponse>>> GetPlayerQuests(string username)
        {
            var response = await UserService.GetPlayerQuests(username);
            return Ok(response);
        }        
        
        [HttpGet("activities")]
        public async Task<ActionResult<ResponseWrapper<PlayerMetricsServiceResponse>>> GetAllActivities([FromQuery] int size)
        {
            var response = await UserService.GetAllActivities(size);
            return Ok(response);
        }   
        
        [HttpPut("track/{username}")]
        public async Task<ActionResult<ResponseWrapper<Boolean>>> TrackUser(String username)
        {
            var response = await UserService.TrackUser(username);
            return Ok(response);
        }
        
        // following

        [HttpGet("following")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<ICollection<String>>>> Following()
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.GetFollowedPlayerNames(applicationUser);
            return Ok(response);
        }
        
        // following

        [HttpGet("following/activities")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<ICollection<ActivityResponse>>>> FollowingActivities([FromQuery] int size)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.GetFollowedPlayerActivities(applicationUser, size);
            return Ok(response);
        }

        [HttpPut("follow/{username}")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<String>>> FollowPlayer(String username)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.FollowPlayer(username, applicationUser);
            return Ok(response);
        }        
        
        [HttpPut("unfollow/{username}")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<String>>> UnfollowPlayer(String username)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.UnfollowPlayer(username, applicationUser);
            return Ok(response);
        }
        
        [HttpGet("rs3rsn")]
        [Authorize]
        public async Task<ActionResult<string>> TestAuth()
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);
            var rsn = applicationUser.Rs3Rsn;
            if (rsn == null)
            {
                rsn = "";
            }
            return Ok(rsn);
        }
        
        // user rsn
        
        [HttpPut("rs3rsn/{username}")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<string>>> UpdateRs3Rsn(String username)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.UpdateRs3Rsn(username, applicationUser);
            return Ok(response);
        } 
        
        [HttpPut("like/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<Activity>>> LikeActivity(int id)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.LikeActivity(applicationUser, id);
            return Ok(response);
        }  
        
        [HttpPut("unlike/{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseWrapper<Activity>>> UnlikeActivity(int id)
        {
            var user = User.Claims.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()?.Value;
            var applicationUser = await UserService.SearchForUser(user);

            var response = await UserService.UnlikeActivity(applicationUser, id);
            return Ok(response);
        }   
        
        [HttpPut("ironstatus/{username}")]
        public async Task<ActionResult<ResponseWrapper<Player>>> UpdateAccountStatus(string username)
        {
            var response = await UserService.UpdateIronStatus(username);
            return Ok(response);
        }           
        [HttpGet("ironstatus/{username}")]
        public async Task<ActionResult<ResponseWrapper<AccountType>>> GetIronStatus(String username)
        {
            var response = await UserService.GetIronStatus(username);
            return Ok(response);
        }   

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
