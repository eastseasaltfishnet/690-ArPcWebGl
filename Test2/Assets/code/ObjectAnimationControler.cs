using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAnimationControler : MonoBehaviour
{
    // Start is called before the first frame update


    public int currentObjectNumber;
    public List<GameObject> gameObjectList;

    public bool placedOrNot;

    public int currentAnimationNumber;
    public List<Animator> animationList;




    private GameObject currentGameObject;
    private bool isPlaced;
    private Animator currentAnimation;


    void Start()
    {


        currentGameObject = gameObjectList[currentObjectNumber];

        // ‰»Î∂Øª≠ 
        currentAnimation = animationList[currentAnimationNumber];
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void switchAnimatoin(int currentAnimationIndex,int currentObjectIndex)
    {
        this.currentAnimationNumber = currentAnimationIndex;
        this.currentObjectNumber = currentObjectIndex;

        currentGameObject = gameObjectList[currentObjectNumber];
        currentAnimation = animationList[currentAnimationNumber];

    }

}
