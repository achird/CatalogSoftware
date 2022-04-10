namespace catalog.Infrastructure.Library.Extensions;

public class RWLock : IDisposable
{
    public struct WriteLockToken : IDisposable
    {
        private readonly ReaderWriterLockSlim @lock;
        public WriteLockToken(ReaderWriterLockSlim @lock)
        {
            this.@lock = @lock;
            @lock.EnterWriteLock();
        }
        public void Dispose() => @lock.ExitWriteLock();
    }
    public struct ReadLockToken : IDisposable
    {
        private readonly ReaderWriterLockSlim @lock;
        public ReadLockToken(ReaderWriterLockSlim @lock)
        {
            this.@lock = @lock;
            @lock.EnterReadLock();
        }
        public void Dispose() => @lock.ExitReadLock();
    }
    public struct UpgradeableReadLockToken : IDisposable
    {
        private readonly ReaderWriterLockSlim @lock;
        public UpgradeableReadLockToken(ReaderWriterLockSlim @lock)
        {
            this.@lock = @lock;
            @lock.EnterUpgradeableReadLock();
        }
        public void Dispose() => @lock.ExitUpgradeableReadLock();
    }

    private readonly ReaderWriterLockSlim @lock = new ReaderWriterLockSlim();

    public ReadLockToken ReadLock() => new ReadLockToken(@lock);
    public UpgradeableReadLockToken UpgradeableReadLock() => new UpgradeableReadLockToken(@lock);
    public WriteLockToken WriteLock() => new WriteLockToken(@lock);

    public void Dispose() => @lock.Dispose();
}
