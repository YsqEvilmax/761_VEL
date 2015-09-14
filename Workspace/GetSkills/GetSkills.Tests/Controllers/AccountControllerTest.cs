﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using GetSkills.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GetSkills.Controllers;
using System.Web.Mvc;
namespace GetSkills.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void AccountController_AdminLogin_Test01()
        {
            // Initialization
            AccountController ctl = new AccountController();
            string url = "http://getskills.azurewebsites.net/";

            // To test if it is null
            ViewResult result = ctl.AdminLogin(url) as ViewResult;
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_AdminLogin_Test02()
        {
            // Initialization
            AccountController ctl = new AccountController();
            user model = new user();
            string url = "http://getskills.azurewebsites.net/";

            // To test if it is null
            var result = ctl.AdminLogin(model, url);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_Login_Test01()
        {
            // Initialization
            AccountController ctl = new AccountController();
            string url = "http://getskills.azurewebsites.net/";

            // To test if it is null
            ViewResult result = ctl.Login(url) as ViewResult; 
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_Login_Test02()
        {
            // Initialization
            AccountController ctl = new AccountController();
            LoginViewModel model = new LoginViewModel();
            model.Email = "aaaa";
            model.Password = "123456";
            model.RememberMe = false;
            string url = "http://getskills.azurewebsites.net/";

            // To test if it is null
            var result = ctl.Login(model, url);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_VerifyCode_Test01()
        {
            // Initialization
            AccountController ctl = new AccountController();
            string provider = "Admin";
            string url = "http://getskills.azurewebsites.net/";
            bool rememberMe = false;

            // To test if it is null
            var result = ctl.VerifyCode(provider, url, rememberMe);
            Assert.IsNotNull(result);          
        }

        [TestMethod]
        public void AccountController_VerifyCode_Test02()
        {
            // Initialization
            AccountController ctl = new AccountController();
            VerifyCodeViewModel model = new VerifyCodeViewModel();

            // To test if it is null
            var result = ctl.VerifyCode(model);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_Regitser_Test01()
        {
            // Initialization
            AccountController ctl = new AccountController();

            // To test if it is null
            var result = ctl.Register();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_Regitser_Test02()
        {
            // Initialization
            AccountController ctl = new AccountController();
            RegisterViewModel model = new RegisterViewModel();

            // To test if it is null
            var result = ctl.Register(model);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_ConfirmEmail_Test()
        {
            // Initialization
            AccountController ctl = new AccountController();
            string id = "0";
            string code = "888888";

            // To test if it is null
            var result = ctl.ConfirmEmail(id, code);
            Assert.IsNotNull(result);

            // Check the content
            Assert.AreEqual("ConfirmEmail", result.Succeeded);
        }

        [TestMethod]
        public void AccountController_ForgotPassword_Test01()
        {
            // Initialization
            AccountController ctl = new AccountController();

            // To test if it is null
            var result = ctl.ForgotPassword();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_ForgotPassword_Test02()
        {
            // Initialization
            AccountController ctl = new AccountController();
            ForgotPasswordViewModel model = new ForgotPasswordViewModel();

            // To test if it is null
            var result = ctl.ForgotPassword(model);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AccountController_ResetPassword_Test01()
        {
            // Initialization
            AccountController ctl = new AccountController();
            string code = "888888";

            // To test if it is null
            var result = ctl.ResetPassword(code);
            Assert.IsNotNull(result);

            // Check the content
            Assert.AreEqual("Error", result);
        }

        [TestMethod]
        public void AccountController_ResetPassword_Test02()
        {
            // Initialization
            AccountController ctl = new AccountController();
            ResetPasswordViewModel model = new ResetPasswordViewModel();

            // To test if it is null
            var result = ctl.ResetPassword(model);
            Assert.IsNotNull(result);
        }
    }
}
