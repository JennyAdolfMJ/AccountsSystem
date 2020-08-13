using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseAccount
{
    class AlipayImporter : Importer
    {
        enum CSVFormat
        {
            Time = 1,
            Product = 3,
            Price = 4
        }

        public AlipayImporter(string filename) : base(filename)
        {
            source = "支付宝";
        }

        public override void Import()
        {
            string line = streamReader.ReadLine();

            while (line != null)
            {
                // We are now at table header
                if (!line.StartsWith("#"))
                {
                    line = streamReader.ReadLine();
                    break;
                }
                line = streamReader.ReadLine();
            }

            while (line != null && !line.StartsWith("#"))
            {
                string[] fields = line.Split(',');
                Transaction transaction = new Transaction();

                transaction.TimeStamp = DateTime.Parse(fields[(int)CSVFormat.Time]);
                transaction.Product = fields[(int)CSVFormat.Product];
                transaction.Price = float.Parse(fields[(int)CSVFormat.Price]);
                transaction.Source = source;

                ExpenseDBProvider.Add(transaction);
                line = streamReader.ReadLine();
            }
            ExpenseDBProvider.Save();
        }
    }
}
