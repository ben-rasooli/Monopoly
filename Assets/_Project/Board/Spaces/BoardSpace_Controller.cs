using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Project
{
  public class BoardSpace_Controller : MonoBehaviour
  {
    public BoardSpaceType SpaceType;

    [TextArea]
    [HideIf("@SpaceType == BoardSpaceType.Land || SpaceType == BoardSpaceType.RailRoad || SpaceType == BoardSpaceType.Utility")]
    public string BoardSpaceDisplayDetails;

    [ShowIf(nameof(SpaceType), BoardSpaceType.Land)]
    public LandSpaceDetails LandDetails;

    [ShowIf("@SpaceType == BoardSpaceType.RailRoad || SpaceType == BoardSpaceType.Utility")]
    public SimplePropertySpaceDetails PropertyDetails;

    public Vector3 Position => transform.position;

    public List<BoardSpace_Controller> ConnectedSpaces;
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
