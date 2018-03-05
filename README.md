[![Build status](https://ci.appveyor.com/api/projects/status/8idf8kwnvaiudnig?svg=true)](https://ci.appveyor.com/project/testpossessed/sqlrepo)

# SqlRepo
We are please to announce the release of 2.0.0 of SqlRepo, our implementation of the Repository Pattern that allows you to build and execute CRUD SQL statements using Lambda Expressions and strong typing.  This release includes significant performance improvements and some bug fixes as well as one breaking change.
## Breaking Change!!!
We have bumped the major version number because v2 has removed support for passing connection information to the Go method of Statements.  You must now configure a default implementation of IConnectionProvider in your bootstrap code.  For details see our updated Getting Started guides or [Connection Providers](https://github.com/sqlrepo/sqlrepo/wiki/Connection-Providers).


[Read the documentation on our Wiki](https://github.com/sqlrepo/sqlrepo/wiki)

or get started using one of our guides

[Getting Started (IoC)](https://github.com/sqlrepo/sqlrepo/wiki/Getting-Started-IoC)
[Getting Started (Static Factory)](https://github.com/sqlrepo/sqlrepo/wiki/Getting-Started-Static-Factory)

## Features
* Compatible with .NET Standard 2.0 and .NET Framework 4.7
* Intuitively build SQL statements using C# Lambda Expressions
* Map SQL query results to plain old CLR objects
* Low memory footprint and [high performance](https://github.com/SqlRepo/Benchmarks)
* Almost 100% unit test coverage

## Installation

**NuGet Package Manager**
```
Install-Package SqlRepo.SqlServer
```

**dotnet cli**
```
dotnet add package SqlRepo.SqlServer
```

## Example
```csharp

public class GettingStarted
{
    private IRepositoryFactory repositoryFactory;

    public GettingStarted(IRepositoryFactory repositoryFactory)
    {
        this.repositoryFactory = repositoryFactory;
    }

    public void DoIt()
    {
         var repository = this.repositoryFactory.Create<ToDo>();
         var results = repository.Query()
         .Select(e => e.Id, e => e.Task, e => e.CreatedDate)
         .Where(e => e.IsCompleted == false)
         .Go();
    }
}

```
Generates the following SQL statement and maps the results back to the list of ToDo objects.

```sql

SELECT [dbo].[ToDo].[Id], [dbo].[ToDo].[Task], [dbo].[ToDo].[CreatedDate]
FROM [dbo].[ToDo]
WHERE [dbo].[ToDo].[IsCompleted] = 0;

```
