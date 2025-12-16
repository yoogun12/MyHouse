using UnityEngine;

public class PlayerHarvester : MonoBehaviour
{
    [Header("채굴")]
    public float rayDistance = 5f;
    public LayerMask hitMask = ~0;
    public float hitCooldown = 0.15f;

    [Header("도구")]
    public int toolDamage = 1;

    [Header("설치 미리보기")]
    public GameObject selectedBlock;

    private float _nextHitTime;
    private Camera _cam;

    private Inventory inventory;
    private InventoryUI invenUI;

    void Awake()
    {
        _cam = Camera.main;

        inventory = GetComponent<Inventory>();
        if (inventory == null)
            inventory = gameObject.AddComponent<Inventory>();

        invenUI = FindObjectOfType<InventoryUI>();
    }

    void Update()
    {
        if (invenUI == null) return;

        UpdateToolDamage();

        if (invenUI.selectedIndex < 0)
        {
            HandleMining();
        }
        else
        {
            HandlePlacement();
        }
    }

    void HandleMining()
    {
        HidePreview();

        if (!Input.GetMouseButton(0) || Time.time < _nextHitTime)
            return;

        _nextHitTime = Time.time + hitCooldown;

        if (RaycastCenter(out RaycastHit hit))
        {
            Block block = hit.collider.GetComponent<Block>();
            if (block != null)
                block.Hit(toolDamage, inventory);
        }
    }

    void HandlePlacement()
    {
        if (RaycastCenter(out RaycastHit hit))
        {
            Vector3Int placePos = AdjacentCellOnHitFace(hit);
            ShowPreview(placePos);

            if (Input.GetMouseButtonDown(0))
            {
                ItemType selected = invenUI.GetInventorySlot();
                if (inventory.Consume(selected, 1))
                {
                    WorldManager.Instance.PlaceTile(placePos, selected);
                }
            }
        }
        else
        {
            HidePreview();
        }
    }

    void UpdateToolDamage()
    {
        // 아무 슬롯도 선택 안 했으면 기본 데미지
        if (invenUI.selectedIndex < 0)
        {
            toolDamage = 1;
            return;
        }

        ItemType selected = invenUI.GetInventorySlot();

 
    }

    void ShowPreview(Vector3Int pos)
    {
        if (selectedBlock == null) return;

        selectedBlock.transform.localScale = Vector3.one;
        selectedBlock.transform.position = pos;
        selectedBlock.transform.rotation = Quaternion.identity;
    }

    void HidePreview()
    {
        if (selectedBlock == null) return;
        selectedBlock.transform.localScale = Vector3.zero;
    }

    bool RaycastCenter(out RaycastHit hit)
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        return Physics.Raycast(ray, out hit, rayDistance, hitMask, QueryTriggerInteraction.Ignore);
    }

    static Vector3Int AdjacentCellOnHitFace(in RaycastHit hit)
    {
        Vector3 baseCenter = hit.collider.transform.position;
        Vector3 adjCenter = baseCenter + hit.normal;
        return Vector3Int.RoundToInt(adjCenter);
    }
}