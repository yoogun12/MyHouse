using UnityEngine;
using System.Collections.Generic;

public class NoiseVoxelMap : MonoBehaviour
{
    public MapProfile profile;

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
                        PlaceInternal(profile.grassPrefab, ItemType.Grass, pos);
                    else
                        PlaceInternal(profile.dirtPrefab, ItemType.Dirt, pos);
                }

                // 장식 블럭
                if (Random.value < profile.decorChance)
                {
                    Vector3Int decorPos = new Vector3Int(
                        x + startX,
                        h + 1,
                        z + startZ
                    );
                    PlaceInternal(profile.decorPrefab, ItemType.Water, decorPos);
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
        if (HasBlockAt(worldPos))
            return false; // 이미 있음

        PlaceInternal(GetPrefab(type), type, worldPos);
        return true;
    }
    void PlaceInternal(GameObject prefab, ItemType type, Vector3Int pos)
    {
        if (blocks.ContainsKey(pos)) return;

        var go = Instantiate(prefab, pos, Quaternion.identity, transform);

        var block = go.GetComponent<Block>();
        if (block == null)
            block = go.AddComponent<Block>();

        block.type = type;
        block.mineable = true;
        block.maxHP = (type == ItemType.Water) ? 1 : 3;
        block.dropCount = 1;

        blocks.Add(pos, block);
    }
    GameObject GetPrefab(ItemType type)
    {
        switch (type)
        {
            case ItemType.Dirt:
                return profile.dirtPrefab;
            case ItemType.Grass:
                return profile.grassPrefab;
            case ItemType.Water:
                return profile.decorPrefab;
            default:
                return null;
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
        foreach (Transform child in transform)
        {
            if (Vector3Int.RoundToInt(child.position) == worldPos)
                return true;
        }
        return false;
    }
}