using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using QASite.Data;
using System.IO;

public class QAContextFactory : IDesignTimeDbContextFactory<QASiteContext>
{
    public QASiteContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}QASite.Web"))
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

        return new QASiteContext(config.GetConnectionString("ConStr"));
    }
}


