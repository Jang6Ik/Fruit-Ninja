using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public ParticleSystem[] fireEffect;

    private void Update()
    {
        for (int i = 0; i < fireEffect.Length; i++)
        {
            fireEffect[i].Play();
        }

        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            FindObjectOfType<Audio>().AudioPlay("Bomb");
            FindObjectOfType<Director>().Explode();
        }
    }
}
