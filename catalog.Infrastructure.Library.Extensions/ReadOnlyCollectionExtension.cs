using System.Collections;

namespace catalog.Infrastructure.Library.Extensions;

public static class ReadOnlyCollectionExtension
{
    public static IReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        return source as IReadOnlyCollection<T> ?? new ReadOnlyCollectionAdapter<T>(source);
    }

    sealed class ReadOnlyCollectionAdapter<T> : IReadOnlyCollection<T>
    {
        readonly ICollection<T> source;
        public ReadOnlyCollectionAdapter(ICollection<T> source) => this.source = source;
        public int Count => source.Count;
        public IEnumerator<T> GetEnumerator() => source.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
