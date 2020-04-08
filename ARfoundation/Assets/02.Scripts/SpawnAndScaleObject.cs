using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class SpawnAndScaleObject : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();
    private List<GameObject> prefabObjects = new List<GameObject>();

    private GameObject placeablePrefab;    //놓으려고 하는 오브젝트
    public GameObject spawnedObject;      //놓여진 오브젝트

    public Slider scaleSlider;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        //슬라이더에 ScaleChanged 메소드 연결
        scaleSlider.onValueChanged.AddListener(ScaleChanged);
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        //유효터치가 발생하면 첫 터치의 포지션값을 할당하고 true값 반환
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    private void Update()
    {
        //유효터치 없음
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (raycastManager.Raycast(touchPosition, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = raycastHits[0].pose;

            //레이캐스트를 쏜 곳(touchPosition)의 첫번째 포즈값에 SpawnPrefab
            SpawnPrefab(hitPose);
            ScaleChanged(scaleSlider.value);

            spawnedObject.transform.position = hitPose.position;

        }
    }

    //버튼 클릭하여 놓을 수 있는 프리팹 변경
    public void SetPrefabType(GameObject prefabType)
    {
        placeablePrefab = prefabType;
    }

    private void SpawnPrefab(Pose hitPose)
    {
        spawnedObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
        prefabObjects.Add(spawnedObject);  //생성된 오브젝트 리스트화
    }

    private void ScaleChanged(float newValue)
    {
        spawnedObject.transform.localScale = Vector3.one * newValue;

    }

    public void DeleteObject()
    {
        for (int i = 0; i < prefabObjects.Count; i++)
        {
            Destroy(prefabObjects[i].gameObject);
        }
    }
}
