using System.Data;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace ZwsXmlClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            await Send("sa", "123", "progis.spo.grodno.energo.net", await AddPolyLine("Test:test_zws_api", "6", "1", "53.1234,25.1234 53.4321,25.4321"));
            await Send("sa", "123", "progis.spo.grodno.energo.net", await DeleteElement("Test:test_zws_api", "123"));                     
        }

        static async Task<string> DeleteElement(string _layer, string _elemId)
        {
            var request = new ZuluServerRoot();
            request.command.CurrentCommand = new LayerDeleteElementCommand(_layer, _elemId);
            return await Task.FromResult(await CreateXml(request));

        }

        static async Task<string> AddPolyLine(string _layer, string _typeid, string _modenum, string _coordinates = "", string _crs = "EPSG:4326")
        {
            var request = new ZuluServerRoot();
            request.command.CurrentCommand = new LayerAddPolylineCommand(_layer, _typeid, _modenum, _coordinates);
            return await Task.FromResult(await CreateXml(request));
        }

        static async Task<string> CreateXml(ZuluServerRoot _root)
        {
            string xml = "";
            XmlSerializer serializer = new XmlSerializer(typeof(ZuluServerRoot));
            await using (var stringWriter = new StringWriterUTF8())
            {
                await using (XmlWriter writer = XmlWriter.Create(stringWriter, new XmlWriterSettings() { Async = true }))
                {
                    serializer.Serialize(writer, _root);
                    xml = stringWriter.ToString();
                }
            }
            return await Task.FromResult(xml);
        }

        static async Task Send(string login, string password, string host, string xml )
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                var byteArray = Encoding.ASCII.GetBytes($"{login}:{password}");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));                
                HttpContent body = new StringContent(xml, Encoding.UTF8, "application/xml");
                var response = client.PostAsync($"http://{host}:6473/zws", body).Result;
                var response_xml = await response.Content.ReadAsStringAsync();
            }
        }
    }

    public class StringWriterUTF8 : StringWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
