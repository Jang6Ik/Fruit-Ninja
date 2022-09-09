using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    public Vector3 direction;

    public Camera mainCamera;

    public Collider sliceCollider;
    private TrailRenderer sliceTrail;

    public bool slicing;

    private Director director;

    private void Awake()
    {
        mainCamera = Camera.main;
        sliceCollider = GetComponent<Collider>();
        sliceTrail = GetComponentInChildren<TrailRenderer>();
        director = FindObjectOfType<Director>();

        StopSlice();
    }

    private void Update()
    {
        if (director.IsStart == true && FindObjectOfType<Button>().isPause == false)
        {

            if (Input.GetMouseButtonDown(0))
            {
                StartSlice();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                StopSlice();
            }

            if (slicing)
            {
                Slice();
            }

        }
        else
        {
            StopSlice();
        }
    }

    private void StartSlice()
    {
        slicing = true;
        sliceCollider.enabled = true;

        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;
        transform.position = position;
        sliceTrail.enabled = true;
        sliceTrail.Clear();

    }

    private void StopSlice()
    {
        slicing = false;
        sliceCollider.enabled = false;
        sliceTrail.enabled = false;
    }

    private void Slice()
    {
        Vector3 newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0f;
        
        direction = newPosition - transform.position; 

        float velocity = direction.magnitude / Time.deltaTime; //벡터의 길이 / 시간 = 속도
        sliceCollider.enabled = velocity > 0.01f;
        
        transform.position = newPosition;
    }
}
