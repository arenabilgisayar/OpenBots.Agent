﻿using OpenBots.Agent.Core.Enums;
using OpenBots.Agent.Core.Model;
using OpenBots.Agent.Core.Model.RDP;
using OpenBots.Agent.Core.Utilities;
using OpenBots.Service.Client.Manager.Logs;
using OpenBots.Service.Client.Manager.Win32;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenBots.Service.Client.Manager.Execution
{
    /// <summary>
    /// Class that allows running applications with full admin rights. In
    /// addition the application launched will bypass the Vista UAC prompt.
    /// </summary>
    public class Executor
    {
        private Win32Utilities _win32Helper;
        private FileLogger _fileLogger;
        private RemoteDesktopState _rdpConnectionState = RemoteDesktopState.Unknown;

        public Executor(FileLogger fileLogger)
        {
            _win32Helper = new Win32Utilities();
            _fileLogger = fileLogger;
        }

        /// <summary>
        /// Launches given application in the security context of specified user
        /// </summary>
        /// <param name="commandLine">Command line containing automation info to be run by the executor</param>
        /// <param name="machineCredential">Machine credentials contaning the User Account info to run the job for.</param>
        /// <param name="serverSettings">Server Connection Settings containing screen resolution info for RDP session.</param>
        public void RunAutomation(String commandLine, MachineCredential machineCredential, ServerConnectionSettings serverSettings)
        {
            bool isRDPSession = false;
            IntPtr hPToken = IntPtr.Zero;
            RemoteDesktop rdpUtil = null;

            try
            {
                // Obtain the currently active session id for given User Credentials
                bool sessionFound = _win32Helper.GetUserSessionToken(machineCredential, ref hPToken);

                _fileLogger?.LogEvent("GetActiveUserSession", "Existing active user session " +
                    (!sessionFound ? "not found" : "found"), LogEventLevel.Information);

                // If unable to find an Active User Session (for given User Credentials)
                if (!sessionFound)
                {
                    _fileLogger?.LogEvent("ValidateUser", "Validate User Account Credentials", LogEventLevel.Information);

                    int errorCode;
                    if (!_win32Helper.ValidateUser(machineCredential, out errorCode))
                        throw new Exception($"Unable to Create an Active User Session " +
                            $"as the provided User Credentials are invalid.\n" +
                            (errorCode != 0 ? $"Error Code: {errorCode}" : string.Empty));

                    _fileLogger?.LogEvent("CreateRDPConnection", "Start RDP Connection", LogEventLevel.Information);

                    // Attempt to create a Remote Desktop Session
                    rdpUtil = new RemoteDesktop();
                    rdpUtil.ConnectionStateChangedEvent += OnConnectionStateChanged;
                    Task.Run(() => rdpUtil.CreateRdpConnection(
                        Environment.MachineName,
                        machineCredential.UserName,
                        machineCredential.Domain,
                        machineCredential.PasswordSecret,
                        serverSettings.ResolutionWidth,
                        serverSettings.ResolutionHeight));

                    _fileLogger?.LogEvent("WaitForRDPConnection", "Wait for RDP Connection", LogEventLevel.Information);

                    // Wait for RDP connection to be established within 60 sec
                    bool isConnected = WaitForRDPConnection();

                    if (!isConnected)
                    {
                        _fileLogger?.LogEvent("CreateRDPConnection", "Unable to create the RDP Session", LogEventLevel.Error);
                        
                        _fileLogger?.LogEvent("LogonUser", "Logon User to perform automation in non-interactive session", LogEventLevel.Error);

                        if (!(sessionFound = _win32Helper.LogonUserA(machineCredential, ref hPToken)))
                            throw new Exception($"Unable to Create an Active User Session for provided Credential \"{machineCredential.Name}\" ");
                    }
                    else
                    {
                        _fileLogger?.LogEvent("CreateNewSession", $"New Session Created.", LogEventLevel.Information);

                        // Obtain the id of Remote Desktop Session
                        sessionFound = _win32Helper.GetUserSessionToken(machineCredential, ref hPToken);

                        // If unable to find/create an Active User Session
                        if (!sessionFound)
                            throw new Exception($"Unable to Create an Active User Session for provided Credential \"{machineCredential.Name}\" ");

                        _fileLogger?.LogEvent("GetActiveUserSession", "Session " +
                            (!sessionFound ? "not found" : "found"), LogEventLevel.Information);

                        isRDPSession = isConnected;
                    }
                }

                _fileLogger?.LogEvent("RunProcessAsCurrentUser", $"Start Automation", LogEventLevel.Information);

                // Start a new process in the current user's logon session
                _win32Helper.RunProcessAsCurrentUser(hPToken, commandLine);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (hPToken != IntPtr.Zero)
                    _win32Helper.ClosePtrHandle(hPToken);

                if (isRDPSession)
                    rdpUtil.DisconnectRDP();
            }
        }

        private bool WaitForRDPConnection(int seconds = 60)
        {
            bool isConnected = true;

            int sec = 0;
            while (sec < seconds)
            {
                if (_rdpConnectionState == RemoteDesktopState.Connected ||
                    _rdpConnectionState == RemoteDesktopState.Errored ||
                    _rdpConnectionState == RemoteDesktopState.Disconnected)
                    break;

                Thread.Sleep(1000);
                sec++;
            }

            if (sec == 60 ||
                _rdpConnectionState == RemoteDesktopState.Errored || 
                _rdpConnectionState == RemoteDesktopState.Disconnected)
            {
                isConnected = false;
            }

            return isConnected;
        }

        private void OnConnectionStateChanged(object sender, RemoteDesktopEventArgs rdEventArgs)
        {
            _rdpConnectionState = rdEventArgs.ConnectionState;
            _fileLogger?.LogEvent("OnConnectionStateChanged", $"RDP Connection State: {_rdpConnectionState}" +
                ((_rdpConnectionState == RemoteDesktopState.Disconnected ||
                _rdpConnectionState == RemoteDesktopState.Errored) ? $", Error Code: {rdEventArgs.ErrorCode}, " +
                $"Error Description: {rdEventArgs.ErrorDescription}" : ""), LogEventLevel.Information);
        }
    }
}
