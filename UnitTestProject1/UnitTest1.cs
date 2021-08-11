using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var key = new RTech.Lib.ProductKey.RiddhaKey("HrmplDesk", System.DateTime.Now.AddYears(1));
            string mac = key.GetMacAddress();
            var pk = key.getProductKey();
        }
    }
}
