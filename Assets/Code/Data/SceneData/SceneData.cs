using UnityEngine;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/Data/SceneData", order = 4)]
public class SceneData : ScriptableObject
{
    [SerializeField] private Scenes _scene;

    public void AssingSceneToGo(Scenes sceneToGo) => _scene = sceneToGo;

    public Scenes Scene => _scene;
}
