using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project
{
  public class GameManager : IInitializable
  {
    public int TurnCount { get; private set; }

    public List<TurnStats> TurnStatsList => _turnStatsList;

    public void Move(out bool didRollDouble)
    {
      Dice dice = RollDice();
      didRollDouble = dice.IsDouble;

      bool shouldGoToJail = false;
      if (dice.IsDouble)
      {
        switch (_playerDoubleDiceState)
        {
          case PlayerDoubleDiceState.NoDouble:
            _playerDoubleDiceState = PlayerDoubleDiceState.FirstDouble;
            break;
          case PlayerDoubleDiceState.FirstDouble:
            _playerDoubleDiceState = PlayerDoubleDiceState.SecondDouble;
            break;
          case PlayerDoubleDiceState.SecondDouble:
            shouldGoToJail = true;
            break;
        }
      }
      else
      {
        _UIManager.DisableMoveButton();
        _UIManager.EnableEndTurnButton();
      }

      if (shouldGoToJail)
        _jail.SendToJail(_currentPlayer);
      else
        _boardManager.Move(_currentPlayer, dice);
    }

    public void RewardPlayer(int amount)
    {
      _currentPlayer.Wealth += amount;
      _UIManager.UpdatePlayerWealth();
      _UIManager.ShowMessage($"Received ${amount}");
    }

    public Dice RollDice()
    {
      var result = new Dice
      {
        Die_1 = Random.Range(1, 7),
        Die_2 = Random.Range(1, 7)
      };

      _UIManager.ShowMessage(result.ToString());

      _turnStats.Index = TurnCount;
      _turnStats.PlayerName = _currentPlayer.Name;
      _turnStats.PlayerLocationID = _currentPlayer.LocationID;
      _turnStats.Dice = result;
      _turnStatsList.Add(_turnStats);

      return result;
    }

    public void EndTurn()
    {
      TurnCount++;
      _playerDoubleDiceState = PlayerDoubleDiceState.NoDouble;
      runTurn();
    }

    public void Restart()
    {
      Initialize();
    }

    #region dependencies
    [Inject] List<Player> _players;
    [Inject] BoardManager _boardManager;
    [Inject] UIManager_Controller _UIManager;
    [Inject] Camera_Controller _camera_Controller;
    [Inject] Jail _jail;
    #endregion

    public void Initialize()
    {
      _turnStatsList.Clear();
      _playerQueue.Clear();
      foreach (var player in _players)
      {
        player.Init();
        _playerQueue.Enqueue(player);
      }
      TurnCount = 1;
      runTurn();
    }

    #region details
    Queue<Player> _playerQueue = new Queue<Player>();
    List<TurnStats> _turnStatsList = new List<TurnStats>();
    Player _currentPlayer;
    PlayerDoubleDiceState _playerDoubleDiceState;
    TurnStats _turnStats = new TurnStats();

    void runTurn()
    {
      _currentPlayer = _playerQueue.Dequeue();
      _playerQueue.Enqueue(_currentPlayer);
      _UIManager.Configure(_currentPlayer);
      _camera_Controller.ChangeCamera(_currentPlayer.Name);
      if (_currentPlayer.IsInJail)
        _jail.ShowGetOutOfJail(_currentPlayer);
      else
        _UIManager.EnableMoveButton();
    }

    enum PlayerDoubleDiceState
    {
      NoDouble,
      FirstDouble,
      SecondDouble
    }
    #endregion
  }
}