using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project
{
  public class BoardManager_Installer : Installer<BoardManager_Installer>
  {
    [Inject] List<BoardSpace_Controller> _boardSpace_Controllers;
    [Inject] int _jailLocationID;

    public override void InstallBindings()
    {
      Container.BindInterfacesAndSelfTo<BoardManager>().AsSingle().WithArguments(_jailLocationID);
      Container.BindInstance(_boardSpace_Controllers).AsCached();

      for (int i = 0; i < _boardSpace_Controllers.Count; i++)
      {
        BoardSpace_Controller spaceController = _boardSpace_Controllers[i];
        switch (_boardSpace_Controllers[i].SpaceType)
        {
          case BoardSpaceType.GO:
          case BoardSpaceType.Jail:
          case BoardSpaceType.Parking:
            Container.Bind<BoardSpace>().AsCached().WithArguments(i, spaceController);
            break;

          case BoardSpaceType.GoToJail:
            Container.Bind<BoardSpace>().To<GoToJailSpace>().AsCached().WithArguments(i, spaceController);
            break;

          case BoardSpaceType.IncomeTax:
            Container.Bind<BoardSpace>().To<IncomeTaxSpace>().AsCached().WithArguments(i, spaceController);
            break;

          case BoardSpaceType.SuperTax:
            Container.Bind<BoardSpace>().To<SuperTaxSpace>().AsCached().WithArguments(i, spaceController);
            break;

          case BoardSpaceType.Chance:
            Container.Bind<BoardSpace>().To<ChanceSpace>().AsCached().WithArguments(i, spaceController);
            break;

          case BoardSpaceType.CommunityChest:
            Container.Bind<BoardSpace>().To<CommunityChestSpace>().AsCached().WithArguments(i, spaceController);
            break;

          case BoardSpaceType.Land:
            spaceController.BoardSpaceDisplayDetails = spaceController.LandDetails.ToString();
            Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<LandSpace>().AsCached().WithArguments(i, spaceController.LandDetails, spaceController);
            break;

          case BoardSpaceType.RailRoad:
            spaceController.BoardSpaceDisplayDetails = spaceController.PropertyDetails.ToString();
            Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<RailRoadSpace>().AsCached().WithArguments(i, spaceController.PropertyDetails, spaceController);
            break;

          case BoardSpaceType.Utility:
            spaceController.BoardSpaceDisplayDetails = spaceController.PropertyDetails.ToString();
            Container.Bind(typeof(BoardSpace), typeof(PropertySpace)).To<UtilitySpace>().AsCached().WithArguments(i, spaceController.PropertyDetails, spaceController);
            break;
        }
      }
    }
  }
}