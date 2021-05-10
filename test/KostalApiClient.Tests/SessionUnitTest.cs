using System.Threading.Tasks;
using KostalApiClient.Api;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KostalApiClient.Tests
{
    [TestClass]
    public class SessionUnitTest
    {
        [TestMethod]
        public async Task LoginTest()
        {
            Session session = new ("{YOUR_HOST}");
            await session.Login("{YOUR_PASSWORD}");
        }
    }
}
