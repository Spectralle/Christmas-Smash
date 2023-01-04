using System;

namespace CasualGame
{
    public class PauseManager : Singleton<PauseManager>
    {
        public static event EventHandler OnPaused;
        public static event EventHandler OnResumed;
        
        private bool _isPaused;


        public static void Pause()
        {
            Instance._isPaused = true;
            OnPaused?.Invoke(Instance, EventArgs.Empty);
        }

        public static void Resume()
        {
            Instance._isPaused = false;
            OnResumed?.Invoke(Instance, EventArgs.Empty);
        }

        public static bool IsPaused() => Instance._isPaused;
    }
}