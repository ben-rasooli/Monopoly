using Zenject;

namespace Project
{
  public class Jail
  {
    public void SendToJail(Player player)
    {
      player.LocationID = _boardManager.GetJailSpace.ID;
      player.IsInJail = true;
      player.MoveSilently(_boardManager.GetJailSpace.Position);
      _gameManager.EndTurn();
    }

    public void ShowGetOutOfJail(Player player)
    {
      _UIManager.ShowGetOutOfJailPopup(onPayJail, onRollDouble);
    }

    #region dependencies
    [Inject] GameManager _gameManager;
    [Inject] BoardManager _boardManager;
    [Inject] UIManager_Controller _UIManager;
    #endregion

    #region details
    void onPayJail(Player player)
    {
      if (player.Wealth >= jailCost)
      {
        player.Wealth -= jailCost;
        _UIManager.EnableMoveButton();
        _UIManager.ShowMessage($"Paid ${jailCost}");
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
    int jailCost = 50;

    void onRollDouble(Player player)
    {
      var dice = _gameManager.RollDice();
      if (dice.Die_1 == dice.Die_2)
      {
        _UIManager.DisableMoveButton();
        _UIManager.EnableEndTurnButton();
        _boardManager.Move(player, dice);
        _UIManager.ShowMessage("Roll double succeed");
      }
      else
      {
        _UIManager.ShowError("Roll double failed");
        _gameManager.EndTurn();
      }
    }
    #endregion
  }
}
