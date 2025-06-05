using Microsoft.VisualStudio.TestTools.UnitTesting;
using CarRental.Domain.ServiceModule;

namespace CarRental.Controllers.Tests.ServiceModule
{
    [TestClass]
    [TestCategory("Domain")]
    public class ServiceTest
    {
        [TestMethod]
        public void ShouldCreateValidService()
        {
            Service service = new Service(0, "name", true, 100f);
            Assert.AreEqual("VALID", service.Validate());
        }

        [TestMethod]
        public void ShouldCreateInvalidService()
        {
            Service service = new Service(0, "", true, 0f);
            Assert.AreEqual("The name cannot be null\nThe value cannot be null", service.Validate());
        }
    }
}
