using UnityEngine;

public class PlayerBehaviorManager : MonoBehaviour
{
    [Header("Player behavior")] 
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private CameraInputDetector cameraInputDetector;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerInputDetector playerInputDetector;

    public void EnablePlayerBehavior()
    {
        playerManager.enabled = true;
        cameraController.enabled = true;
        playerController.enabled = true;
        cameraInputDetector.enabled = true;
        playerInputDetector.enabled = true;
    }
    
    public void DisablePlayerBehavior()
    {
        playerManager.enabled = false;
        cameraController.enabled = false;
        playerController.enabled = false;
        cameraInputDetector.enabled = false;
        playerInputDetector.enabled = false;
    }
}