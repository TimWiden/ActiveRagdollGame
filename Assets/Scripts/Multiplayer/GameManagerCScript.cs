using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class GameManagerCScript : MonoBehaviourPunCallbacks
{
    public static GameManagerCScript Instance;

    [SerializeField]
    Vector3 Spawnpoint = new(0f, 5f, 0f);

    [Tooltip("The prefab to use for representing the player")]
    public GameObject PlayerPrefab;

    private void Start()
    {
        Instance = this;

        if (PlayerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager", this);
        }

        if (PlayerManagerCS.LocalPlayerInstance == null) // if there is no previous instance of the local player
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            // We are in a room
            // Spawn a character for the local player
            // It gets synced by using PhotonNetwork.Instantiate
            PhotonNetwork.Instantiate(PlayerPrefab.name, Spawnpoint, Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    #region Photon Callbacks

    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene..
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerEnteredRoom(Player player)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", player.NickName); // not seen if you're the player connecting

        /*since I dont have rooms with adaptable player count it is uneccessary to load a larger scene
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient (0)", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadMap();
        }*/
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", player.NickName); // seen when other disconnects

        /* since I dont have rooms with adaptable player count it is uneccessary to load a smaller scene
        if (PhotonNetwork.IsMasterClient)
        {
            Debug LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

            LoadMap();
        }*/
    }

    #endregion

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #region Private Methods

    void LoadMap()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork: Trying to Load a level but we are not the master Client");
            return;
        }

        Debug.LogFormat("Loading Game", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Game"); // We don't use SceneManager.LoadScene since using PhotonNetwork.LoadLevel automatically syncs the scenes for all players on that network
    }

    #endregion
}