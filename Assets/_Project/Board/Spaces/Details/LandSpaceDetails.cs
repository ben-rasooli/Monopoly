using UnityEngine;

namespace Project
{
  [CreateAssetMenu(fileName = "Default Land Space Details", menuName = "Project/LandSpaceDetails", order = 0)]
  public class LandSpaceDetails : ScriptableObject
  {
    public string Name;
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

    public override string ToString()
    {
      return
        $"{Name}\nPrice\t\t\t${Price}\nRent\t\t\t${BaseRent}\nWith 1 House\t${RentWithOneHouse}" +
        $"\nWith 2 Houses\t${RentWithTwoHouses}\nWith 3 Houses\t${RentWithThreeHouses}\nWith 4 Houses\t${RentWithFourHouses}" +
        $"\nWith Hotel\t\t${RentWithHotel}\nMortgage Value\t${MortgageValue}\nHouses Cost ${HouseCost} each\nHotel, ${HotelCost} + 4 Houses";
    }
  }
}