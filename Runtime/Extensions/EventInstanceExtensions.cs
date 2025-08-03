using System.Runtime.CompilerServices;
using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Summoner.Fmod.Extensions
{
    public static class EventInstanceExtensions
    {
        public static void SetVolume(this EventInstance eventInstance, float volume)
        {
            eventInstance.setVolume(volume);
        }

        public static void ResetVolume(this EventInstance eventInstance)
        {
            eventInstance.setVolume(1);
        }
        
        public static void AttachToTransform(this EventInstance eventInstance, Transform transform)
        {
            RuntimeManager.AttachInstanceToGameObject(eventInstance, transform);
        }

        public static void SetParameter(this EventInstance eventInstance, string parameterName, float value)
        {
            eventInstance.setParameterByName(parameterName, value);
        }
        
        public static bool IsOneShot(this EventInstance eventInstance)
        {
            eventInstance.getDescription(out EventDescription description);
            description.isOneshot(out bool isOneShot);
            return isOneShot;
        }
        
        public static bool IsSameEvent(this EventInstance eventInstance, EventReference eventReference)
        {
            if (!eventInstance.isValid() || eventReference.IsNull) return false;
            
            eventInstance.getDescription(out EventDescription eventDescription);
            eventDescription.getID(out GUID eventInstanceGuid);

            return eventInstanceGuid == eventReference.Guid;
        }

        public static bool IsUnloading(this EventInstance eventInstance) => GetLoadingState(eventInstance) == LOADING_STATE.UNLOADING;
        public static bool IsUnloaded(this EventInstance eventInstance) => GetLoadingState(eventInstance) == LOADING_STATE.UNLOADED;
        public static bool IsLoading(this EventInstance eventInstance) => GetLoadingState(eventInstance) == LOADING_STATE.LOADING;
        public static bool IsLoaded(this EventInstance eventInstance) => GetLoadingState(eventInstance) == LOADING_STATE.LOADED;
        public static bool IsInErrorLoadingState(this EventInstance eventInstance) => GetLoadingState(eventInstance) == LOADING_STATE.ERROR;
        private static LOADING_STATE GetLoadingState(EventInstance eventInstance)
        {
            eventInstance.getDescription(out EventDescription eventDescription);
            eventDescription.getSampleLoadingState(out LOADING_STATE state);
            return state;
        }

        public static bool IsInPlaybackState(this EventInstance eventInstance, PLAYBACK_STATE playbackState)
        {
            eventInstance.getPlaybackState(out PLAYBACK_STATE state);
            return state == playbackState;
        }

        public static float GetVolume(this EventInstance eventInstance)
        {
            eventInstance.getVolume(out float volume);
            return volume;
        }
        
        public static float GetPitch(this EventInstance eventInstance)
        {
            eventInstance.getPitch(out float pitch);
            return pitch;
        }

        // ReSharper disable once InconsistentNaming
        public static Tween DOParameter(this EventInstance eventInstance, string parameterName, float endValue, float duration)
        {
            return DOTween.To(() =>
            {
                eventInstance.getParameterByName(parameterName, out float value);
                return value;
            }, x => eventInstance.setParameterByName(parameterName, x), endValue, duration);
        }
        
        public static void Stop(this EventInstance eventInstance, FMODStopFlags fmodStopFlags = 0)
        {
            eventInstance.stop(HasStopFlags(FMODStopFlags.Immediate, fmodStopFlags) ? STOP_MODE.IMMEDIATE : STOP_MODE.ALLOWFADEOUT);

            if (HasStopFlags(FMODStopFlags.Release, fmodStopFlags))
                eventInstance.release();

            if (HasStopFlags(FMODStopFlags.ClearHandle, fmodStopFlags))
                eventInstance.clearHandle();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasStopFlags(this FMODStopFlags flags1, FMODStopFlags flags2) => (flags1 & flags2) != 0;
    }
}