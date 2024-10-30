using System;
using System.Collections.Generic;
using System.Linq;
using static Program;

internal class Program
{
    public interface IReport
    {
        string Generate();
    }
    public class SalesReport : IReport
    {
        public string Generate()
        {
            return "SalesReport";
        }
    }
    public class UserReport : IReport
    {
        public string Generate()
        {
            return "UserReport";
        }
    }

    public abstract class ReportDecorator : IReport
    {
        private IReport report;

        protected ReportDecorator(IReport report)
        {
            this.report = report;
        }

        public virtual string Generate()
        {
            return report.Generate();
        }
    }

    public class DateFilterDecorator : ReportDecorator
    {
        private DateTime startDate;
        private DateTime endDate;

        public DateFilterDecorator(IReport report, DateTime startDate, DateTime endDate) : base(report)
        {
            this.startDate = startDate;
            this.endDate = endDate;

        }
        public override string Generate()
        {
            var data = base.Generate();
            return "Filter from " + startDate + " " + data;
        }
    }
    public class SortingDecorator : ReportDecorator
    {
        private string sortBy;

        public SortingDecorator(IReport report, string sortBy) : base(report)
        {
            this.sortBy = sortBy;
        }

        public override string Generate()
        {
            return $"Sorted by {sortBy}: {base.Generate()}";
        }
    }

    public class CsvExportDecorator : ReportDecorator
    {
        public CsvExportDecorator(IReport report) : base(report) { }

        public override string Generate()
        {
            return base.Generate() + " (Exported as CSV)";
        }
    }

    public class PdfExportDecorator : ReportDecorator
    {
        public PdfExportDecorator(IReport report) : base(report) { }

        public override string Generate()
        {
            return base.Generate() + " (Exported as PDF)";
        }
    }

    static void Main(string[] args)
    {
        IReport report = new SalesReport();

        report = new DateFilterDecorator(report, DateTime.Now.AddYears(-1), DateTime.Now);
        report = new SortingDecorator(report, "Date");
        report = new CsvExportDecorator(report);
        report = new PdfExportDecorator(report);
        Console.WriteLine(report.Generate());
    }
}





