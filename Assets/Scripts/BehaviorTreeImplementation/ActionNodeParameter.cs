using System; 
using UnityEngine; 
using System.Collections; 
 public class ActionNodeParameter<T> : Node { 
    /* Method signature for the action. */ 
    public delegate NodeStates ActionNodeDelegate(T arg); 
 
    /* The delegate that is called to evaluate this node */ 
    private ActionNodeDelegate m_action; 
    private T m_arg;
 
    /* Because this node contains no logic itself, 
     * the logic must be passed in in the form of  
     * a delegate. As the signature states, the action 
     * needs to return a NodeStates enum */ 
    public ActionNodeParameter(ActionNodeDelegate action, T arg) { 
        m_action = action; 
        m_arg = arg;
    } 

    public T getArg() {return m_arg; }
 
    /* Evaluates the node using the passed in delegate and  
     * reports the resulting state as appropriate */ 
    public override NodeStates Evaluate() { 
        switch (m_action(m_arg)) { 
            case NodeStates.SUCCESS: 
                m_nodeState = NodeStates.SUCCESS; 
                return m_nodeState; 
            case NodeStates.FAILURE: 
                m_nodeState = NodeStates.FAILURE; 
                return m_nodeState; 
            case NodeStates.RUNNING: 
                m_nodeState = NodeStates.RUNNING; 
                return m_nodeState; 
            default: 
                m_nodeState = NodeStates.FAILURE; 
                return m_nodeState; 
        } 
    } 
}