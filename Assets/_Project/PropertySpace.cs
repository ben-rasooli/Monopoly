using Zenject;

namespace Project
{
  public abstract class PropertySpace : BoardSpace
  {
    public Player Owner;

    public override void Process(Player player)
    {
      switch (_state)
      {
        case State.Unowned:
          _UIManager.EnablePurchaseButton(onPurchaseProperty);
          break;
        case State.Owned:
          if (Owner != player)
            _UIManager.EnablePayButton(onPayRent);
          break;
      }
    }
    protected State _state;

    protected abstract void onPurchaseProperty(Player player);
    protected abstract void onPayRent(Player player);

    protected enum State
    {
      Unowned,
      Owned,
      Mortgaged
    }
  }

  public class LandSpace : PropertySpace
  {
    [Inject] LandSpaceDetails _details;

    protected override void onPurchaseProperty(Player player)
    {
      if (player.Wealth >= _details.Price)
      {
        player.Wealth -= _details.Price;
        Owner = player;
        _state = State.Owned;
        _UIManager.DisablePurchaseButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }

    protected override void onPayRent(Player player)
    {
      int rent = _details.BaseRent;
      if (player.Wealth >= rent)
      {
        player.Wealth -= rent;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
  }

  public class RailRoadSpace : PropertySpace
  {
    [Inject] RailRoadSpaceDetails _details;

    protected override void onPurchaseProperty(Player player)
    {
      if (player.Wealth >= _details.Price)
      {
        player.Wealth -= _details.Price;
        Owner = player;
        _state = State.Owned;
        _UIManager.DisablePurchaseButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }

    protected override void onPayRent(Player player)
    {
      if (player.Wealth >= _details.Rent)
      {
        player.Wealth -= _details.Rent;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
  }
}