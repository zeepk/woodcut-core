using dotnet5_webapp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dotnet5_webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private List<Contact> contacts = new List<Contact>
        {
            new Contact { Id= 1, FirstName = "Matt", LastName = "Hughes", NickName = "zee", Place = "San Diego"},
            new Contact { Id= 2, FirstName = "Brandon", LastName = "Balancio", NickName = "Alvin", Place = "Livermore"}
        };
        // GET: api/<ContactController>
        [HttpGet]
        public ActionResult<IEnumerable<Contact>> Get()
        {
            return contacts;
        }

        // GET api/<ContactController>/5
        [HttpGet("{id}")]
        public ActionResult<Contact> Get(int id)
        {
            Contact contact = contacts.FirstOrDefault(c => c.Id == id);
            if(contact == null)
            {
                return NotFound(new { Message = "Contact has not been found." });
            }
            return Ok(contact);
        }

        // POST api/<ContactController>
        [HttpPost]
        public ActionResult<IEnumerable<Contact>> Post(Contact newContact)
        {
            contacts.Add(newContact);
            return contacts;
        }

        // PUT api/<ContactController>/5
        [HttpPut("{id}")]
        public ActionResult<IEnumerable<Contact>> Put(int id, Contact updatedContact)
        {
            Contact contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound(new { Message = "Contact has not been found." });
            }
            contact.NickName = updatedContact.NickName;
            contact.IsDeleted = updatedContact.IsDeleted;
            return contacts;
        }

        // DELETE api/<ContactController>/5
        [HttpDelete("{id}")]
        public ActionResult<IEnumerable<Contact>> Delete(int id)
        {
            Contact contact = contacts.FirstOrDefault(c => c.Id == id);
            if (contact == null)
            {
                return NotFound(new { Message = "Contact has not been found." });
            }
            contacts.Remove(contact);
            return contacts;
        }
    }
}
