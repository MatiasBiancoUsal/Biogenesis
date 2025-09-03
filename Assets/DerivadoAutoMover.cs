using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DerivadoAutoMover : AutoMover
{
    // Start is called before the first frame update

    

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(!quieto)
        {
            base.Update();
            SpriteRenderer img = GetComponent<SpriteRenderer>();
            img.flipX = false;
        }
        else
        {
            SpriteRenderer img = GetComponent<SpriteRenderer>();
            img.flipX = true;
        }


            animator.SetBool("atacar", quieto);
    }
}
