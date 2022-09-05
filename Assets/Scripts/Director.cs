using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Director : MonoBehaviour
{
    public bool IsStart = false;

    public GameObject[] paints;

    public Image[] Lifes;

    public GameObject Title;
    public GameObject Game;
    public GameObject Pause;
    public GameObject End;

    [Header("Score")]
    public Text scoreText;
    public Text bestScoreText;
    public float score; 
    public float bestScore;
    public string key = "BestScore";
    public Text Endscore;
    public Text EndbestScore;

    [Header ("Combo")]
    public bool isCombo = false;
    public int ComboStack = 0;
    public float comboTime = 0;
    public float comboLimit = 1;
    public Text comboText;
    public ParticleSystem comboEffect;

    public Image fadeImage;

    
    void Start()
    {
        bestScore = PlayerPrefs.GetFloat(key, 0);
        bestScoreText.text = "Best : " + Mathf.Round(bestScore).ToString();
    }

    void Update()
    {
        scoreText.text = "X " + Mathf.Round(score).ToString();

        if (score >= bestScore)
        {
            bestScore = score;
            bestScoreText.text = "Best : " + Mathf.Round(bestScore).ToString();
        }

        ComboBonus();
    }

    public void ComboBonus()
    {
        if (isCombo == true)
        {
            comboText.text = ComboStack + " COMBO";
            comboTime += Time.deltaTime;

            if (ComboStack > 1)
            {
                comboText.enabled = true;
            }

            if (comboLimit <= comboTime)
            {
                isCombo = false;
            }
        }
        else if (isCombo == false)
        {
            comboText.enabled = false;
            if (ComboStack > 1)
            {
                score += ComboStack;
                ComboStack = 0;
                comboTime = 0;
            }
            else
            {
                ComboStack = 0;
                comboTime = 0;
            }
        }
    }

    public void paint(GameObject fruits)
    {
        Vector3 paintPosition = new Vector3(fruits.transform.position.x, fruits.transform.position.y, 4.9f);

        switch (fruits.name)
        {
            case "Apple":
                GameObject P0 = Instantiate(paints[0], paintPosition, Quaternion.identity);
                Destroy(P0, 2f);
                break;
            case "Apple_Green":
                GameObject P1 = Instantiate(paints[1], paintPosition, Quaternion.identity);
                Destroy(P1, 2f);
                break;
            case "Coconut":
                GameObject P2 = Instantiate(paints[2], paintPosition, Quaternion.identity);
                Destroy(P2, 2f);
                break;
            case "Lemon":
                GameObject P3 = Instantiate(paints[3], paintPosition, Quaternion.identity);
                Destroy(P3, 2f);
                break;
            case "Orange":
                GameObject P4 = Instantiate(paints[4], paintPosition, Quaternion.identity);
                Destroy(P4, 2f);
                break;
            case "Watermelon":
                GameObject P5 = Instantiate(paints[5], paintPosition, Quaternion.identity);
                Destroy(P5, 2f);
                break;
        }
    }


    public void LifeCheck()
    {
        if (Lifes[0].color == Color.red)
        {
            Lifes[0].color = Color.black;
        }
        else if (Lifes[1].color == Color.red)
        {
            Lifes[1].color = Color.black;
        }
        else if (Lifes[2].color == Color.red)
        {
            Lifes[2].color = Color.black;
            GameOver();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0;

        if (score >= bestScore)
        {
            PlayerPrefs.SetFloat(key, score);
        }

        IsStart = false;
        Endscore.text = "Score : " + score;
        EndbestScore.text = "Best : " + bestScore;
        End.SetActive(true);
        FindObjectOfType<Audio>().AudioPlay("Over");
    }

    public void ResetGame()
    {
        GameObject[] fruits = GameObject.FindGameObjectsWithTag("Fruit");
        for (int i = 0; i < fruits.Length; i++)
        {
            Destroy(fruits[i]);
        }

        GameObject[] paints = GameObject.FindGameObjectsWithTag("Paint");
        for (int i = 0; i < paints.Length; i++)
        {
            Destroy(paints[i]);
        }

        GameObject[] Bombs = GameObject.FindGameObjectsWithTag("Bomb");
        for (int i = 0; i < Bombs.Length; i++)
        {
            Destroy(Bombs[i]);
        }

        for (int i = 0; i < Lifes.Length; i++)
        {
            Lifes[i].color = Color.red;
        }

        score = 0;
        bestScore = PlayerPrefs.GetFloat(key, 0);
        bestScoreText.text = "Best : " + Mathf.Round(bestScore).ToString();
        IsStart = false;
        isCombo = false;

        FindObjectOfType<Generator>().bombChance = 0.1f;
        FindObjectOfType<Generator>().CreateTime = 0;
    }

    public void Explode()
    {
        StartCoroutine(ExplodeSequence());
        Invoke("GameOver",0.4f);
    }

    private IEnumerator ExplodeSequence()
    {
        float elapsed = 0f;
        float duration = 0.5f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.clear, Color.white, t);

            Time.timeScale = 1f - t;
            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        for (int i = 0; i < 3; i++)
        {
            Lifes[i].color = Color.black;
        }

        yield return new WaitForSecondsRealtime(1f);

        elapsed = 0f;

        while (elapsed < duration)
        {
            float t = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = Color.Lerp(Color.white, Color.clear, t);

            elapsed += Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 1f;
        StopAllCoroutines();
    }
}
