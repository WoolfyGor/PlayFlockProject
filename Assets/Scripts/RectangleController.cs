using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class RectangleController : MonoBehaviour
{
    Image Img; 

    void Start()
    {
        Img = GetComponent<Image>(); // Получение ссылки на изображение
        System.Random rnd = new System.Random(); //Объявление новой переменной для создания случайного цвета
        Img.color = new Color32(Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), Convert.ToByte(rnd.Next(0, 256)), 255); // присваивается новый случайный цвет
    }
   

}
