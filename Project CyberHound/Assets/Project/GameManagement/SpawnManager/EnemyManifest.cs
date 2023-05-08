using System.Collections.Generic;
using Project.Game_Entities.Enemies;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy Manifest", fileName = "New Manifest")]
public class EnemyManifest : ScriptableObject
{
    public List<EnemyController> enemies = new ();
}