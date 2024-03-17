using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreadManager : MonoBehaviour
{
    static bool queued = false;
    static List<Action> backlog = new List<Action>();
    static List<Action> actions = new List<Action>();

    public static void RunOnMainThread(Action action)
    {
        lock (backlog)
        {
            backlog.Add(action);
            queued = true;
        }
    }

    void Update()
    {
        if (queued == true)
        {
            List<Action> tmp = actions;
            actions = backlog;
            backlog = tmp;
            queued = false;
        }

        foreach (Action action in actions)
            action();

        actions.Clear();
    }
}
