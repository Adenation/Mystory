using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YouSpinMeRightRound : MonoBehaviour
{
    [SerializeField] float spinSpeed = .01f;
    [SerializeField] Vector3 direction = Vector3.back;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction, spinSpeed);
    }
}
