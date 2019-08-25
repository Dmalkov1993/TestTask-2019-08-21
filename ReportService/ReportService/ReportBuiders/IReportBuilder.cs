namespace ReportService.ReportBuiders
{
    using ReportService.Objects;
    using System.Collections.Generic;
    using System.Data;

    public interface IReportBuilder
    {
        DataTable BuildReport(ReportSetting reportSetting, IEnumerable<ConstructionObject> constructionObjects, IEnumerable<DataVersion> dataVersions);
    }
}
