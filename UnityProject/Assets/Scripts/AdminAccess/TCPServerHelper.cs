// This work is licensed under the Creative Commons Attribution-ShareAlike 4.0 International License. 
// To view a copy of this license, visit http://creativecommons.org/licenses/by-sa/4.0/ 
// or send a letter to Creative Commons, PO Box 1866, Mountain View, CA 94042, USA.
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class TCPServerHelper
{
    #region private members
    /// <summary> 	
    /// TCPListener to listen for incomming TCP connection 	
    /// requests. 	
    /// </summary> 	
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload. 	
    /// </summary> 	
    private Thread tcpListenerThread;
    /// <summary> 	
    /// Create handle to connected tcp client. 	
    /// </summary> 	
    private TcpClient connectedTcpClient;
    #endregion

    public UnityAction<string> ReceivedTextEvent { get; set; }

    // Use this for initialization
    public TCPServerHelper()
    {
        // Start TcpServer background thread 		
        tcpListenerThread = new(new ThreadStart(ListenForIncommingRequests))
        {
            IsBackground = true
        };
        tcpListenerThread.Start();
    }

    /// <summary> 	
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests 	
    /// </summary> 	
    private void ListenForIncommingRequests()
    {
        try
        {
            // Create listener on localhost port 8052. 			
            tcpListener = new TcpListener(IPAddress.Parse("0.0.0.0"), 6382);
            tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    // Get a stream object for reading 					
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {
                        int length;
                        // Read incomming stream into byte arrary. 						
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message. 							
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            Debug.Log("client message received as: " + clientMessage);
                            ReceivedTextEvent.Invoke(clientMessage);
                        }
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    ///// <summary> 	
    ///// Send message to client using socket connection. 	
    ///// </summary> 	
    //private void SendMessage()
    //{
    //    if (connectedTcpClient == null)
    //    {
    //        return;
    //    }

    //    try
    //    {
    //        // Get a stream object for writing. 			
    //        NetworkStream stream = connectedTcpClient.GetStream();
    //        if (stream.CanWrite)
    //        {
    //            string serverMessage = "This is a message from your server.";
    //            // Convert string message to byte array.                 
    //            byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
    //            // Write byte array to socketConnection stream.               
    //            stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
    //            Debug.Log("Server sent his message - should be received by client");
    //        }
    //    }
    //    catch (SocketException socketException)
    //    {
    //        Debug.Log("Socket exception: " + socketException);
    //    }
    //}
}