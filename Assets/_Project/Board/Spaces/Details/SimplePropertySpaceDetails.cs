using UnityEngine;

namespace Project
{
  [CreateAssetMenu(fileName = "Default Simple Property Space Details", menuName = "Project/SimplePropertySpaceDetails", order = 0)]
  public class SimplePropertySpaceDetails : ScriptableObject
  {
    public string Name;
    public int Price;
    public string Rent;
    public int MortgageValue;

    public override string ToString()
    {
      return
        $"{Name}\nPrice\t\t\t${Price}\nRent\n{Rent}\nMortgage Value\t${MortgageValue}";
    }
  }
}
