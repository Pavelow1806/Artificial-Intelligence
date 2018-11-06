using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// Possible locations for the miner.
public enum Locations
{
    Nowhere,
    GoldMine,
    Bar,
    Bank,
    Home
}

// Our miner class.
// We have this hardworking miner, named Mike, who does the following:
// 1) Work - meaning he is digging for gold nuggets. However he can only fit one in each of his pockets. 
//           Obviously he has 2 pockets.
// 2) Deliver Gold - meaning he is exchanging the gold nuggets for actual money in the bank. 
// 3) Drink - He is always taking a break from work whenever he is really thirsty at the local pub. 
// 4) Sleep - A man can only do so much. Mike is getting tired, so he needs to sleep at his house. 
//            He always wakes up at night and drinks in secret.

public class Miner : MonoBehaviour {

    #region Miner Properties

    private FiniteStateMachine<Miner> MyFSM;

    public Locations CurrentLocation = Locations.GoldMine;

    // How many nuggets is Mike carrying in his pockets
    public int GoldCarried = 0;

    // The money currently stored in bank for Mike
    public int MoneyInBank = 0;

    // Mike's thirst levels.
    public float Thirst = 0;

    // Mike's fatigue levels
    public int Fatigue = 0;

    // The amount of thirst Mike actually recovers when he drinks at the pub
    public float QuenchAmount = 7;

    private Vector3 CurrentLocationInSpace = Vector3.zero;
    private Vector3 TargetLocationInSpace = Vector3.zero;

    #endregion

    #region Inspector Variables

    // *** You need to set these up in the Inspector before running the Demo. *** //
    // These are the User Interface text and the 4 locations that our capsule miner will go to.

    public Text UIText;

    public GameObject HomeLoc;
    public GameObject BarLoc;
    public GameObject MineLoc;
    public GameObject BankLoc;

    #endregion

    #region FSM Functionality

    public void Awake()
    {
        Debug.Log("Miner Awakes..");

        // (1) Create a new FSM and 
        // (2) Setup that digging nuggets is the first thing a miner should do.

        MyFSM = new FiniteStateMachine<Miner>();
        MyFSM.Configure(this, EnterMineAndDigForNuggets.Instance);
        UIText.text = EnterMineAndDigForNuggets.Instance.ToString();
    }

    public void ChangeState(State<Miner> newState)
    {
        MyFSM.ChangeState(newState);
        UIText.text = newState.ToString();
    }
    
    public void Update()
    {
        /// As the time progresses, the miner is getting more thirsty.
        Thirst += 0.05f;

        /// If he is travelling to another node, then his state's update will not run until he has reached the right position.
        /// We manage to do this by checking if his X and Z are the same with the target location X and Z.
        CurrentLocationInSpace = transform.position;
        if (CurrentLocationInSpace.x == TargetLocationInSpace.x && CurrentLocationInSpace.z == TargetLocationInSpace.z)
        {
            MyFSM.Update();
        }

    }

    #endregion

    #region Gameplay

    public void ChangeLocation(Locations Loc)
    {
        // This will change the location enum
        CurrentLocation = Loc;

        // based on the enum, his actual position in 3D space will be calculated.
        switch (CurrentLocation)
        {
            case Locations.GoldMine:
                TargetLocationInSpace = MineLoc.GetComponent<Transform>().position;
                break;
            case Locations.Bar:
                TargetLocationInSpace = BarLoc.GetComponent<Transform>().position;
                break;
            case Locations.Bank:
                TargetLocationInSpace = BankLoc.GetComponent<Transform>().position;
                break;
            case Locations.Home:
                TargetLocationInSpace = HomeLoc.GetComponent<Transform>().position;
                break;
            default:
                break;
        }

        // When the position is calculated, we will call his NavMeshAgent Component 
        // to setup his destination based on the baked navmesh on the map.
        GetComponent<NavMeshAgent>().SetDestination(TargetLocationInSpace);
        
    }

    public void AddToGoldCarried(int amount)
    {
        // Got some gold
        GoldCarried += amount;
    }

    public void AddToMoneyInBank(int amount)
    {
        MoneyInBank += amount;
        GoldCarried = 0;
    }
    
    public void DrinkWater(float amount)
    {
        Thirst -= amount;
    }

    public void Sleep()
    {
        Fatigue -= 1;
    }

    public void IncreaseFatigue()
    {
        Fatigue+=15;
    }

    public void SpendMoney()
    {
        MoneyInBank -= 5;
    }

    #endregion

    #region Miner Getter / Setters
    
    public bool PocketsFull()
    {
        bool full = GoldCarried == 2 ? true : false;
        return full;
    }

    public bool Thirsty()
    {
        bool thirsty = Thirst > 10 ? true : false;
        return thirsty;
    }

    #endregion
}


