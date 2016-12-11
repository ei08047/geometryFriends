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
    /// A rectangle agent implementation for the GeometryFriends game that demonstrates simple random action selection.
    /// </summary>
    public class RectangleAgent : AbstractRectangleAgent
    {
        //agent implementation specificiation
        private bool implementedAgent;
        private string agentName = "myRectangle";
        public Boolean mup = false;

        //auxiliary variables for agent action
        private Moves currentAction;
        private List<Moves> possibleMoves;
        private long lastMoveTime;
        private Random rnd;

        //Area of the game screen
        protected Rectangle area;

        //predictor of actions for the rectangle TODO:
        private ActionSimulator predictor = null;
        private DebugInformation[] debugInfo = null;
        private int debugCircleSize = 20;

        //debug agent predictions and history keeping
        private List<CollectibleRepresentation> caughtCollectibles;
        private List<CollectibleRepresentation> uncaughtCollectibles;
        private object remainingInfoLock = new Object();
        private List<CollectibleRepresentation> remaining;

        //Sensors Information
        private CountInformation numbersInfo;
        private RectangleRepresentation rectangleInfo;
        private CircleRepresentation circleInfo;
        private ObstacleRepresentation[] obstaclesInfo;
        private ObstacleRepresentation[] rectanglePlatformsInfo;
        private ObstacleRepresentation[] circlePlatformsInfo;
        private CollectibleRepresentation[] collectiblesInfo;

        private int nCollectiblesLeft;

        private List<AgentMessage> messages;

        //low level representation
        Grid gridWorld = new Grid();
        State currentState;
        Graph gr;

        //goals
        ArrayList goals = new ArrayList();

        //plans
        ArrayList plans = new ArrayList();
        Plan currentPlan;
        Action nextAction;


        public RectangleAgent()
        {
            //Change flag if agent is not to be used
            implementedAgent = true;

            lastMoveTime = DateTime.Now.Second;
            currentAction = Moves.NO_ACTION;
            rnd = new Random();

            //prepare the possible moves  
            possibleMoves = new List<Moves>();
            possibleMoves.Add(Moves.MOVE_LEFT);
            possibleMoves.Add(Moves.MOVE_RIGHT);
            possibleMoves.Add(Moves.MORPH_UP);
            possibleMoves.Add(Moves.MORPH_DOWN);

            //messages exchange
            messages = new List<AgentMessage>();
        }

        /// <summary>
        /// initializers
        /// </summary>
        public void initGrid() {
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
            gridWorld.setEmptyCells();
            gridWorld.setFloor();
            gridWorld.setEdges();
        }
        public void initAgentState()
        {
            switch (agentName)
            {
                case "myRectangle":
                    {
                        currentState = new State(rectangleInfo.VelocityX, rectangleInfo.VelocityY, rectangleInfo.X, rectangleInfo.Y, rectangleInfo.Height);
                        int x, y;

                        Cell AgentCell = gridWorld.locate(currentState);
                        break;
                    }
                case "myCircle":
                    {
                        currentState = new State(circleInfo.VelocityX, circleInfo.VelocityY, circleInfo.X, circleInfo.Y, circleInfo.Radius);
                        Cell AgentCell = gridWorld.locate(currentState);
                        break;
                    }

            }
        }
        public void initGraph()
        {
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
                Log.LogInformation("created goal: " + goalId + "  on position: " + c.X + " --" + c.Y);
                goalId++;
                Goal updated = t.generateNew(GoalState);
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
                gridWorld.setEmptyCells();
                gridWorld.setAdjMatrix();
                //copy gridWorld rep for goal
                Node AgentNode = gr.getNodeByCellId(gridWorld.locate(currentState).id);
                pl.setGridWorld(gridWorld);
                pl.setAgent(currentState);
               // int noEdges = gr.createEdges();
                //GeometryFriends.Log.LogInformation(agentName + "->" + noEdges + "edges were created");
                gr.InitValGraph();
                //gr.sortGraph();
                //gr.pruneGraph(AgentNode);
                pl.setGraph(gr);
                //find path
                //pl.buildPath();
                //pl.logPath();
                plans.Add(pl);

            }
        }

        //implements abstract rectangle interface: used to setup the initial information so that the agent has basic knowledge about the level
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
            this.area = area;
            //init grid
            initGrid();
            //init agent state
            initAgentState();
            //Generate graph
            initGraph();
            //create goals
            initGoals();
            //create plans
            initPlans();

            //evaluate path
            //order or remove plans
            foreach (Plan pla in plans)
            {
                    pla.active = true;
            }
            currentPlan = getActivePlan();
            if (currentPlan == null)
            {
                Log.LogInformation(agentName + "-> No active plan!!");
            }
            //comunicate
            ///for each goal at least one agent should have an active plan for it
            //negociate
            ///if both have only one should keep it

            //send a message to the rectangle informing that the circle setup is complete and show how to pass an attachment: a pen object
            messages.Add(new AgentMessage("Setup complete, testing to send an object as an attachment.", new Pen(Color.BlanchedAlmond)));

        }

        //implements abstract rectangle interface: registers updates from the agent's sensors that it is up to date with the latest environment information
        public override void SensorsUpdated(int nC, RectangleRepresentation rI, CircleRepresentation cI, CollectibleRepresentation[] colI)
        {
            nCollectiblesLeft = nC;
            rectangleInfo = rI;
            circleInfo = cI;
            collectiblesInfo = colI;
            /*
            if (currentPlan != null)
            {
                if (currentPlan.active)
                {
                    currentPlan.updatePlan(currentState);
                    nextAction = currentPlan.executePlan();
                    if (nextAction == null)
                    {
                        Log.LogError(agentName + "  -got null action executing plan");
                    }
                    else
                    {
                        Log.LogError(agentName + "  -next action is: " + nextAction.getMove());
                    }
                }
            }
            else
            {
                Log.LogError(agentName + "  -no active plan");
            }
            */
            
        }
        
        //implements abstract rectangle interface: signals if the agent is actually implemented or not
        public override bool ImplementedAgent()
        {
            return implementedAgent;
        }

        //implements abstract rectangle interface: provides the name of the agent to the agents manager in GeometryFriends
        public override string AgentName()
        {
            return agentName;
        }

        private void UpdateAgentState()
        {
            currentState.updateState(rectangleInfo.X + rectangleInfo.VelocityX, rectangleInfo.Y, rectangleInfo.VelocityX, rectangleInfo.VelocityY, rectangleInfo.Height / 2);
            currentPlan.setAgent(currentState);
        }

        private Plan getActivePlan()
        {
            foreach (Plan p in plans)
            {
                if (p.active = true)
                {
                    gridWorld = p.worldRep;
                    return p;
                }
            }
            return null;

        }
        //simple algorithm for choosing a random action for the rectangle agent
        private void RandomAction()
        {
            /*
             Rectangle Actions
             MOVE_LEFT = 5
             MOVE_RIGHT = 6
             MORPH_UP = 7
             MORPH_DOWN = 8
            */

            currentAction = possibleMoves[rnd.Next(possibleMoves.Count)];

            //send a message to the circle agent telling what action it chose
            messages.Add(new AgentMessage("Going to :" + currentAction));
        }

        private void InformedAction()
        {

            currentAction = nextAction.getMove();
            messages.Add(new AgentMessage("Going to :" + currentAction));
        }

        //implements abstract rectangle interface: GeometryFriends agents manager gets the current action intended to be actuated in the enviroment for this agent
        public override Moves GetAction()
        {
            return currentAction;
        }

        //implements abstract rectangle interface: updates the agent state logic and predictions
        public override void Update(TimeSpan elapsedGameTime)
        {
            UpdateAgentState();
            //Every second one new action is choosen
            if (lastMoveTime == 60)
            {
                lastMoveTime = 0;
            }

          

            if ((lastMoveTime) <= (DateTime.Now.Second) && (lastMoveTime < 60))
            {
                if (!(DateTime.Now.Second == 59))
                {
                    Log.LogError(agentName + "->" + "update info:" + circleInfo.X + " :: " + circleInfo.Y);
                    try
                    {
                        try
                        {
                            UpdateAgentState();
                            if (!this.mup) // pushup
                            {
                                nextAction = currentPlan.executePlan();

                            }
                            else {
                                Log.LogError("rectangle state" + rectangleInfo.Height);
                                if (rectangleInfo.Height > 190)
                                {
                                    mup = false;
                                }
                            }
  
                            
                            
                            if (nextAction.getMove() == GeometryFriends.AI.Moves.MORPH_DOWN)
                            {
                            
                                messages.Add(currentPlan.set());
                            
                            }
                            
                            if (nextAction.getMove() == GeometryFriends.AI.Moves.MORPH_UP)
                            {
                                messages.Add(new AgentMessage( " ja ta", null));
                            }

                            Log.LogInformation(agentName +" next action" + nextAction.getMove());
                            InformedAction();
                        }
                        catch (Exception e)
                        {
                            Log.LogError(agentName + "  -could not execute plan 2.0");
                        }
                        
                    }
                    catch (Exception e)
                    {
                        Log.LogInformation(agentName + " informed not possible");
                    }


                    lastMoveTime = lastMoveTime + 1;
                    //DebugSensorsInfo();                    
                }
                else
                    lastMoveTime = 60;
            }
            prepareDebugInfo();
        }

        //typically used console debugging used in previous implementations of GeometryFriends
        protected void DebugSensorsInfo()
        {
            Log.LogInformation("Rectangle Aagent - " + numbersInfo.ToString());

            Log.LogInformation("Rectangle Aagent - " + rectangleInfo.ToString());

            Log.LogInformation("Rectangle Aagent - " + circleInfo.ToString());

            foreach (ObstacleRepresentation i in obstaclesInfo)
            {
                Log.LogInformation("Rectangle Aagent - " + i.ToString("Obstacle"));
            }

            foreach (ObstacleRepresentation i in rectanglePlatformsInfo)
            {
                Log.LogInformation("Rectangle Aagent - " + i.ToString("Rectangle Platform"));
            }

            foreach (ObstacleRepresentation i in circlePlatformsInfo)
            {
                Log.LogInformation("Rectangle Aagent - " + i.ToString("Circle Platform"));
            }

            foreach (CollectibleRepresentation i in collectiblesInfo)
            {
                Log.LogInformation("Rectangle Aagent - " + i.ToString());
            }
        }

        //implements abstract rectangle interface: signals the agent the end of the current level
        public override void EndGame(int collectiblesCaught, int timeElapsed)
        {
            Log.LogInformation("RECTANGLE - Collectibles caught = " + collectiblesCaught + ", Time elapsed - " + timeElapsed);
        }

        //implements abstract circle interface: gets the debug information that is to be visually represented by the agents manager
        public override DebugInformation[] GetDebugInformation()
        {
            return debugInfo;
        }

        public void prepareDebugInfo() {
            //prepare all the debug information to be passed to the agents manager
            List<DebugInformation> newDebugInfo = new List<DebugInformation>();
            //clear any previously passed debug information (information passed to the manager is cumulative unless cleared in this way)
            newDebugInfo.Add(DebugInformationFactory.CreateClearDebugInfo());
            //create grid debug information
            ArrayList n = new ArrayList();
            foreach (Cell c in gridWorld.grid)
            {
                /*
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
                */
                newDebugInfo.Add(DebugInformationFactory.CreateTextDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), c.vector[1].ToString(), GeometryFriends.XNAStub.Color.Black));
                //newDebugInfo.Add(DebugInformationFactory.CreateRectangleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), new Size(c.vector[0], c.vector[1]), GeometryFriends.XNAStub.Color.Black));

                if (gridWorld.locate(currentState).id == c.id)
                {
                    if (agentName == "myCircle")
                    {
                        newDebugInfo.Add(DebugInformationFactory.CreateCircleDebugInfo(new PointF(gridWorld.CelltoWidth(c.pos[0]), gridWorld.CelltoHeight(c.pos[1])), 5, GeometryFriends.XNAStub.Color.Orange));
                    }
                    else
                    {
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

        //implememts abstract agent interface: send messages to the circle agent
        public override List<GeometryFriends.AI.Communication.AgentMessage> GetAgentMessages()
        {
            List<AgentMessage> toSent = new List<AgentMessage>(messages);
            messages.Clear();
            return toSent;
        }

        //implememts abstract agent interface: receives messages from the circle agent
        public override void HandleAgentMessages(List<GeometryFriends.AI.Communication.AgentMessage> newMessages)
        {
            foreach (AgentMessage item in newMessages)
            {
                Log.LogInformation("Rectangle: received message from circle: " + item.Message);
                if (item.Attachment != null)
                {
                    Log.LogInformation("Received message has attachment: " + item.Attachment.ToString());
                    if (item.Attachment.GetType() == typeof(Pen))
                    {
                        Log.LogInformation("The attachment is a pen, let's see its color: " + ((Pen)item.Attachment).Color.ToString());
                    }
                }
                if (item.Message == "pushup")
                {
                    
                    if (item.Attachment != null)
                    {
                        Log.LogInformation("Received message has attachment: " + item.Attachment.ToString());
                        if (item.Attachment.GetType() == typeof(State))
                        {
                            State t = (State)item.Attachment;
                            Log.LogInformation("circle on state" + t.getState());
                            Action up = new Action(t, Moves.MORPH_UP);
                            mup = true;
                            nextAction = up;
                        }
                    }
                    Log.LogInformation(agentName + " received pushup");
                }


            }
            

        }
    }
}