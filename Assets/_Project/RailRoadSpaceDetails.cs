using UnityEngine;

namespace Project
{
  [CreateAssetMenu(fileName = "Default RailRoad Space Details", menuName = "Project/RailRoadSpaceDetails", order = 0)]
  public class RailRoadSpaceDetails : ScriptableObject
  {
    public int Price;
    public int Rent;
    public int MortgageValue;
  }
}
