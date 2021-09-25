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
          enablePurchaseButton(player);
          break;
        case State.Owned:
          if (Owner != player)
            enablePayButton(player);
          break;
        case State.Mortgaged:
          break;

      }
    }
    protected State _state;

    protected abstract void enablePurchaseButton(Player player);
    protected abstract void enablePayButton(Player player);

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

    protected override void enablePurchaseButton(Player player)
    {
      _UIManager.EnablePurchaseButton(onPurchaseProperty, _details);
    }

    protected override void enablePayButton(Player player)
    {
      _UIManager.EnablePayButton(onPayRent, Owner, _details.BaseRent);
    }

    void onPurchaseProperty(Player player)
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

    void onPayRent(Player player, Player owner)
    {
      int rent = _details.BaseRent;
      if (player.Wealth >= rent)
      {
        player.Wealth -= rent;
        owner.Wealth += rent;
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

    protected override void enablePurchaseButton(Player player)
    {
      _UIManager.EnablePurchaseButton(onPurchaseProperty, _details);
    }

    protected override void enablePayButton(Player player)
    {
      _UIManager.EnablePayButton(onPayRent, Owner, _details.Rent);
    }

    void onPurchaseProperty(Player player)
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

    void onPayRent(Player player, Player owner)
    {
      int rent = _details.Rent;
      if (player.Wealth >= rent)
      {
        player.Wealth -= rent;
        owner.Wealth += rent;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
  }
}