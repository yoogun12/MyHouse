using UnityEngine;

[CreateAssetMenu(menuName = "Map/Map Profile")]
public class MapProfile : ScriptableObject
{
    [Header("크기")]
    public int width = 16;
    public int depth = 16;
    public int maxHeight = 5;

    [Header("노이즈")]
    public float noiseScale = 10f;

    [Header("지형 프리팹")]
    public GameObject grassPrefab;
    public GameObject dirtPrefab;

    [Header("데코 설정 (한 종류만)")]
    public ItemType decorType = ItemType.None;
    [Range(0f, 1f)]
    public float decorChance = 0.1f;

    [Header("데코 프리팹")]
    public GameObject slimePrefab;
    public GameObject skullPrefab;
    public GameObject crystalPrefab;
}