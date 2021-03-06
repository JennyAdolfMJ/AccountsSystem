﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace AccountsSystem
{
    class WeChatImporter : Importer
    {
        enum CSVFormat
        {
            Time = 0,
            Seller = 2,
            Product = 3,
            InOut = 4,
            Price = 5
        }

        public WeChatImporter (string filename):base(filename)
        {
            source = "微信";
        }

        public override bool Import()
        {
            string line = streamReader.ReadLine();

            while(line != null)
            {
                if(line.StartsWith("----"))
                {
                    // We are now at table header
                    line = streamReader.ReadLine();
                    line = streamReader.ReadLine();
                    break;
                }
                line = streamReader.ReadLine();
            }

            List<Expense> expenses = new List<Expense>();
            while (line != null)
            {
                string[] fields = line.Split(',');
                Expense transaction = new Expense();

                if (fields[(int)CSVFormat.InOut].Equals("收入"))
                {
                    line = streamReader.ReadLine();
                    continue;
                }

                transaction.TimeStamp = DateTime.Parse(fields[(int)CSVFormat.Time]);
                transaction.Product = string.Format("{0} - {1}", fields[(int)CSVFormat.Seller], fields[(int)CSVFormat.Product]);
                transaction.Price = float.Parse(fields[(int)CSVFormat.Price].Replace("¥", ""));
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
