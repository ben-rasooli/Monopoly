using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Project
{
  public class Camera_Controller : MonoBehaviour
  {
    public void ChangeCamera(string playerName)
    {
      foreach (var pair in _virtualCameras)
        if (pair.Player_Controller.Name == playerName)
          pair.VCamera.enabled = true;
        else
          pair.VCamera.enabled = false;
    }

    [SerializeField] List<PlayerCameraPair> _virtualCameras;
  }

  [System.Serializable]
  public struct PlayerCameraPair
  {
    public Player_Controller Player_Controller;
    public CinemachineVirtualCamera VCamera;
  }
}
