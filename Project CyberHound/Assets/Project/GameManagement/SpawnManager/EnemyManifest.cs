using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy Manifest", fileName = "New Manifest")]
public class EnemyManifest : ScriptableObject
{
    public List<EnemyController> enemies = new ();
}