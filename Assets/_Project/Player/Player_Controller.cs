using System;
using UnityEngine;

namespace Project
{
  public class Player_Controller : MonoBehaviour
  {
    public string Name;

    internal void Move(Vector3 position)
    {
      transform.position = position;
    }
  }
}
