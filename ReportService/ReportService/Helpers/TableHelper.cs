using Microsoft.AspNetCore.Html;
using System.Data;
using System.Linq;

namespace ReportService.Helpers
{
    /// <summary>
    /// TODO: Подумать, стоит ли переносить в MyUtils
    /// </summary>
    public static class TableHelper
    {
        public static HtmlString DataTableToHtmlTable(DataTable dt)
        {
            string html = "<table cellspacing='2' border='1' cellpadding='5'>";

            // добавим строку-заголовок
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            }

            html += "</tr>";

            // добавим основные строки
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                }

                html += "</tr>";
            }

            html += "</table>";

            return new HtmlString(html);
        }
    }
}
