// --------------------------------------------------------------
// Shake Shock - CreateAndJoinRooms                     3/12/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public void JoinRoom()
    {
        if (!PhotonNetwork.CreateRoom("text"))
        {
            PhotonNetwork.JoinRoom("text");
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
