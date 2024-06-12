using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZeroGravity.Db.DbContext;

namespace ZeroGravity.Db.Tests
{
    [TestClass]
    public abstract class DbUnitTestBase
    {
        protected ZeroGravityContext Context { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            InitContext();
        }

        [TestCleanup]
        public void CleanUp()
        {
            Context?.Database.EnsureDeleted();
        }

        private void InitContext()
        {
            var connectionString = GenerateConnectionString();
            var builder = new DbContextOptionsBuilder<ZeroGravityContext>();
            builder.UseSqlServer(connectionString);

            Context = new ZeroGravityContext(builder.Options);

            Context.Database.EnsureCreated();
        }

        private string GenerateConnectionString()
        {
            var guid = Guid.NewGuid();

            var connectionString = $"Server=(localdb)\\mssqllocaldb;Database=ZeroGravity.Test-{guid};Trusted_Connection=True;MultipleActiveResultSets=true";

            return connectionString;
        }
    }
}
