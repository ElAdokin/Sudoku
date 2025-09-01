using UnityEngine;

[CreateAssetMenu(fileName = "CellViewData", menuName = "ScriptableObjects/Data/CellViewData", order = 3)]
public class CellViewData : ScriptableObject
{
    [SerializeField] private Color _assingColor;
    [SerializeField] private Color _correctColor;
    [SerializeField] private Color _selectionColor;

    public Color AssingColor => _assingColor;
    public Color CorrectColor => _correctColor;
    public Color SelectionColor => _selectionColor;
}
