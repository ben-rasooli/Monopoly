using System;

namespace Project
{
  [Serializable]
  public struct TurnStats
  {
    public int Index;
    public string PlayerName;
    public int PlayerLocationID;
    public Dice Dice;

    public override string ToString()
    {
      return $"Turn:[{Index}] - {Dice} - [{PlayerName}] Location:[{PlayerLocationID}]";
    }
  }
}
