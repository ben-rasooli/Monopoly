using System;

namespace Project
{
  [Serializable]
  public struct Dice
  {
    public int Die_1;
    public int Die_2;

    public bool IsDouble => Die_1 == Die_2;

    public override string ToString()
    {
      return IsDouble ? $"Dice:[Double {Die_1}]" : $"Dice:[{Die_1} + {Die_2}]";
    }
  }
}
