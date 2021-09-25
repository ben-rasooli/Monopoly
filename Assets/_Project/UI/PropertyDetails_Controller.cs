using Doozy.Engine.UI;
using TMPro;
using UnityEngine;

namespace Project
{
  public class PropertyDetails_Controller : MonoBehaviour
  {
    /// <summary>
    /// The property's details should be converted to string,
    /// and each detail should be in a single line.
    /// </summary>
    /// <param name="value"></param>
    public void Show(string propertyDetails)
    {
      _textUI.SetText(propertyDetails);
      _myUI.Show();
    }

    public void Hide() => _myUI.Hide();

    [SerializeField] UIView _myUI;
    [SerializeField] TextMeshProUGUI _textUI;
  }
}
