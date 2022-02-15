using ProjectGame.Actions;
using System.Collections.Generic;
using ProjectGame.Utils;
using UnityEngine;

namespace ProjectGame
{
    public class ActionManager : MonoBehaviour, ISystem
    {
        private const int DEFAULT_QUEUE_CAPACITY = 5;

        private List<Action> _actionQueue = new List<Action>(DEFAULT_QUEUE_CAPACITY);

        private Action _currentAction;

        private void Awake()
        {
            Game.RegisterSystem(this);
        }

        private void Update()
        {
            if (_currentAction != null && !_currentAction.IsDone)
            {
                _currentAction.Tick();
            }
            else if (_actionQueue.Count > 0)
            {
                _currentAction = NextAction();
                _currentAction.OnStart();
            }
        }

        /// <summary>
        /// Добавляет <see cref="Action"/> в конец очереди
        /// </summary>
        public void AddToBottom(Action action)
        {
            _actionQueue.Add(action);
        }

        /// <summary>
        /// Добавляет <see cref="Action"/> в начало очереди
        /// </summary>
        public void AddToTop(Action action)
        {
            if (_actionQueue.Count == 0)
                _actionQueue.Add(action);
            else
                _actionQueue.Insert(0, action);
        }

        /// <summary>
        /// Удаляет следующий <see cref="Action"/> и возвращает его
        /// </summary>
        private Action NextAction()
        {
            Action action = _actionQueue.Remove(0);
            return action;
        }
    }
}
