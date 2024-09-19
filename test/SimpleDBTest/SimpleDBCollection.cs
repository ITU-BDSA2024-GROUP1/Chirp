namespace SimpleDBTest;

[CollectionDefinition("SimpleDB Collection", DisableParallelization = true)]
public class SimpleDBCollection : ICollectionFixture<SimpleDBFixture>
{
}