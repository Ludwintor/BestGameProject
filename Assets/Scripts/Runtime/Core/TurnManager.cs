using System.Collections;
using ProjectGame.Actions;
using ProjectGame.Characters;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame
{
    public class TurnManager : MonoBehaviour, ISystem
    {
        [SerializeField] private Button _endTurnButton;

        public int Turns => _turns;

        private int _turns;
        private bool _endTurnRequested = false;

        private void Awake()
        {
            Game.RegisterSystem(this);
            _endTurnButton?.onClick.AddListener(OnEndTurnPressed);
        }

        private void Start()
        {
            Game.Dungeon.SelectNextRoom();
        }

        public void StartCombat()
        {
            _turns = 0;
            Game.Dungeon.Player.TriggerCombatStart();
            StartTurn();
        }

        public void EndCombat()
        {
            Game.Dungeon.Player.TriggerCombatEnd();
        }

        public void StartTurn()
        {
            _turns++;
            Dungeon dungeon = Game.Dungeon;
            dungeon.Player.TriggerStartTurn(_turns);
            foreach (Enemy enemy in dungeon.Enemies)
                enemy.TriggerStartTurn(_turns);
            _endTurnRequested = false;
        }

        public void EndTurn()
        {
            Dungeon dungeon = Game.Dungeon;
            ActionManager actionManager = Game.GetSystem<ActionManager>();
            dungeon.Player.TriggerEndTurn(_turns);
            foreach (Enemy enemy in dungeon.Enemies)
                if (enemy.IsAlive)
                    enemy.TriggerEndTurn(_turns); // TODO: Probably (99%) TriggerEndTurn must be called within EnemyTurnAction
            actionManager.AddToBottom(new StartTurnAction(this));
        }

        private void OnEndTurnPressed()
        {
            if (_endTurnRequested)
                return;
            ActionManager actionManager = Game.GetSystem<ActionManager>();
            actionManager.AddToBottom(new EndTurnAction(this));
            _endTurnRequested = true;
        }
    }
}
