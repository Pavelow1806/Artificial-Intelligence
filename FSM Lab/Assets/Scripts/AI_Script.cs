using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Our Finite State Machine. Or in short, FSM, also sometimes called context class
public class FiniteStateMachine<T>
{
    // The Owner of the FSM. In our case, it will be a Miner.
    private T Owner;

    // We need references to the last and Current states, 
    // in order for Entering and Exiting transitional code to work.
    private State<T> CurrentState;
    private State<T> PreviousState;

    // FSM Initialization. both current and prev states are set to null
    public void Awake()
    {
        CurrentState = null;
        PreviousState = null;
    }

    // Our owner is configured as well as the first state of the FSM.
    public void Configure(T Owner, State<T> InitialState)
    {
        this.Owner = Owner;
        ChangeState(InitialState);
    }

    // Execute the code of the current active state.
    public void Update()
    {
        if (CurrentState != null) CurrentState.Execute(Owner);
    }

    // State transitional code execution
    public void ChangeState(State<T> NewState)
    {
        // Changes Current and previous state
        PreviousState = CurrentState;
        CurrentState = NewState;

        // Calls the Exiting code for the previous state.
        if (PreviousState != null)
        {
            PreviousState.Exit(Owner);
        }

        // Calls the Entering code for the new state.
        if (CurrentState != null)
        {
            CurrentState.Enter(Owner);
        }
    }
}


// Our ordinary, generic state.
// It can trigger code OnEnter and OnExit. (Enter and Exit). These are abstract methods which must be overridden in the concrete state subclasses
// It also has default code that's run as the game plays out. (Execute)
abstract public class State<T>
{
    abstract public void Enter(T Entity);
    abstract public void Execute(T Entity);
    abstract public void Exit(T Entity);
}


// Work State
public sealed class EnterMineAndDigForNuggets : State<Miner>
{
    #region The State Instance
    static readonly EnterMineAndDigForNuggets instance = new EnterMineAndDigForNuggets();
    public static EnterMineAndDigForNuggets Instance
    {
        get
        {
            return instance;
        }
    }
    static EnterMineAndDigForNuggets() { }
    private EnterMineAndDigForNuggets() { }
    #endregion

    #region State Overrides
    public override void Enter(Miner miner)
    {
        if (miner.CurrentLocation != Locations.GoldMine)
        {
            Debug.Log("Entering Mine...");
            miner.ChangeLocation(Locations.GoldMine);
        }
    }

    // Execute tests the conditions to decide whether to change state
    public override void Execute(Miner miner)
    {
        if (miner.GoldCarried > 100)
            miner.ChangeState(GoHomeAndBuyBooze.Instance);

        if (miner.Fatigue > 100)
            miner.ChangeState(GoHomeAndRest.Instance);

        if (miner.Thirsty())
            miner.ChangeState(HaveABreakAndDrinkWater.Instance);

        miner.AddToGoldCarried(1);
        Debug.Log("Picking ap nugget and that's..." + miner.GoldCarried);

        miner.IncreaseFatigue();
        if (miner.Fatigue > 100)
            miner.ChangeState(GoHomeAndRest.Instance);

        if (miner.PocketsFull())
            miner.ChangeState(VisitBankAndDepositGold.Instance);
    }

    public override void Exit(Miner miner)
    {
        Debug.Log("Leaving the mine with my pockets full...");
    }
    #endregion
}

// Deliver Gold State
public sealed class VisitBankAndDepositGold : State<Miner>
{
    #region The State Instance
    static readonly VisitBankAndDepositGold instance = new VisitBankAndDepositGold();
    public static VisitBankAndDepositGold Instance
    {
        get
        {
            return instance;
        }
    }
    static VisitBankAndDepositGold() { }
    private VisitBankAndDepositGold() { }
    #endregion

    #region State Overrides
    public override void Enter(Miner miner)
    {
        if (miner.CurrentLocation != Locations.Bank)
        {
            Debug.Log("Entering the bank...");
            miner.ChangeLocation(Locations.Bank);
        }
    }

    // Execute tests the conditions to decide whether to change state
    // Exchanging his gold nuggets for actual money. 
    // When that is done, naturally, he goes back to work!
    public override void Execute(Miner miner)
    {
        Debug.Log("Feeding The System with MY gold... " + miner.MoneyInBank);
        miner.AddToMoneyInBank(miner.GoldCarried);
        if (miner.MoneyInBank > 100)
        {
            miner.ChangeState(GoHomeAndBuyBooze.Instance);
        }
        else
        {
            miner.ChangeState(EnterMineAndDigForNuggets.Instance);
        }
    }

    public override void Exit(Miner miner)
    {
        Debug.Log("Leaving the bank...");
    }
    #endregion
}

// Drink State
public sealed class HaveABreakAndDrinkWater : State<Miner>
{
    #region The State Instance

    static readonly HaveABreakAndDrinkWater instance = new HaveABreakAndDrinkWater();
    public static HaveABreakAndDrinkWater Instance
    {
        get
        {
            return instance;
        }
    }
    static HaveABreakAndDrinkWater() { }
    private HaveABreakAndDrinkWater() { }

    #endregion

    #region State Overrides

    public override void Enter(Miner miner)
    {
        if (miner.CurrentLocation != Locations.Bar)
        {
            Debug.Log("Taking a break from work...");
            miner.ChangeLocation(Locations.Bar);
        }
    }

    // Execute tests the conditions to decide whether to change state
    public override void Execute(Miner miner)
    {
        // Mike keeps on drinking until he is not thirsty. 
        miner.DrinkWater(miner.QuenchAmount);

        if (!miner.Thirsty())
        {
            // In which case, he always goes back to work!
            miner.ChangeState(EnterMineAndDigForNuggets.Instance);
        }
    }

    public override void Exit(Miner miner)
    {
        Debug.Log("Back to work...");
    }
    #endregion

}

// Sleep State
public sealed class GoHomeAndRest : State<Miner>
{
    #region The State Instance
    static readonly GoHomeAndRest instance = new GoHomeAndRest();
    public static GoHomeAndRest Instance
    {
        get
        {
            return instance;
        }
    }
    static GoHomeAndRest() { }
    private GoHomeAndRest() { }
    #endregion

    #region State Overrides
    public override void Enter(Miner miner)
    {
        if (miner.CurrentLocation != Locations.Home)
        {
            Debug.Log("Phew! Enough Work for today! Let's go home!");
            miner.ChangeLocation(Locations.Home);
        }
    }

    // Execute tests the conditions to decide whether to change state
    public override void Execute(Miner miner)
    {
        // Sleeping is good for your health... And fatigue!
        miner.Sleep();

        // Sometimes when you sleep you do wake up and crawl to the kitchen to drink some water...
        if (miner.Thirsty())
        {
            miner.DrinkWater(1);
        }

        // Our miner is sleeping until his fatigue is completely removed
        // in which case, he is changing state to go for nugget digging
        if (miner.Fatigue <= 0)
        {
            miner.ChangeState(EnterMineAndDigForNuggets.Instance);
        }
    }

    public override void Exit(Miner miner)
    {
        Debug.Log("A new day arrives, more work to do!");
    }
    #endregion
}

// Task: 
// When Money is over 100, make Mike go Home! 

// He will return when he has spent all his money on booze. Mike will spend 5 gold per update

//Booze State
public sealed class GoHomeAndBuyBooze : State<Miner>
{
    #region The State Instance
    static readonly GoHomeAndBuyBooze instance = new GoHomeAndBuyBooze();
    public static GoHomeAndBuyBooze Instance
    {
        get
        {
            return instance;
        }
    }
    static GoHomeAndBuyBooze() { }
    private GoHomeAndBuyBooze() { }
    #endregion
    #region State Overrides
    public override void Enter(Miner miner)
    {
        if (miner.CurrentLocation != Locations.Home)
        {
            Debug.Log("Time to go home and buy some booze, boiiiii!");
            miner.ChangeLocation(Locations.Home);
        }
    }
    public override void Execute(Miner miner)
    {
        if (miner.MoneyInBank > 0)
        {
            miner.SpendMoney();
            miner.IncreaseFatigue();
        }
        else
        {
            miner.ChangeState(EnterMineAndDigForNuggets.Instance);
        }
    }
    public override void Exit(Miner miner)
    {
        Debug.Log("Oh damnit Janet, I've spent all of my dang money on booze, bois");
    }
    #endregion
}