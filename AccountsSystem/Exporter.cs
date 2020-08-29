﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;

namespace AccountsSystem
{
    class Exporter
    {
        public void Export(List<ProjectExpenseModel> data)
        {
            Application excel = new Application();
            excel.Visible = false;

            Workbooks workbooks = excel.Workbooks;
            Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);

            Worksheet worksheet = (Worksheet)workbook.Worksheets[1];

            initHeader(worksheet);

            int rowIndex = 2;     
            int colIndex = 1;  

            foreach(ProjectExpenseModel projectExpense in data)
            {
                worksheet.Cells[rowIndex, colIndex++] = rowIndex - 1;
                worksheet.Cells[rowIndex, colIndex++] = projectExpense.ProjectName;
                worksheet.Cells[rowIndex, colIndex++] = projectExpense.Product;
                worksheet.Cells[rowIndex, colIndex++] = projectExpense.Usage;
                worksheet.Cells[rowIndex, colIndex++] = projectExpense.Price;
                worksheet.Cells[rowIndex, colIndex++] = projectExpense.TimeStamp;
                rowIndex++;
                colIndex = 1;
            }

            worksheet.Columns.AutoFit();

            string filePath = @"D:\Export.xlsx";

            workbook.SaveAs(filePath);

            workbook.Close(true, Type.Missing, Type.Missing);
            excel.Quit();
        }

        void initHeader(Worksheet worksheet)
        {
            int colIndex = 1;

            List<string> headers = new List<string>() { "序号", "账号/项目", "报销物品", "报销用途", "金额" , "采购时间", "截图附件编号" };

            foreach(string header in headers)
            {
                worksheet.Cells[1, colIndex] = header;

                Range range = worksheet.Cells[1, colIndex++];
                range.Font.Bold = true;
                range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
            }
        }
    }
}