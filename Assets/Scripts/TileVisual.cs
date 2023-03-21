using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileVisual : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private static readonly int MergeTrigger = Animator.StringToHash("MergeTrigger");
    private static readonly int SpawnTrigger = Animator.StringToHash("SpawnTrigger");
    private static readonly int DeleteTrigger = Animator.StringToHash("DeleteTrigger");


    public void PlayMergeAnimation()
    {
        _animator.SetTrigger(MergeTrigger);
    }
    public void PlaySpawnAnimation()
    {
        _animator.SetTrigger(SpawnTrigger);
    }
    
    public void PlayDeleteAnimation()
    {
        _animator.SetTrigger(DeleteTrigger);
    }

}