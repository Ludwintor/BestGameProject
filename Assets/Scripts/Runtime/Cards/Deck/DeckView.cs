using TMPro;
using UnityEngine;

namespace ProjectGame.Cards
{
    public class DeckView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private float _moveDuration = 0.4f;

        public void Init(Deck deck)
        {
            deck.View = this;
        }

        public void UpdateCount(int cardsCount)
        {
            _countText.SetText(cardsCount.ToString());
        }

        public void MoveToDeck(Card card)
        {
            CardView cardView = card.View;
            cardView.Move(transform.localPosition, _moveDuration, () => Game.CardsPool.Release(cardView));
            cardView.Scale(0f, _moveDuration);
        }
    }
}
