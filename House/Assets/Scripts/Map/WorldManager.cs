using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    public static WorldManager Instance;

    [Header("청크")]
    public NoiseVoxelMap chunkPrefab;
    public MapProfile[] profiles;

    [Header("청크 간격")]
    public int chunkSpacing = 3;   // 청크 사이 간격

    private List<NoiseVoxelMap> chunks = new List<NoiseVoxelMap>();

    [Header("Gizmo 미리보기")]
    public int previewHeight = 10;   // 미리보기용 높이

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

            // 다음 청크 위치 (너비 + 간격)
            currentX += profiles[i].width + chunkSpacing;
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

        Debug.LogWarning($"청크 없음: {worldPos}");
    }

    void OnDrawGizmos()
    {
        if (profiles == null || profiles.Length == 0)
            return;

        int currentX = 0;

        for (int i = 0; i < profiles.Length; i++)
        {
            if (profiles[i] == null)
                continue;

            int width = profiles[i].width;
            int depth = profiles[i].depth;
            int height = previewHeight;

            Vector3 center = new Vector3(
                currentX + width / 2f,
                height / 2f,
                depth / 2f
            );

            Vector3 size = new Vector3(width, height, depth);

            //  청크 영역
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);

            //  간격 영역
            if (chunkSpacing > 0)
            {
                Gizmos.color = Color.red;

                Vector3 gapCenter = new Vector3(
                    currentX + width + chunkSpacing / 2f,
                    height / 2f,
                    depth / 2f
                );

                Vector3 gapSize = new Vector3(chunkSpacing, height, depth);
                Gizmos.DrawWireCube(gapCenter, gapSize);
            }

            currentX += width + chunkSpacing;
        }
    }

}