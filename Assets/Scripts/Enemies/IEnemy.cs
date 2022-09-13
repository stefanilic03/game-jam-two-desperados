using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{ 
    public bool InTheTimeTunnel { get; set; }
    public void DestroyEnemy();
}
