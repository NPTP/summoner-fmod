using System;

namespace Summoner.Fmod
{
    // ReSharper disable once InconsistentNaming
    [Flags]
    public enum FMODStopFlags
    {
        AllowFadeout = 1 << 0,
        Immediate = 1 << 1,
        Release = 1 << 2,
        ClearHandle = 1 << 3
    }
}
