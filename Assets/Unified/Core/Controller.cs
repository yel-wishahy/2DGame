using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//An interface that defines the any base controller instance to be used
//to control a UEntity.
//
//Init() should be called in the implementation class's constructor.
 public interface Controller
{
    void Move();

    void Jump();

    void Attack();

    void Update();

    void Init();

    void HandleAnimations();
}
