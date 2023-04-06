using System;

namespace DefaultNamespace
{
    public interface IUITimerProvider
    {
        public event Action<float> OnTimeChanged;
    }
}