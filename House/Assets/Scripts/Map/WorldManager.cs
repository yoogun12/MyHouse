using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [Header("Chunk 생성")]
    public NoiseVoxelMap chunkPrefab;
    public MapProfile[] profiles;

    [Header("청크 간 간격")]
    public int chunkGap = 3; 

    private List<NoiseVoxelMap> chunks = new List<NoiseVoxelMap>();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GenerateWorld();
    }

    void GenerateWorld()
    {
        int currentX = 0;

        for (int i = 0; i < profiles.Length; i++)
        {
            NoiseVoxelMap chunk = Instantiate(chunkPrefab, transform);

            chunk.profile = profiles[i];
            chunk.startX = currentX;
            chunk.startZ = 0;

            chunk.Generate();

            chunks.Add(chunk);

            currentX += profiles[i].width + chunkGap;
        }
    }

    public void PlaceTile(Vector3Int worldPos, ItemType type)
    {
        foreach (var chunk in chunks)
        {
            if (chunk.Contains(worldPos))
            {
                chunk.PlaceTile(worldPos, type);
                return;
            }
        }

        Debug.LogWarning($"해당 위치에 Chunk 없음: {worldPos}");
    }
}