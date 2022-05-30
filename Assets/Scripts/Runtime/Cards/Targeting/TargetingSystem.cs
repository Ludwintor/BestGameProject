using ProjectGame.Characters;
using System;
using System.Collections;
using UnityEngine;

namespace ProjectGame.Cards
{
    public class TargetingSystem : MonoBehaviour
    {
        public event Action<Card, Character> TargetSelected;
        public event Action<Card> TargetingAborted;
        public bool IsTargeting => _isTargeting;

        private bool _isTargeting;
        private CardView _cardView;
        private Character _currentTarget;
        private TargetArrow _targetArrow;
        private UIManager _uiManager;

        private void Start()
        {
            _targetArrow = GetComponentInChildren<TargetArrow>();
            _uiManager = Game.GetSystem<UIManager>();
        }

        public void BeginTargeting(CardView cardView)
        {
            _cardView = cardView;
            cardView.StopDrag();
            _isTargeting = true;
            if (_cardView.Card.NeedTarget)
            {
                _targetArrow.Show();
                cardView.Interactable = false;
                cardView.CanRegainInteraction = false;
                cardView.Move(transform.localPosition, 0.2f);
            }
            StartCoroutine(Targeting());
        }

        public void StopTargeting()
        {
            _targetArrow.Hide();
            _cardView = null;
            _currentTarget = null;
            _isTargeting = false;
        }

        private IEnumerator Targeting()
        {
            while (_isTargeting)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    TargetingAborted?.Invoke(_cardView.Card);
                    StopTargeting();
                    break;
                }
                if (_cardView.Card.NeedTarget)
                {
                    _targetArrow.UpdateArrow(transform.position, _uiManager.ScreenToWorld(Input.mousePosition));
                    _currentTarget = RaycastTarget();
                    if (_currentTarget == null)
                    {
                        yield return null;
                        continue;
                    }
                }
                else
                {
                    _cardView.transform.position = _uiManager.ScreenToWorld(Input.mousePosition);
                }
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
                {
                    TargetSelected?.Invoke(_cardView.Card, _currentTarget);
                }
                yield return null;
            }
        }

        private Character RaycastTarget()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider != null && hit.collider.TryGetComponent(out Hitbox hitbox))
                return hitbox.Owner;
            return null;
        }
    }
}
