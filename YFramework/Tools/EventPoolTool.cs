// ========================================================
// Des：事件池
// 带参数的回调只会返回最后一个方法的执行结果，但所有方法仍然将被依次执行，
// 所以单次回调调用中，引用类型参数的值将被每个方法依次计算，
// 而值类型参数相当于只调用了最后一个方法。
// 
// Author：yeyichen
// CreateTime：2018/05/19 10:53:07 
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 20?? ??????
 * Copyright (c) 2018 yeyichen
 * 
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 ****************************************************************************/

namespace YFramework
{
    using System.Collections;
    using System.Collections.Generic;
    using System;
    using UnityEngine;

    public class EventPoolTool : QSingleton<EventPoolTool>
    {
#if UNITY_EDITOR
        #region
        public class ExampleClass
        {
            public int j;
        }

        static int i = 0;
        static ExampleClass newI;
        static string eventName = "NumEvent";
        static string eventNameI = "NumEventI";

        public static void Example()
        {
            Debug.Log(i);
            EventPoolTool.Instance.AddEventListener(eventName, AddI);
            EventPoolTool.Instance.AddEventListener(eventName, AddI);
            EventPoolTool.Instance.TriggerEvent(eventName);
            Debug.Log("expect:2,now:" + i);

            EventPoolTool.Instance.RemoveEventListener(eventName, AddI);
            EventPoolTool.Instance.TriggerEvent(eventName);
            Debug.Log("expect:3,now:" + i);

            EventPoolTool.Instance.RemoveEventListenerAll(eventName);
            EventPoolTool.Instance.TriggerEvent(eventName);
            Debug.Log("expect:3,now:" + i);

            newI = new ExampleClass { j = 0 };
            EventPoolTool.Instance.AddEventListener<ExampleClass, int, int>(eventNameI, IAdd);
            i = EventPoolTool.Instance.TriggerEvent<ExampleClass, int, int>(eventNameI, newI, 2);
            Debug.Log("expect:2,now:" + i);

            EventPoolTool.Instance.AddEventListener<ExampleClass, int, int>(eventNameI, IMulti);
            i = EventPoolTool.Instance.TriggerEvent<ExampleClass, int, int>(eventNameI, newI, 3);
            Debug.Log("expect:15,now:" + i);

            EventPoolTool.Instance.RemoveEventListener<ExampleClass, int, int>(eventNameI, IMulti);
            i = EventPoolTool.Instance.TriggerEvent<ExampleClass, int, int>(eventNameI, newI, 2);
            Debug.Log("expect:17,now:" + i);

            EventPoolTool.Instance.RemoveEventListenerAll(eventNameI);
            i = EventPoolTool.Instance.TriggerEvent<ExampleClass, int, int>(eventNameI, newI, 11111111);
            Debug.Log("expect:0,now:" + i);

        }

        static int IAdd(ExampleClass ii, int num)
        {
            ii.j += num;
            return ii.j;
        }

        static int IMulti(ExampleClass ii, int num)
        {
            ii.j *= num;
            return ii.j;
        }

        static void IA(ExampleClass ii)
        {
            ii.j++;
        }

        static void IM(ExampleClass ii)
        {
            ii.j--;
        }

        static int Add(int num)
        {
            return i + num;
        }

        static int Multi(int num)
        {
            return num * i;
        }

        static void AddI()
        {
            i++;
        }

        static void MinusI()
        {
            i--;
        }
        #endregion
#endif

        private EventPoolTool()
        {
            
        }

        //永久性的消息，在Cleanup的时候，这些消息的响应是不会被清理的。  
        private List<string> permanentEvents = new List<string>();  
        private Dictionary<string, Delegate> delegateMap = new Dictionary<string, Delegate>(); 

        #region 添加事件监听
        //检验是否能正确添加
        private bool OnListenerAdding(string eventType, Delegate listenerBeingAdded)  
        {  
            if (!delegateMap.ContainsKey(eventType))  
            {  
                delegateMap.Add(eventType, null);  
            }  
            Delegate delegate2 = delegateMap[eventType];  
            if ((delegate2 != null) && (delegate2.GetType() != listenerBeingAdded.GetType()))  
            {  
                UnityEngine.Debug.LogError(string.Format("Try to add not correct event {0}. Current type is {1}, adding type is {2}.", eventType, delegate2.GetType().Name, listenerBeingAdded.GetType().Name));
                return false;
            }
            return true;
        }  

        //没有返回值的五种重载,0-4个参数
        public void AddEventListener(string eventType, Action handler)  
        {  
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Action)Delegate.Combine((Action)delegateMap[eventType], handler);  
            }
        }  

        public void AddEventListener<T>(string eventType, Action<T> handler)  
        {  
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Action<T>)Delegate.Combine((Action<T>)delegateMap[eventType], handler);  
            }
        }  
        public void AddEventListener<T, U>(string eventType, Action<T, U> handler)  
        {  
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Action<T, U>)Delegate.Combine((Action<T, U>)delegateMap[eventType], handler);  
            }
        }  
        public void AddEventListener<T, U, V>(string eventType, Action<T, U, V> handler)  
        {  
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Action<T, U, V>)Delegate.Combine((Action<T, U, V>)delegateMap[eventType], handler);  
            }
        }  
        public void AddEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)  
        {  
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Action<T, U, V, W>)Delegate.Combine((Action<T, U, V, W>)delegateMap[eventType], handler);  
            }
        }  

        //有返回值的四种重载，最后一个泛型是返回值类型，0-4个参数
        public void AddEventListener<R>(string eventType,Func<R> handler)
        {
            if(OnListenerAdding(eventType, handler)) 
            {
                delegateMap[eventType] = (Func<R>)Delegate.Combine((Func<R>)delegateMap[eventType], handler);  
            }
        }

        public void AddEventListener<T,R>(string eventType, Func<T,R> handler)
        {
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Func<T, R>)Delegate.Combine((Func<T, R>)delegateMap[eventType], handler);
            }
        }

        public void AddEventListener<T,U,R>(string eventType, Func<T,U,R> handler)
        {
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Func<T, U, R>)Delegate.Combine((Func<T, U, R>)delegateMap[eventType], handler);
            }
        }

        public void AddEventListener<T,U,V,R>(string eventType, Func<T,U,V,R> handler)
        {
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Func<T, U, V, R>)Delegate.Combine((Func<T, U, V, R>)delegateMap[eventType], handler);
            }
        }

        public void AddEventListener<T, U, V, W, R>(string eventType, Func<T, U, V, W, R> handler)
        {
            if(OnListenerAdding(eventType, handler))
            {
                delegateMap[eventType] = (Func<T, U, V, W, R>)Delegate.Combine((Func<T, U, V, W, R>)delegateMap[eventType], handler);
            }
        }
        #endregion

        #region 移除事件监听
        private void OnListenerRemoved(string eventType)  
        {  
            if (delegateMap.ContainsKey(eventType) && (delegateMap[eventType] == null))  
            {  
                delegateMap.Remove(eventType);  
            }  
        }  

        //检验是否能正确移除
        private bool OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)  
        {  
            if (!delegateMap.ContainsKey(eventType))  
            {  
                return false;  
            }  
            Delegate delegate2 = delegateMap[eventType];  
            if ((delegate2 != null) && (delegate2.GetType() != listenerBeingRemoved.GetType()))  
            {  
                UnityEngine.Debug.LogError(string.Format("Remove listener {0}\" failed, Current type is {1}, adding type is {2}.", eventType, delegate2.GetType(), listenerBeingRemoved.GetType()));  
            }  
            return true;  
        }  

        //没有返回值的五种重载,0-4个参数
        public void RemoveEventListener(string eventType, Action handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Action)Delegate.Remove((Action)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T>(string eventType, Action<T> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Action<T>)Delegate.Remove((Action<T>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T, U>(string eventType, Action<T, U> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Action<T, U>)Delegate.Remove((Action<T, U>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T, U, V>(string eventType, Action<T, U, V> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Action<T, U, V>)Delegate.Remove((Action<T, U, V>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T, U, V, W>(string eventType, Action<T, U, V, W> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Action<T, U, V, W>)Delegate.Remove((Action<T, U, V, W>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        //有返回值的四种重载，最后一个泛型是返回值类型，0-4个参数
        public void RemoveEventListener<R>(string eventType, Func<R> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Func<R>)Delegate.Remove((Func<R>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T,R>(string eventType, Func<T,R> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Func<T,R>)Delegate.Remove((Func<T,R>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T,U,R>(string eventType, Func<T,U,R> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Func<T,U,R>)Delegate.Remove((Func<T,U,R>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T,U,V,R>(string eventType, Func<T,U,V,R> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Func<T,U,V,R>)Delegate.Remove((Func<T,U,V,R>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListener<T,U,V,W,R>(string eventType, Func<T,U,V,W,R> handler)  
        {  
            if (OnListenerRemoving(eventType, handler))  
            {  
                delegateMap[eventType] = (Func<T,U,V,W,R>)Delegate.Remove((Func<T,U,V,W,R>)delegateMap[eventType], handler);  
                OnListenerRemoved(eventType);  
            }  
        }  

        public void RemoveEventListenerAll(string eventType)
        {
            if(ContainsEvent(eventType))
            {
                delegateMap.Remove(eventType);
            }
        }
        #endregion

        #region 触发事件
        public void TriggerEvent(string eventType)  
        {  
            Delegate delegate2;  
            if (delegateMap.TryGetValue(eventType, out delegate2))  
            {  
                Delegate[] invocationList = delegate2.GetInvocationList();  
                for (int i = 0; i < invocationList.Length; i++)  
                {  
                    Action action = invocationList[i] as Action;  
                    if (action == null)  
                    {  
                        UnityEngine.Debug.LogError(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));  
                    }  
                    try  
                    {  
                        action();  
                    }  
                    catch (Exception exception)  
                    {  
                        UnityEngine.Debug.LogError(exception.Message);  
                    }  
                }  
            }  
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called"+eventType);
            }
        }  

        public void TriggerEvent<T>(string eventType, T arg1)  
        {  
            Delegate delegate2;  
            if (delegateMap.TryGetValue(eventType, out delegate2))  
            {  
                Delegate[] invocationList = delegate2.GetInvocationList();  
                for (int i = 0; i < invocationList.Length; i++)  
                {  
                    Action<T> action = invocationList[i] as Action<T>;  
                    if (action == null)  
                    {  
                        UnityEngine.Debug.LogError(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));  
                    }  
                    try  
                    {  
                        action(arg1);  
                    }  
                    catch (Exception exception)  
                    {  
                        UnityEngine.Debug.LogError(exception.Message);  
                    }  
                }  
            }  
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called"+eventType);
            }
        }  

        public void TriggerEvent<T, U>(string eventType, T arg1, U arg2)  
        {  
            Delegate delegate2;  
            if (delegateMap.TryGetValue(eventType, out delegate2))  
            {  
                Delegate[] invocationList = delegate2.GetInvocationList();  
                for (int i = 0; i < invocationList.Length; i++)  
                {  
                    Action<T, U> action = invocationList[i] as Action<T, U>;  
                    if (action == null)  
                    {  
                        UnityEngine.Debug.LogError(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));  
                    }  
                    try  
                    {  
                        action(arg1, arg2);  
                    }  
                    catch (Exception exception)  
                    {  
                        UnityEngine.Debug.LogError(exception.Message);  
                    }  
                }  
            }  
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called "+eventType);
            }
        }  

        public void TriggerEvent<T, U, V>(string eventType, T arg1, U arg2, V arg3)  
        {  
            Delegate delegate2;  
            if (delegateMap.TryGetValue(eventType, out delegate2))  
            {  
                Delegate[] invocationList = delegate2.GetInvocationList();  
                for (int i = 0; i < invocationList.Length; i++)  
                {  
                    Action<T, U, V> action = invocationList[i] as Action<T, U, V>;  
                    if (action == null)  
                    {  
                        UnityEngine.Debug.LogError(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));  
                    }  
                    try  
                    {  
                        action(arg1, arg2, arg3);  
                    }  
                    catch (Exception exception)  
                    {  
                        UnityEngine.Debug.LogError(exception.Message);  
                    }  
                }  
            }  
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called "+eventType);
            }
        }  

        public void TriggerEvent<T, U, V, W>(string eventType, T arg1, U arg2, V arg3, W arg4)  
        {  
            Delegate delegate2;  
            if (delegateMap.TryGetValue(eventType, out delegate2))  
            {  
                Delegate[] invocationList = delegate2.GetInvocationList();  
                for (int i = 0; i < invocationList.Length; i++)  
                {  
                    Action<T, U, V, W> action = invocationList[i] as Action<T, U, V, W>;  
                    if (action == null)  
                    {  
                        UnityEngine.Debug.LogError(string.Format("TriggerEvent {0} error: types of parameters are not match.", eventType));  
                    }  
                    try  
                    {  
                        action(arg1, arg2, arg3, arg4);  
                    }  
                    catch (Exception exception)  
                    {  
                        UnityEngine.Debug.LogError(exception.Message);  
                    }  
                }  
            }  
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called "+eventType);
            }
        }  

        //有返回值的四种重载，最后一个泛型是返回值类型，0-4个参数
        public R TriggerEvent<R>(string eventType)  
        {
            R r = default(R);
            if (delegateMap.ContainsKey(eventType))
            {
                Func<R> callback;
                callback = delegateMap[eventType] as Func<R>;
                try
                {
                    if (callback != null)
                        r = callback.Invoke();//如果不为空调用，unity2017以下不可简写    
                }
                catch(Exception exception)
                {
                    UnityEngine.Debug.LogError(exception.Message);  
                }
            }
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called " + eventType);
            }
            return r;
        }  

        public R TriggerEvent<T,R>(string eventType,T arg1)  
        {
            R r = default(R);
            if (delegateMap.ContainsKey(eventType))
            {
                Func<T,R> callback;
                callback = delegateMap[eventType] as Func<T,R>;
                UnityEngine.Debug.LogError(callback.GetInvocationList().Length);
                try
                {
                    if (callback != null)
                        r = callback.Invoke(arg1);
                }
                catch(Exception exception)
                {
                    UnityEngine.Debug.LogError(exception.Message);  
                }
            }
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called " + eventType);
            }
            return r;
        }  

        public R TriggerEvent<T,U,R>(string eventType,T arg1,U arg2)  
        {
            R r = default(R);
            if (delegateMap.ContainsKey(eventType))
            {
                Func<T,U,R> callback;
                callback = delegateMap[eventType] as Func<T,U,R>;
                try
                {
                    if (callback != null)
                        r = callback.Invoke(arg1,arg2);//如果不为空调用，unity2017以下不可简写    
                }
                catch(Exception exception)
                {
                    UnityEngine.Debug.LogError(exception.Message);  
                }
            }
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called " + eventType);
            }
            return r;
        }  

        public R TriggerEvent<T,U,V,R>(string eventType,T arg1,U arg2,V arg3)  
        {
            R r = default(R);
            if (delegateMap.ContainsKey(eventType))
            {
                Func<T,U,V,R> callback;
                callback = delegateMap[eventType] as Func<T,U,V,R>;
                try
                {
                    if (callback != null)
                        r = callback.Invoke(arg1,arg2,arg3);//如果不为空调用，unity2017以下不可简写    
                }
                catch(Exception exception)
                {
                    UnityEngine.Debug.LogError(exception.Message);  
                }
            }
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called " + eventType);
            }
            return r;
        }  

        public R TriggerEvent<T,U,V,W,R>(string eventType,T arg1,U arg2,V arg3,W arg4)  
        {
            R r = default(R);
            if (delegateMap.ContainsKey(eventType))
            {
                Func<T,U,V,W,R> callback;
                callback = delegateMap[eventType] as Func<T,U,V,W,R>;
                try
                {
                    if (callback != null)
                        r = callback.Invoke(arg1,arg2,arg3,arg4);//如果不为空调用，unity2017以下不可简写    
                }
                catch(Exception exception)
                {
                    UnityEngine.Debug.LogError(exception.Message);  
                }
            }
            else
            {
                UnityEngine.Debug.Log("TriggerEvent doesn't contains Delegate called " + eventType);
            }
            return r;
        }  
        #endregion

        /// <summary>
        /// 清除所有非永久注册事件，一般在切换场景时需要清理一下
        /// </summary>
        public void CleanUp()  
        {  
            List<string> list = new List<string>();  
            foreach (KeyValuePair<string, Delegate> pair in delegateMap)  
            {  
                bool flag = false;  
                foreach (string str in permanentEvents)  
                {  
                    if (pair.Key == str)  
                    {  
                        flag = true;  
                        break;  
                    }  
                }  
                if (!flag)  
                {  
                    list.Add(pair.Key);  
                }  
            }  
            foreach (string str in list)  
            {  
                delegateMap.Remove(str); 
            }  
        }  

        public bool ContainsEvent(string eventType)  
        {  
            return delegateMap.ContainsKey(eventType);  
        }  

        public void MarkAsPermanent(string eventType)  
        {  
            if(!permanentEvents.Contains(eventType))
            {
                permanentEvents.Add(eventType);  
            }
        }  

        public Dictionary<string, Delegate> Map  
        {  
            get  
            {  
                return delegateMap;  
            }  
        }  
    }  
}
