using Project.Game_Entities.Character;

namespace Project.Game_Entities.Enemies
{
    public class EnemyDetectionModule : DetectionModule
    {
        protected override bool FilterDetectedEntities(GameEntity entity)
        {
            return entity is PlayerController;
        }
    }
}