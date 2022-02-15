using ProjectGame.Actions;
using ProjectGame.Cards;
using UnityEngine;

namespace ProjectGame.Effects
{
    public abstract class EffectData : ScriptableObject
    {
        protected ActionManager ActionManager => Game.GetSystem<ActionManager>();

        /// <summary>
        /// ��������� ������� ������
        /// </summary>
        public abstract void Execute(Card card, Character source, Character target);

        /// <summary>
        /// ���������� "�����" ���� ������� � ����������� �� ������.
        /// </summary>
        public virtual int GetDamage(int timesUpgraded) => throw new System.NotImplementedException();
        /// <summary>
        /// ���������� "�����" ���� ������� � ����������� �� ������.
        /// </summary>
        public virtual int GetBlock(int timesUpgraded) => throw new System.NotImplementedException();
        /// <summary>
        /// ���������� "�����" �������������� �������� ������� � ����������� �� ������.
        /// </summary>
        public virtual int GetMiscValue(int timesUpgraded) => throw new System.NotImplementedException();

        /// <summary>
        /// ��������� <see cref="Action"/> � ������ �������
        /// </summary>
        protected void AddToTop(Action action)
        {
            ActionManager.AddToTop(action);
        }

        /// <summary>
        /// ��������� <see cref="Action"/> � ����� �������
        /// </summary>
        protected void AddToBottom(Action action)
        {
            ActionManager.AddToBottom(action);
        }
    }
}
