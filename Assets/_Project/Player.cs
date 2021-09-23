using System;

namespace Project
{
  public class Player
  {
    public string Name { get; set; }

    public int Wealth { get; set; }

    public int LocationID { get; set; }

    public bool IsInJail { get; set; }

    public void Init()
    {
      Wealth = 1500;
      LocationID = 0;
    }

    #region details
    #endregion
  }
}

// public override bool Equals(object obj)
// {
//   if (obj is null)
//     return false;
//   else
//     return obj is Player other && other.Name == Name;
// }

// public override int GetHashCode()
// {
//   return Name.GetHashCode();
// }
