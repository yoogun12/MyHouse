using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


public class NoiseVoxelMap : MonoBehaviour
{
    [Header("블럭 프리팹")]
    public GameObject blockPrefab;      //블록 프리팹
    public GameObject grassPrefab;      //잔디 프리팹
    public GameObject waterPrefab;      //물 프리팹

    [Header("크기")]
    public int width = 20;              //가로
    public int depth = 20;              //깊이
    public int maxheight = 16;          //최대 높이
    public int waterLevel = 3;          //물 높이

    [SerializeField] float noiseScale = 20f;



    // Start is called before the first frame update
    void Start()
    {
        float offsetX = Random.Range(-9999f, 9999f);
        float offsetZ = Random.Range(-9999f, 9999f);

        for (int x = 0; x < width; x++)
        {
            for(int z = 0; z < depth; z++)
            {
                float nx = (x + offsetX) / noiseScale;
                float nz = (z + offsetZ) / noiseScale;

                float noise = Mathf.PerlinNoise(nx, nz);            // 0 ~ 1 사이 랜덤한 값 출력 (nx, nz에 따라 연속되게 )

                int h = Mathf.FloorToInt(noise * maxheight);        // 최대 높이만큼 곱해주고 소수점 버림
               

                if (h <= 0) continue;                               // 0 이하일땐 생성 안함

                for (int y = 0; y <= h; y++)
                {
                    if (y == h)
                    {
                        GrassPlace(x, y, z);
                    }
                    else Place(x, y, z);
                }
                                                                                                                                                                     
                if (h < waterLevel)
                {
                    // h보다 높은 곳부터 seaLevel까지 물로 채움                
                    for (int y = h + 1; y <= waterLevel; y++)                  
                    {           
                        WaterPlace(x, y, z);                                
                    }
                }

            }
        }
    }

    private void Place(int x, int y, int z)
    {
        var go = Instantiate(blockPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Dirt_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Dirt;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }
        
    private void GrassPlace(int x, int y, int z)
    {
        var go = Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Grass_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Grass;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    private void WaterPlace(int x, int y, int z)
    {
        var go = Instantiate(waterPrefab, new Vector3(x, y, z), Quaternion.identity, transform);
        go.name = $"Water_{x}_{y}_{z}";

        var b = go.GetComponent<Block>() ?? go.AddComponent<Block>();
        b.type = ItemType.Water;
        b.maxHP = 3;
        b.dropCount = 1;
        b.mineable = true;
    }

    public void PlaceTile(Vector3Int pos, ItemType type)
    {
        switch (type)
        {
            case ItemType.Dirt:
                Place(pos.x, pos.y, pos.z);
                break;
            case ItemType.Grass:
                GrassPlace(pos.x, pos.y, pos.z);
                break;
            case ItemType.Water:
                WaterPlace(pos.x, pos.y, pos.z);
                break;

        }
    }

}
