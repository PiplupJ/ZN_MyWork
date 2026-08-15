using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject respawnPoint;

    public event Action Respawn;

    public InputAction playerRespawn;     //リスポーンボタン

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("GameManager already exists.");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        var playerActionMap = InputManager.Instance.PlayerActionMap;

        this.playerRespawn = playerActionMap.FindAction("Respawn");

        RespawnPlayer();
    }

    private void Update()
    {
        if (PlayerHP.instance.health <= 0 && this.playerRespawn.WasPressedThisFrame()) {
            Debug.Log("Respawn");
            Respawn?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
        }
    }

    private void OnEnable()
    {
        //UI_MainGame.instance.Respawn += RespawnPlayer;
    }

    private void OnDisable()
    {
        //UI_MainGame.instance.Respawn -= RespawnPlayer;
    }

    //プレイヤーの復活
    private void RespawnPlayer()
    {
        Instantiate(playerPrefab, respawnPoint.transform.position, Quaternion.identity);
        PlayerHP.instance.NewLife();
    }


}
