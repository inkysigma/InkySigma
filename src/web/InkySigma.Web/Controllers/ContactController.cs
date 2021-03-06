﻿using System;
using System.Threading.Tasks;
using InkySigma.Authentication.AspNet.Attributes;
using InkySigma.Authentication.Dapper.Models;
using InkySigma.Authentication.Managers;
using InkySigma.Web.Infrastructure.Exceptions;
using InkySigma.Web.RequestModel;
using Microsoft.AspNet.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace InkySigma.Web.Controllers
{
    public class ContactController : Controller
    {
        public UserService<User> UserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        [IdentityAuthorize("User")]
        public async Task<bool> RequestContact(AddContactRequestModel contact)
        {
            if (contact == null)
                throw new ParameterNullException(nameof(contact));
            if (string.IsNullOrEmpty(contact.Contact))
                throw new InvalidParameterException(nameof(contact.Contact));
            if (string.IsNullOrEmpty(contact.Type))
                throw new InvalidParameterException(nameof(contact.Type));
            contact.Type = contact.Type.ToLower();
            var userName = HttpContext.User.Identity.Name;
            if (contact.Type.ToLower() == "username")
            {
                var user = await UserService.FindUserByUsername(userName);
                if (user == null)
                    return false;
                user = await UserService.GetUserProperties(user);
            }
            throw new NotImplementedException();
        }
    }
}
