using ProjectGame.Cards;
using ProjectGame.Characters;

namespace ProjectGame.Actions
{
    public class ShuffleEmptyDeckAction : Action
    {
        private readonly Player _player;

        public ShuffleEmptyDeckAction(Player player)
        {
            _player = player;
        }

        public override void OnStart()
        {
            _player.DiscardDeck.Shuffle();
            while (_player.DiscardDeck.Count > 0)
            {
                Card card = _player.DiscardDeck.TakeFromTop();
                _player.DrawDeck.Add(card);
            }
            Done();
        }

        public override void Tick() { }
    }
}
