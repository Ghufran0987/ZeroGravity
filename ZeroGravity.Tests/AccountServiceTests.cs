using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ZeroGravity.Db.DbContext;
using ZeroGravity.Db.Repository;
using ZeroGravity.Db.Tests;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Models.Accounts;
using ZeroGravity.Services;

namespace ZeroGravity.Tests
{
    [TestClass]
    public class AccountServiceTests : DbUnitTestBase
    {
        [TestMethod]
        public void CanCreateAccounts_ReturnsTrue()
        {
            var sut = CreateSut();

            var accountsToCreate = 5;
            var accounts = CreateAccounts(sut, accountsToCreate).ToList();
            var accountsInDb = sut.GetAll().ToList();

            Assert.AreEqual(accountsToCreate, accounts.Count);

            var expectedNumberOfAccountsInDb = accountsToCreate + 1; //admin
            Assert.AreEqual(expectedNumberOfAccountsInDb, accountsInDb.Count);

            foreach (var account in accounts)
            {
                Assert.IsTrue(accountsInDb.Any(x => x.Id == account.Id));
            }
        }

        [TestMethod]
        public void CanDeleteAccounts_ReturnsTrue()
        {
            var sut = CreateSut();

            var accountsToCreate = 10;
            var accountsToDelete = 3;
            var indicesToDelete = new[] {1, 4, 6};
            var accounts = CreateAccounts(sut, accountsToCreate).ToList();
            
            var deletedAccounts = new List<AccountResponse>();
            foreach (var index in indicesToDelete)
            {
                var account = accounts[index];
                
                sut.Delete(account.Id);

                deletedAccounts.Add(account);
            }

            var allUsers = sut.GetAll().ToList();

            var expectedUserCount = accountsToCreate - accountsToDelete + 1 /*default admin*/;
            Assert.AreEqual(expectedUserCount, allUsers.Count);

            foreach (var deletedAccount in deletedAccounts)
            {
                Assert.IsFalse(allUsers.Any(x => x.Id == deletedAccount.Id), "Deleted user exists in remaining user list.");
            }
            
        }

        private IEnumerable<AccountResponse> CreateAccounts(IAccountService sut, int number)
        {
            var createRequests = CreateUserRequests(number);
            var accounts = new List<AccountResponse>();

            foreach (var createRequest in createRequests)
            {
                var accountResponse = sut.Create(createRequest);
                accounts.Add(accountResponse);
            }

            return accounts;
        }

        private IEnumerable<CreateRequest> CreateUserRequests(int amount)
        {
            var requests = new List<CreateRequest>();
            for (var i = 0; i < amount; i++)
            {
                var request = new CreateRequest
                {
                    Email = $"user{i}@dummy.xyz",
                    FirstName = $"Firstname{i}",
                    LastName = $"Lastname{i}",
                    Password = $"Password{i}",
                    ConfirmPassword = $"Password{i}",
                    Role = $"User",
                    Title = "Mr."
                };

                requests.Add(request);
            }

            return requests;
        }

        private IAccountService CreateSut()
        {
            var mockedAppSettings = new Mock<IOptions<AppSettings>>();
            var mockedEmailService = new Mock<IEmailService>();
            var mockedStringLocalizer = new Mock<IStringLocalizer<AccountService>>();
            mockedStringLocalizer.Setup(localizer => localizer[It.IsAny<string>()]).Returns(new LocalizedString("name", "value"));
            var mockedLogger = new Mock<ILogger<AccountService>>();
            var mockedRepository = new Mock<IRepository<ZeroGravityContext>>();
            var mockedConfiguration = new Mock<IConfiguration>();

            var autoMapperProfile = new AutoMapperProfile();

            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(autoMapperProfile);
            });
            var mapper = mapperConfig.CreateMapper();
            var appSettings = mockedAppSettings.Object;
            var emailService = mockedEmailService.Object;
            var stringLocalizer = mockedStringLocalizer.Object;
            var logger = mockedLogger.Object;
            var repository = mockedRepository.Object;
            var configuration = mockedConfiguration.Object;

            var sut = new AccountService(Context, mapper, appSettings, emailService, stringLocalizer, logger, repository, configuration);

            return sut;
        }
    }
}
