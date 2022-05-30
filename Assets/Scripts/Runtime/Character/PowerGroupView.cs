using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGame.Powers
{
    public class PowerGroupView : MonoBehaviour
    {
        [SerializeField] private List<PowerView> _powers;
        [SerializeField] private PowerView _powerViewPrefab;

        public void Init(PowerGroup powergroup)
        {
            powergroup.PowerApplied += AddPower;
            powergroup.PowerRemoved += RemovePower;
            powergroup.PowersCleared += Clear;
        }

        private void AddPower(Power power)
        {
            PowerView view = Instantiate(_powerViewPrefab, transform);
            view.Init(power);
            _powers.Add(view);
        }

        private void RemovePower(Power power)
        {
            PowerView view = Get(power);
            _powers.Remove(view);
            Destroy(view.gameObject);
        }

        private void Clear()
        {
            foreach (PowerView view in _powers)
                Destroy(view.gameObject);
            _powers.Clear();
        }

        private PowerView Get(Power power)
        {
            foreach (PowerView view in _powers)
                if (power == view.Power)
                    return view;
            return null;
        }
    }
}
