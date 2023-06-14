using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour,IPointerClickHandler
{
    public RectTransform rectTransform
    {
        get {
            return GetComponent<RectTransform>();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.LogError(GetComponentInChildren<Text>().text);
    }

   
}
