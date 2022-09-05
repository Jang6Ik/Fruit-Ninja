using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Director director;
    private Generator generator;

    public GameObject startFruits;
    private Fruit sliceFruit;

    private void Awake()
    {
        director = GetComponent<Director>();
        generator = GetComponent<Generator>();
        setStartFruit();
    }

    public void setStartFruit()
    {
        GameObject Fruits = Instantiate(generator.fruitPrefabs[Random.Range(0, generator.fruitPrefabs.Length)], startFruits.transform.position, startFruits.transform.rotation);
        Fruits.GetComponent<Rigidbody>().useGravity = false;
        sliceFruit = Fruits.GetComponent<Fruit>();
        Fruits.transform.parent = startFruits.transform;
    }

    public void GameStart()
    {
        FindObjectOfType<Audio>().AudioPlay("Bt");
        sliceFruit.Slice();

        Invoke("StartGame", 2);
    }

    public void StartGame()
    {
        director.IsStart = true;

        director.GetComponent<AudioSource>().Play();
        director.Title.SetActive(false);
        director.Game.SetActive(true);
    }

    public void GamePause()
    {
        FindObjectOfType<Audio>().AudioPlay("Bt");
        director.GetComponent<AudioSource>().Pause();
        Time.timeScale = 0;

        director.Pause.SetActive(true);
    }

    public void GameResume()
    {
        FindObjectOfType<Audio>().AudioPlay("Bt");
        Time.timeScale = 1;
        director.GetComponent<AudioSource>().Play();
        director.Pause.SetActive(false);
    }

    public void GameTitle()
    {
        FindObjectOfType<Audio>().AudioPlay("Bt");
        director.GetComponent<AudioSource>().Stop();
        Time.timeScale = 0;
        director.ResetGame();

        setStartFruit();

        director.Title.SetActive(true);
        director.Game.SetActive(false);
        director.Pause.SetActive(false);
        director.End.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameRestart()
    {
        FindObjectOfType<Audio>().AudioPlay("Bt");
        director.GetComponent<AudioSource>().Stop();
        director.GetComponent<AudioSource>().Play();
        director.ResetGame();


        director.Pause.SetActive(false);
        director.End.SetActive(false);

        Time.timeScale = 1;
        director.IsStart = true;
    }

}
