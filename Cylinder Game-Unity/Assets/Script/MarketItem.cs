using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketItem : MonoBehaviour
{
    public int itemId, wearId;
    public int price;

    public Button buyButton, equipButton, unequipButton;
    public Text priceText;

    public GameObject itemPrefab;
    public bool HasItem()
    {

        // 0 : Daha sat�n al�nmam��
        // 1 : Sat�n al�nm�� ama giyilmemi�
        // 2 : Hem sat�n al�nm�� hem de giyilmi�
        bool hasItem = PlayerPrefs.GetInt("item" + itemId.ToString(), 5) != 0;

        return hasItem;
    }
    public bool IsEquipped()
    {

        bool equippredItem = PlayerPrefs.GetInt("item" + itemId.ToString(), 5) == 2;

        return equippredItem;
    }

    public void InitializeItem()
    {
        priceText.text = price.ToString();
        if (HasItem())
        {
            buyButton.gameObject.SetActive(false);

            if (IsEquipped())
            {
                EquipItem();
            }
            else
            {
                equipButton.gameObject.SetActive(true);
            }
        }
        else
        {
            buyButton.gameObject.SetActive(true);
        }

    }

    public void BuyItem()
    {
        if (!HasItem())
        {
            int money = PlayerPrefs.GetInt("money");
            if (money >= price)
            {
                PlayerController.Current.itemAudioSource.PlayOneShot(PlayerController.Current.buyAudioClip, 0.1f);
                levellcontrol.Current.GiveMoneyToPlayer(-price);
                PlayerPrefs.SetInt("item" + itemId.ToString(), 1);
                buyButton.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(true);
            }
        }
    }
    public void EquipItem()
    {
        UnEquipItem();
        MarketController.Current.equippedItems[wearId] = Instantiate(itemPrefab, PlayerController.Current.wearSpots[wearId].transform).GetComponent<Items>();
        MarketController.Current.equippedItems[wearId].itemId = itemId;
        equipButton.gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(true);
        PlayerPrefs.SetInt("item" + itemId.ToString(), 2);
    }
    public void UnEquipItem()
    {
        Items equippedItem = MarketController.Current.equippedItems[wearId];
        if (equippedItem != null)
        {
            MarketItem marketItem = MarketController.Current.items[equippedItem.itemId];
            PlayerPrefs.SetInt("item" + marketItem.itemId, 1);
            marketItem.equipButton.gameObject.SetActive(true);
            marketItem.unequipButton.gameObject.SetActive(false);
            Destroy(equippedItem.gameObject);
        }
    }

public void EquipItemButton()
    {
        PlayerController.Current.itemAudioSource.PlayOneShot(PlayerController.Current.equipItemAudioClip, 0.1f);
        EquipItem(); 
    }
public void UnequipItemButton()
    {
        PlayerController.Current.itemAudioSource.PlayOneShot(PlayerController.Current.unequipItemAudioClip, 0.1f);
        UnEquipItem(); 
    }

}

