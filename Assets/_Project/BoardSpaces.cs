using UnityEngine;
using Zenject;

namespace Project
{
  public class GoToJailSpace : BoardSpace
  {
    [Inject(Id = "jail Location ID")] int _jailLocationID;

    public override void Process(Player player)
    {
      player.LocationID = _jailLocationID;
      player.IsInJail = true;
    }
  }

  public class UtilitySpace : BoardSpace
  {
    public override void Process(Player player)
    {
      Debug.Log("pay utility bill");
    }
  }

  public class TaxSpace : BoardSpace
  {
    public override void Process(Player player)
    {
      _UIManager.EnablePayButton(onPayRent);
    }

    void onPayRent(Player player)
    {
      if (player.Wealth >= tax)
      {
        player.Wealth -= tax;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
    int tax = 200;
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

  public class BoardSpace
  {
    [Inject] public int ID { get; private set; }
    [Inject] protected UIManager _UIManager;
    public virtual void Process(Player player) { }
  }
}