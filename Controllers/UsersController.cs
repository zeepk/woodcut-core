﻿using System;
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

        [HttpGet]
        public ActionResult<User> GetUser()
        {
            User newUser = new User()
            {
                DateCreated = DateTime.Now,
                Username = "username",
                DisplayName = "displayname"
            };
            return Ok(newUser);
        }

        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<User>>> GetUser()
        // {
        //     return await _context.User.ToListAsync();
        // }

        // GET: api/Users/zee+pk
        // returns specific user and their records
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetUser(string username)
        {
            //TODO: if user not found, run a function that will create one if found, or return 404 if not in official API
            var users = _context.User.Include(u => u.StatRecords);
            var user = await users.FirstOrDefaultAsync(user => user.Username == username);
            if (user == null)
            {
                var newUser = await UserService.CreateNewUser(username);
                if (newUser == null)
                {
                    return NotFound();
                }
                _context.User.Add(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }

            //List<StatRecord> statRecords = await _context.StatRecord.Where(r => r.UserId == user.Id).ToListAsync();
            //for (int i = 0; i < statRecords.Count; i++)
            //{
            //    List<Skill> skills = await _context.Skill.Where(s => s.StatRecordId == statRecords[i].Id).ToListAsync();
            //    List<Minigame> minigames = await _context.Minigame.Where(s => s.StatRecordId == statRecords[i].Id).ToListAsync();

            //}
            //user.StatRecords = statRecords;

            return user;
        }

        // PUT: api/Users/updateall
        // add a new record to each user
        [HttpPut("updateall")]
        public async Task<IActionResult> UpdateAllUsers()
        {
            var users = await _context.User.ToListAsync();
            if (users == null)
            {
                return NotFound();
            }
            var updatedUsers = UserService.AddNewStatRecordForAllUsers(users);
            await _context.SaveChangesAsync();


            return Ok(updatedUsers);

        }

        // PUT: api/Users/update/5
        // add a new record to the specified user
        [HttpPut("update/{username}")]
        public async Task<IActionResult> UpdateUser(string username)
        {
            var users = _context.User.Include(u => u.StatRecords);
            var user = await users.FirstOrDefaultAsync(user => user.Username == username);
            if (user == null)
            {
                return NotFound();
            }
            var newStatRecord = await UserService.AddNewStatRecord(user);
            await _context.SaveChangesAsync();


            return Ok(newStatRecord);

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
