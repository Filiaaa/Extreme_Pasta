using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class InventorySys : MonoBehaviour
{
    [Header("UI References")]
    public GameObject inventoryGo;
    public Image[] slotsUI;
    public Sprite defaultSprite;

    [Header("Settings")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private float fadeDuration = 0.3f;
    [SerializeField] private float errorFlashDuration = 0.1f;
    [SerializeField] private int errorFlashLoops = 2;

    [Header("Data")]
    public InventorySlot[] slots;
    public Transform player;

    [Header("Interactive Object References")]
    public GameObject currentInteractiveObj;
    public InventoryObj currentInventoryObj;
    public Transform currentMovingToPoint;
    public int requiredItemID = -1;

    [Header("Visual")]
    public CanvasGroup inventoryCanvasGroup;

    private void Awake()
    {
        if (slots == null || slots.Length != maxSlots)
        {
            slots = new InventorySlot[maxSlots];
            for (int i = 0; i < maxSlots; i++) slots[i] = new InventorySlot();
        }

        if (inventoryCanvasGroup == null)
            inventoryCanvasGroup = inventoryGo.GetComponent<CanvasGroup>();

        inventoryGo.SetActive(false);
        UpdateUI();
    }


    public bool AddItem(ItemData newItem, int amount = 1)
    {
        if (newItem.maxStack > 1)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.item.id == newItem.id && slot.quantity < slot.item.maxStack)
                {
                    slot.quantity += amount;
                    return true;
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i].item = newItem;
                slots[i].quantity = amount;
                return true;
            }
        }

        return false; 
    }

    public bool HasItem(int itemId)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item.id == itemId)
                return true;
        }
        return false;
    }

    public bool PickupWorldItem(ItemData itemData, Vector3 worldPosition)
    {
        if (HasItem(itemData.id) || !AddItem(itemData, 1))
            return false;

        int slotIndex = -1;
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].IsEmpty && slots[i].item.id == itemData.id)
            {
                slotIndex = i;
                break;
            }
        }

        if (slotIndex != -1)
        {
            AnimatePickupToWorldSlot(itemData, worldPosition, slotIndex, UpdateUI);
            return true;
        }

        UpdateUI();
        return true;
    }

    public void UseItemFromSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        if (slots[slotIndex].IsEmpty) return;

        if (currentInteractiveObj == null)
        {
            UseItem(slotIndex);
            return;
        }

        InventorySlot slot = slots[slotIndex];

        if (slot.item.id == requiredItemID)
        {
            currentInventoryObj?.Use(slot.item.id);
            slot.Clear();
            UpdateUI();

            ToggleInventory(false);
        }
        else
        {
            FlashSlotError(slotIndex);
        }
    }

    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Length) return;
        if (slots[slotIndex].IsEmpty) return;

        InventorySlot slot = slots[slotIndex];
        Debug.Log($"Использован предмет: {slot.item.itemName}");


        slot.quantity--;
        if (slot.quantity <= 0) slot.Clear();
        UpdateUI();
    }

    public void AnimatePickupToWorldSlot(ItemData itemData, Vector3 worldPosition, int targetSlotIndex, System.Action onComplete)
    {
        ToggleInventory(true);

        RectTransform targetSlot = slotsUI[targetSlotIndex].rectTransform;
        Canvas canvas = targetSlot.GetComponentInParent<Canvas>();

        LayoutRebuilder.ForceRebuildLayoutImmediate(targetSlot.parent as RectTransform);

        GameObject visualItem = new GameObject("PickupVisualUI");
        Image image = visualItem.AddComponent<Image>();
        image.sprite = itemData.icon;
        image.raycastTarget = false;

        RectTransform visualRect = visualItem.GetComponent<RectTransform>();
        visualItem.transform.SetParent(canvas.transform, false);

        visualRect.pivot = new Vector2(0.5f, 0.5f);
        visualRect.anchorMin = new Vector2(0.5f, 0.5f);
        visualRect.anchorMax = new Vector2(0.5f, 0.5f);
        visualRect.sizeDelta = new Vector2(100, 100);
        visualRect.localScale = Vector3.zero;

        Vector2 screenPoint = Camera.main.WorldToScreenPoint(worldPosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)canvas.transform,
            screenPoint,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localStartPoint
        );
        visualRect.anchoredPosition = localStartPoint;

        DOTween.Sequence()
            .Join(visualRect.DOMove(targetSlot.position, moveDuration).SetEase(Ease.OutQuad))
            .Join(visualRect.DOScale(Vector3.one, moveDuration).SetEase(Ease.OutQuad))
            .OnComplete(() => {
                slotsUI[targetSlotIndex].sprite = itemData.icon;
                Destroy(visualItem);
                ToggleInventory(false);
                onComplete?.Invoke();
            });
    }




    public void FlashSlotError(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slotsUI.Length) return;

        Image slotImage = slotsUI[slotIndex];
        Color originalColor = slotImage.color;

        slotImage.DOColor(Color.red, errorFlashDuration)
            .SetLoops(errorFlashLoops * 2, LoopType.Yoyo)
            .OnComplete(() => slotImage.color = originalColor);
    }

    private void UpdateUI()
    {
        for (int i = 0; i < slotsUI.Length; i++)
        {
            if (i < slots.Length && !slots[i].IsEmpty)
            {
                slotsUI[i].sprite = slots[i].item.icon;
                slotsUI[i].color = Color.white;
                slotsUI[i].enabled = true;
            }
            else
            {
                slotsUI[i].sprite = defaultSprite;
                slotsUI[i].color = Color.white;
                slotsUI[i].enabled = true;
            }
        }
    }

    public void ToggleInventory(bool show)
    {
        inventoryGo.SetActive(true);

        if (show)
        {
            inventoryCanvasGroup.DOFade(1, fadeDuration).SetUpdate(true);
        }
        else
        {
            inventoryCanvasGroup.DOFade(0, fadeDuration)
                .SetUpdate(true)
                .OnComplete(() => inventoryGo.SetActive(false));
        }
    }

    public void StartInteraction(InventoryObj interactiveObj, int requiredItemID, Transform movingToPoint)
    {
        currentInteractiveObj = interactiveObj.gameObject;
        currentInventoryObj = interactiveObj;
        this.requiredItemID = requiredItemID;
        currentMovingToPoint = movingToPoint;

        ToggleInventory(true);
    }

    public void EndInteraction()
    {
        currentInteractiveObj = null;
        currentInventoryObj = null;
        requiredItemID = -1;
        currentMovingToPoint = null;

        ToggleInventory(false);
    }

    public int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].IsEmpty) return i;
        }
        return -1;
    }
}