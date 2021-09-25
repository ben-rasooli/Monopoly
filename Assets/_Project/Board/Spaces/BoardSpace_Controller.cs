using UnityEngine;
using Sirenix.OdinInspector;

namespace Project
{
  public class BoardSpace_Controller : MonoBehaviour
  {
    public BoardSpaceType SpaceType;
    [ShowIf(nameof(SpaceType), BoardSpaceType.Land)] public LandSpaceDetails LandDetails;
    [ShowIf(nameof(SpaceType), BoardSpaceType.RailRoad)] public RailRoadSpaceDetails RailRoadDetails;

    public Vector3 Position => transform.position;
  }

  public enum BoardSpaceType
  {
    GO,
    Jail,
    Parking,
    GoToJail,
    Utility,
    IncomeTax,
    SuperTax,
    Chance,
    CommunityChest,
    Land,
    RailRoad
  }
}
