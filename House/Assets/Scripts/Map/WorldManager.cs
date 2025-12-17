using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [Header("没农")]
    public NoiseVoxelMap chunkPrefab;
    public MapProfile[] profiles;

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

            chunk.generateTerrain = (profiles[i] != null);

            chunk.Generate();

            chunks.Add(chunk);

            currentX += profiles[i].width;
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

        Debug.LogWarning($"没农 绝澜: {worldPos}");
    }
}