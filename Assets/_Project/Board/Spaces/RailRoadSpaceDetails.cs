using UnityEngine;

namespace Project
{
  [CreateAssetMenu(fileName = "Default RailRoad Space Details", menuName = "Project/RailRoadSpaceDetails", order = 0)]
  public class RailRoadSpaceDetails : ScriptableObject
  {
    public string Name;
    public int Price;
    public int Rent;
    public int MortgageValue;

    public override string ToString()
    {
      return
        $"{Name}\n{Price}\n{Rent}\n{MortgageValue}";
    }
  }
}
