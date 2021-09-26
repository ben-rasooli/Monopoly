using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace Project
{
  public class BoardManager : IInitializable
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

    public string GetSpaceDetails(int locationID)
    {
      return _boardSpaces[locationID].DisplayDetails;
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
    [Inject] List<BoardSpace_Controller> _boardSpace_Controllers;
    [Inject] int _jailLocationID;
    #endregion

    public void Initialize()
    {
      // Here we are trying to find the connected properties for each property.
      // There is a 1 to 1 relation between the _boardSpaces and _boardSpace_Controllers.
      // Also, the _propertySpaces is a subset of _boardSpaces.
      foreach (var property in _propertySpaces)
      {
        var spaces = new List<PropertySpace>();
        foreach (var controller in property.BoardSpace_Controller.ConnectedSpaces)
        {
          int indexOfProperty = _boardSpaces.FindIndex(space => space.BoardSpace_Controller == controller);
          spaces.Add((PropertySpace)_boardSpaces[indexOfProperty]);
        }
        property.SetConnectedProperties(spaces);
      }
    }

    #region details
    int _completeLapRewardAmount = 200;
    #endregion
  }

}