using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleEFDbSeeder;

namespace DbSeederConsoleApp;


class Program
{
    private IServiceProvider _serviceProvider;

    public Program(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    static void Main(string[] args)
    {
        // Configura el servicio
        var serviceProvider = new ServiceCollection()
            .AddScoped<DbSeeder>()
            .AddDbContext<DbContext>(options => options.UseSqlServer("Server=tcp:localhost,1433;Initial Catalog=YourDbName;User ID=YourUserId;Password=YourPassword;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=false;"))
            .BuildServiceProvider();

        // Crea una instancia de Program y pasa el ServiceProvider
        var program = new Program(serviceProvider);

        Console.WriteLine("Type the number of the operation you would like to do?\n - SeedToTables (1) - DeleteFromTables (2)\n\n");

        var operationOption = Console.ReadLine();
        var tables = string.Empty;

        switch (operationOption)
        {
            case "1":
                Console.WriteLine("Type the tables you want to seed comma separated or type all to seed all.Here is a list of all the table names from your database:\n\n");
                program.ShowTables();
                tables = Console.ReadLine();
                program.DoWork(operationOption, tables);
                break;

            case "2":
                Console.WriteLine("Type the tables you want to delete comma separated, or tipe all for deleting all");
                program.ShowTables();
                tables = Console.ReadLine();
                program.DoWork(operationOption, tables);
                break;

            default:
                Console.WriteLine("This option does not exist");
                Console.ReadLine();
                break;
        }


    }

    private void DoWork(string operationOption, string tables)
    {
        var entities = tables.Split(",").ToList();
        var trimmedEntities = entities.Select(x => x.Trim()).ToList();

        using (var scope = _serviceProvider.CreateScope())
        {
            var dbSeeder = (DbSeeder)scope.ServiceProvider.GetRequiredService(typeof(DbSeeder));
            switch (operationOption)
            {
                case "1":
                    dbSeeder.AddSeedData(trimmedEntities);
                    break;

                case "2":
                    dbSeeder.RemoveData(trimmedEntities);
                    break;

                default:

                    break;
            }
        }

        Console.WriteLine("Completed");
    }

    private void ShowTables()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbSeeder = (DbSeeder)scope.ServiceProvider.GetRequiredService(typeof(DbSeeder));

            var tables = dbSeeder.GetTableNames();

            var tableNames = string.Join(", ", tables);

            Console.WriteLine(tableNames + "\n\n");
        }
    }
}

