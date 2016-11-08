using System;
using System.Collections;
using System.Linq;
using System.Text;
using GeometryFriends.AI;
namespace GeometryFriendsAgents
{
    class idastar
    {

        //   node current node
        public Node currentCircleNode;
        //g  the cost to reach current node
        int g = 0;
        //f estimated cost of the cheapest path(root..node..goal)
        //h(node)           estimated cost of the cheapest path(node..goal)
        public float heuristic(Node node, Node goal )
        {
            float h;
            Position n, g;
            n = new Position(node.getState().getX(), node.getState().getY());
            g = new Position(goal.getState().getX(), goal.getState().getY());
            h = g.Mandistance(g);
            return h;
        }

        //cost(node, succ)  step cost function
        public float cost(Node node, Node succ)
        {
            return 1;
        }

        //is_goal(node)     goal test
        public Boolean is_goal(Node node)
        {
            if (currentCircleNode.Equals(node))
                return true;
            else return false;
        }

        //successors(node)  node expanding function
        public ArrayList successors() {
            ArrayList succ = new ArrayList();
                return succ;
            }
        }

    /*


procedure ida_star(root)
bound := h(root)
loop
 t := search(root, 0, bound)
 if t = FOUND then return bound
 if t = ∞ then return NOT_FOUND
 bound := t
end loop
end procedure

function search(node, g, bound)
f := g + h(node)
if f > bound then return f
if is_goal(node) then return FOUND
min := ∞
for succ in successors(node) do
 t := search(succ, g + cost(node, succ), bound)
 if t = FOUND then return FOUND
 if t < min then min := t
end for
return min
end function

}
         */
}
