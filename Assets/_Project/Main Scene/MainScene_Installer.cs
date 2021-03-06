using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project
{
  public class MainScene_Installer : MonoInstaller
  {
    [SerializeField] List<BoardSpace_Controller> _boardSpace_Controllers;
    [SerializeField] List<Player_Controller> _player_Controller;
    [SerializeField] Camera_Controller _camera_Controller;
    [SerializeField] int _jailLocationID;

    public override void InstallBindings()
    {
      Container.BindInstance(_boardSpace_Controllers).AsCached().WhenInjectedInto<BoardManager_Installer>();
      Container.BindInstance(_jailLocationID).AsCached().WhenInjectedInto<BoardManager_Installer>();
      Container.BindInterfacesAndSelfTo<BoardManager>().FromSubContainerResolve().ByInstaller<BoardManager_Installer>().AsSingle();
      Container.Bind<Jail>().AsSingle();
      Container.Bind<UIManager_Controller>().FromComponentsInHierarchy().AsSingle();
      bindPlayers();
      Container.BindInstance(_camera_Controller).AsSingle();
      Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
    }

    void bindPlayers()
    {
      foreach (var playerController in _player_Controller)
        Container.Bind<Player>().AsCached().WithArguments(playerController.Name, playerController);
    }
  }
}