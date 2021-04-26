﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnet5_webapp.Data;
using dotnet5_webapp.Models;
using dotnet5_webapp.Services;
using Microsoft.Extensions.Configuration;
using dotnet5_webapp.Internal;

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
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/zee+pk
        // returns specific user and their records
        [HttpGet("{username}")]
        public async Task<ActionResult<UserSearchResponse>> GetUser(string username)
        {
            var response = await UserService.SearchForUser(username);
            return Ok(response);
        }
        
        // GET: api/Users/gains/zee+pk
        // returns specific user and their records
        [HttpGet("gains/{username}")]
        public async Task<ActionResult<CurrentGainForUserServiceResponse>> GetGainsForUser(string username)
        {
            var response = await UserService.CurrentGainForUser(username);
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

        // PUT: api/Users/update/zee+pk
        // add a new record to the specified user
        [HttpPost("update/{username}")]
        public async Task<ActionResult<User>> UpdateUser(string username)
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
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

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

            return Ok(user);

        }

        // POST: api/Users
        // autogenerated
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        // autogenerated
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
