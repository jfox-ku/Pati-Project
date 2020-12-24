using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
    using Unity.Notifications.iOS;
#endif

public class Notifications : MonoBehaviour
{
#if UNITY_IOS
    private string notificationId = "Notification Title";
    private int identifier;

    void Start()
    {
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = new TimeSpan(0, 0, 10),
            Repeats = false
        };

    }

    void OnApplicationPause(bool isPause)
    {


        if (isPause)
        {

            System.DateTime dateNotify = DateTime.Now.AddSeconds(30);

            iOSNotification notification = new iOSNotification()
            {
                Identifier = "notification",
                Title = "PATİ!",
                Subtitle = "A test notification",
                Body = "Minik doslarımızın sana her zaman ihtiyaçları olduğunu unutma!",
                ShowInForeground = true,
                ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
                CategoryIdentifier = "category_a",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };

            iOSNotificationCenter.ScheduleNotification(notification);
        }
    }
#endif
}
