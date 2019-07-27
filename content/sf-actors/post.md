# Actor Programming Model

The Actor programming model helps you build scalable systems whether you are optimizing for multi-core processors or distributed systems.
With the Actor programming model you are focusing on breaking up your logic by data consistency boundaries instead of functionality. What it means in practice is that instead of having a Customer Service that deals with all the requests concerning all the customers, you will have Customer Actors that will deal with individual customers. This model (and its supporting frameworks) will take away the complexities of multithreading, concurrency and synchronization, and enable you to easily achieve optimal performance and scalability.

You could imagine Actors like auto-scaled and partitioned service instances, but they are much more lightweight than a service process, they have tiny memory footprint and can be created and cleaned up very quickly on demand and thousands or even millions of them will run on across your cluster.

From another angle, you can also think of Actors as RPC on steroids. You have the interface and while you don't know the IP address of the receiver, you know its ID. The Actor framework does the resolution for you based on that ID and connects you to the right Actor instance wherever it is located in the cluster.

# Customer Service vs Actor

Before I'd go into the details of Service Fabric's Actors, I'd like to walk through a simple example of the mentioned Customer service to show the differences between traditional services and actors.

Lets assume we want to build a super simple Customer Service that has CRUD (Create, Read, Update, Delete) operations. The Customer's unique ID will be its username. The interface for the traditional service would look something like this:

```csharp
public class Customer
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Status { get; set; }
}

public interface ICustomerService
{
    Task Create(string username, string email);
    Task<Customer> Get(string username);
    Task Update(string username, string status);
    Task Delete(string username);
}

// Calling the service will look something like this
await customerService.Create("User1", "email@provider.com");
await customerService.Update("User1", "Hello World");
```

We all built webservices before, so I don't have to explain the complexities of the implementation of this seemingly simple service.
Even if this service only has a single instance, you'd have to deal with some complex locking mechanism to make sure a customer won't get `Update` and `Delete` requests at the same time, or that a `Get` call coming after an `Update` won't return outdated data. And if the service has multiple instances behind a load-balancer, these locking/synchronisation mechanisms will become significantly more complex. In most traditional service implementations push this responsibility to the database and we just accept these issues and blame then on "eventual consistency".

But there's a better way of doing things, and that is the Actor programming model.

The high level interface definition for this Customer Actor would look something like this:

```csharp
public interface ICustomerActor : IActor
{
    Task Create(string email);
    Task<Customer> Get();
    Task Update(string status);
    Task Delete();
}

// Calling the actor will look something like this
var customer = ActorProxy.Create<ICustomerActor>("User1");
await customer.Create("email@provider.com");
await customer.Update("Hello World");
```

Instead of a service trying to deal with all the customers, lets just try to build the necessary functionality scoped to a single customer. 
* With Actor frameworks you have to request a reference to the actor you want to interact with - in this example, the Customer Actor. This reference - after some routing done by the framework - will point you to the exact instance of the actor that deals with a particular user. This means that every request to a given customer will be processed by one specific Customer Actor instance, similarly to how partitioned services work - you get deterministic load balancing and won't have to worry about eventual consistency.
* All operation performed on an actor are "single threaded", meaning they will never overlap, so you won't have to worry too much about concurrency and race conditions either.
* Because we know that we only care about the consistency of the "bounded context" by an Actor, many (and I mean, MANY) actors can exist at the same time and serve requests in parallel without having to worry they'd clash, so you get pretty good scalability both in single-machine environments (just spreading across the available CPU cores) and in distributed environments (spreading across cluster nodes).

# Actors in Service Fabric

In Service Fabric the foundation for the Actor programming model is a *Partitioned Stateful Reliable Service*. 

## Lifecycle

An important aspect of Service Fabric's Actor implementation is the concept of *Virtual Actors*, meaning actors virtually exist all the time, you don't have to explicitly create or delete them, just reference and use them and the framework takes care of the rest of it for you. If an Actor doesn't exist, it will be created automatically on demand and after not using it for some time, it will get garbage collected so it won't take up precious memory space. Because of this virtual nature, Actors' state is automatically persisted, so it can be re-loaded if a new instance has to be created. This is why the Service Fabric Actor "Runtime" is a *Stateful Reliable Service*.

## Placement

Actor instances are "randomly" distributed throughout the cluster nodes. When I say "randomly", I mean they are hosted on service *partitions* based on their ID.

The partitioning is done using Range Partitions and using the full range of `Int64`.

The location of individual Actor instances are determined by their IDs. An Actor ID can be `Guid`, `String` or `Int64`. In case of a `Guid` or `String` an `Int64` hash is generated that will be used to decide which partition should host the Actor, in case of `Int64`, the partition will be a direct mapping. If you care about Actor placement and want to make sure that certain actor instances are hosted on the same node, you can guarantee this by using `Int64` IDs. This can enable much less communication overhead between the co-hosted Actor instances, but it also puts even distribution of Actors across the nodes in danger.

## Recovery

As the Service Fabric Reliable Actors are backed by *Reliable Services*, we get the exact same guarantees on the service level. If a partition goes down, Service Fabric will restart it on another cluster node.
And thanks to the concept of Virtual Actors, they are already expected to come and go, so in case an Actor dies, the next request will re-activate it.

## Scaling

While I introduced Actors as the pinnacle of scalability, there are a few things worth noting regarding Service Fabric's implementation.

In Service Fabric, Actors are hosted in *numeric range partitioned* services. If you read one of the previous articles about the different types of partitioning methods Service Fabric supports, you'll know that this one has certain limitations.

The number of partitions in a numeric range partitioned setup is fixed and must be pre-defined when deploying the service. You can have more partitions than nodes in the cluster, so you at least have a little bit of elasticity, if you add more nodes to the cluster, Service Fabric will make sure the partitions are nicely distributed across the available nodes. 

But, if you have 50 partitions and only 5 nodes in the cluster, you will have quite a bit of overhead of hosting 10 partitions per node. You also have to consider that even though that 10 partitions worth of Actors are going to be hosted on the same node, they will still need to communicate with each other through inter-process channels that includes serialization.

In an upcoming article I will write about Microsoft Orleans which is the original and much more powerful implementation of Virtual Actors.

## State

Hosting Actors in partitions might make scaling sub-optimal, but it has its benefits when it comes to state management. Because Service Fabirc Actors are hosted by *Stateful Reliable Services*, and we know that actor placement is tied to partitions, it's easy to see that storing the state of an actor is also going to be tied to a partition. What it means is that reading and writing the state of an Actor will always happen on a local disk and not over the network. It means very low latencies for state updates and very fast initialisation of new Actor instances (with existing state).