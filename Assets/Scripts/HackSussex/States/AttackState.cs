//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AttackState : StateMachineBehaviour
//{
//    private int _aliveChansCount = 0;
//    private Animator _animator;
    
//    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
//    {
//        EventManager.Instance.OnAgentDoneEvent += OnAgentDone;
//        _animator = animator;
//        EnvironmentControler.Instance.Activate();
//    }

//    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
//    {
//        GameManager.Instance.Money += Time.deltaTime;
//        GUIManager.Instance._moneyText.text = "Money: " + Mathf.Floor(GameManager.Instance.Money).ToString();
//        GUIManager.Instance._healthText.text = "Health: " + GameManager.Instance.Health.ToString();
//    }

//    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
//    {
//        EventManager.Instance.OnAgentDoneEvent -= OnAgentDone;
//        _aliveChansCount = 0;
//    }

//    void OnAgentDone(object sender, AgentDoneEventArgs eventArgs)
//    {
//        _aliveChansCount++;
//        if (_aliveChansCount == GameConfig.MAX_CHAN_NUM)
//        {
//            _animator.SetTrigger("TurnDone");
//        }
//    }
//}
