using UnityEngine;
using TMPro;

public class IteractiveSink : InventoryObj
{
    public int iceItemId = 6;
    public int icePiecesRequired = 3;
    public SpriteRenderer sinkRenderer;
    public Sprite partialFreezeSprite1;
    public Sprite partialFreezeSprite2;
    public Sprite frozenSinkSprite;
    public Collider2D interactionTrigger;
    [SerializeField] private Collider2D waterCol;
    [SerializeField] private TextMeshProUGUI iceCountText;

    [SerializeField] private GameObject iceObjects;
    int _iceDelivered;
    bool _frozen;

    void Awake()
    {
        numberOfNeedItem = iceItemId;
    }

    public override void Use(int numberOfItem)
    {
        if (_frozen || numberOfItem != iceItemId)
            return;

        _iceDelivered++;
        iceCountText.text = _iceDelivered.ToString() + "/" + icePiecesRequired.ToString();
        UpdateSinkVisual();

        if (_iceDelivered >= icePiecesRequired)
        {
            _frozen = true;
            iceObjects.SetActive(true);
            if (interactionTrigger != null)
                interactionTrigger.enabled = false;
            else
                GetComponent<Collider2D>().enabled = false;
            waterCol.enabled = false;
        }
    }

    void UpdateSinkVisual()
    {
        if (sinkRenderer == null)
            return;

        if (_iceDelivered >= icePiecesRequired && frozenSinkSprite != null)
            sinkRenderer.sprite = frozenSinkSprite;
        else if (_iceDelivered >= 2 && partialFreezeSprite2 != null)
            sinkRenderer.sprite = partialFreezeSprite2;
        else if (_iceDelivered >= 1 && partialFreezeSprite1 != null)
            sinkRenderer.sprite = partialFreezeSprite1;
    }
}
