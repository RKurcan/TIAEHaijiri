using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Riddhasoft.Services.Common
{
    public static class Extension
    {
        public static DataTable ToDataTable<T>(this IList<T> data, string tableName = "datatable1")
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable(tableName);
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        public static dynamic[] ToPivotArray<T, TColumn, TRow, TData>(
 this IEnumerable<T> source,
 Func<T, TColumn> columnSelector,
 Expression<Func<T, TRow>> rowSelector,
 Func<IEnumerable<T>, TData> dataSelector)
        {

            var arr = new List<object>();
            var cols = new List<string>();
            String rowName = ((MemberExpression)rowSelector.Body).Member.Name;
            var columns = source.Select(columnSelector).Distinct();

            cols = (new[] { rowName }).Concat(columns.Select(x => x.ToString())).ToList();


            var rows = source.GroupBy(rowSelector.Compile())
                             .Select(rowGroup => new
                             {
                                 Key = rowGroup.Key,
                                 Values = columns.GroupJoin(
                                     rowGroup,
                                     c => c,
                                     r => columnSelector(r),
                                     (c, columnGroup) => dataSelector(columnGroup))
                             }).ToArray();


            foreach (var row in rows)
            {
                var items = row.Values.Cast<object>().ToList();
                items.Insert(0, row.Key);
                var obj = GetAnonymousObject(cols, items);
                arr.Add(obj);
            }
            return arr.ToArray();
        }
        private static dynamic GetAnonymousObject(IEnumerable<string> columns, IEnumerable<object> values)
        {
            IDictionary<string, object> eo = new ExpandoObject() as IDictionary<string, object>;
            int i;
            for (i = 0; i < columns.Count(); i++)
            {
                eo.Add(columns.ElementAt<string>(i), values.ElementAt<object>(i));
            }
            return eo;
        }
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..

            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;

            internal DynamicRow(DataRow row) { _row = row; }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }
        public static List<dynamic> ToDynamicList(this DataTable dt)
        {
            var dynamicDt = new List<dynamic>();
            foreach (DataRow row in dt.Rows)
            {
                dynamic dyn = new ExpandoObject();
                dynamicDt.Add(dyn);
                foreach (DataColumn column in dt.Columns)
                {
                    var dic = (IDictionary<string, object>)dyn;
                    dic[column.ColumnName] = row[column];
                }
            }
            return dynamicDt;
        }
        public static string ConvertEngDayNameToNep(string engDay)
        {
           string nepDay="";
            switch (engDay)
	            {
		            case "Sunday":
                    nepDay="आइतबार";
                    break;
                   case "Monday":
                    nepDay="सोमवार";
                    break;
                   case "Tuesday":
                    nepDay="मंगलवार";
                    break;
                   case "Wednesday":
                    nepDay="बुधवार";
                    break;
                   case "Thursday":
                    nepDay="बिहीबार";
                    break;
                   case "Friday":
                    nepDay="शुक्रबार";
                    break;
                   case "Saturday":
                    nepDay="शनिबार";
                    break;default:
                    break;
	            }
            return nepDay;
        }

        //public static void ExportToExcel<T>(this List<T> lst, string excelFilePath = null)
        //{
        //    DataTable tbl = lst.ToDataTable<T>();

        //    try
        //    {
        //        if (tbl == null || tbl.Columns.Count == 0)
        //            throw new Exception("ExportToExcel: Null or empty input table!\n");

        //        // load excel, and create a new workbook
        //        var excelApp = new Microsoft.Office.Interop.Excel.Application();
        //        excelApp.Workbooks.Add();

        //        // single worksheet
        //        Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;

        //        // column headings
        //        for (var i = 0; i < tbl.Columns.Count; i++)
        //        {
        //            workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
        //        }

        //        // rows
        //        for (var i = 0; i < tbl.Rows.Count; i++)
        //        {
        //            // to do: format datetime values before printing
        //            for (var j = 0; j < tbl.Columns.Count; j++)
        //            {
        //                workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
        //            }
        //        }

        //        // check file path
        //        if (!string.IsNullOrEmpty(excelFilePath))
        //        {
        //            try
        //            {

        //                workSheet.SaveAs(excelFilePath);
        //                excelApp.Quit();
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
        //                                    + ex.Message);
        //            }
        //        }
        //        else
        //        { // no file path is given
        //            excelApp.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("ExportToExcel: \n" + ex.Message);
        //    }
        //}

        //public static object ExportToExcel<T>(this List<T> lst, string excelFilePath = null)
        //{
        //    DataTable tbl = lst.ToDataTable<T>();
            
        //    try
        //    {
        //        if (tbl == null || tbl.Columns.Count == 0)
        //            throw new Exception("ExportToExcel: Null or empty input table!\n");

        //        // load excel, and create a new workbook
        //        var excelApp = new Microsoft.Office.Interop.Excel.Application();
        //        excelApp.Workbooks.Add();
        //        Microsoft.Office.Interop.Excel._Worksheet workSheet = excelApp.ActiveSheet;
        //        // single worksheet


        //        // column headings
        //        for (var i = 0; i < tbl.Columns.Count; i++)
        //        {
        //            workSheet.Cells[1, i + 1] = tbl.Columns[i].ColumnName;
        //        }

        //        // rows
        //        for (var i = 0; i < tbl.Rows.Count; i++)
        //        {
        //            // to do: format datetime values before printing
        //            for (var j = 0; j < tbl.Columns.Count; j++)
        //            {
        //                workSheet.Cells[i + 2, j + 1] = tbl.Rows[i][j];
        //            }
        //        }

        //        // check file path
        //        if (!string.IsNullOrEmpty(excelFilePath))
        //        {
        //            try
        //            {

                        
        //                workSheet.SaveAs(excelFilePath);
        //                excelApp.Quit();
                        
        //            }
        //            catch (Exception ex)
        //            {
        //                throw new Exception("ExportToExcel: Excel file could not be saved! Check filepath.\n"
        //                                    + ex.Message);
        //            }
        //        }
        //        else
        //        { // no file path is given
        //            //return workSheet;
        //            excelApp.Visible = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("ExportToExcel: \n" + ex.Message);
        //    }
        //    return false;
        //}
       
    }
}
