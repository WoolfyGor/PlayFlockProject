using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MouseActions : MonoBehaviour
{

    public GameObject Rectangle; // Префаб прямоугольника
    private float doubleClickTimeLimit = 0.2f; // Задержка для отслеживания двойного клика
    int ClickTimes = 0; // Переменная для отслеживания статуса создания связи
    bool MakingConnection = false, AddingFigures = false; // Статусы действий
    LineController Obj1Ctrl; // Скрипт создания связи
    Button ConnectionButton,AddingButton; // Ссылки на кнопки
  public  GameObject FirstCube, SecondCube,ObjCanvas; // Ссылки на объекты для создания связи и добавления прямоугольника
    Image AddingImg;
    
    // Start is called before the first frame update
    void Start()
    {
        AddingButton = GameObject.Find("AddFigure").GetComponent<Button>();
        AddingImg = AddingButton.GetComponent<Image>();
        ConnectionButton = GameObject.Find("SetConnection").GetComponent<Button>();
        StartCoroutine(InputListener()); // Старт корутины для отслеживания кликов
    }
    private void SingleClick() // Функция при одиночном клике
    {

        if (AddingFigures) {
        MadeRectangle();
        }
        else if(MakingConnection)
        {
            MadeConnection();
        }
    }
    private void DoubleClick() // Функция при двойном клике
    {
        if (!MakingConnection)
        {
            DeleteRectangle();
        }
    }
    void MadeConnection()
    {
        doubleClickTimeLimit = 0f;
        ClickTimes++; // инкремент кол-ва нажатий для отслеживания стадии создания связи
        switch (ClickTimes)
        {
            case 2:
                Debug.Log("Первый клик");
                try
                {
                    doubleClickTimeLimit = 0f;
                    AddingButton.interactable = false;
                    RaycastHit2D hit = ShotRay(); 
                    if (hit.collider.transform.name == "RectanglePrefab") // Если луч от камеры в курсор попадает в прямоугольник, то ссылка на объект сохраняется
                    {
                        FirstCube = hit.transform.parent.gameObject;
                    }
                }
                catch // Если луч не вернул значения, то создание связи сбрасывается
                {
                    ClickTimes = 0;
                    doubleClickTimeLimit = 0.2f;
                    MakingConnection = !MakingConnection;
                    ConnectionButton.interactable = true;
                    AddingButton.interactable = true; 
                    AddingImg.color = new Color(1, 1, 1, 1);
                }
                break;
            case 3:
                RaycastHit2D hit2 = ShotRay();
                try
                {
                    if (hit2.collider.transform.name == "RectanglePrefab")  // Если луч от камеры в курсор попадает в прямоугольник, то ссылка на объект сохраняется
                    {
                        SecondCube = hit2.transform.parent.gameObject;
                    }
                    else
                    {
                        SecondCube = FirstCube;
                    }
                    Debug.Log("Второй клик");
                    if (Obj1Ctrl = FirstCube.GetComponent<LineController>()) // Если у первого объекта уже есть линия - то добавляется новая точка
                    {
                        Obj1Ctrl.SettedConnections++; // Увеличивается число линий
                        Obj1Ctrl.AddConnection(SecondCube); // Добавляется новая точка
                    }
                    else // Если у первого объекта линии нет - создаётся линия и добавляется точка
                    {
                        Obj1Ctrl = FirstCube.AddComponent<LineController>(); // Добавляется скрипт создания линии
                        Obj1Ctrl.SettedConnections++; // Увеличивается число линий
                        Obj1Ctrl.AddConnection(SecondCube); // Добавляется новая точка
                    }
                }
                finally { // Если связь была создана или нет - происходит обнуление стадии
                    ClickTimes = 0;
                    doubleClickTimeLimit = 0.2f;
                    MakingConnection = !MakingConnection;
                    ConnectionButton.interactable = true;
                    AddingButton.interactable = true;
                    AddingImg.color = new Color(1, 1, 1, 1);
                }
                break;

        }
    }
    
  public void  SetConnection() // При нажатии на клавишу создания связи происходит изменение статуса
    {
        MakingConnection = !MakingConnection;
        ConnectionButton.interactable = false;
        AddingFigures = false;
        AddingButton.interactable = false;
        AddingImg.color = new Color(0.6f, 0.6f, 0.6f, 1);
    }
    public void AddFigure() // При нажатии на клавишу создания фигуры изменяется статус
    {
        AddingFigures = !AddingFigures;
        if (AddingFigures)
        {
            AddingImg.color = new Color(0.6f, 0.6f, 0.6f,1);
        }
        else
        {
            AddingImg.color = new Color(1, 1, 1, 1);
        }
     
    }
    void MadeRectangle() // Функция создания прямоугольника
    {
        Vector3 FixedCoordinates = GetCoordinates(); // Получение координат создания
        RaycastHit2D hit = ShotRay(); // Проверка пересечения луча с коллайдером других кубов
        if (!hit) // Если пересечения нет - создаётся прямоугольник
        {
            GameObject InnerRect = Instantiate(Rectangle, FixedCoordinates, Quaternion.Euler(0, 0, 0));
            InnerRect.transform.SetParent(ObjCanvas.transform, true);
          //  InnerRect.transform.parent = ObjCanvas.transform; 
            InnerRect.transform.localScale = new Vector2(2, 1);
        }
      
    }

    void DeleteRectangle() // Функция удаления прямоугольника по двойному щелчку
    {
        RaycastHit2D hit = ShotRay(); // проверка попадания по прямоугольнику
        try //В случае если попадани не было - блок будет пропущен
        {
            if (hit.collider.transform.name == "RectanglePrefab") // Если попало во внутренний коллайдер прямоугольника
            {
                Destroy(hit.collider.transform.parent.gameObject); // удаление прямоугольника
            }
        }
        catch 
        {
        
        }
    }
    RaycastHit2D ShotRay() // функция выстреливания луча
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Указание параметров луча (в точку мыши)
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity); // Выстрел луча из изначальной точки (камеры) по бесконечное расстояние
        return hit; // возврат луча
    }

    Vector3 GetCoordinates()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0); // Получение координатов нажатия на экран
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition); // Изменение координат с координат экрана на координаты мира
        Vector3 FixedCoordinates = new Vector3(worldPosition.x, worldPosition.y, 0); // Изменение Z координаты 
        return FixedCoordinates; // вовзращение координаты
    }
    private IEnumerator InputListener() // Корутина отслеживающая клики
    {
        while (enabled) // активна всегда
        { 
            if (Input.GetMouseButtonDown(0))    yield return ClickEvent(); // Если была нажата ЛКМ, вызывается функция одиночного клика
            yield return null;
        }
    }

    private IEnumerator ClickEvent()
    {
        yield return new WaitForEndOfFrame();
        float count = 0f; // кол-во нажатий
        while (count < doubleClickTimeLimit) // Если произведено больше чем 1 нажатие во временный промежуток задержки отклика - вызывается двойное нажатие
        {
            if (Input.GetMouseButtonDown(0))
            {
                DoubleClick();
                yield break;
            }
            count += Time.deltaTime;
            yield return null;
        }
        SingleClick(); // иначе одиночное нажатие
    }

}
