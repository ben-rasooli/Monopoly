using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Project
{
  public abstract class PropertySpace : BoardSpace
  {
    public Player Owner;
    public abstract string Details { get; }

    public void SetConnectedProperties(List<PropertySpace> connectedProperties)
    {
      _connectedProperties = connectedProperties;
    }

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
    protected List<PropertySpace> _connectedProperties;
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
    public override string Details => $"Location:  {ID}\t\tName: {_details.Name}\t\t\tRent: ${_details.BaseRent}";

    [Inject] LandSpaceDetails _details;

    protected override void enablePurchaseButton(Player player)
    {
      _UIManager.EnablePurchaseButton(onPurchaseProperty, _details.Price);
    }

    protected override void enablePayButton(Player player)
    {
      _UIManager.EnablePayButton(this.onPayRent, Owner, calculateRent());
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
      int rent = calculateRent();
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

    int calculateRent()
    {
      int result = _details.BaseRent;
      // if all connected properties are own by the same owner, double the rent
      if (!_connectedProperties.Any(property => property.Owner != Owner))
        result *= 2;
      return result;
    }
  }

  public class RailRoadSpace : PropertySpace
  {
    public override string Details => $"Location: {ID}\t\tName: {_details.Name}\t\tRent: ${_details.Rent}";

    [Inject] SimplePropertySpaceDetails _details;

    protected override void enablePurchaseButton(Player player)
    {
      _UIManager.EnablePurchaseButton(onPurchaseProperty, _details.Price);
    }

    protected override void enablePayButton(Player player)
    {
      _UIManager.EnablePayButton(onPayRent, Owner, calculateRent());
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
      int rent = calculateRent();
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

    int calculateRent()
    {
      int result = 25;
      int numberOfConnectedProperties = _connectedProperties.Count(property => property.Owner == Owner);
      switch (numberOfConnectedProperties)
      {
        case 1: result += 25; break;
        case 2: result += 75; break;
        case 3: result += 175; break;
      }
      return result;
    }
  }

  public class UtilitySpace : PropertySpace
  {
    public override string Details => $"Location: {ID}\t\tName: {_details.Name}\t\tRent: ${_details.Rent}";

    [Inject] SimplePropertySpaceDetails _details;

    protected override void enablePurchaseButton(Player player)
    {
      _UIManager.EnablePurchaseButton(onPurchaseProperty, _details.Price);
    }

    protected override void enablePayButton(Player player)
    {
      _UIManager.EnablePayButton(onPayRent, Owner, calculateRent());
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
      int rent = _calculatedRent;
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
    int _calculatedRent;

    int calculateRent()
    {
      int result = 0;
      var dice = _gameManager.RollDice();
      int numberOfConnectedProperties = _connectedProperties.Count(property => property.Owner == Owner);
      if (numberOfConnectedProperties == 1)
        result = 10 * (dice.Die_1 + dice.Die_2);
      else
        result = 4 * (dice.Die_1 + dice.Die_2);
      _calculatedRent = result;
      return result;
    }
  }
}