using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//필수 컴포넌트를 지정 할 때 사용, 클래스 위에 선언
[RequireComponent(typeof(ARRaycastManager))]
public class SpawnObject : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<GameObject> placedPrefabList = new List<GameObject>();

    [SerializeField]
    private int maxPrefaSpawnCount = 0;
    private int placedPrefabCount;

    //[SerializeField]
    private GameObject placeablePrefab;

    private static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private bool tryGetTouchPosition(out Vector2 touchPosition)
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
        if (!tryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }
        if (raycastManager.Raycast(touchPosition, raycastHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = raycastHits[0].pose;
            if (placedPrefabCount < maxPrefaSpawnCount)
            {
                SpawnPrefab(hitPose);
            }
        }
    }

    public void setPrefabType(GameObject prefabType)
    {
        placeablePrefab = prefabType;
    }

    private void SpawnPrefab(Pose hitPose)
    {
        spawnedObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
        placedPrefabList.Add(spawnedObject);
        placedPrefabCount++;
    }
}
