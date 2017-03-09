﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Searches through the graph
public class Search
{
    //Creating references for nodes, and graph
    public Graph graph;
    public List<Node> reachable; //Open
    public List<Node> explored;  //Closed
    public List<Node> path;
    public Node targetNode;

    //Tracks how many iterations have been completed for debuging purposes
    public int iterations;
    public bool finished;

    //Constructor : takes a graph
    public Search(Graph graph)
    {
        this.graph = graph;
    }

    //Create the search method which takes in a start and target node
    public void Start(Node start, Node target)
    {
        //Add the start node to the reachable/open list
        reachable = new List<Node>();
        reachable.Add(start);

        targetNode = target;

        //Create the explored/closed list and path list
        explored = new List<Node>();
        path = new List<Node>();
        iterations = 0;

        //Clear the graph in case we have ran this previously
        for(var i = 0; i < graph.nodes.Length; i++)
        {
            graph.nodes[i].Clear();
        }
    }

    //Checks possible moves that can be made
    public void Step()
    {
        if (path.Count > 0)
        {
            return;
        }

        //Check if we ran out of options
        if (reachable.Count == 0)
        {
            finished = true;
            return;
        }

        //Track number of iterations for performance purposes
        iterations++;

        //Pick a node to start the search from
        var node = ChooseNode();

        //Check if the node is the target node
        //Add the node to the path and set the node as the previous node
        if(node == targetNode)
        {
            while(node != null)
            {
                path.Insert(0, node);
                node = node.previous;
            }
            finished = true;
            return;
        }

        //Remove the current node from the open list
        //Add it in the closed list
        reachable.Remove(node);
        explored.Add(node);

        //Iterate through adjacent nodes
        //For all values, add adjacent nodes
        for (var i = 0; i < node.adjacent.Count; i++)
        {
            AddAdjacent(node, node.adjacent[i]);
        }
    }

    //Loops through all adjacent nodes and finds next
    //available options. Makes that node available (open List)
    //and creates connection at previous node.
    public void AddAdjacent(Node node, Node adjacent)
    {
        //If found, we return the node and we have found a new path
        if(FindNode(adjacent, explored) || FindNode(adjacent, reachable))
        {
            return;
        }

        //Set the previous node from adj to current node
        //Add adj node to open list
        adjacent.previous = node;
        reachable.Add(adjacent);
    }
    
    //Finds a node in the list
    public bool FindNode(Node node, List<Node> list)
    {
        return getNodeIndex(node, list) >= 0;
    }

    //Tests is node is in the list
    public int getNodeIndex(Node node, List<Node> list)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if(node == list[i])
            {
                return i;
            }
        }

        return -1;
    }

    //Chooses the node to check

    public Node ChooseNode()
    {
        //return reachable[Random.Range(0, reachable.Count)];

        while(reachable.Count > 0)
        {
            Node currentNode = reachable[0];
            for (int i = 0; i < reachable.Count; i++)
            {
                if(reachable[i].f < currentNode.f || reachable[i].f == currentNode.f && reachable[i].h < currentNode.h)
                {
                    currentNode = reachable[i];
                }
            }
        }

        return;

    }
}
