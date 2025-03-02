namespace _Project.Scripts.Infrastructure.Data
{
    public class PersistentProgress
    {
        public PlayerData Data;

        public PersistentProgress()
        {
            Data = new PlayerData();
        }
        
        public PersistentProgress(PlayerData data)
        {
            Data = data;
        }
    }
}