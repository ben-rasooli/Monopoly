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

    public void Init()
    {
      Wealth = 1500;
      LocationID = 0;
    }

    public void Move(Vector3 position)
    {
      _controller.Move(position);
    }

    #region dependencies
    [Inject] Player_Controller _controller;
    #endregion
  }
}
