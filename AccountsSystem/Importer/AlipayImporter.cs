﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountsSystem
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

        public override bool Import()
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

            List<Expense> expenses = new List<Expense>();
            while (line != null && !line.StartsWith("#"))
            {
                string[] fields = line.Split(',');
                Expense transaction = new Expense();

                transaction.TimeStamp = DateTime.Parse(fields[(int)CSVFormat.Time]);
                transaction.Product = fields[(int)CSVFormat.Product];
                transaction.Price = float.Parse(fields[(int)CSVFormat.Price]);
                transaction.Source = source;

                expenses.Add(transaction);
                line = streamReader.ReadLine();
            }

            if (expenses.Count == 0)
                return false;

            ExpenseDBProvider.Instance().AddRange(expenses);
            ExpenseDBProvider.Instance().Save();
            return true;

        }
    }
}
