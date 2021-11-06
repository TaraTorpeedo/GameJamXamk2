using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))] 
public class MultipleTargetCamera : MonoBehaviour
{
    int playerCount = 0;

    public List<Transform> targets;

    public Vector3 offset;

    float smoothTime = 0.5f;
    private Vector3 velocity;

    public float minZoom = 40.0f;
    public float maxZoom = 10.0f;
    public float zoomLimiter = 50.0f;

    private Camera cam;

    [SerializeField] GameManager gm;
    bool lockCamera = true;

    [SerializeField] GameObject StartingColliders;
    [SerializeField] GameObject FinishLine;

    float distance;
    float minDist = Mathf.Infinity;
    Transform tMin = null;
    public float priorityMultiplier;

    public GameObject Canvas;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (gm.gameIsStarted)
            StartCoroutine(StartGame());

        if (lockCamera)
            return;

        if (targets.Count == 0)
        {
            return;
        }
        Move();
        Zoom();
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();


        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    Vector3 GetCenterPoint()
    {
        if(targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            distance = Vector3.Distance(targets[i].position, FinishLine.transform.position);
            if(distance < minDist)
            {
                tMin = targets[i];
                minDist = distance;
            }
            Vector3 PriorityTarget = new Vector3(targets[i].position.x + (minDist * priorityMultiplier), targets[i].position.y, targets[i].position.z);
            bounds.Encapsulate(PriorityTarget);
        }

        return bounds.center;
    }
    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for(int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    public void AddPlayer(Transform target) {
        targets.Add(target);

    }

    IEnumerator StartGame()
    {
        Canvas.GetComponent<StartTexts>().StartWrite();

        while (Canvas.GetComponent<StartTexts>().canPlay == false)
        {
            yield return null;
        }

        gm.NoMorePlayers();
        StartingColliders.SetActive(false);
        lockCamera = false;
    }


}
