using System;
using UnityEngine;
using Zenject;

namespace Project
{
  public class Player
  {
    [Inject] public string Name { get; set; }

    public int Wealth { get; set; }

    public int LocationID { get; set; }

    public bool IsInJail { get; set; }

    public Transform Transform => _controller.transform;

    public void Init()
    {
      Wealth = 1500;
      LocationID = 0;
    }

    /// <summary>
    /// It uses the navmesh agent to move the player.
    /// It also notifies the UI Manager to show and hide the UI controls.
    /// </summary>
    /// <param name="position"></param>
    public void Move(Vector3 position)
    {
      _controller.Move(position, _UIManager_Controller.OnPlayerStartMoving, _UIManager_Controller.OnPlayerStopedMoving);
    }

    #region dependencies
    [Inject] Player_Controller _controller;
    [Inject] UIManager_Controller _UIManager_Controller;
    #endregion
  }
}
