namespace ProjectGame.Characters
{
    public class Enemy : Character
    {
        public EnemyView View { get; }

        public Enemy(EnemyView view)
        {
            View = view;
            View.Init(this);
        }
    }
}
