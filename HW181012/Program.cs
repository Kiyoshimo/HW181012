
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace HW181012
{
    class Program
    {
        static void Main(string[] args)
        {

            XDocument xDoc = XDocument.Load(@".\..\..\TT\CnToEn.xml");//打開xml位置
            XElement root = xDoc.Root; 


            GetXElement(root);
            Console.ReadKey();
        }

        private static void GetXElement(XElement root)
        {
            foreach (XElement element in root.Elements())
            {
                if (element.Elements().Count() > 0)
                {
                    Console.WriteLine(element.Name);
                    GetXElement(element);
                }
                else
                {
                    Console.WriteLine(element.Value);
                }
            }
        }

    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace HW181012
{
    class Program
    {

     
        static void Main(string[] args)
        {
            Console.Write("AAAAA");

            Console.ReadLine();
        }

        //取值函数
        public static string getValue(XElement node, string propertyName)
        {
            return node.Element(propertyName)?.Value.Trim();
        }

        public class CnToEn
        {
            public string Postcode { get; set; }
            public string Loc { get; set; }
            public string Name { get; set; }


        }

        //读取XML
        public List<CnToEn> Xml_Load()
        {

            XDocument docNew = XDocument.Load(@".\..\..\TT\CnToEn.xml");//打開xml位置

            IEnumerable<XElement> nodes = docNew.Element("table").Elements("row");
            var nodeList = new List<CnToEn>();
            nodeList = nodes
                .Select(node => {
                    var item = new CnToEn();
                    item.Postcode = getValue(node, "Col1");
                    item.Loc = getValue(node, "Col2");
                    item.Name = getValue(node, "Col3");
                    return item;
                }).ToList();

            return nodeList;


        }
     
    }

    

}

*/
