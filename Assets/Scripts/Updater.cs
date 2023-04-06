using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Updater : MonoBehaviour
    {
        public event Action<float> OnUpdate;

        private bool _isPaused;

        public void Pause()
        {
            _isPaused = true;
        }

        public void Resume()
        {
            _isPaused = false;
        }

        private void Update()
        {
            if (_isPaused)
            {
                return;
            }

            OnUpdate?.Invoke(Time.deltaTime);
        }
    }
}