// ========================================================
// Des：在协程内打开线程
// Author：叶宜宸的MacBook Air 
// CreateTime：2018/05/19 10:53:07 
// Version：v 1.0
// ========================================================
/****************************************************************************
 * Copyright (c) 20?? ???
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
    using System;
    using System.Threading;
    using UnityEngine;
    using ThreadPriority = System.Threading.ThreadPriority;
    using YFramework.Extension;

    class WaitForThreadedTask : CustomYieldInstruction
    {
        /// <summary>  
        /// If the thread is still running  
        /// </summary>  
        private bool isRunning;

        Thread currentTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="WaitForThreadedTask"/> class.
        /// </summary>
        /// <param name="task">线程要执行的任务</param>
        /// <param name="priority">优先级</param>
        public WaitForThreadedTask(Action task, ThreadPriority priority = ThreadPriority.Normal)
        {
            isRunning = true;

            if (YFrameworkManager.Instance == null)
                Resources.Load<GameObject>("YFrameworkManager").Instantiate_L();

            currentTask = new Thread(() => {
                task();
                isRunning = false;
                YFrameworkManager.Instance.threadList.Remove(currentTask);
            });

            YFrameworkManager.Instance.threadList.Add(currentTask);
            currentTask.Start();
        }

        public override bool keepWaiting
        {
            get
            {
                return isRunning;
            }
        }
    }

}
