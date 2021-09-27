using System;
using System.Text;
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
      _stringBuilder.Clear();
      _stringBuilder.Append($"{Name}\nPrice\t\t\t${Price}\nRent");
      if (Rent.Length > 10) _stringBuilder.Append(newLine);
      _stringBuilder.Append($"{Rent}\nMortgage Value\t${MortgageValue}");
      return _stringBuilder.ToString();
    }
    StringBuilder _stringBuilder = new StringBuilder();
    const string newLine = "\n";
  }
}
