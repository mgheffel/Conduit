using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Conduit
{
    public partial class SaveFile: Form
    {
        private MainWindow x;
        public SaveFile(MainWindow y)
        {
            InitializeComponent();
            x = y;
        }

        private void button1_Click(object sender, EventArgs e)
        {
         /*if(x.Nodes.Count > 0)
            {
                //string one = x.Connectors.ToString();
                string one = "";
                foreach (Connector c in x.Connectors)
                {
                    one = one + "," + c.ToString();
                    //MessageBox.Show(k);
                }
                string two = "\r\n";
               // string three =  x.Nodes.ToString();
                string three = "";
                foreach (Node n in x.Nodes)
                {
                    three = three + "," + n.ToString();
                    //MessageBox.Show(k);
                }

                string save = one + two + three;

                saveFileDialog1.Filter = "XML Files (*.xml)|*.xml";
                saveFileDialog1.DefaultExt = "xml";
                saveFileDialog1.AddExtension = true;

                saveFileDialog1.ShowDialog();
                XmlHelper.ToXmlFile(x.Nodes, saveFileDialog1.FileName);
                //System.IO.File.WriteAllText(saveFileDialog1.FileName, save);
            }
            else
            {
                MessageBox.Show("Need at least one node to save layout");
            }*/
              
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string[] filelines = File.ReadAllLines(filename);
                string[] split1 = filelines[0].Split(',');
                
                x.Nodes.Add(split1[0] as Node);
                
                x.Nodes.Add(Convert.ToNode(split1[1]));
                //x.Connectors = filelines[0];
                var list = XmlHelper.FromXmlFile<List<Node>>(filename);
                x.Nodes = list;
            }*/

        }
        /*public static class XmlHelper
        {
            // Specifies whether XML attributes each appear on their own line
            const bool newLineOnAttributes = false;

            public static bool NewLineOnAttributes { get; set; }
            /// <summary>
            /// Serializes an object to an XML string, using the specified namespaces.
            /// </summary>
            public static string ToXml(object obj, XmlSerializerNamespaces ns)
            {
                Type T = obj.GetType();

                var xs = new XmlSerializer(T);
                var ws = new XmlWriterSettings { Indent = true, NewLineOnAttributes = newLineOnAttributes, OmitXmlDeclaration = true };

                var sb = new StringBuilder();
                using (XmlWriter writer = XmlWriter.Create(sb, ws))
                {
                    xs.Serialize(writer, obj, ns);
                }
                return sb.ToString();
            }

            /// <summary>
            /// Serializes an object to an XML string.
            /// </summary>
            public static string ToXml(object obj)
            {
                var ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                return ToXml(obj, ns);
            }

            /// <summary>
            /// Deserializes an object from an XML string.
            /// </summary>
            public static T FromXml<T>(string xml)
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StringReader sr = new StringReader(xml))
                {
                    return (T)xs.Deserialize(sr);
                }
            }

            /// <summary>
            /// Serializes an object to an XML file.
            /// </summary>
            public static void ToXmlFile(Object obj, string filePath)
            {
                var xs = new XmlSerializer(obj.GetType());
                var ns = new XmlSerializerNamespaces();
                var ws = new XmlWriterSettings { Indent = true, NewLineOnAttributes = NewLineOnAttributes, OmitXmlDeclaration = true };
                ns.Add("", "");

                using (XmlWriter writer = XmlWriter.Create(filePath, ws))
                {
                    xs.Serialize(writer, obj);
                }
            }

            /// <summary>
            /// Deserializes an object from an XML file.
            /// </summary>
            public static T FromXmlFile<T>(string filePath)
            {
                StreamReader sr = new StreamReader(filePath);
                try
                {
                    var result = FromXml<T>(sr.ReadToEnd());
                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception(e.InnerException.Message);
                }
                finally
                {
                    sr.Close();
                }
            }
        }*/
    }
}
