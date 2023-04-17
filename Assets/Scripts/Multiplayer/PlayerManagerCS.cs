using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayerManagerCS : MonoBehaviourPunCallbacks, IPunObservable
{
    #region Fields 

    Rigidbody rb;

    [SerializeField] float MovementSpeed = 1;

    Animator anim;

    [Tooltip("The loacal player instance. Use this to know if the local player is represented in the scene")]
    public static GameObject LocalPlayerInstance;

    [Tooltip("The player's UI GameObject Prefab")]
    public GameObject PlayerUIPrefab;

    public float Health = 100f;

    #endregion

    #region IPunObservable implementation

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            Debug.Log("Writing to stream");
            // We own this player: send the others our data
            // stream.SendNext writes to the serialized variable
            stream.SendNext(Health);
        }
        else
        {
            Debug.Log("Reading stream");
            // Network player, receive data
            // stream.ReceiveNext reads the variable
            Health = (float)stream.ReceiveNext();
        }
    }

    #endregion

    private void Awake()
    {
        // Keeps track of the local player instance to prevent instantiation when levels are synchronized
        if(photonView.IsMine)
        {
            PlayerManagerCS.LocalPlayerInstance = gameObject;
        }
        // When levels are loaded the player instance does not get destroyed and thus does not require the game manager to reinstantiate
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

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

    private void OnTriggerEnter(Collider other)
    {
        if(!photonView.IsMine)
        {
            return;
        }

        if(other.name.Contains("Sword"))
        {
            Debug.Log("Received damagae");

            Health -= 10f;

            if(Health <= 0 )
            {
                // Make the player leave the game room
                GameManagerCScript.Instance.LeaveRoom();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If this is not the local player instance 
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 input = new(x, 0, y);

        rb.MovePosition(transform.position + input * Time.deltaTime * MovementSpeed);

        if (Input.GetMouseButton(0))
        {
            // Attack
            //anim.Play("SwordSwing");
            anim.ResetTrigger("Attack");
            anim.SetTrigger("Attack");
        }
    }
}
