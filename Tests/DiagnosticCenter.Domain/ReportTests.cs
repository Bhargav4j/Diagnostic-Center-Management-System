using Xunit;
using DiagnosticCenter.Domain.Entities;
using System;

namespace Tests.DiagnosticCenter.Domain;

public class ReportTests
{
    [Fact]
    public void Report_Constructor_InitializesWithDefaultValues()
    {
        var report = new Report();

        Assert.Equal(0, report.Id);
        Assert.Equal(0, report.TestEntryId);
        Assert.Equal(string.Empty, report.ResultData);
        Assert.Null(report.Notes);
        Assert.True(report.IsActive);
        Assert.Equal("System", report.CreatedBy);
        Assert.Null(report.ModifiedBy);
    }

    [Fact]
    public void Report_SetTestEntryId_UpdatesTestEntryId()
    {
        var report = new Report { TestEntryId = 1 };

        Assert.Equal(1, report.TestEntryId);
    }

    [Fact]
    public void Report_SetResultData_UpdatesResultData()
    {
        var report = new Report { ResultData = "Negative" };

        Assert.Equal("Negative", report.ResultData);
    }

    [Fact]
    public void Report_SetNotes_UpdatesNotes()
    {
        var report = new Report { Notes = "Additional notes" };

        Assert.Equal("Additional notes", report.Notes);
    }

    [Fact]
    public void Report_SetReportDate_UpdatesReportDate()
    {
        var date = DateTime.UtcNow;
        var report = new Report { ReportDate = date };

        Assert.Equal(date, report.ReportDate);
    }

    [Fact]
    public void Report_SetIsActive_UpdatesIsActive()
    {
        var report = new Report { IsActive = false };

        Assert.False(report.IsActive);
    }

    [Fact]
    public void Report_SetCreatedDate_UpdatesCreatedDate()
    {
        var date = DateTime.UtcNow.AddDays(-1);
        var report = new Report { CreatedDate = date };

        Assert.Equal(date, report.CreatedDate);
    }

    [Fact]
    public void Report_SetModifiedDate_UpdatesModifiedDate()
    {
        var date = DateTime.UtcNow;
        var report = new Report { ModifiedDate = date };

        Assert.Equal(date, report.ModifiedDate);
    }

    [Fact]
    public void Report_SetCreatedBy_UpdatesCreatedBy()
    {
        var report = new Report { CreatedBy = "Admin" };

        Assert.Equal("Admin", report.CreatedBy);
    }

    [Fact]
    public void Report_SetModifiedBy_UpdatesModifiedBy()
    {
        var report = new Report { ModifiedBy = "Technician" };

        Assert.Equal("Technician", report.ModifiedBy);
    }

    [Fact]
    public void Report_SetId_UpdatesId()
    {
        var report = new Report { Id = 1 };

        Assert.Equal(1, report.Id);
    }

    [Fact]
    public void Report_WithAllProperties_SetsCorrectly()
    {
        var reportDate = DateTime.UtcNow.AddDays(-1);
        var createdDate = DateTime.UtcNow.AddDays(-2);
        var modifiedDate = DateTime.UtcNow;

        var report = new Report
        {
            Id = 1,
            TestEntryId = 5,
            ResultData = "Positive",
            Notes = "Urgent review needed",
            ReportDate = reportDate,
            IsActive = true,
            CreatedDate = createdDate,
            ModifiedDate = modifiedDate,
            CreatedBy = "System",
            ModifiedBy = "Doctor"
        };

        Assert.Equal(1, report.Id);
        Assert.Equal(5, report.TestEntryId);
        Assert.Equal("Positive", report.ResultData);
        Assert.Equal("Urgent review needed", report.Notes);
        Assert.Equal(reportDate, report.ReportDate);
        Assert.True(report.IsActive);
        Assert.Equal(createdDate, report.CreatedDate);
        Assert.Equal(modifiedDate, report.ModifiedDate);
        Assert.Equal("System", report.CreatedBy);
        Assert.Equal("Doctor", report.ModifiedBy);
    }

    [Fact]
    public void Report_WithNullNotes_AllowsNull()
    {
        var report = new Report { Notes = null };

        Assert.Null(report.Notes);
    }

    [Fact]
    public void Report_WithNullModifiedDate_AllowsNull()
    {
        var report = new Report { ModifiedDate = null };

        Assert.Null(report.ModifiedDate);
    }

    [Fact]
    public void Report_IsActiveDefault_IsTrue()
    {
        var report = new Report();

        Assert.True(report.IsActive);
    }

    [Fact]
    public void Report_WithEmptyResultData_AllowsEmptyString()
    {
        var report = new Report { ResultData = "" };

        Assert.Equal(string.Empty, report.ResultData);
    }
}
