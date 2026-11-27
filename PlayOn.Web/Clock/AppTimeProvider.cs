using Microsoft.Extensions.Options;

namespace PlayOn.Web.Clock
{

    public sealed class AppTimeProvider : TimeProvider
    {
        public const string ServiceKey = "6defcfbb-0570-4750-ab9f-cffb0221952e";

        private readonly TimeProvider realTimeProvider;
        private readonly ClockOptions initial;
        private readonly TimeZoneInfo timeZoneInfo;
        private readonly object lockObject = new();

        private TimeSpan offset;
        private DateTimeOffset? frozenUtc;

        public AppTimeProvider(IOptions<ClockOptions> options)
            : this(options.Value, System)
        {
        }

        internal AppTimeProvider(ClockOptions options, TimeProvider realTime)
        {
            initial = options;
            realTimeProvider = realTime;
            timeZoneInfo = string.IsNullOrWhiteSpace(options.TimeZoneId)
                ? TimeZoneInfo.Local
                : TimeZoneInfo.FindSystemTimeZoneById(options.TimeZoneId);

            ApplyInitial();
        }

        public override DateTimeOffset GetUtcNow()
        {
            lock (lockObject)
            {
                return frozenUtc ?? realTimeProvider.GetUtcNow() + offset;
            }
        }

        public override TimeZoneInfo LocalTimeZone => timeZoneInfo;


        public bool IsFrozen
        {
            get
            {
                lock (lockObject)
                {
                    return frozenUtc.HasValue;
                }
            }
        }


        public void SetNow(DateTimeOffset value)
        {
            lock (lockObject)
            {
                if (frozenUtc.HasValue)
                    frozenUtc = value.ToUniversalTime();
                else
                    offset = value.ToUniversalTime() - realTimeProvider.GetUtcNow();
            }
        }

        public void SetLocal(DateTime wallClock)
        {
            var unspecified = DateTime.SpecifyKind(wallClock, DateTimeKind.Unspecified);
            var utc = TimeZoneInfo.ConvertTimeToUtc(unspecified, timeZoneInfo);
            SetNow(new DateTimeOffset(utc, TimeSpan.Zero));
        }

        public void Advance(TimeSpan delta)
        {
            lock (lockObject)
            {
                if (frozenUtc.HasValue)
                    frozenUtc += delta;
                else
                    offset += delta;
            }
        }

        public void Freeze()
        {
            lock (lockObject)
            {
                frozenUtc ??= realTimeProvider.GetUtcNow() + offset;
            }
        }

        public void Resume()
        {
            lock (lockObject)
            {
                if (frozenUtc.HasValue)
                {
                    offset = frozenUtc.Value - realTimeProvider.GetUtcNow();
                    frozenUtc = null;
                }
            }
        }

        public void Reset()
        {
            lock (lockObject)
            {
                ApplyInitial();
            }
        }

        private void ApplyInitial()
        {
            offset = TimeSpan.Zero;
            frozenUtc = null;

            if (initial.Mode == ClockMode.System)
            {
                return;
            }
            if (initial.StartAt is null)
            {
                throw new InvalidOperationException("Clock:StartAt is required when Clock:Mode is Fixed or Offset.");
            }
            var wall = DateTime.SpecifyKind(initial.StartAt.Value, DateTimeKind.Unspecified);
            var start = new DateTimeOffset(TimeZoneInfo.ConvertTimeToUtc(wall, timeZoneInfo), TimeSpan.Zero);

            if (initial.Mode == ClockMode.Fixed)
            {
                frozenUtc = start;
            }
            else
            {
                offset = start - realTimeProvider.GetUtcNow();
            }
        }
    }
}

