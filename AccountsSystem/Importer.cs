using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace ExpenseAccount
{
    abstract class Importer
    {
        private FileStream fileStream = null;
        protected StreamReader streamReader = null;
        protected string source = null;

        protected Importer(string filename)
        {
            fileStream = File.OpenRead(filename);
            streamReader = new StreamReader(fileStream, Encoding.GetEncoding("GB2312"));
        }

        ~Importer()
        {
            streamReader.Dispose();
            fileStream.Dispose();
        }

        public abstract void Import();
    }
}
