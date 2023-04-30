using System.Linq;
using UnityEditor;
using UnityEngine;

public class SpawnPoint : PathPoint
{
    [SerializeField] private SpawnPointData data;
    public SpawnPointData GetData() => data;
    
    protected override void OnDrawGizmos()
    {
        var position = transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(position, .5f);
        if (data)
        {
            var result = int.Parse(new string(data.name.Reverse()
                .TakeWhile(char.IsDigit)
                .Reverse()
                .ToArray()));
            Handles.Label(transform.position + Vector3.up * 2, result.ToString());
        }

        var next = nextPoint;
        while (next)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(position, next.transform.position);

            position = next.transform.position;
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(position, .5f);

            next = next.nextPoint;
        }
    }

    private const string DataAssetPath = "Assets/Project/Scriptables/SpawnPoint";
    
    [MenuItem("CONTEXT/SpawnPoint/GenerateSpawnData")]
    public static void GenerateSpawnData()
    {
        foreach (SpawnPoint spawnPoint in FindObjectsOfType<SpawnPoint>())
        {
            if (spawnPoint.data != null) continue;
            
            SpawnPointData[] allSpawnPointData = AssetDatabase.FindAssets("t:SpawnPointData")
                .Select(guid => AssetDatabase.LoadAssetAtPath<SpawnPointData>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            SpawnPointData nextAvailableSpawnPointData = allSpawnPointData
                .FirstOrDefault(data => !AssetDatabase.IsMainAsset(data) && !AssetDatabase.IsSubAsset(data));

            if (nextAvailableSpawnPointData == null)
            {
                int nextSpawnPointDataNumber = allSpawnPointData.Length + 1;
                string newSpawnPointDataName = "SpawnPointData" + nextSpawnPointDataNumber.ToString();
                string newSpawnPointDataPath = DataAssetPath + "/" + newSpawnPointDataName + ".asset";
                nextAvailableSpawnPointData = ScriptableObject.CreateInstance<SpawnPointData>();
                AssetDatabase.CreateAsset(nextAvailableSpawnPointData, newSpawnPointDataPath);
                AssetDatabase.SaveAssets();
            }

            spawnPoint.data = nextAvailableSpawnPointData;
        }
    }
}