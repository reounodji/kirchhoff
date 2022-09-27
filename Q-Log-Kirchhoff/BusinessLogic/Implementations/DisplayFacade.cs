
using KnausTabbert.Display.Escape;
using KnausTabbert.Display.GTX;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace MVC.BusinessLogic.Implementations
{
    public class DisplayFacade : IDisplayFacade
    {
        private readonly ILogger<DisplayFacade> _logger;
        private readonly IServiceProvider _serviceProvider;

        private const int MAX_LINES_FOR_A_DISLPAY = 5;
        private const byte DISPLAY_MODULE_ADRESSE_1 = 1;
        private const byte DISPLAY_MODULE_ADRESSE_2 = 2;
        private const byte DISPLAY_MODULE_ADRESSE_3 = 3;
        private const byte DISPLAY_MODULE_ADRESSE_4 = 4;



        private List<List<OpenRegistration>> previousRegistrationsToShow = new List<List<OpenRegistration>>();
        private List<List<OpenRegistration>> previousOpenRegistrationsForTheFirtsScreen = new List<List<OpenRegistration>>();
        private List<List<OpenRegistration>> previousOpenRegistrationsForTheSecondScreen = new List<List<OpenRegistration>>();

        private readonly byte[] _hornProtocol;

        public DisplayFacade(ILogger<DisplayFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

            _hornProtocol = new byte[5];
            _hornProtocol[0] = 0x1B;     // ESC  Escape
            _hornProtocol[1] = 0x21;     // I assume this is the address ?
            _hornProtocol[2] = 0x52;     // this and the next one are probably for the timing of the sound.
            _hornProtocol[3] = 0x38;
            _hornProtocol[4] = 0x0D;     // CR Carriage Return

        }

        public List<List<OpenRegistration>> Update(List<List<OpenRegistration>> previousRegistrationsToShow)
        {
            this.previousRegistrationsToShow = previousRegistrationsToShow;
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                var displayConfigurations = _displayRepository.GetAll();

                var _openRegistrationRepos = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                var openRegistrations = _openRegistrationRepos.GetAll();

                for (int i = 0; i < displayConfigurations.Count; i++)
                {

                    List<OpenRegistration> previous = new List<OpenRegistration>();
                    try
                    {
                        if (previousRegistrationsToShow.Count > i)
                            previous = previousRegistrationsToShow.ElementAt(i);
                        else
                        {
                            previousRegistrationsToShow.Add(previous);  // if no list for this display existst, add a new one.
                        }
                    }
                    catch (Exception)
                    {
                        _logger.LogWarning("Could not find previousRegistrationsToShow for displayconfiguration: " + displayConfigurations[i].Name);
                    }


                    DisplayRegistredData(displayConfigurations[i], openRegistrations, previous);

                }
            }
            return previousRegistrationsToShow;
        }


        private void DisplayRegistredData(DisplayConfiguration configuration, List<OpenRegistration> openRegistrations, List<OpenRegistration> previousRegistrations)
        {
            try
            {
                var relevantRegistrations = GetRelevantRegistrations(configuration, openRegistrations);

                bool useHorn = false;

                foreach (var relevant in relevantRegistrations)
                {
                    if (previousRegistrations == null || !previousRegistrations.Contains(relevant))
                        useHorn = true;
                }

                var relevantRegistrationsForPage = relevantRegistrations.Take(MAX_LINES_FOR_A_DISLPAY);
                var secondRelevantRegistrationsForPage = relevantRegistrations.Skip(MAX_LINES_FOR_A_DISLPAY).Take(MAX_LINES_FOR_A_DISLPAY);


                var registrationsToDisplay = GetRegistrationsToDisplay(configuration, relevantRegistrationsForPage.ToList());
                var secondRegistrationsToDisplay = GetRegistrationsToDisplay(configuration, secondRelevantRegistrationsForPage.ToList());

                var stringsToDisplay = GetStringsToDisplay(configuration.Rows, configuration.CharsPerLine, registrationsToDisplay);
                var stringsToSecondDisplay = GetStringsToDisplay(configuration.Rows, configuration.CharsPerLine, secondRegistrationsToDisplay);

                byte[] gtxProtocolForAddress1 = CreateProtocol(stringsToDisplay, DISPLAY_MODULE_ADRESSE_1).ToArray();
                byte[] indicatorProtocolForAddress1 = CreateIndicatorProtocol(registrationsToDisplay, configuration.Rows);

                byte[] gtxProtocolForAddress2 = CreateProtocol(stringsToSecondDisplay, DISPLAY_MODULE_ADRESSE_2).ToArray();
                byte[] indicatorProtocolForAddress2 = CreateIndicatorProtocol(secondRegistrationsToDisplay, configuration.Rows, 0x22);

                byte[] gtxProtocolForAddress3 = CreateProtocol(stringsToDisplay, DISPLAY_MODULE_ADRESSE_3).ToArray();

                byte[] gtxProtocolForAddress4 = CreateProtocol(stringsToSecondDisplay, DISPLAY_MODULE_ADRESSE_4).ToArray();

                IPAddress ip = IPAddress.Parse(configuration.IPAddress);

                Send(gtxProtocolForAddress1.ToArray(), ip, configuration.Port, configuration.TcpTimeoutInMs, "gtx");
                Send(gtxProtocolForAddress2.ToArray(), ip, configuration.Port, configuration.TcpTimeoutInMs, "gtx");
                Send(gtxProtocolForAddress3.ToArray(), ip, configuration.Port, configuration.TcpTimeoutInMs, "gtx");
                Send(gtxProtocolForAddress4.ToArray(), ip, configuration.Port, configuration.TcpTimeoutInMs, "gtx");

                var byteIndicatorListe = indicatorProtocolForAddress1.ToArray().ToList();
                byteIndicatorListe.AddRange(indicatorProtocolForAddress2.ToArray());

                Send(byteIndicatorListe.ToArray(), ip, configuration.Port, configuration.TcpTimeoutInMs, "indicator");


                if (useHorn)
                {
                    Send(_hornProtocol, ip, configuration.Port, configuration.TcpTimeoutInMs, "relay");
                }

                previousRegistrations.Clear();
                previousRegistrations.AddRange(relevantRegistrations);

            }
            catch (Exception e)
            {
                _logger.LogWarning("Exception while sending data to the displays. Message: " + e.Message);
                return;
            }
        }

        private byte[] CreateIndicatorProtocol(List<OpenRegistration> registrationsToDisplay, int rows, byte address = 0x21)
        {
            // each row there are 2 escape displays ( X and ->)
            byte[] data = new byte[2 * rows];

            for (int i = 0; i < rows; i++)
            {
                if (i < registrationsToDisplay.Count)
                {
                    // if the current registration is allowed to enter, turn on the -> and turn off the x
                    if (registrationsToDisplay[i].TimeOfCall != new DateTime())
                    {
                        data[i * 2] = 0x30;
                        data[i * 2 + 1] = 0x31;
                    }
                    // if the current registration isnt allowed to enter, turn on the x and turn off the ->
                    else
                    {
                        data[i * 2] = 0x31;
                        data[i * 2 + 1] = 0x30;
                    }
                }
                // if the registration doesnt exist, turn off both the x and the ->
                else
                {
                    data[i * 2] = 0x30;
                    data[i * 2 + 1] = 0x30;
                }
            }
            return new EscapeProtocol(data, address).ToArray();
        }

        /// <summary>
        /// Creates a Socket and Sends the data passed as param. if it is gtx it will check for ack
        /// </summary>
        /// <param name="data"></param>
        /// <param name="isEsc"></param>
        /// <returns></returns>
        private void Send(byte[] data, IPAddress ipAddress, int port, int timeout, string source)
        {

            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
            using (Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.SendTimeout = timeout;
                socket.ReceiveTimeout = timeout;

                try
                {
                    IAsyncResult result = socket.BeginConnect(ipEndPoint, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(timeout, true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                if (!socket.Connected)
                {
                    _logger.LogWarning("Could not connect to display with ip: " + ipAddress + " and port: " + port + " while sending " + source);
                    return;
                }

                try
                {
                    socket.Send(data);
                }
                catch (Exception ex)
                {
                    if (socket.Connected)
                        socket.Disconnect(false);

                    throw ex;
                }
                if (socket.Connected)
                    socket.Disconnect(false);
            }
        }

        private GtxProtocol CreateProtocol(List<string> messageList, byte adress)
        {

            var protocol = new GtxProtocol(messageList, (byte)messageList.Count, adress, false, true, true, false);

            return protocol;
        }

        private List<string> GetStringsToDisplay(int rows, int charsPerLine, List<OpenRegistration> openRegistrations)
        {
            var list = new List<string>();

            foreach (var regist in openRegistrations)
            {
                int gateLength = 2;
                if (!string.IsNullOrWhiteSpace(regist.Gate))
                    gateLength = regist.Gate.Length;

                var line = "";
                if (charsPerLine <= regist.LicensePlate.Length + gateLength)
                {
                    for (int i = 0; i < charsPerLine - 1 - gateLength; i++)
                    {
                        if (regist.LicensePlate.Length > i)
                        {
                            line += regist.LicensePlate.ElementAt(i);
                        }
                        else
                        {
                            line += " ";
                        }
                    }
                    line += " ";

                    if (regist.TimeOfCall != new DateTime())
                        line += regist.Gate;
                    else
                        line = line.PadRight(charsPerLine);
                }
                else
                {
                    line = regist.LicensePlate;
                    line = line.PadRight(charsPerLine - gateLength, ' ');

                    if (regist.TimeOfCall != new DateTime())
                        line += regist.Gate;
                    else
                        line = line.PadRight(charsPerLine);
                }
                list.Add(line);
            }

            for (int i = openRegistrations.Count; i < rows; i++)
            {
                var line = "";
                line = line.PadLeft(charsPerLine, ' ');
                list.Add(line);
            }

            return list;
        }

        /// <summary>
        /// Select those openRegistrations that have been called and should be displayed.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="openRegistrations"></param>
        /// <returns></returns>
        private List<OpenRegistration> GetRelevantRegistrations(DisplayConfiguration configuration, List<OpenRegistration> openRegistrations)
        {

            var called = openRegistrations.OrderBy(x => x.TimeOfRegistration).ToList();

            var relevant = new List<OpenRegistration>();
            if (configuration.EntryRemovalType == EEntryRemovalType.Entry)
            {
                relevant = (from r in called
                            where r.TimeOfEntry == new DateTime()
                            select r).ToList();
            }
            else if (configuration.EntryRemovalType == EEntryRemovalType.Exit)
            {
                relevant = (from r in called
                            where r.TimeOfExit == new DateTime()
                            select r).ToList();
            }

            return relevant;
        }


        /// <summary>
        /// Returns those OpenRegistrations that should be displayed in this cycle for this display. this takes care of paging.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="relevant"></param>
        /// <returns></returns>
        private List<OpenRegistration> GetRegistrationsToDisplay(DisplayConfiguration configuration, List<OpenRegistration> relevant)
        {
            var list = new List<OpenRegistration>();


            if (relevant.Count <= configuration.Rows)
            {
                configuration.curDisplayedStartingIndex = 0;
                list.AddRange(relevant);
            }
            else
            {
                // more entries are shown than are relevant atm.
                if (configuration.curDisplayedStartingIndex >= relevant.Count)
                {
                    // restart at the first page

                    list.AddRange(relevant.GetRange(0, Math.Min(relevant.Count, configuration.Rows)));

                    configuration.curDisplayedStartingIndex = Math.Min(relevant.Count, configuration.Rows);
                }
                else
                {
                    // if there are more relevant than there have been shown so far
                    if (relevant.Count > configuration.curDisplayedStartingIndex)
                    {
                        var remainingRelevant = relevant.Count - configuration.curDisplayedStartingIndex;
                        list.AddRange(relevant.GetRange(configuration.curDisplayedStartingIndex, Math.Min(remainingRelevant, configuration.Rows)));
                        configuration.curDisplayedStartingIndex += configuration.Rows;
                    }
                }
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayConfigurationRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                _displayConfigurationRepository.Edit(configuration);
            }
            return list.OrderBy(x => x.TimeOfRegistration).ToList();
        }


    }
}
