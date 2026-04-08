using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerManager : MonoBehaviour
{
    public List<Sprite> playerSprite;
    public List<LocalMultiplayerController> players;
    public List<LocalMultiplayerController> playerScripts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayerJoin(PlayerInput playerInput)
    {
        LocalMultiplayerController player = playerInput.GetComponent<LocalMultiplayerController>();

        players.Add(player);
        player.manager = this;

        SpriteRenderer sr = player.GetComponent<SpriteRenderer>();
        sr.sprite = playerSprite[player.playerInput.playerIndex];
    }

    public void PlayerAttacking(PlayerInput playerInput)
    {
        LocalMultiplayerController player = playerInput.GetComponent<LocalMultiplayerController>();

        for (int i = 0; i < players.Count; i++)
        {
            if (player == players[i]) continue;

            if (Vector2.Distance(player.transform.position, players[i].transform.position) < 0.5f)
            {
                Debug.Log("Player " + playerInput.playerIndex + " hit player " + players[i].playerInput.playerIndex);

                players[i].GotHit();
            }
        }
    }
        
}
