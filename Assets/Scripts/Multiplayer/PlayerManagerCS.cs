using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

/// <summary>
/// The script which manages most of the player's syncing with photon
/// </summary>
public class PlayerManagerCS : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Fields 

    Animator anim;

    [Tooltip("The loacal player instance. Use this to know if the local player is represented in the scene")]
    public static GameObject LocalPlayerInstance;

    [Tooltip("The player's UI GameObject Prefab")]
    public GameObject PlayerUIPrefab;

    [Tooltip("All of the body parts in the player gets multiplied by this value")]
    public float healthMultiplier = 1;
    TakeDamageGeneric[] limbs;

    public GameObject Camera;

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            Debug.Log("Writing to stream");
            // We own this player: send the others our data
            // stream.SendNext writes to the serialized variable
            //stream.SendNext(Health);

            // For every health component in the heirarchy, write their health value to the server
            foreach (TakeDamageGeneric healthComp in limbs)
            {
                stream.SendNext(healthComp.currentHealth);
            }
        }
        else
        {
            Debug.Log("Reading stream");
            // Network player, receive data
            // stream.ReceiveNext reads the variable
            //Health = (float)stream.ReceiveNext();

            foreach (TakeDamageGeneric healthComp in limbs)
            {
                healthComp.currentHealth = (float)stream.ReceiveNext();
            }
        }
    }

    #endregion

    private void Awake()
    {
        // When levels are loaded the player instance does not get destroyed and thus does not require the game manager to reinstantiate
        DontDestroyOnLoad(gameObject);


        // Make an array with all of the player's mechs limbs and their health components
        limbs = GetComponentsInChildren<TakeDamageGeneric>();

        foreach (TakeDamageGeneric healthComp in limbs)
        {
            healthComp.health *= healthMultiplier;
        }

        // If this is not the local player instance 
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            Destroy(GetComponent<CameraController>());
            Destroy(GetComponent<MechController>());
            Destroy(GetComponent<Attack>());

            Debug.LogFormat("[0] is not the local player instance", gameObject.name);

            return;
        }
        // Keeps track of the local player instance to prevent instantiation when levels are synchronized
        LocalPlayerInstance = gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

        if(PlayerUIPrefab != null)
        {
            GameObject uiGo = Instantiate(PlayerUIPrefab);
            // When the player UI GameObject has been instantiated we now send it a message that calls the function "SetTarget" which is located in PlayerUI.cs
            // In the message we input the PlayerController target value as 'this'
            uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            // We require a receiver which does so that if there is no receiver that gets our messsage we get informed
            // We could use uiGo.GetComponenet<PlayerUI>().SetTarget(); instead, and it is generally recommended
        }
        else
        {
            Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUIPrefab reference on player Prefab.", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            Camera.SetActive(true);
        }
        else
        {
            Camera.SetActive(false);
            print("Incorrect PhotonView");
        }

        /*
        // If this is not the local player instance 
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }*/
    }

    private void OnDrawGizmos()
    {
        if(LocalPlayerInstance == gameObject)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 5);
        }
    }
}
