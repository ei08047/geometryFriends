using GeometryFriends.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeometryFriendsAgents
{
    public class Action
    {
        State inicial, expected, final;
        Moves move;
        GeometryFriends.XNAStub.GameTime iniciated;

        public Action(State init, GeometryFriends.AI.Moves move) {
            inicial = init;
            this.move = move;
        }
        public State calculateExpected() {

        switch ((int)move)
                {
                    case (int)GeometryFriends.AI.Moves.NO_ACTION:
                        {
                            
                            break;
                        }
                    case (int)GeometryFriends.AI.Moves.ROLL_LEFT:
                        {
                            break;
                        }
                    case (int)GeometryFriends.AI.Moves.ROLL_RIGHT:
                        {
                        break;
                        }
                    case (int)GeometryFriends.AI.Moves.JUMP:
                        {

                            break;
                        }
                   case (int)GeometryFriends.AI.Moves.GROW:
                        {
                            break;
                        }
                    case (int)GeometryFriends.AI.Moves.MOVE_LEFT:
                        {
                            break;
                        }
                    case (int)GeometryFriends.AI.Moves.MOVE_RIGHT:
                        {
                            break;
                        }
                    case (int)GeometryFriends.AI.Moves.MORPH_UP:
                        {
                            break;
                        }
                    case (int)GeometryFriends.AI.Moves.MORPH_DOWN:
                        {
                            break;
                        }
                        }

            return expected;
        }
        public void getHorizontalMovement() {
            if (inicial.getVx() > 0)
            {
                expected._posX = inicial.getX() + inicial.getVx();
            }
            else {
                expected._posX = inicial.getX() - inicial.getVx();
            }
        }
        public void getVerticalMovement()
        {
            if (inicial.getVy() > 0)
            {
                expected._posY = inicial.getY() + inicial.getVy();
            }
            else
            {
                expected._posY = inicial.getY() - inicial.getVy();
            }
        }
        public Moves getMove()
        {
            return move;
        }

    }
}
