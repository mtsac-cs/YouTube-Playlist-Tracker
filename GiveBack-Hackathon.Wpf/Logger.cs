using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Media.Animation;
using System.Timers;
using Timer = System.Timers.Timer;

namespace GiveBack_Hackathon.Wpf
{
    class Logger : Lib.Logger
    {
        private static Logger instance;
        DoubleAnimation showPopupAnim;
        DoubleAnimation hidePopupAnim;

        bool msgQueueRunning = false;
        Queue<LogEvents> msgQueue;

        public Logger()
        {
            if (instance == null)
                InitializeLogger();
        }

        public void InitializeLogger()
        {
            instance = this;
            msgQueue = new Queue<LogEvents>();

            var windowWidth = MainWindow.instance.ActualWidth;
            CreateAnimation(ref showPopupAnim, 0, windowWidth);
            CreateAnimation(ref hidePopupAnim, windowWidth, 0);
            hidePopupAnim.Completed += HidePopupAnim_Completed;

            MessageLogged += Logger_MessageLogged;
        }

        private void CreateAnimation(ref DoubleAnimation animation, double from, double to)
        {
            animation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
            animation.From = from;
            animation.To = to;
            animation.Duration = TimeSpan.FromSeconds(1);
            animation.EasingFunction = new QuarticEase();
        }

        private void Logger_MessageLogged(object sender, Logger.LogEvents e)
        {
            if (e.MessageDisplayTime <= 0)
                e.MessageDisplayTime = 2000;

            msgQueue.Enqueue(e);

            if (msgQueue.Peek() == e) //Only one in queue because first one is the one we just added
                RunMsgThread();
        }

        private void RunMsgThread()
        {
            var msgThread = new Thread(() =>
            {
                LogEvents currentMessage;
                
                while (msgQueue.Count > 0)
                {
                    if (!msgQueueRunning)
                    {
                        msgQueueRunning = true;
                        currentMessage = msgQueue.Peek();
                        DisplayMessage(currentMessage.Message, currentMessage.MessageDisplayTime);
                    }
                    Thread.Sleep(100);
                }
            });

            msgThread.IsBackground = true;
            msgThread.Start();
        }

        private void DisplayMessage(string message, int msgDisplayTime)
        {
            var main = MainWindow.instance;
            main.Dispatcher.Invoke(() =>
            {
                main.MsgText.Text = message;
                main.MsgPopupGrid.Visibility = Visibility.Visible;
                main.MsgPopupGrid.BeginAnimation(MainWindow.WidthProperty, showPopupAnim);

                var hidePopupTmer = new Timer(msgDisplayTime);
                hidePopupTmer.Elapsed += HideMsgPopup;
                hidePopupTmer.AutoReset = false;
                hidePopupTmer.Start();
            });   
        }


        private void HideMsgPopup(object sender, ElapsedEventArgs e)
        {
            var main = MainWindow.instance;
            main.Dispatcher.Invoke(() =>
            {
                main.MsgPopupGrid.BeginAnimation(MainWindow.WidthProperty, hidePopupAnim);
            });

            var originalTimer = sender as Timer;
            originalTimer.Dispose();
        }

        private void HidePopupAnim_Completed(object sender, EventArgs e)
        {
            MainWindow.instance.MsgPopupGrid.Visibility = Visibility.Collapsed;
            msgQueue.Dequeue();
            msgQueueRunning = false;   
        }
    }
}
