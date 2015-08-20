﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InkySigma.Identity.Validator;

namespace InkySigma.Identity.Models
{
    public class FormValidatorOptions
    {
        public IValidator UsernameValidator { get; set; } = new UsernameValidator();
        public IValidator PasswordValidator { get; set; } = new PasswordValidator();
        public IValidator EmailValidator { get; set; } = new EmailValidator();
    }
}
