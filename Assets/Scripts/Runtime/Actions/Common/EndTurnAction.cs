namespace ProjectGame.Actions
{
    public class EndTurnAction : Action
    {
        private readonly TurnManager _turnManager;

        public EndTurnAction(TurnManager turnManager)
        {
            _turnManager = turnManager;
        }

        public override void OnStart()
        {
            _turnManager.EndTurn();
            Done();
        }

        public override void Tick() { }
    }
}
