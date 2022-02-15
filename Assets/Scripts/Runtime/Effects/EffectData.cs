using ProjectGame.Actions;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame.Effects
{
    public abstract class EffectData : ScriptableObject
    {
        protected ActionManager ActionManager => Game.GetSystem<ActionManager>();

        /// <summary>
        /// Выполняет текущий эффект
        /// </summary>
        public abstract void Execute(Card card, Character source, Character target);

        /// <summary>
        /// Возвращает "сырой" урон эффекта в зависимости от уровня.
        /// </summary>
        public virtual int GetDamage(int timesUpgraded) => throw new System.NotImplementedException();
        /// <summary>
        /// Возвращает "сырой" блок эффекта в зависимости от уровня.
        /// </summary>
        public virtual int GetBlock(int timesUpgraded) => throw new System.NotImplementedException();
        /// <summary>
        /// Возвращает "сырое" дополнительное значение эффекта в зависимости от уровня.
        /// </summary>
        public virtual int GetMiscValue(int timesUpgraded) => throw new System.NotImplementedException();

        /// <summary>
        /// Добавляет <see cref="Action"/> в начало очереди
        /// </summary>
        protected void AddToTop(Action action)
        {
            ActionManager.AddToTop(action);
        }

        /// <summary>
        /// Добавляет <see cref="Action"/> в конец очереди
        /// </summary>
        protected void AddToBottom(Action action)
        {
            ActionManager.AddToBottom(action);
        }
    }
}
