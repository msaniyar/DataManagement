using System;
using DataManagement;
using DataManagement.Controllers;
using DataManagement.Services;
using DataManagement.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;

namespace Tests
{
    [TestFixture]
    public class Tests
    {

        private readonly IDataControl _dataControl;
        private readonly Startup _startup;
        private readonly DataController _controller;

        public Tests()
        {
            var options = new DbContextOptionsBuilder<DataManagementContext>()
                .UseInMemoryDatabase(databaseName: "TreeListTable")
                .Options;
            var db = new DataManagementContext(options);
            _dataControl = new DataControl(db);

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            _startup = new Startup(config);
            _controller = new DataController(_dataControl);

        }

        [SetUp]
        public void Setup()
        {

        }

        [Test, Order(1)]
        public void AddPostAsyncTest()
        {

            var testModel = new TreeListTable
            {
                Tree = "hqfoLjEw7HLYnap51O2Lb6tjNI+Rdn1UXo5rihOy/MHn+myU4YqEKonVRaR8FItX",
                Id = Guid.NewGuid()
            };
            var result = _dataControl.AddPostAsync(testModel);

            Assert.That(result.Result, Is.EqualTo(testModel.Id), "Result id is not equal to inserted id.");
        }

        [Test, Order(2)]
        public void DecryptAesManagedTest()
        {
            var text = "hqfoLjEw7HLYnap51O2Lb6tjNI+Rdn1UXo5rihOy/MHn+myU4YqEKonVRaR8FItX";
            var secret = _startup.Configuration.GetSection("AppConfiguration")["SecretKey"];
            var vector = _startup.Configuration.GetSection("AppConfiguration")["vector"];

            var result = _dataControl.DecryptAesManaged(text, secret, vector);
            Assert.That(result, Is.EqualTo("{\"test.txt\":\"2019-04-13T16:31:38Z\"}"), "Decrypted text is not correct." );
        }

    }
}