using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragDrop : MonoBehaviour,IDragHandler // Расширение класса для использования Drag/Drop функций
    
{
    private RectTransform rectTransform;

        private void Awake()
    { 
        rectTransform = GetComponent<RectTransform>();// Получение ссылки на компонент
    }



    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta; // Изменение позиции при зажатии мыши
    }
    private void OnCollisionExit(Collision collision)
    {
        
    
        try
        {
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        catch { }
    }



}
