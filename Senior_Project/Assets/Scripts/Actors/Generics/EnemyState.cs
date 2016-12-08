using UnityEngine;
using System.Collections;

public class EnemyState : StateMachine
{
    //might think of some use for this eventually
    private static EnemyState ths = E0Builder.Load();//will later change to handle multiple state machines
    //will eventually remove builders and stick a file load here instead
    public static EnemyState generate() { return ths; }
}
