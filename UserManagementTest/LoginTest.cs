using System;
using System.Text.Json;
using System.Threading.Tasks;
using Anoroc_User_Management.Controllers;
using Anoroc_User_Management.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace UserManagementTest
{
    //TODO: Add test cases
    public class LoginTest
    {
        private LoginController _loginController; 
        [SetUp]
        public void Setup()
        {
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("http://localhost:5001"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/userManagement"));
            
            var httpContext = Mock.Of<HttpContext>(_ => 
                _.Request == request.Object
            );

            //Controller needs a controller context 
            var controllerContext = new ControllerContext() {
                HttpContext = httpContext,
            };

            _loginController = new LoginController(){
                ControllerContext = controllerContext,
            };
        }

        [Test]
        public void ApiReturnsString()
        {
            var test = new Login()
            {
                Email = "tn.selahle@gmail.com",
                Password = "thisIs"
            };

            var actual =  _loginController.Post(test);

            Assert.IsInstanceOf<string>(actual);
        }
        
        [Test]
        public void TokenExist()
        {
            var test = new Login()
            {
                Email = "tn.selahle@gmail.com",
                Password = "thisIs"
            };

            var actual = _loginController.Post(test);
            var actualObj = JsonSerializer.Deserialize<Login>(actual);
            
            Assert.AreNotEqual(null, actualObj.Token);
        }
    }
}