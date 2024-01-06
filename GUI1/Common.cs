using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Controls;
using System.Xml;

namespace GUI1
{
    internal class Common : INotifyCollectionChanged
    {
        private int _BaudRate;
        public int BudRate
        {
            get { return _BaudRate; }
            set { _BaudRate = value; OnpropertyChanged("BaudRate"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnpropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        private int _txtsum;
        public int txtsum
        {
            get { return _txtsum; }
            set { _txtsum = value; OnpropertyChanged("_txtsum"); }
        }
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
    public static class GetConfig
    {
        private static XmlDocument doc;

        private const string FileName = "Configuration.xml";
        private static MemoryStream ms;

        static GetConfig()
        {
            doc = new XmlDocument { PreserveWhitespace = true };
            byte[] buffer;
            using (var fs = new FileStream(FileName, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                using (ms = new MemoryStream(buffer))
                {
                    ms.Position = 0;
                    doc.Load(ms);
                }
                fs.Close();
                ms = new MemoryStream(buffer) { Position = 0 };
            }
            ms = null;

        }
        private static void Save()
        {
            doc.Save(FileName);
        }
        public static void SetParam(string Xpath, string strValue)
        {
            doc.SelectSingleNode("/Configuration/" + Xpath).InnerText = strValue;
            Save();
        }

        public static string GetParam(string xPath)
        {
            return doc.SelectSingleNode("/Configuration/" + xPath).InnerText;
        }
    }
}







