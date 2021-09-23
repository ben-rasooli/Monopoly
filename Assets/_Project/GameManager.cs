using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project
{
  public class GameManager : IInitializable
  {
    public TurnStats TurnStats { get; set; } = new TurnStats();

    public void Move()
    {
      Dice dice = rollDice();
      TurnStats.Dice = dice;
      _boardManager.Move(_currentPlayer, dice);
    }

    public void EndTurn()
    {
      TurnStats.Index = _turnCount;
      TurnStats.PlayerName = _currentPlayer.Name;
      _turnStatsList.Add(TurnStats);
      _currentPlayer = _playerQueue.Dequeue();
      _UIManager.Configure(_currentPlayer);
      if (_currentPlayer.IsInJail)
        _UIManager.EnablePayButton(onPayRent);
      _playerQueue.Enqueue(_currentPlayer);
    }

    public void FinishGame()
    {
      foreach (var item in _turnStatsList)
      {
        Debug.Log(item);
      }
    }

    public void Restart()
    {
      Initialize();
    }

    #region dependencies
    [Inject] List<Player> _players;
    [Inject] BoardManager _boardManager;
    [Inject] UIManager _UIManager;
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
      _turnCount = 1;
      _UIManager.Configure(_playerQueue.Peek());
    }

    #region details
    Queue<Player> _playerQueue = new Queue<Player>();
    List<TurnStats> _turnStatsList = new List<TurnStats>();
    Player _currentPlayer;
    int _turnCount;

    void onPayRent(Player player)
    {
      if (player.Wealth >= jailCost)
      {
        player.Wealth -= jailCost;
        _UIManager.DisablePayButton();
        _UIManager.EnableEndTurnButton();
      }
      else
        _UIManager.ShowError("insufficient funds");
    }
    int jailCost = 200;

    Dice rollDice()
    {
      return new Dice
      {
        Die_1 = Random.Range(1, 7),
        Die_2 = Random.Range(1, 7)
      };
    }
    #endregion
  }
}