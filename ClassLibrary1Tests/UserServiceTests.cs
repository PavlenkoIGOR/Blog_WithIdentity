using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MainBlog.Data;
using MainBlog.BL.Services;

namespace ClassLibrary1.Tests
{
    [TestClass()]
    public class UserServiceTests
    {


        [TestMethod()]
        public void GetUSerTest()
        {
            var optionsBuilderData = new DbContextOptionsBuilder<MainBlogDBContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
            var db = new MainBlogDBContext(optionsBuilderData.Options);

            //arrange
            UserService userService = new UserService();

            //action
            //var result = userService.GetUSer();

            //assert
            //Assert.IsTrue(result);
            
        }
    }
}