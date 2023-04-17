using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    #region Fields

    [Tooltip("UI Text to display Player's Name")]
    [SerializeField]
    TextMeshProUGUI playerNameText;

    [Tooltip("UI slider to display player's health")]
    [SerializeField]
    Slider playerHealthSlider;

    [Tooltip("Pixel offset from the player target")]
    [SerializeField]
    Vector3 screenOffset = new(0, 30, 0);

    float characterHeight = 0;
    Transform targetTransform;
    Renderer targetRenderer;
    CanvasGroup canvasGroup;
    Vector3 targetPosition;

    PlayerManagerCS targetPlayer;

    #endregion

    #region Methods

    private void Awake()
    {
        // If a new scene is loaded the UI does not get destroyed
        DontDestroyOnLoad(gameObject);

        // Since the UI needs to be a child of a Canvas you find the Canvas in the current scene and set that as the parent
        // Definitely a bit of hardcoding here
        transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(), false);

        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetTarget(PlayerManagerCS target)
    {
        if (target == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.", this);
            return;
        }
        targetPlayer = target;
        if (playerNameText != null)
        {
            playerNameText.text = target.photonView.Owner.NickName;
        }

        targetTransform = target.transform;
        targetRenderer = target.GetComponent<Renderer>();
        characterHeight = target.transform.lossyScale.y * 2;
    }

    void Update()
    {
        // Maybe not super efficient to check in the update function if the target player has been destroyed (player left) but it works for now
        if (targetPlayer == null)
        {
            Destroy(gameObject);
            return;
        }

        // Reflect the Player Health
        if (playerHealthSlider != null)
        {
            //Debug.LogFormat("The health of {0} is {1}", targetPlayer.gameObject.name, targetPlayer.Health);
            playerHealthSlider.value = targetPlayer.Health;
        }
    }

    private void LateUpdate()
    {
        // Do not show the UI if we are not visible to the camera, thus avoiding potential bugs with seeing the UI but not the player itself.
        if (targetRenderer != null)
        {
            canvasGroup.alpha = targetRenderer.isVisible ? 1 : 0;
        }

        // Follow the target GameObject on screen
        if (targetTransform != null)
        {
            targetPosition = targetTransform.position;
            targetPosition.y += characterHeight;
            transform.position = Camera.main.WorldToScreenPoint(targetPosition) + screenOffset;
        }
    }

    #endregion
}
