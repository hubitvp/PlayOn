namespace PlayOn.Web.Clock
{

    public enum ClockMode
    {
        /// <summary>Real time.</summary>
        System,

        /// <summary>Frozen at StartAt until you advance / resume it.</summary>
        Fixed,

        /// <summary>Starts at StartAt, then keeps ticking like a real clock.</summary>
        Offset
    }
}
