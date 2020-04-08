using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    public RectTransform backGround;
    public RectTransform joyStick;
    public float moveSpeed;

    private float radius;
    private GameObject selectedObject;

    private bool isTouch = false;
    private Vector3 movePosition;

    SpawnAndScaleObject spawnAndScaleObject;

    void Awake()
    {
        spawnAndScaleObject = 
            GameObject.Find("AR Session Origin").GetComponent<SpawnAndScaleObject>();
    }
    void Start()
    {
        radius = backGround.rect.width * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        selectedObject = spawnAndScaleObject.spawnedObject;

        if (isTouch)
            selectedObject.transform.position += movePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)backGround.position;
       
        value = Vector2.ClampMagnitude(value, radius);
        joyStick.localPosition = value;

        float distance = Vector2.Distance(backGround.position, joyStick.position) / radius;
        value = value.normalized;

        movePosition = new Vector3(value.x * moveSpeed * Time.deltaTime * distance, 0, value.y * moveSpeed * Time.deltaTime * distance);

        //Vector3 valueZ = new Vector3(0, 0, value.y * moveSpeed * Time.deltaTime * distance);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        joyStick.localPosition = Vector3.zero;
        movePosition = Vector3.zero;
    }
}
