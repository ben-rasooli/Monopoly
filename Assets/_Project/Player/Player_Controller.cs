using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Project
{
  public class Player_Controller : MonoBehaviour
  {
    public string Name;

    public void Move(Vector3 position, Action onStartMoving, Action onStopMoving)
    {
      _myAnimator.SetBool("Moving", true);
      _myNavAgent.isStopped = false;
      _myNavAgent.SetDestination(position);
      onStartMoving.Invoke();
      StartCoroutine(nameof(watchForDestinationReach), onStopMoving);
    }

    IEnumerator watchForDestinationReach(Action onReachDestination)
    {
      while (true)
      {
        yield return null;
        if (_myNavAgent.remainingDistance < 0.2)
        {
          _myNavAgent.isStopped = true;
          _myAnimator.SetBool("Moving", false);
          onReachDestination.Invoke();
          break;
        }
      }
    }

    #region dependencies
    [SerializeField] NavMeshAgent _myNavAgent;
    [SerializeField] Animator _myAnimator;
    #endregion
  }
}
