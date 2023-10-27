# DbSeeder
This is a command console application to seed your db with .net and ef. It contains 2 operations: Add data and Remove data.

- When selecting adding data -> It will show you all the tables that you will have in your db. You can pass all the table names that you want to seed , separated. Example: (Languages, Currencies, Countries) or type all and it will seed all of them.
- When selecting removing data -> It will show you all the tables that you will have in your db. You can pass all the table names that you want to remove data from , separated. Example: (Languages, Currencies, Countries) or type all and it will empty all of them.

The data you want to seed from is contained in the static class SampleData. I left as example Currencies, Countries and Languages because i consider them useful. But just add your custom ones that match your entities.

Remember to add your dbContext connection string in Program.cs and also change the default EF dbContext for your custom dbContext, for example ApplicationDbContext if needed.

I hope that you will find it useful and keep creating!
