// ========================================================
// Des：检测一段代码的运行时间
// Author：yeyichen
// CreateTime：2018/05/19 10:53:07 
// Version：v 1.0
// ========================================================
/****************************************************************************
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
    using System.Diagnostics;

    /// <summary>
    /// 执行一段函数所用的时间
    /// 先new，在需要查看运行时间的函数前后分别加上Begin和Stop
    /// 
    /// 例子
    /// 
    //CodeRunTimeTool tool=new CodeRunTimeTool(true);
    //tool.Begin();
    //
    //int a=0;
    //for(int i=0;i<1000000;i++)
    //{
    //  a+=i;
    //}
    //
    //tool.Stop();
    /// 
    /// </summary>
    public class CodeRunTimeTool
    {
        Stopwatch sw;

        bool useMicroSecond = false;

        string tip;

        int index = 1;

        public CodeRunTimeTool()
        {
            this.tip = "该段函数";
            sw = new Stopwatch();
        }

        public CodeRunTimeTool(bool useMicroSecond)
        {
            this.useMicroSecond = useMicroSecond;
            this.tip = "该段函数";
            sw = new Stopwatch();
        }

        public CodeRunTimeTool(string tip)
        {
            this.tip = tip;
            sw = new Stopwatch();
        }

        public CodeRunTimeTool(bool useMicroSecond,string tip)
        {
            this.tip = tip;
            this.useMicroSecond = useMicroSecond;
            sw = new Stopwatch();
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Begin()
        {
            sw.Reset();
            sw.Start();
        }

        /// <summary>
        /// 结束计时并打印结果
        /// </summary>
        public void Stop()
        {
            sw.Stop();

            //有精度要求或者用时过短,使用微秒
            if (useMicroSecond || sw.ElapsedMilliseconds < 1)
            {
                UnityEngine.Debug.Log("执行" + tip + "共用了" + (sw.ElapsedTicks / 10).ToString() + "微秒");
            }
            else
            {
                UnityEngine.Debug.Log("执行" + tip + "共用了" + sw.ElapsedMilliseconds.ToString() + "毫秒");
            }

            sw.Reset();
        }

        /// <summary>
        /// 暂停计时
        /// </summary>
        public void Pause()
        {
            sw.Stop();

            //有精度要求或者用时过短,使用微秒
            if (useMicroSecond || sw.ElapsedMilliseconds < 1)
            {
                UnityEngine.Debug.Log("执行" + tip + "第" + index + "部分共用了" + (sw.ElapsedTicks / 10).ToString() + "微秒");
            }
            else
            {
                UnityEngine.Debug.Log("执行" + tip + "第" + index + "部分共用了" + sw.ElapsedMilliseconds.ToString() + "毫秒");
            }

            index++;
        }

        /// <summary>
        /// 继续计时
        /// </summary>
        public void Resume()
        {
            sw.Start();
        }
    }
}
