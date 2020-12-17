using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using DataTable = System.Data.DataTable;

namespace CommonDll.Helps
{
    public static class ParseExcelDocument
    {
        public static readonly string EngAlp = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static Tuple<DataTable, string> Read(string Path, string LetterStart, string LetterEnd, Dictionary<string, string> PathsAndFields, int startRow, out Exception Exception, [Optional] int RecurseTry)
        {
            DataTable dataTable = new DataTable();
            string ExceptionMessage = "";

            lock (Helper_EXCEL.LockObject_ForCreateProcessKill)
            {
                Application MyAppField = Helper_EXCEL.GetApp(out int idIndexField, true);
                Workbook MyBookField = Helper_EXCEL.GetWorkbook(MyAppField, Path, out Exception, 0);

                if (!(MyBookField is null))
                {
                    MyAppField.Visible = false;
                    Sheets MySheetFields = MyBookField.Sheets;

                    Worksheet MySheetField_Start = default(Worksheet);
                    int lastRowFields_Start = default(int);
                    List<Tuple<string, string>> FieldInExcel_Start = new List<Tuple<string, string>>();
                    int index_Start = default(int);
                    bool FieldsIsFound = false;
                    List<Worksheet> worksheets = new List<Worksheet>();

                    foreach (Worksheet MySheetField in MySheetFields)
                        worksheets.Add(MySheetField);

                    foreach (Worksheet MySheetField in worksheets)
                    {
                        int lastRowFields = MySheetField.Cells.SpecialCells(XlCellType.xlCellTypeLastCell).Row;

                        for (int i = 1; i <= startRow; i++)
                        {
                            List<Tuple<string, string>> FieldInExcel = new List<Tuple<string, string>>();

                            Array a = (Array)MySheetField.get_Range(LetterStart + "" + i + "", LetterEnd + "" + i + "").Cells.Value;
                            int NumbInEngAlp = EngAlp.ToUpper().IndexOf(LetterStart + "".ToUpper());

                            foreach (var aa in a)
                            {
                                string Field = (NumbInEngAlp > 25 ? EngAlp[0] + "" : EngAlp[NumbInEngAlp] + "") + (NumbInEngAlp > 25 ? EngAlp[NumbInEngAlp - 26] + "" : "");
                                FieldInExcel.Add(new Tuple<string, string>(Field + "" + i + "", aa + ""));
                                NumbInEngAlp++;
                            }

                            bool IsExist = false;

                            if (PathsAndFields is null ? false : PathsAndFields.Count > 0) // Совпадение по хэдэру
                            {
                                foreach (var entry in PathsAndFields)
                                    foreach (var v in FieldInExcel)
                                        if (v.Item1 == entry.Key)
                                            if (entry.Value is null ? false : v.Item2 is null ? false : entry.Value.Contains(v.Item2) || v.Item2.Contains(entry.Value))
                                                IsExist = true;
                                            else
                                            {
                                                IsExist = false;

                                                break;
                                            }
                            }
                            else
                                IsExist = true;  // нет примеров для хэдэра

                            if (IsExist)
                            {
                                FieldsIsFound = true;
                                index_Start = i;
                                MySheetField_Start = MySheetField;
                                FieldInExcel_Start = FieldInExcel;
                                lastRowFields_Start = lastRowFields;

                                if (PathsAndFields is null ? true : PathsAndFields.Count == 0)
                                    break;
                            }
                        }

                        if (FieldsIsFound & (PathsAndFields is null ? true : PathsAndFields.Count == 0))
                            break;
                    }

                    if (FieldsIsFound)
                    {
                        List<List<string>> ReadyDicitonary = new List<List<string>>();

                        for (int index = index_Start; index <= lastRowFields_Start; index++)
                        {
                            List<string> ValueInExcell = new List<string>();
                            {
                                Array a = null;

                                try
                                {
                                    a = (Array)MySheetField_Start.get_Range(LetterStart + "" + index + "", LetterEnd + "" + index + "").Cells.Value;
                                }
                                catch (Exception ex)
                                {
                                    ConsoleWriteLine.WriteInConsole(null, null, "Failed", ex.Message.ToString(), ConsoleColor.Red);
                                }

                                if (!(a is null))
                                    foreach (var aa in a)
                                        ValueInExcell.Add(aa + "");
                            }

                            ReadyDicitonary.Add(ValueInExcell);
                        }

                        dataTable = new DataTable();

                        int iColumn = EngAlp.ToUpper().IndexOf(LetterStart + "".ToUpper());

                        foreach (var r in ReadyDicitonary[0])
                        {
                            string Field = (iColumn > 25 ? EngAlp[0] + "" : EngAlp[iColumn] + "") + (iColumn > 25 ? EngAlp[iColumn - 26] + "" : "");
                            dataTable.Columns.Add(new DataColumn(Field + "" + ""));
                            iColumn++;
                        }

                        foreach (var dict in ReadyDicitonary)
                        {
                            DataRow dataRow = dataTable.NewRow();
                            bool IsContinue = true;

                            foreach (var r in dict)
                                if (r != "")
                                {
                                    IsContinue = false;

                                    break;
                                }

                            if (IsContinue)
                                continue;

                            iColumn = EngAlp.ToUpper().IndexOf(LetterStart + "".ToUpper());

                            foreach (var r in dict)
                            {
                                string Field = (iColumn > 25 ? EngAlp[0] + "" : EngAlp[iColumn] + "") + (iColumn > 25 ? EngAlp[iColumn - 26] + "" : "");
                                dataRow[Field + "" + ""] = r;

                                iColumn++;
                            }

                            dataTable.Rows.Add(dataRow);
                        }
                    }

                    MySheetFields = null;

                    try
                    {
                        MyBookField.Close(false);
                    }
                    catch
                    {

                    }

                    try
                    {
                        MyAppField.Quit();
                    }
                    catch
                    {

                    }

                    MyBookField = null;
                }

                MyAppField = null;
                Helper_EXCEL.KillApp(idIndexField, true);
                Helper_EXCEL.Clear();
            }

            if (!string.IsNullOrEmpty(ExceptionMessage))
                ConsoleWriteLine.WriteInConsole(null, null, "Failed", string.Format("{0}: {1}", MethodBase.GetCurrentMethod().Name, ExceptionMessage), ConsoleColor.Red);

            return new Tuple<DataTable, string>(dataTable, ExceptionMessage);
        }
    }
}
