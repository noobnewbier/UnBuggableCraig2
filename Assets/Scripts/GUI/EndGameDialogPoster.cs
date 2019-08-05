using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameDialogPoster : MonoBehaviour
{
    [SerializeField] DialogueData _data;

    private void Start()
    {
        ServiceLocator.EventAggregator.Publish(new DialogueEvent(_data));
    }
}
