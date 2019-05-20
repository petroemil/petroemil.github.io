After getting familiar with the various techniques in Service Fabric to build scalable and realiable services, the next step is to talk about Stateful Services.

In a traditional webservice the service layer is stateless and the data is stored externally in some sort of database, like an SQL Server or CosmosDB. This inherently adds an extra bit of latency to the queries as they have to pass through the services and eventually get passed further down the pipe to the database. This means our service layer can be stateless and will be easily scalable but only as long as the database won't start being a bottleneck.

Of course, as I mentioned in the previous articles, with the right partitioning strategy we can split the data access through partitions and build partitioned cache within the services to quickly process requests.
But in case, you're building a data-heavy system and the data access is still a bottleneck to the overall performance, you have an extra option to consider: Stateful Services.