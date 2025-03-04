using MoreMountains.NiceVibrations;
using Utilities;

namespace Events
{
    #region StateChange

    public readonly struct
        SignalChangeState : ISignalChangeSplineSpeed, ISignalChangeAnimation
    {
        public float SplineSpeed { get; }
        public string Animation { get; }


        public SignalChangeState(float splineSpeed, string animation)
        {
            (SplineSpeed, Animation) = (splineSpeed, animation);
        }
    }


    public interface ISignalChangeAnimation
    {
        string Animation { get; }
    }

    public interface ISignalChangeSplineSpeed
    {
        float SplineSpeed { get; }
    }

    #endregion

    public readonly struct SignalSaveLevel
    {
    }

    public interface ISignalPlayHaptic
    {
        public HapticTypes HapticType { get; }
    }

    public interface ISignalPlaySound
    {
        public AudioAndHapticManager.AudioType AudioType { get; }
    }

}