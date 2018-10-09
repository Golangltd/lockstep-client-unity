﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using cocosocket4unity;
using UnityEngine.SceneManagement;

public static class MsgProcessor
{


    public static void ProcessMsg(ByteBuf bb)
    {
        var p = PacketWraper.ReadPacket(bb.GetRaw());
        UnityEngine.Debug.LogFormat("msg:{0}-[{1}]", p.id,p.data.Length);
        var id = (pb.ID)p.id;
        switch (id)
        {
            case pb.ID.MSG_Connect:
                {
                    UnityEngine.Debug.Log("pb.S2C_Connect");

                    Network.Instance.Send(pb.ID.MSG_JoinRoom);
                }
                break;
            case pb.ID.MSG_JoinRoom:
                {
                    
                    var msg = pb.S2C_JoinRoomMsg.ParseFrom(p.data);
                    GameLogic.Instance.Data.MyID = msg.Id;
                    GameLogic.Instance.JoinRoom(msg.Id);

                    foreach (var pid in msg.OthersList)
                    {
                        GameLogic.Instance.JoinRoom(pid);
                    }
                    SceneManager.LoadScene(1);

                }
                break;
            case pb.ID.MSG_Progress:
                {
                    var msg = pb.S2C_ProgressMsg.ParseFrom(p.data);
                    GameLogic.Instance.SetProgress(msg.Id,msg.Pro);
                }
                break;
            case pb.ID.MSG_Ready:
                break;
            case pb.ID.MSG_Start:
                SceneManager.LoadScene(2);
                Game.Instance.DoStart();
                break;
            case pb.ID.MSG_Frame:
            {
                var msg = pb.S2C_FrameMsg.ParseFrom(p.data);
                Game.Instance.PushFrameData(msg.FramesList.ToList());

            }
                break;
            case pb.ID.MSG_END:
            {
                  UnityEngine.Debug.LogFormat("msg:{0}-[{1}]", p.id,p.data.Length);

                }
                break;
        }
       
    }

    
}
