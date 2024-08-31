using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;
using System.Collections;

namespace W3Labs.ViralRunner.Network
{


    public class Dispatcher : MonoBehaviour
    {
        public static void RunAsync(Action action)
        {
            ThreadPool.QueueUserWorkItem(o => action());
        }

        public static void RunAsync(Action<object> action, object state)
        {
            ThreadPool.QueueUserWorkItem(o => action(o), state);
        }

        public static void RunOnMainThread(Action action)
        {
            lock (_backlog)
            {
                _backlog.Add(action);
                _queued = true;
            }
        }

        public static void RunOnLateUpdate(Action action)
        {
            lock (_backlogL)
            {
                _backlogL.Add(action);
                _queuedL = true;
            }
        }

        public static void RunDelayed(Action action, float timeDelay)
        {
            if (_instance == null) Initialize();
            _instance.StartCoroutine(DelayedActionCoroutine(action, timeDelay));
        }

        private static IEnumerator DelayedActionCoroutine(Action action, float timeDelay)
        {
            var elapsed = 0f;
            while (elapsed >= timeDelay)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            action?.Invoke();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance == null)
            {
                _instance = new GameObject("Dispatcher").AddComponent<Dispatcher>();
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        private void Update()
        {
            if (_queued)
            {
                lock (_backlog)
                {
                    var tmp = _actions;
                    _actions = _backlog;
                    _backlog = tmp;
                    _queued = false;
                }

                foreach (var action in _actions)
                    action?.Invoke();

                _actions.Clear();
            }
        }

        private void LateUpdate()
        {
            if (_queuedL)
            {
                lock (_backlogL)
                {
                    var tmp = _actions;
                    _actionsL = _backlogL;
                    _backlogL = tmp;
                    _queuedL = false;
                }

                foreach (var action in _actionsL)
                    action();

                _actionsL.Clear();
            }
        }

        static Dispatcher _instance;
        static volatile bool _queued = false;
        static List<Action> _backlog = new List<Action>(8);
        static List<Action> _actions = new List<Action>(8);

        static volatile bool _queuedL = false;
        static List<Action> _backlogL = new List<Action>(8);
        static List<Action> _actionsL = new List<Action>(8);


    }
}