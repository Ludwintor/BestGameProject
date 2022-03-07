using UnityEngine;

namespace ProjectGame.Actions
{
    public abstract class Action
    {
        public float Duration => _duration;
        public bool IsDone => _isDone;

        private float _duration;
        private bool _isDone;

        public Action() : this(0f) { }

        public Action(float duration)
        {
            _isDone = false;
            _duration = duration;
        }

        /// <summary>
        /// Вызывается каждый кадр через <see cref="ActionManager"/>
        /// </summary>
        public abstract void Tick();

        /// <summary>
        /// Вызывается один раз, когда достаётся из очереди в <see cref="ActionManager"/>
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// Вызывается один раз после вызова <see cref="Done"/>
        /// </summary>
        protected virtual void OnDone() { }

        /// <summary>
        /// Завершить выполнение данного <see cref="Action"/>.
        /// </summary>
        protected void Done()
        {
            if (_isDone)
                return;

            _isDone = true;
            OnDone();
        }

        /// <summary>
        /// Использовать только в <see cref="Tick"/>, если данный <see cref="Action"/> имеет заданный <see cref="Duration"/>.
        /// По истечении времени вызывает <see cref="Done"/>
        /// </summary>
        protected void TickDuration()
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0f)
                Done();
        }
    }
}
