using UnityEngine;

[System.Serializable]
public class SceneMaterials
{
    public Material mat;
    public string fieldName = "_MainTex";
    public Texture albedoTex;

    [Header("Raining Change")]
    [Range(0, 1)]
    public float alphaCutout = 0;
    [Range(0, 1)]
    public float mettalic = 0;
    [Range(0, 1)]
    public float smoothness = 0;
}

[CreateAssetMenu(fileName = "EnvProfile", menuName = ("Env Profile"))]
public class EnvProfile : ScriptableObject
{
    public Material skybox;
    
    [Space(10)]

    [Header("Material Change")]
    public SceneMaterials [] materialChange;
}
