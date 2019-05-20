After getting familiar with the various techniques in Service Fabric to build scalable and realiable services, the next step is to talk about Stateful Services.

In a traditional webservice the service layer is stateless and the data is stored externally in some sort of database, like an SQL Server or CosmosDB. This inherently adds an extra bit of latency to the queries as they have to pass through the services and eventually get passed further down the pipe to the database. This means our service layer can be stateless and will be easily scalable but only as long as the database won't start being a bottleneck.

Of course, as I mentioned in the previous articles, with the right partitioning strategy we can split the data access through partitions and build partitioned cache within the services to quickly process requests.
But in case, you're building a data-heavy system and the data access is still a bottleneck to the overall performance, you have an extra option to consider: Stateful Services.

With Stateful Services you have the ability to have so called data locality, meaning, the (persistent) data is stored on the same machines where the services are being hosted, giving them a nice boost in data access performance both in terms of bandwidth and latency.

# Replicas

When you are using Stateful Services, all of your partitions will have a certain number of replicas. These replicas are not extra instances to balance the load to your service but are there to deal with data persistence and replication to ensure reliable persistence of your data. Also, if the primary (active) replica of your service goes down, it won't be restarted at a random node, but instead one of it's secondary (passive) replicas will become the new primary (active) replica... and the lost replica will be rebuilt on another node as a secondary (passive) replica.

This ensures reliable persistence of your data, it's always going to be available usually at least on 3 nodes, and also your service will almost always have all of its data locally accessible without the need to go over the wire.

# Reliable Collections

There are two APIs available to interact with this reliable storage system: **Reliable Dictionary** and **Reliable Queue**.

They both work exactly as you’d expect a Dictionary (key-value pair) or Queue (FIFO) to work but with the addition of being reliably persisted and accessing them transactionally.

You can store strongly typed data that will be serialized with traditional serialisation techniques (DataContractSerializer or custom IStateSerializer), but you can also just store a byte[] array if that’s what suits your needs, which is pretty much the equivalent of storing a file, but in this case you are not storing in a folder hierarchy, but in a dictionary instead – where you could still have a folder hierarchy like key for your entries.

## Transactions

With Reliable Collections that can be accessed from multiple nodes at the same time, we get transactions to guarantee the consistency of the persisted collections. You can have as many collections as you want, in any combination and can still guarantee transactionality for the overall operation potentially touching multiple collections.

Again, they work as you’d expect from a transaction, you open a transaction, associate it with all the actions you take on the various collections and then commit or roll back the changes.

---

# Actors

For the last article of this introductory series, I left the Reliable Actors.

We started this journey from the most simple “monolithic” Console Apps and built it all the way up to Reliable and Stateful Services. While from an infrastructure perspective we already maxed out Service Fabric’s offerings, there’s one more step we can take to get the most out of our distributed system: adopt the Actor programming model.