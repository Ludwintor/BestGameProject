using ProjectGame.Intents;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class EnemyView : CharacterView
    {
        [SerializeField] private IntentView _intentView;

        private Enemy _enemy;

        public void Init(Enemy enemy)
        {
            _enemy = enemy;
            base.Init(enemy);
            _enemy.IntentDetermined += ShowIntent;
            UpdateView();
        }

        public void ShowIntent(IntentData intent)
        {
            _intentView.UpdateIntent(intent.Sprite, intent.HasCounter ? intent.GetValue(_enemy).ToString() : "");
            _intentView.Show();
        }

        public void HideIntent()
        {
            _intentView.Hide();
        }

        private void UpdateView()
        {
            if (_enemy.CurrentIntent != null)
                ShowIntent(_enemy.CurrentIntent);
            else
                HideIntent();
        }
    }
}
