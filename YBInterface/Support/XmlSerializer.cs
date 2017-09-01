using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace YBInterface1.Support
{
    public static class XmlSerializer
    {
        private const string NAME_SPACE = "http://www.section9.org/cms/referral/data";

        public static void SaveToXml(string filePath, object sourceObj, string xmlRootName)
        {
            if (!string.IsNullOrEmpty(filePath) && sourceObj != null)
            {
                Type type = sourceObj.GetType();

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    try
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add(string.Empty, NAME_SPACE);

                        System.Xml.Serialization.XmlSerializer xmlSerializer = string.IsNullOrEmpty(xmlRootName) ?
                            new System.Xml.Serialization.XmlSerializer(type) :
                            new System.Xml.Serialization.XmlSerializer(type, new XmlRootAttribute(xmlRootName));

                        xmlSerializer.Serialize(writer, sourceObj, ns);
                    }
                    catch
                    {
                        if (writer != null)
                        {
                            writer.Dispose();
                        }
                        throw;
                    }
                }
            }
        }

        public static TResponse LoadFromXml<TResponse, TResponseBody>(string filePath)
            where TResponse : Support.Response<TResponseBody>, new()
            where TResponseBody : class, new()
        {
            TResponse response = default(TResponse);

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(TResponse));
                    response = (TResponse)xmlSerializer.Deserialize(reader);
                    if (response != null)
                    {
                        response.status = (response.head.stateCode == Constants.OK_STATE_CODE) ? Status.OK : Status.ERROR;
                    }
                }
            }

            return response;
        }
    }
}
