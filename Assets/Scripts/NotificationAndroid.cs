using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationAndroid : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {   
        //Creates the Android Notification Channel to sned messages through.
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        //Creates the Notification that is going to be sent.
        var notification = new AndroidNotification();
        notification.Title = "PATİ";
        notification.Text = "Minik doslarımızın sana her zaman ihtiyaçları olduğunu unutma!";
        notification.FireTime = System.DateTime.Now.AddSeconds(10);

        //Sends the Notification
        AndroidNotificationCenter.SendNotification(notification, "channel_id");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
