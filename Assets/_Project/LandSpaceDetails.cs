using UnityEngine;

namespace Project
{
  [CreateAssetMenu(fileName = "Default Land Space Details", menuName = "Project/LandSpaceDetails", order = 0)]
  public class LandSpaceDetails : ScriptableObject
  {
    public int Price;
    public int BaseRent;
    public int RentWithOneHouse;
    public int RentWithTwoHouses;
    public int RentWithThreeHouses;
    public int RentWithFourHouses;
    public int RentWithHotel;
    public int MortgageValue;
    public int HouseCost;
    public int HotelCost;
  }
}