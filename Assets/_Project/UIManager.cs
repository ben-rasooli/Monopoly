using System;
using Doozy.Engine.UI;
using UnityEngine;
using Zenject;

namespace Project
{
  public class UIManager : MonoBehaviour
  {
    public void EnablePurchaseButton(Action<Player> purchaseAction)
    {
      _purchaseButton.EnableButton();
      _purchaseButton.Button.onClick.RemoveAllListeners();
      _purchaseButton.Button.onClick.AddListener(() => { purchaseAction(_player); });
    }

    public void DisablePurchaseButton()
    {
      _purchaseButton.DisableButton();
    }

    public void EnablePayButton(Action<Player> payAction)
    {
      DisableEndTurnButton();
      _payButton.EnableButton();
      _payButton.Button.onClick.RemoveAllListeners();
      _payButton.Button.onClick.AddListener(() => { payAction(_player); });
    }

    public void DisablePayButton()
    {
      _payButton.DisableButton();
    }

    public void EnableMoveButton(Action<Player> moveAction)
    {
      _moveButton.EnableButton();
      _moveButton.Button.onClick.RemoveAllListeners();
      _moveButton.Button.onClick.AddListener(() => { moveAction(_player); });
    }

    public void DisableMoveButton()
    {
      _moveButton.DisableButton();
    }

    public void EnableEndTurnButton()
    {
      _endTurnButton.EnableButton();
    }

    public void DisableEndTurnButton()
    {
      _endTurnButton.DisableButton();
    }

    public void ShowError(string message)
    {
      Debug.Log(message);
    }

    public void Configure(Player player)
    {
      _player = player;
      _endTurnButton.DisableButton();
      _purchaseButton.DisableButton();
      _payButton.DisableButton();
      _moveButton.EnableButton();

      // set the title
    }

    #region dependencies
    [Inject] GameManager _gameManager;
    [Inject] BoardManager _boardManager;
    [Inject(Id = "end turn button")] UIButton _endTurnButton;
    [Inject(Id = "purchase button")] UIButton _purchaseButton;
    [Inject(Id = "pay button")] UIButton _payButton;
    [Inject(Id = "move button")] UIButton _moveButton;
    [Inject(Id = "finish game button")] UIButton _finishGameButton;
    #endregion

    void OnEnable()
    {
      _endTurnButton.Button.onClick.AddListener(onEndTurn);
    }

    private void OnDisable()
    {
      _endTurnButton.Button.onClick.RemoveAllListeners();
    }

    #region details
    Player _player;

    void onMove()
    {
      DisableMoveButton();
      EnableEndTurnButton();
      _gameManager.Move();
    }

    void onEndTurn()
    {
      DisableEndTurnButton();
      _gameManager.EndTurn();
    }

    void onFinishGame()
    {
      Debug.Log("Game Over");
    }

    void onShowAssets()
    {
      var assets = _boardManager.GetPlayerAssets(_player);
      // configure the Player assets menu
      // show the menu
    }
    #endregion
  }
}
