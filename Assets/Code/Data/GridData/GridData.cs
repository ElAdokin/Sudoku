using UnityEngine;

[CreateAssetMenu(fileName = "GridData", menuName = "ScriptableObjects/Data/GridData", order = 2)]
public class GridData : ScriptableObject
{
    [SerializeField] private Vector2Int _gridSize;
    public Vector2Int GridSize => _gridSize;
}
