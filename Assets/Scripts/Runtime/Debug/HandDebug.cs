using ProjectGame.Cards;
using TMPro;
using UnityEngine;

namespace ProjectGame
{
    public class HandDebug : MonoBehaviour
    {
        [SerializeField] private HandView _handView;
        [SerializeField] private TextMeshProUGUI _hoveredText;
        [SerializeField] private TextMeshProUGUI _draggedText;

        private void Update()
        {
            _hoveredText.SetText($"Hovered: {_handView.Hovered?.Name ?? "None"}");
            _draggedText.SetText($"Dragged: {_handView.Dragged?.Name ?? "None"}");
        }
    }
}
