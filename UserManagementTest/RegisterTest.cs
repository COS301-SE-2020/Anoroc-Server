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
    public class RegisterTest
    {
        private RegisterController _registerController; 
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

            _registerController = new RegisterController(){
                ControllerContext = controllerContext,
            };
        }

        [Test]
        public void APIReturnsString()
        {
            var test = new Register()
            {
                Email = "tn.selahle@gmail.com",
                Password = "thisIs",
                Surname = "Selahle",
                Username = "Tebogo"
            };

            var actual =  _registerController.Post(test);

            Assert.IsInstanceOf<string>(actual);
        }
        
        [Test]
        public void TokenExist()
        {
            var test = new Register()
            {
                Email = "tn.selahle@gmail.com",
                Password = "thisIs",
                Surname = "Selahle",
                Username = "Tebogo"
            };

            var actual = _registerController.Post(test);
            var actualObj = JsonSerializer.Deserialize<Register>(actual);
            
            Assert.AreNotEqual(null, actualObj.Token);
        }
    }
}