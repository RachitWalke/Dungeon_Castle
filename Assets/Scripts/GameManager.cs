using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform PlayerSpwanPos;
    //player lives
    public GameObject player;
    public HealthBar healthbar;
    public int playerLives = 5;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        spwanPlayer();
        healthbar.setMaxHealth(playerLives);
        healthbar = GetComponent<HealthBar>();
        PlayerSpwanPos = GetComponentInChildren<Transform>();
    }

    private void Update()
    {
        if(playerLives == 0)
        {
            SceneManager.LoadScene("GameOver");
            Destroy(this.gameObject);
        }
    }

    public event UnityAction<int> onUpdateScore;
    public void UpdateScore(int num)
    {
      if(onUpdateScore != null)
        {
            onUpdateScore(num);
        }
    }

    public void spwanPlayer()
    {
        Transform _psp = PlayerSpwanPos;
        if(playerLives != 0)
        {
            Instantiate(player, _psp.position, _psp.rotation);
        }
    }

    public void takeDamage(int damage)
    {
        playerLives -= damage;
        healthbar.setHealth(playerLives);
    }
}
