using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ReportService.Objects;

namespace ReportService.ReportBuiders
{
    public class MockReportBuilder : IReportBuilder
    {
        public DataTable BuildReport(ReportSetting reportSetting, IEnumerable<ConstructionObject> constructionObjects, IEnumerable<DataVersion> dataVersions)
        {
            DataTable mockDataTable = new DataTable();

            mockDataTable.Columns.Add("One", typeof(int));
            mockDataTable.Columns.Add("Two", typeof(int));
            mockDataTable.Columns.Add("Three", typeof(int));
            mockDataTable.Columns.Add("Four", typeof(int));

            mockDataTable.Rows.Add(new object[] { 1, 2, 3, 4 });
            mockDataTable.Rows.Add(new object[] { 2, 3, 4, 5 });
            mockDataTable.Rows.Add(new object[] { 3, 4, 5, 6 });
            mockDataTable.Rows.Add(new object[] { 4, 5, 6, 7 });
            mockDataTable.Rows.Add(new object[] { 5, 6, 7, 8 });

            return mockDataTable;
        }
    }
}
