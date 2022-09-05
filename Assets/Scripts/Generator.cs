using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    private Collider spawnArea;
    private Director director;

    public GameObject[] fruitPrefabs;

    public float minAngle = -15.0f;
    public float maxAngle = 15.0f;

    public float minForce = 20.0f;
    public float maxForce = 25.0f;

    public float maxCreateTime = 4.0f;
    public float nowCreateTime = 0.0f;
    public float CreateTime = 0.0f;

    public float tripleChance = 0.3f;

    public GameObject bombPrefab;
    public float bombChance = 0.1f;

    public struct FruitData
    {
        public GameObject prefab { get; set; }
        public Vector3 position { get; set; }
        public Quaternion rotation { get; set; }

    }

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
        director = GetComponent<Director>();
    }

    private void Update()
    {
        if (director.IsStart)
        {
            nowCreateTime = Random.Range(2.0f, maxCreateTime);

            CreateTime += Time.deltaTime;

            if (nowCreateTime <= CreateTime)
            {
                float tripleCreate = Random.Range(0.0f, 1.0f);

                if (tripleCreate > tripleChance)
                {
                    CreateFruits();
                }
                else
                {
                    CreateFruits3();
                }

                float bombCreate = Random.Range(0.0f, 1.0f);
                if (bombCreate < bombChance)
                {
                    if (director.score > 100)
                    {
                        CreateBombs();
                        CreateBombs();
                        CreateBombs();
                    }
                    else if (director.score >= 50 && director.score < 100 )
                    {
                        CreateBombs();
                        CreateBombs();
                    }
                    else if (director.score < 50)
                    {
                        CreateBombs();
                    }
                }

                if (director.score >= 100)
                {
                    bombChance = 0.7f;
                }
                else if (director.score > 50 && director.score < 100)
                {
                    bombChance = 0.35f;
                }
                else
                {
                    bombChance = 0.1f;
                }
            }
        }
    }

    public void CreateFruits()
    {
        FruitData fruitdata = Fruits();

        GameObject fruit = Instantiate(fruitdata.prefab, fruitdata.position, fruitdata.rotation);

        float force = Random.Range(minForce, maxForce);
        fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);

        CreateTime = 0;
    }

    public void CreateFruits3()
    {
        FruitData fruitdata1 = Fruits();
        FruitData fruitdata2 = Fruits();
        FruitData fruitdata3 = Fruits();

        GameObject fruit1 = Instantiate(fruitdata1.prefab, fruitdata1.position, fruitdata1.rotation);
        GameObject fruit2 = Instantiate(fruitdata2.prefab, fruitdata2.position, fruitdata2.rotation);
        GameObject fruit3 = Instantiate(fruitdata3.prefab, fruitdata3.position, fruitdata3.rotation);

        float force1 = Random.Range(minForce, maxForce);
        float force2 = Random.Range(minForce, maxForce);
        float force3 = Random.Range(minForce, maxForce);

        fruit1.GetComponent<Rigidbody>().AddForce(fruit1.transform.up * force1, ForceMode.Impulse);
        fruit2.GetComponent<Rigidbody>().AddForce(fruit2.transform.up * force2, ForceMode.Impulse);
        fruit3.GetComponent<Rigidbody>().AddForce(fruit3.transform.up * force3, ForceMode.Impulse);

        CreateTime = 0;

    }

    public FruitData Fruits()
    {
        FruitData fruit = new FruitData();

        GameObject prefab = fruitPrefabs[Random.Range(0, fruitPrefabs.Length)];
        fruit.prefab = prefab;

        Vector3 position = new Vector3();
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = transform.position.z;
        fruit.position = position;

        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        fruit.rotation = rotation;

        return fruit;
    }

    public void CreateBombs()
    {
        FruitData Bombdata = Bombs();

        GameObject bomb = Instantiate(Bombdata.prefab, Bombdata.position, Bombdata.rotation);

        float force = Random.Range(minForce, maxForce);
        bomb.GetComponent<Rigidbody>().AddForce(bomb.transform.up * force, ForceMode.Impulse);

    }

    public FruitData Bombs()
    {
        FruitData Bomb = new FruitData();

        GameObject prefab = bombPrefab;
        Bomb.prefab = prefab;

        Vector3 position = new Vector3();
        position.x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        position.y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        position.z = transform.position.z;
        Bomb.position = position;

        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(minAngle, maxAngle));
        Bomb.rotation = rotation;

        return Bomb;
    }
}
