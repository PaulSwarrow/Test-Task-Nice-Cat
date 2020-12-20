namespace DefaultNamespace
{
    public interface IGameSystem
    {
        void Init(GameManager.Properties properties);
        void Start();
        void Update();
        void Stop();
    }
}