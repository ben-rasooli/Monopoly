using System.Collections.Generic;
using System.Linq;
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
        player.Wealth += 200;

      player.LocationID = nextLocationID;
      _boardSpaces[nextLocationID].Process(player);
    }

    public List<PropertySpace> GetPlayerAssets(Player player)
    {
      return _propertySpaces.Where(space => space.Owner == player).ToList();
    }

    #region dependencies
    [Inject] List<BoardSpace> _boardSpaces;
    [Inject] List<PropertySpace> _propertySpaces;
    #endregion

    #region details
    #endregion
  }

}