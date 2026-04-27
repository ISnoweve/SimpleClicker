using Sirenix.OdinInspector;
using UnityEngine;

public class HitObjectSpawner : MonoBehaviour
{
    [Title("Spawn Settings")]
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10, 5, 2);
    [SerializeField] private Color gizmoColor = Color.cyan;
    public Vector3 GetRandomPosition()
    {
        Vector3 center = transform.position;
        return new Vector3(
            Random.Range(center.x - spawnAreaSize.x / 2f, center.x + spawnAreaSize.x / 2f),
            Random.Range(center.y - spawnAreaSize.y / 2f, center.y + spawnAreaSize.y / 2f),
            Random.Range(center.z - spawnAreaSize.z / 2f, center.z + spawnAreaSize.z / 2f)
        );
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}