using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using YBInterface1;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //string xmlFile = @"d:\xml\门诊慢性病患者疾病代码查询.C010005.RESPONSE.xml";
            //var response = YBInterface1.Support.XmlSerializer.LoadFromXml<YBInterface1.Support.Response<YBInterface1.Support.C010005.responseBody>, YBInterface1.Support.C010005.responseBody>(xmlFile);
            
            string xmlFile = @"d:\xml\门诊预结算.B010001.RESPONSE.xml";
            var response1 = YBInterface1.Support.XmlSerializer.LoadFromXml<YBInterface1.Support.Response<YBInterface1.Support.B010001.responseBody>, YBInterface1.Support.B010001.responseBody>(xmlFile);
        }
    }
}
