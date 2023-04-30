using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PathPoint : MonoBehaviour
{
    [SerializeField] public PathPoint nextPoint;

    public PathPoint GetNext() => nextPoint;
    public Vector3 GetPoint() => transform.position;

    protected virtual void OnDrawGizmos()
    {
        var position = transform.position;

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(position, .5f);
        
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

    [MenuItem("PathPoint/CreateNewPathPointFromSelected ^g")]
    public static void CreateNewPathPointFromSelected(MenuCommand menuCommand)
    {
        Debug.Log("Creating Path Points");
        var newPoints = new List<GameObject>();
        foreach (var selected in Selection.gameObjects)
        {
            if(selected.TryGetComponent(out PathPoint pathPoint) == false) continue;

            var newPoint = new GameObject(selected.name.IterateSuffix());
            var pp = newPoint.AddComponent<PathPoint>();
            pp.nextPoint = pathPoint;
            newPoint.transform.SetParent(selected.transform.parent);
            newPoint.transform.position = selected.transform.position;
            newPoints.Add(newPoint);
        }

        Selection.objects = newPoints.ToArray();
    }   
    
    [MenuItem("PathPoint/CreateNewSpawnPointFromSelected ^b")]
    public static void CreateNewSpawnPointFromSelected(MenuCommand menuCommand)
    {
        Debug.Log("Creating Path Points");
        var newPoints = new List<GameObject>();
        foreach (var selected in Selection.gameObjects)
        {
            if(selected.TryGetComponent(out PathPoint pathPoint) == false) continue;

            var newPoint = new GameObject(selected.name.IterateSuffix());
            var pp = newPoint.AddComponent<SpawnPoint>();
            pp.nextPoint = pathPoint;
            newPoint.transform.SetParent(selected.transform.parent);
            newPoint.transform.position = selected.transform.position;
            newPoints.Add(newPoint);
        }

        Selection.objects = newPoints.ToArray();
    }
}

public static partial class Utilities
{
    public static string IterateSuffix(this string str)
    {
        var sub = str.Split('_');
        var last = sub[^1];
        if (int.TryParse(last, out var n))
        {
            sub[^1] = (n + 1).ToString();
            str = string.Join("_",sub);
        }

        return str;
    }
}