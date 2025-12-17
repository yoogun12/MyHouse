using UnityEngine;
using System.Collections.Generic;

public class NoiseVoxelMap : MonoBehaviour
{
    [Header("프로파일")]
    public MapProfile profile;

    [Header("청크 설정")]
    public bool generateTerrain = true; //  false = 빈 청크

    [Header("월드 오프셋")]
    public int startX;
    public int startZ;

    private Dictionary<Vector3Int, Block> blocks = new Dictionary<Vector3Int, Block>();

    void Awake()
    {
        transform.position = Vector3.zero;
    }

    public void Generate()
    {
        if (!generateTerrain)
            return;

        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < profile.width; x++)
        {
            for (int z = 0; z < profile.depth; z++)
            {
                float nx = (x + startX + offsetX) / profile.noiseScale;
                float nz = (z + startZ + offsetZ) / profile.noiseScale;

                int h = Mathf.Max(
                    1,
                    Mathf.FloorToInt(Mathf.PerlinNoise(nx, nz) * profile.maxHeight)
                );

                for (int y = 0; y <= h; y++)
                {
                    Vector3Int pos = new Vector3Int(
                        x + startX,
                        y,
                        z + startZ
                    );

                    if (y == h)
                        PlaceInternal(profile.grassPrefab, ItemType.floor1, pos);
                    else
                        PlaceInternal(profile.dirtPrefab, ItemType.floor2, pos);
                }

                // 데코 (프로파일에서 지정한 1종류만)
                if (profile.decorType != ItemType.None &&
                    Random.value < profile.decorChance)
                {
                    Vector3Int decorPos = new Vector3Int(
                        x + startX,
                        h + 1,
                        z + startZ
                    );

                    PlaceInternal(
                        GetPrefab(profile.decorType),
                        profile.decorType,
                        decorPos
                    );
                }
            }
        }
    }


    public bool Contains(Vector3Int worldPos)
    {
        return worldPos.x >= startX &&
               worldPos.x < startX + profile.width &&
               worldPos.z >= startZ &&
               worldPos.z < startZ + profile.depth;
    }

    public bool PlaceTile(Vector3Int worldPos, ItemType type)
    {
        if (blocks.ContainsKey(worldPos))
            return false;

        GameObject prefab = GetPrefab(type);
        if (prefab == null)
            return false;

        PlaceInternal(prefab, type, worldPos);
        return true;
    }

    void PlaceInternal(GameObject prefab, ItemType type, Vector3Int pos)
    {
        if (blocks.ContainsKey(pos)) return;

        GameObject go = Instantiate(prefab, pos, Quaternion.identity, transform);

        Block block = go.GetComponent<Block>();
        if (block == null)
            block = go.AddComponent<Block>();

        block.type = type;

        // 빈 청크에서는 채굴 불가
        block.mineable = generateTerrain;

        block.maxHP = (type == ItemType.Slime) ? 1 : 3;
        block.dropCount = 1;

        blocks.Add(pos, block);
    }

    GameObject GetPrefab(ItemType type)
    {
        switch (type)
        {
            case ItemType.floor1: return profile.grassPrefab;
            case ItemType.floor2: return profile.dirtPrefab;

            case ItemType.Slime: return profile.slimePrefab;
            case ItemType.Skull: return profile.skullPrefab;
            case ItemType.Crystal: return profile.crystalPrefab;

            default: return null;
        }
    }

    public void RemoveBlock(Vector3Int pos)
    {
        if (blocks.TryGetValue(pos, out Block block))
        {
            Destroy(block.gameObject);
            blocks.Remove(pos);
        }
    }

    public bool HasBlockAt(Vector3Int worldPos)
    {
        return blocks.ContainsKey(worldPos);
    }
}