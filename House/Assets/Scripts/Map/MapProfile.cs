using UnityEngine;

[CreateAssetMenu(menuName = "Map/Map Profile")]
public class MapProfile : ScriptableObject
{
    [Header("크기")]
    public int width = 20;
    public int depth = 20;
    public int maxHeight = 16;

    [Header("노이즈")]
    public float noiseScale = 20f;

    [Header("블럭 프리팹")]
    public GameObject dirtPrefab;
    public GameObject grassPrefab;
    public GameObject decorPrefab;

    [Header("장식 확률")]
    [Range(0f, 1f)]
    public float decorChance = 0.15f;
}