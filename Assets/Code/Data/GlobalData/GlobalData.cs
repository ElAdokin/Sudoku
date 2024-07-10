using UnityEngine;

[CreateAssetMenu(fileName = "GlobalData", menuName = "ScriptableObjects/Data/GlobalData", order = 1)]
public class GlobalData : ScriptableObject
{
    [SerializeField] private SudokuData _sudokuData;
    [SerializeField] private SceneData _sceneData;

    public SudokuData SudokuData => _sudokuData;
    public SceneData SceneData => _sceneData;
}
