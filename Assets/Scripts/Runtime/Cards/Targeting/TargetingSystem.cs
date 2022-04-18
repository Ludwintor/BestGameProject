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
        private Card _card;
        private Character _currentTarget;
        private TargetArrow _targetArrow;
        private UIManager _uiManager;

        private void Start()
        {
            _targetArrow = GetComponentInChildren<TargetArrow>();
            _uiManager = Game.GetSystem<UIManager>();
        }

        public void BeginTargeting(Card card)
        {
            _card = card;
            card.View.StopDrag();
            _isTargeting = true;
            if (card.NeedTarget)
            {
                _targetArrow.Show();
                card.View.Interactable = false;
                card.View.CanRegainInteraction = false;
                card.View.Move(transform.localPosition, 0.2f);
            }
            StartCoroutine(Targeting());
        }

        public void StopTargeting()
        {
            _targetArrow.Hide();
            _card = null;
            _currentTarget = null;
            _isTargeting = false;
        }

        private IEnumerator Targeting()
        {
            while (_isTargeting)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    TargetingAborted?.Invoke(_card);
                    StopTargeting();
                    break;
                }
                if (_card.NeedTarget)
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
                    _card.View.transform.position = _uiManager.ScreenToWorld(Input.mousePosition);
                }
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
                {
                    TargetSelected?.Invoke(_card, _currentTarget);
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
