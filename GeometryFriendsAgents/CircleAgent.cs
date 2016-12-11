using GeometryFriends;
using GeometryFriends.AI;
using GeometryFriends.AI.ActionSimulation;
using GeometryFriends.AI.Communication;
using GeometryFriends.AI.Debug;
using GeometryFriends.AI.Interfaces;
using GeometryFriends.AI.Perceptions.Information;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace GeometryFriendsAgents
{
    /// <summary>
    /// A circle agent implementation for the GeometryFriends game that demonstrates prediction and history keeping capabilities.
    /// </summary>
    public class CircleAgent : AbstractCircleAgent
    {
        //agent implementation specificiation
        private bool implementedAgent;
        private string agentName = "myCircle";
        Boolean s = false;  // will
        Boolean stop = false;
        Boolean growAll = false;
        Goal real=null;

        //auxiliary variables for agent action
        private Moves currentAction;
        private List<Moves> possibleMoves;
        private long lastMoveTime;
        private Random rnd;

        //predictor of actions for the circle
        private ActionSimulator predictor = null;
        private DebugInformation[] debugInfo = null;
        private int debugCircleSize = 20;

        //debug agent predictions and history keeping
        private List<CollectibleRepresentation> caughtCollectibles;
        private List<CollectibleRepresentation> uncaughtCollectibles;
        private object remainingInfoLock = new Object();
        private List<CollectibleRepresentation> remaining;

        //Sensors Information and level state
        private CountInformation numbersInfo;
        private RectangleRepresentation rectangleInfo;
        private CircleRepresentation circleInfo;
        private ObstacleRepresentation[] obstaclesInfo;
        private ObstacleRepresentation[] rectanglePlatformsInfo;
        private ObstacleRepresentation[] circlePlatformsInfo;
        private CollectibleRepresentation[] collectiblesInfo;

        private int nCollectiblesLeft;

        private List<AgentMessage> messages;

        Graph gr;
        //plans
        ArrayList plans = new ArrayList();
        Plan currentPlan;
        Action nextAction;
        //goals
        ArrayList goals = new ArrayList();

        //low level representation
        Grid gridWorld = new Grid();
        State currentState;
        State rectangle;

        //Area of the game screen
        private Rectangle area;

        public CircleAgent()
        {
            //Change flag if agent is not to be used
            implementedAgent = true;

            //setup for action updates
            lastMoveTime = DateTime.Now.Second;
            currentAction = Moves.NO_ACTION;
            rnd = new Random();

            //prepare the possible moves  
            possibleMoves = new List<Moves>();
            possibleMoves.Add(Moves.ROLL_LEFT);
            possibleMoves.Add(Moves.ROLL_RIGHT);
            possibleMoves.Add(Moves.JUMP);
      
      
            //history keeping
            uncaughtCollectibles = new List<CollectibleRepresentation>();
            caughtCollectibles = new List<CollectibleRepresentation>();
            remaining = new List<CollectibleRepresentation>();

            //messages exchange
            messages = new List<AgentMessage>();
        }
        /// <summary>
        /// initializers
        /// </summary>
        public void initGrid()
        {
            gridWorld.setAgent(this.agentName);
            if (this.agentName == "myRectangle")
            {

                gridWorld.add(obstaclesInfo); //static
                gridWorld.add(circlePlatformsInfo); //static
            }
            else
            {
                gridWorld.add(obstaclesInfo); //static
                gridWorld.add(rectanglePlatformsInfo); //static
            }
            int g_empty = gridWorld.setEmptyCells();
            int g_floor = gridWorld.setFloor();
            int g_edges = gridWorld.setEdges();
            Log.LogInformation("Grid initiated with " + g_empty + "  empty cells || " + g_floor + " floor cells ||  " + g_edges + "  cell edges");
        }
        public void initAgentState()
        {
            switch (agentName)
            {
                case "myRectangle":
                    {
                        currentState = new State(rectangleInfo.VelocityX, rectangleInfo.VelocityY, rectangleInfo.X, rectangleInfo.Y, rectangleInfo.Height / 2);
                        Cell AgentCell = gridWorld.locate(currentState);
                        break;
                    }
                case "myCircle":
                    {
                        currentState = new State(circleInfo.VelocityX, circleInfo.VelocityY, circleInfo.X, circleInfo.Y ,circleInfo.Radius);
                        rectangle = new State(rectangleInfo.VelocityX, rectangleInfo.VelocityY, rectangleInfo.X, rectangleInfo.Y, rectangleInfo.Height / 2);
                        Cell AgentCell = gridWorld.locate(currentState);
                        break;
                    }
            }
        }
        public void initGraph() {
            gr = new Graph(gridWorld);
            int noNodes = gr.addNodes();
            GeometryFriends.Log.LogInformation(agentName + "->" + noNodes + "nodes were created");
        }
        public void initGoals()
        {
            int goalId = 0;
            foreach (CollectibleRepresentation c in collectiblesInfo)
            {
                Goal t = new Goal(c, goalId);
                State GoalState = t.getState();
                Log.LogInformation(agentName + "created goal: " + goalId + "  on position: " + c.X + " --" + c.Y);
                goalId++;
                Goal updated = t.generateNew(GoalState);
                Log.LogInformation(agentName + "created goal: " + goalId + "  on position: " + updated.getPosition() );
                goals.Add(t);
            }
        }
        public void initPlans()
        {
            foreach (Goal g in goals)
            {
                //reset values
                gridWorld.clear();
                Plan pl = new Plan();
                pl.setgoal(g);

                Log.LogInformation(agentName + "->" + "created plan for goal -" + g.id + "in: " + g.goal.getX() + ":" + g.goal.getY());
                //flood this goal
                gridWorld.flood(g);
                gridWorld.setEmptyCells(); // needs to exist
                gridWorld.setAdjMatrix();
                //copy gridWorld rep for goal
                Node AgentNode = gr.getNodeByCellId(gridWorld.locate(currentState).id);
                pl.setGridWorld(gridWorld);
                pl.setAgent(currentState);

                gr.InitValGraph();
                //int noEdges = gr.createEdges();
                //GeometryFriends.Log.LogInformation(agentName + "->" + noEdges + "edges were created");
                //gr.sortGraph();
                //gr.pruneGraph(AgentNode);
                pl.setGraph(gr);
                //find path
                //pl.buildPath();
                plans.Add(pl);
            }
        }
        //implements abstract circle interface: used to setup the initial information so that the agent has basic knowledge about the level
        public override void Setup(CountInformation nI, RectangleRepresentation rI, CircleRepresentation cI, ObstacleRepresentation[] oI, ObstacleRepresentation[] rPI, ObstacleRepresentation[] cPI, CollectibleRepresentation[] colI, Rectangle area, double timeLimit)
        {
            numbersInfo = nI;
            nCollectiblesLeft = nI.CollectiblesCount;
            rectangleInfo = rI;
            circleInfo = cI;
            obstaclesInfo = oI;
            rectanglePlatformsInfo = rPI;
            circlePlatformsInfo = cPI;
            collectiblesInfo = colI;
            uncaughtCollectibles = new List<CollectibleRepresentation>(collectiblesInfo);
            this.area = area;
            Log.LogInformation("area w: " + area.Width + "area h:" + area.Height);
            initGrid();
            //set agent state
            initAgentState();
            //Generate initial graph
            initGraph();
            //create goals
            initGoals();
            //create plans
            initPlans();
            //evaluate path
            //order or remove plans
            foreach (Plan pla in plans)
            {
                pla.active = false;
            }
            //comunicate
                ///for each goal at least one agent should have an active plan for it
            //negociate
                ///if both have only one should keep it
            //send a message to the rectangle informing that the circle setup is complete and show how to pass an attachment: a pen object

            //DebugSensorsInfo();
        }

        //implements abstract circle interface: registers updates from the agent's sensors that it is up to date with the latest environment information
        /*WARNING: this method is called independently from the agent update - Update(TimeSpan elapsedGameTime) - so care should be taken when using complex 
         * structures that are modified in both (e.g. see operation on the "remaining" collection)      
         */
        public override void SensorsUpdated(int nC, RectangleRepresentation rI, CircleRepresentation cI, CollectibleRepresentation[] colI)
        {
            nCollectiblesLeft = nC;
            rectangleInfo = rI;
            circleInfo = cI;
            collectiblesInfo = colI;
            UpdateAgentState();
                        //DebugSensorsInfo();
        }

        //implements abstract circle interface: provides the circle agent with a simulator to make predictions about the future level state
        public override void ActionSimulatorUpdated(ActionSimulator updatedSimulator)
        {
            predictor = updatedSimulator;
        }
        
        //implements abstract circle interface: signals if the agent is actually implemented or not
        public override bool ImplementedAgent()
        {
            return implementedAgent;
        }

        //implements abstract circle interface: provides the name of the agent to the agents manager in GeometryFriends
        public override string AgentName()
        {
            return agentName;
        }

        private void UpdateAgentState()
        {
            currentState.updateState(circleInfo.X + circleInfo.VelocityX, circleInfo.Y + circleInfo.VelocityY, circleInfo.VelocityX, circleInfo.VelocityY , circleInfo.Radius );
            rectangle.updateState(rectangleInfo.X, rectangleInfo.Y, rectangleInfo.VelocityX, rectangleInfo.VelocityY, rectangleInfo.Height);
            if (rectangle.h > 190) // morph up complete
            {
                Log.LogError("m_up complete" + rectangleInfo.Height);
                growAll = true;
            }

            //currentPlan.setAgent(currentState);
        }

        private Plan getActivePlan()
        {
            foreach (Plan p in plans)
            {
                if (p.goal.isCaugcht())
                {
                    plans.Remove(p);
                }
                else
                {
                    if (p.active == true)
                    {

                        gridWorld = p.worldRep;
                        messages.Add(p.setGoal()); //call once ??
                        return p;
                    }
                }
            }
            return null;
            
        }


        //simple algorithm for choosing a random action for the circle agent
        private void RandomAction()
        {
            /*
             Circle Actions
             ROLL_LEFT = 1      
             ROLL_RIGHT = 2
             JUMP = 3
             GROW = 4
            */
            currentAction = possibleMoves[rnd.Next(possibleMoves.Count)];
            
            //send a message to the rectangle agent telling what action it chose
            messages.Add(new AgentMessage("Going to :" + currentAction));
        }

        private void InformedAction()
        {
            currentAction = nextAction.getMove();
            messages.Add(new AgentMessage("Going to :" + currentAction));
        }
        //implements abstract circle interface: GeometryFriends agents manager gets the current action intended to be actuated in the enviroment for this agent
        public override Moves GetAction()
        {
            return currentAction;
        }

        //implements abstract circle interface: updates the agent state logic and predictions
        public override void Update(TimeSpan elapsedGameTime)
        {
            //Every second one new action is choosen
            if (lastMoveTime == 60)
            { 
            lastMoveTime = 0;
             }
            if ((lastMoveTime) <= (DateTime.Now.Second) && (lastMoveTime < 60))
            {
                if (!(DateTime.Now.Second == 59))
                {
                    Log.LogError(agentName + "->" + "update info:" + circleInfo.X + " :: " +  circleInfo.Y);
                    //check plan state
                    try
                    {
                        try
                        {
                            UpdateAgentState();
                           /* if (stop )
                            {
                                nextAction = new Action(currentState, Moves.NO_ACTION);
                                messages.Add(currentPlan.pushUp());
                                Log.LogInformation("ESTA NO STOP" + nextAction.getMove());
                            }
                            else
                            {
                            */ 
                            Cell AgentCell = gridWorld.locate(currentState);
                                if (AgentCell.rectangle)
                                {
                                    messages.Add(currentPlan.pushUp());  /// send push up ??
                                nextAction = currentPlan.executePlan();
                             //   nextAction = new Action(currentState, Moves.NO_ACTION);
                                    Log.LogInformation("O STOP ESTA A FALSE" + nextAction.getMove());

                                }
                                else
                                {
                                    if (growAll)
                                    {// aqui
                                    currentPlan.worldRep.clear();
                                    currentPlan.worldRep.updateGrid(rectangle);
                                    currentPlan.setgoal(real);
                                    currentPlan.worldRep.reflood();
                                    nextAction = currentPlan.executePlan();
                                    //nextAction = new Action(currentState, Moves.JUMP);
                                        Log.LogInformation("SAIU DA AREA, TEM QUE SALTAR" + nextAction.getMove());
                                    messages.Add(currentPlan.pushUp());
                                    growAll = false;
                                    }
                                    else
                                    {
                                        nextAction = currentPlan.executePlan();
                                    }
                                }
                          //  }
                        }
                        catch (Exception e)
                        {
                            Log.LogError(agentName + "  -could not execute plan 2.0");
                        }
                        InformedAction();
                    }
                    catch (Exception e) {
                        Log.LogInformation(agentName+ " informed not possible");
                    }
                    
                    lastMoveTime = lastMoveTime + 1;
                    //DebugSensorsInfo();                    
                }
                else
                    lastMoveTime = 60;
            }

            //check if any collectible was caught
            lock (remaining)
            {
                if (remaining.Count > 0)
                {
                    List<CollectibleRepresentation> toRemove = new List<CollectibleRepresentation>();
                    foreach (CollectibleRepresentation item in uncaughtCollectibles)
                    {
                        if (!remaining.Contains(item))
                        {
                            caughtCollectibles.Add(item);
                            toRemove.Add(item);
                        }
                    }
                    foreach (CollectibleRepresentation item in toRemove)
                    {
                        foreach (Plan p in plans)
                        {
                            p.goal.setCatched(item);
                        }
                        uncaughtCollectibles.Remove(item);
                    }
                }
            }
            //predict what will happen to the agent given the current state and current action
            if (predictor != null) //predictions are only possible where the agents manager provided
            {
                /*
                 * 1) simulator can only be properly used when the Circle and Rectangle characters are ready, this must be ensured for smooth simulation
                 * 2) in this implementation we only wish to simulate a future state when whe have a fresh simulator instance, i.e. the generated debug information is empty
                */
                if (predictor.CharactersReady() && predictor.SimulationHistoryDebugInformation.Count == 0)
                {
                    List<CollectibleRepresentation> simCaughtCollectibles = new List<CollectibleRepresentation>();
                    //keep a local reference to the simulator so that it can be updated even whilst we are performing simulations
                    ActionSimulator toSim = predictor;

                    //prepare the desired debug information (to observe this information during the game press F1)
                    toSim.DebugInfo = true;
                    //you can also select the type of debug information generated by the simulator to circle only, rectangle only or both as it is set by default
                    //toSim.DebugInfoSelected = ActionSimulator.DebugInfoMode.Circle;

                    //setup the current circle action in the simulator
                    toSim.AddInstruction(currentAction);
                    //register collectibles that are caught during simulation
                    toSim.SimulatorCollectedEvent += delegate (Object o, CollectibleRepresentation col) { simCaughtCollectibles.Add(col); };

                    //simulate 2 seconds (predict what will happen 2 seconds ahead)
                    toSim.Update(2);
                    /*
                    if ( toSim.CirclePositionX - rectangle.getX() > 30 && toSim.CircleVelocityX < 20)
                    {
                        stop = true;
                    }
                    */

                    //prepare all the debug information to be passed to the agents manager
                    List<DebugInformation> newDebugInfo = new List<DebugInformation>();
                    //clear any previously passed debug information (information passed to the manager is cumulative unless cleared in this way)
                    newDebugInfo.Add(DebugInformationFactory.CreateClearDebugInfo());
                    //add all the simulator generated debug information about circle/rectangle predicted paths
                    newDebugInfo.AddRange(toSim.SimulationHistoryDebugInformation);
                    //create additional debug information to visualize collectibles that have been predicted to be caught by the simulator
                    foreach (CollectibleRepresentation item in simCaughtCollectibles)
                    {
                        newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(item.X - debugCircleSize / 2, item.Y - debugCircleSize / 2), debugCircleSize, GeometryFriends.XNAStub.Color.Red));
                        newDebugInfo.Add(DebugInformationFactory.CreateTextDebugInfo(new PointF(item.X, item.Y), "Predicted catch!", GeometryFriends.XNAStub.Color.White));
                    }
                    //create additional debug information to visualize collectibles that have already been caught by the agent
                    foreach (CollectibleRepresentation item in caughtCollectibles)
                    {
                        newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(item.X - debugCircleSize / 2, item.Y - debugCircleSize / 2), debugCircleSize, GeometryFriends.XNAStub.Color.GreenYellow));
                    }

                    // creadte adicional goals

                    
                    //CANCER
                    //create grid debug information 
                    ArrayList n = new ArrayList();
                    foreach (Cell c in gridWorld.grid)
                    {
                        newDebugInfo.Add(DebugInformationFactory.CreateTextDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]) , gridWorld.CelltoHeight(c.pos[1]) ), c.vector[0].ToString(), GeometryFriends.XNAStub.Color.Black));
                        
                        if (c.obstacle)
                        {
                            newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), 8, GeometryFriends.XNAStub.Color.PaleTurquoise));
                        }
                        else
                        {
                            if (c.floor)
                            {
                                newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), 8, GeometryFriends.XNAStub.Color.Gray));
                            }

                            if (c.edge)
                            {
                                newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), 8, GeometryFriends.XNAStub.Color.Gray));
                            }
                              
                        }
                        


                        if (gridWorld.locate(currentState).id == c.id)
                        {
                            if (agentName == "myCircle")
                            {
                                newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), 5, GeometryFriends.XNAStub.Color.Orange));
                            }
                            else {
                                newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), 5, GeometryFriends.XNAStub.Color.Green));
                            }
                        }
                    }

                    try
                    {
                        foreach (Plan pt in plans)
                        {
                            foreach (Node pa in pt.path._path)
                            {
                                newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(pa.getState().getX(), pa.getState().getY()), 10, GeometryFriends.XNAStub.Color.White));

                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e);
                    }
                    



                    //set all the debug information to be read by the agents manager
                    debugInfo = newDebugInfo.ToArray();
                }
                else {
                    Log.LogError("wazzzzzzup", false);
                }
            }
        }

        //typically used console debugging used in previous implementations of GeometryFriends
        protected void DebugSensorsInfo()
        {
            Log.LogInformation("Circle Agent - " + numbersInfo.ToString());

            Log.LogInformation("Circle Agent - " + rectangleInfo.ToString());

            Log.LogInformation("Circle Agent - " + circleInfo.ToString());

            foreach (ObstacleRepresentation i in obstaclesInfo)
            {
                Log.LogInformation("Circle Agent - " + i.ToString("Obstacle"));
            }

            foreach (ObstacleRepresentation i in rectanglePlatformsInfo)
            {
                Log.LogInformation("Circle Agent - " + i.ToString("Rectangle Platform"));
            }

            foreach (ObstacleRepresentation i in circlePlatformsInfo)
            {
                Log.LogInformation("Circle Agent - " + i.ToString("Circle Platform"));
            }

            foreach (CollectibleRepresentation i in collectiblesInfo)
            {
                Log.LogInformation("Circle Agent - " + i.ToString());
            }
        }

        //implements abstract circle interface: signals the agent the end of the current level
        public override void EndGame(int collectiblesCaught, int timeElapsed)
        {
            Log.LogInformation("CIRCLE - Collectibles caught = " + collectiblesCaught + ", Time elapsed - " + timeElapsed);
        }

        //implements abstract circle interface: gets the debug information that is to be visually represented by the agents manager
        public override DebugInformation[] GetDebugInformation()
        {
            return debugInfo;
        }

        //implememts abstract agent interface: send messages to the rectangle agent
        public override List<GeometryFriends.AI.Communication.AgentMessage> GetAgentMessages()
        {
            List<AgentMessage> toSent = new List<AgentMessage>(messages);
            messages.Clear();
            return toSent;
        }

        //implememts abstract agent interface: receives messages from the rectangle agent
        public override void HandleAgentMessages(List<GeometryFriends.AI.Communication.AgentMessage> newMessages)
        {
            foreach (AgentMessage item in newMessages)
            {
                Log.LogInformation("Circle: received message from rectangle: " + item.Message);
                if (item.Attachment != null)
                {
                    Log.LogInformation("Received message has attachment: " + item.Attachment.ToString());
                    if (item.Attachment.GetType() == typeof(Pen))
                    {
                        Log.LogInformation("The attachment is a pen, let's see its color: " + ((Pen)item.Attachment).Color.ToString());
                    }
                }
                if (item.Message == "stop")
                {
                    if (item.Attachment != null)
                    {
                    }
                    stop = true;
                }
                if (item.Message == "imset")
                {
                    stop = false;
                    if (item.Attachment != null)
                    {
                        Log.LogInformation("Received message has attachment: " + item.Attachment.ToString());
                        if (item.Attachment.GetType() == typeof(State))
                        {
                            State t = (State)item.Attachment;
                            t.updateHeight();
                            Goal toJump = new Goal(t);
                            //Plan newPlan = new Plan();
                            //newPlan.setgoal(toJump);
                            
                            Log.LogInformation("rectangle set on state" + t.getState());

                            Plan cur = (Plan)plans[0]; // ONE GOAL
                            cur.active = true;
                            //cur.addSubPlan(newPlan);
                            currentPlan = getActivePlan();
                            currentPlan.worldRep.clear();
                            currentPlan.worldRep.updateGrid(rectangle);
                            if (real == null)
                            {
                                real = currentPlan.goal;
                            }
                            currentPlan.setgoal(toJump);
                            currentPlan.setGoal();
                            currentPlan.worldRep.reflood();
                        }
                    }
                }
            }
        }
    }
}

