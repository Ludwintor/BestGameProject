using ProjectGame.Intents;
using UnityEngine;

namespace ProjectGame.Characters
{
    public class EnemyView : CharacterView
    {
        public Enemy Enemy => (Enemy)Character;

        [SerializeField] private IntentView _intentView;

        protected override void OnDestroy()
        {
            if (Enemy != null)
                Enemy.IntentDetermined -= ShowIntent;
            base.OnDestroy();
        }

        public void Init(Enemy enemy)
        {
            base.Init(enemy);
            Enemy.IntentDetermined += ShowIntent;
            UpdateView();
        }

        public void ShowIntent(IntentData intent)
        {
            _intentView.UpdateIntent(intent.Sprite, intent.HasCounter ? intent.GetValue(Enemy).ToString() : "");
            _intentView.Show();
        }

        public void HideIntent()
        {
            _intentView.Hide();
        }

        private void UpdateView()
        {
            if (Enemy.CurrentIntent != null)
                ShowIntent(Enemy.CurrentIntent);
            else
                HideIntent();
        }
    }
}
