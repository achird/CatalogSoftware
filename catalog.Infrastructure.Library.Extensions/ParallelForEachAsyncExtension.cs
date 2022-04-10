using System.Collections.Concurrent;

namespace catalog.Infrastructure.Library.Extensions;

/// <summary>
/// Параллельное выполнение делегата по коллекции элементов с разбивкой на пакеты
/// </summary>
public static class ParallelForEachAsyncExtension
{
    /// <summary>
    /// Параллельное выполнение делегата по коллекции элементов с разбивкой на пакеты
    /// </summary>
    public static Task ParallelForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> func, int maxDoP = 4)
    {
        async Task AwaitPartition(IEnumerator<T> partition)
        {
            using (partition)
            {
                while (partition.MoveNext())
                    await func(partition.Current);
            }
        }

        return Task.WhenAll(
            Partitioner
                .Create(source)
                .GetPartitions(maxDoP)
                .AsParallel()
                .Select(p => AwaitPartition(p)));
    }
}
