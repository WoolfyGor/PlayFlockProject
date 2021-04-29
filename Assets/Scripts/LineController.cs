using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    LineRenderer Lr;
    public int SettedConnections=0;
    public GameObject[]  ConnectionsDots = new GameObject[100] ;

    private void Awake()
    {
        Lr = gameObject.AddComponent<LineRenderer>(); // Добавление компонента линии
        Lr.startWidth = 0.1f; // Настройка линии
        Lr.endWidth = 0.1f;
        Lr.positionCount = SettedConnections + 1; // Увеличение кол-ва связей
        Lr.SetPosition(0, transform.position); // установленеи начальной точки линии на этот объект
        ConnectionsDots[0] = this.gameObject; // добавление ссылки на первоначальный объект
    }
    void Update()
    {
        if (SettedConnections > 0) {//Если связей больше, чем одна
            for(int i = 0; i < SettedConnections+1; i++) // То для каждой связи
            {
                if (ConnectionsDots[i] != null) { //Если связь не пуста
                Lr.SetPosition(i, ConnectionsDots[i].transform.position); // То обновляется позиция точки
                }
                else
                {
                    Lr.SetPosition(i, transform.position); // Иначе точка равна первоначальной
                }
            }
        }

    }

    public void AddConnection(GameObject Dot) // Функция добавления точки
    {
        ConnectionsDots[SettedConnections] = Dot; // В массив добавляется передаваемый объект
        Lr.positionCount = SettedConnections+1; // Кол-во точек увеличивается
        Lr.SetPosition(SettedConnections, ConnectionsDots[SettedConnections].transform.position); // Устанавливается позиция точки на координаты переданного объекта
        SettedConnections++; //Увеличивается кол-во связей
        ConnectionsDots[SettedConnections] = transform.gameObject; // Устанавливается новая точка, которая возвращает линию к первоначальному обхек
       Lr.positionCount = SettedConnections + 1;// Кол-во точек увеличивается
        Lr.SetPosition(SettedConnections, ConnectionsDots[SettedConnections].transform.position); // устанавливается позиция новой точки
    }

}
