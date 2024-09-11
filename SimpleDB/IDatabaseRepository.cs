namespace SimpleDB;

internal interface IDatabaseRepository<T>
{
    internal IEnumerable<T> Read(int? limit = null);
    internal void Store(T record);
}