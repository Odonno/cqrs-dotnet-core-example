# A naive introduction to CQRS in C#

### A quick intro

CQRS for Command and Query Responsibility Segregation is a pattern used to separate the logic between commands and queries.

Well, if you are used to create HTTP web API, here is the translation:

* Queries = GET methods
* Commands = POST/PUT/DELETE methods

![CQRS pattern described by Martin Fowler](https://martinfowler.com/bliki/images/cqrs/cqrs.png)

So why CQRS? Of course, you have the Single Responsibility Principle by design and so you get the ability to design a loosely coupled architecture which has multiple benefits:

* A clear Read model with a list of queries and domain objects you can use
* An isolation of each command inside a Write model
* A simple definition of each query and command
* Logging queries and commands
* Being ready to optimize the Read model or the Command model at any time (considering the underlying database)
* And also writing new queries/commands without breaking previous changes 

### The naive example

In this example, we will create a Parking system using .NET Core and a single SQL Server database.

If you want to see the code in details, please check the following repository: https://github.com/Odonno/cqrs-dotnet-core

#### A command

First, create a list of commands that will be used in your system. 

```cs
public class OpenParkingCommand
{
    public string ParkingName { get; set; }
}
```

Now, when you receive the action to open a parking, you set the command and handle it inside your own system, which can looks like this:

```cs
public void Handle(OpenParkingCommand command)
{
    var parking = _dbContext.Set<Models.Parking>()
        .FirstOrDefault(p => p.Name == command.ParkingName);

    if (parking == null)
    {
        throw new Exception($"Cannot find parking '{command.ParkingName}'.");
    }
    if (parking.IsOpened)
    {
        throw new Exception($"Parking '{command.ParkingName}' is already opened.");
    }

    parking.IsOpened = true;
    _dbContext.SaveChanges();

    _commandStoreService.Push(command);
}
```

#### A query

Same for queries, list all queries that will be used in your system.

```cs
public class GetParkingInfoQuery
{
    public string ParkingName { get; set; }
}
```

And handle those queries inside a query handler:

```cs
public ParkingInfo Handle(GetParkingInfoQuery query)
{
    var parking = _dbContext.Set<Models.Parking>()
        .Include(p => p.Places)
        .FirstOrDefault(p => p.Name == query.ParkingName);

    if (parking == null)
    {
        throw new Exception($"Cannot find parking '{query.ParkingName}'.");
    }

    return new ParkingInfo
    {
        Name = parking.Name,
        IsOpened = parking.IsOpened,
        MaximumPlaces = parking.Places.Count,
        AvailablePlaces = 
            parking.IsOpened 
                ? parking.Places.Where(pp => pp.IsFree).Count()
                : 0
    };
}
```

### Beyond CQRS

#### GraphQL

If you are familiar with GraphQL, you may know that it implements CQRS by design:

* Query = Read Model
* Mutation = Write Model

#### Command Sourcing

Once you have a working CQRS architecture, you can persist every command executed in your application inside a database called a Command Store.

A Command Store is pretty much a logging system where you can retrieve every change made in your system.

Because it is like a logging system, you can design your Command Store as a `push only` database/collection. And following our example, it can look like this: 

```cs
public void Push(object command)
{
    _dbContext.Set<Command>().Add(
        new Command
        {
            Type = command.GetType().Name,
            Data = JsonConvert.SerializeObject(command),
            CreatedAt = DateTime.Now,
            UserId = _authenticationService.GetUserId()
        }
    );
    _dbContext.SaveChanges();
}
```

#### Event Sourcing

Event Sourcing is a much more complex pattern designed to create a system around events where:

* Events are the single source of truth (Write Model)
* Data and services are the result of these events (Read Model)

I will not explain this pattern here but it has a strong relationship with CQRS in a way it separates the events (Write Model) from the queries (Read Model).