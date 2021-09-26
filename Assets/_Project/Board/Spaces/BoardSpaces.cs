using UnityEngine;
using Zenject;

namespace Project
{
  public class BoardSpace
  {
    public Vector3 Position => BoardSpace_Controller.Position;
    public BoardSpaceType SpaceType => BoardSpace_Controller.SpaceType;
    public string DisplayDetails => BoardSpace_Controller.BoardSpaceDisplayDetails;
    public virtual void Process(Player player) { }
    [Inject] public int ID { get; private set; }
    [Inject] public BoardSpace_Controller BoardSpace_Controller;
    [Inject] protected UIManager_Controller _UIManager;
    [Inject] protected GameManager _gameManager;
  }

  public class GoToJailSpace : BoardSpace
  {
    [Inject] Jail _jail;

    public override void Process(Player player)
    {
      _jail.SendToJail(player);
    }
  }

  public class IncomeTaxSpace : BoardSpace
  {
    public override void Process(Player player)
    {
      _UIManager.EnablePayButton(onPayRent, taxAmount);
    }

    void onPayRent(Player player)
    {
      if (player.Wealth >= taxAmount)
      {
        player.Wealth -= taxAmount;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
    int taxAmount = 200;
  }

  public class SuperTaxSpace : BoardSpace
  {
    public override void Process(Player player)
    {
      _UIManager.EnablePayButton(onPayRent, taxAmount);
    }

    void onPayRent(Player player)
    {
      if (player.Wealth >= taxAmount)
      {
        player.Wealth -= taxAmount;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
    int taxAmount = 100;
  }

  public class ChanceSpace : BoardSpace
  {
    public override void Process(Player player)
    {
      Debug.Log("draw chance card");
    }
  }

  public class CommunityChestSpace : BoardSpace
  {
    public override void Process(Player player)
    {
      Debug.Log("draw community card");
    }
  }
}