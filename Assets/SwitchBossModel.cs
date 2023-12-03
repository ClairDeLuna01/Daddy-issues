using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchBossModel : StateMachineBehaviour
{
    public enum ModelType
    {
        Shotgun,
        Rifle,
        AR
    };

    public Mesh shotgun;
    public Mesh rifle;
    public Mesh ar;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        MeshFilter meshFilter = GameObject.FindGameObjectWithTag("BossWeapon").GetComponent<MeshFilter>();
        ModelType modelType = (ModelType)GameObject.Find("Boss").GetComponent<Boss>().heldWeaponType;
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
#endif
            Debug.Log("Switching boss model to " + modelType);
            switch (modelType)
            {
                case ModelType.Shotgun:
                    meshFilter.mesh = shotgun;
                    break;
                case ModelType.Rifle:
                    meshFilter.mesh = rifle;
                    break;
                case ModelType.AR:
                    meshFilter.mesh = ar;
                    break;
            }
#if UNITY_EDITOR
        }
#endif


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
