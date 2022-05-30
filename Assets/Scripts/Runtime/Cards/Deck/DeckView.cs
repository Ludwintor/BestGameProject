using ProjectGame.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.Cards
{
    [RequireComponent(typeof(Button))]
    public class DeckView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countText;
        [SerializeField] private float _moveDuration = 0.4f;

        private Deck _deck;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(ToggleCardsView);
        }

        public void Init(Deck deck)
        {
            _deck = deck;
            deck.View = this;
            UpdateCount(deck.Count);
        }

        public void UpdateCount(int cardsCount)
        {
            _countText.SetText(cardsCount.ToString());
        }

        public void MoveToDeck(CardView view)
        {
            view.Move(transform.localPosition, _moveDuration, () => Game.CardsPool.Release(view));
            view.Scale(0f, _moveDuration);
        }

        private void ToggleCardsView()
        {
            CardsWindow window = Game.GetSystem<UIManager>().CardsWindow;
            window.Show(_deck.Cards);
        }
    }
}
