using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Project
{
  public class BoardManager
  {
    public void Move(Player player, Dice dice)
    {
      int numberOfSpaceToMove = dice.Die_1 + dice.Die_2;
      int currentLocationID = player.LocationID;
      int nextLocationID = (currentLocationID + numberOfSpaceToMove) % _boardSpaces.Count; // wrap around when reach end of the list

      if (nextLocationID < currentLocationID)
        _gameManager.RewardPlayer(_completeLapRewardAmount);

      player.LocationID = nextLocationID;
      player.Move(_boardSpaces[nextLocationID].Position);
      _boardSpaces[nextLocationID].Process(player);
    }

    public BoardSpace GetJailSpace => _boardSpaces[_jailLocationID];

    public List<PropertySpace> GetPlayerAssets(Player player)
    {
      return _propertySpaces.Where(space => space.Owner == player).ToList();
    }

    #region dependencies
    [Inject] GameManager _gameManager;
    [Inject] List<BoardSpace> _boardSpaces;
    [Inject] List<PropertySpace> _propertySpaces;
    [Inject] int _jailLocationID;
    #endregion

    #region details
    int _completeLapRewardAmount = 200;
    #endregion
  }

}