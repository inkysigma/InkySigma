using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Infrastructure.Exceptions;
using InkySigma.ViewModel;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Controllers
{
    public class ContactController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddContact(AddContactViewModel contact)
        {
            if (contact == null)
                throw new ParameterNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.Contact))
                throw new InvalidParameterException(nameof(contact.Contact));
            if (string.IsNullOrEmpty(contact.Type))
                throw new InvalidParameterException(nameof(contact.Type));
            contact.Type = contact.Type.ToLower();
            switch (contact.Type)
            {
                case "username":
                    await 
            }
        }
    }
}
