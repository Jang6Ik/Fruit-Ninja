using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    private GameObject whole;
    private GameObject sliced;

    private Rigidbody fruitRigidbody;
    private Collider fruitCollider;
    private ParticleSystem cutEffect;
    

    private Director director;

    private void Awake()
    {
        whole = transform.GetChild(0).gameObject;
        sliced = transform.GetChild(1).gameObject;

        fruitRigidbody = GetComponent<Rigidbody>();
        fruitCollider = GetComponent<Collider>();
        cutEffect = GetComponentInChildren<ParticleSystem>();

        director = FindObjectOfType<Director>();
    }

    public void Slice()
    {
        fruitCollider.enabled = false;
        CutRotate();
        whole.SetActive(false);
        director.paint(whole);

        FindObjectOfType<Audio>().AudioPlay("Cut");
        sliced.SetActive(true);
        cutEffect.Play();

        CutForce();

        Destroy(gameObject, 2);
    }

    private void CutForce()
    {
        Rigidbody[] slices = sliced.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody slice in slices)
        {
            slice.velocity = fruitRigidbody.velocity;

            if (slice.tag == "Left")
            {
                slice.AddForce(Vector3.up * 20f, ForceMode.Impulse);
                slice.AddForce(Vector3.right * 20f, ForceMode.Impulse);
                slice.AddForce(Vector3.down * 20f, ForceMode.Impulse);
            }

            if (slice.tag == "Right")
            {

                slice.AddForce(Vector3.up * 20f, ForceMode.Impulse);
                slice.AddForce(Vector3.left * 20f, ForceMode.Impulse);
                slice.AddForce(Vector3.down * 20f, ForceMode.Impulse);
            }
        }
    }

    public void CutRotate()
    {
        Vector3 mPosition = Input.mousePosition; 
        Vector3 oPosition = transform.position;
        
        mPosition.z = oPosition.z - Camera.main.transform.position.z; 
        Vector3 target = Camera.main.ScreenToWorldPoint(mPosition);

        float dy = target.y - oPosition.y;       
        float dx = target.x - oPosition.x;       

        float rotateDegree =  Mathf.Atan2(dy, dx)*Mathf.Rad2Deg;   

        transform.rotation = Quaternion.Euler (0f, 0f, rotateDegree);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (director.IsStart == true)
        {
            if (other.CompareTag("Player"))
            {
                Slice();
                director.score += 1;

                if (director.isCombo == false)
                {
                    director.isCombo = true;
                    director.ComboStack += 1;
                }
                else if (director.isCombo == true)
                {
                    director.ComboStack += 1;
                    director.comboText.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    ParticleSystem comboEffect = Instantiate(director.comboEffect, transform.position, Quaternion.identity);
                    comboEffect.Play();
                    Destroy(comboEffect.gameObject,1f);
                }
            }

            if (other.CompareTag("Destroy"))
            {
                director.LifeCheck();
            }
        }
    }

}
