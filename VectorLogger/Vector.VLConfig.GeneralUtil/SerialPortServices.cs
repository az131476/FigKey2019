using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Vector.VLConfig.GeneralUtil
{
	public class SerialPortServices
	{
		public enum ErrorCode
		{
			CommProblem,
			CannotSetIPAddress,
			CannotSetSubnetMask
		}

		private static SerialPort _serialPort;

		private static StringBuilder _responseBuffer;

		private static volatile bool _isAborted;

		private static volatile bool _isFinished;

		private static volatile bool _isAbortRequested;

		private static volatile uint _ipAddress;

		private static volatile uint _subnetMask;

		private static volatile uint _defaultIpAddress;

		private static volatile uint _defaultSubnetMask;

		private static volatile bool _isDefaultIPAddress = false;

		private static volatile SerialPortServices.ErrorCode _lastErrorCode;

		private static string _portName = "";

		private static DateTime _realtimeClock;

		private static int _processingDuration = 0;

		private static readonly int DefaultPortBaudRate = 115200;

		private static readonly Parity DefaultParity = Parity.None;

		private static readonly int DefaultDataBits = 8;

		private static readonly StopBits DefaultStopBits = StopBits.One;

		private static readonly Handshake DefaultHandshake = Handshake.None;

		private static readonly int DefaultReadTimeout = 500;

		private static readonly int DefaultWriteTimeout = 500;

		private static readonly int DefaultResponseTimeout = 2000;

		private static readonly string UserName = "root";

		private static readonly string Password = "root";

		private static readonly string PromptUserName = "00 login: ";

		private static readonly string PromptPassword = "Password: ";

		private static readonly string PromptLoggedIn = "GL-SH> ";

		private static readonly string ShellCommand_SetLoggerDate = "rt_setloggerdate {0:D2}.{1:D2}.{2:D4} {3:D2}:{4:D2}:{5:D2}";

		private static readonly string ShellCommand_ReadLoggerDate = "date +%d%m%Y%H%M%S";

		private static readonly int ReadLoggerDateResultLen = 14;

		private static readonly string SetLoggerDateResultSuccess = "Done";

		private static readonly string ShellCommand_IPAdrChangeDir = "cd /etc/init.d";

		private static readonly string ShellCommand_IPAdrWriteAdr = "echo \"ifconfig eth1 {0}.{1}.{2}.{3}\" >S45eth1";

		private static readonly string ShellCommand_IPAdrWriteSubnetMask = "echo \"ifconfig eth1 netmask {0}.{1}.{2}.{3}\" >>S45eth1";

		private static readonly string ShellCommand_IPAdrSetAdr = "ifconfig eth1 {0}.{1}.{2}.{3}";

		private static readonly string ShellCommand_IPAdrSetSubnetMask = "ifconfig eth1 netmask {0}.{1}.{2}.{3}";

		private static readonly string ShellCommand_IPAdrChangeMod = "chmod a+x *";

		private static readonly string ShellCommand_IPAdrGetAdrEth1 = "ifconfig eth1";

		private static readonly string ShellCommand_IPAdrGetAdrEth1Def = "ifconfig eth1:def";

		private static readonly string IPAdrGetAdrResult = "inet addr:";

		private static readonly string IPAdrGetSubnetMaskResult = "Mask:";

		private static object _lock = new object();

		public static readonly int CommunicationTimeout = 15000;

		public static readonly int CommunicationTimeoutSetEthernet = 25000;

		public static bool IsAborted
		{
			get
			{
				return SerialPortServices._isAborted;
			}
		}

		public static bool IsFinished
		{
			get
			{
				return SerialPortServices._isFinished;
			}
		}

		public static DateTime RealtimeClock
		{
			get
			{
				DateTime realtimeClock;
				lock (SerialPortServices._lock)
				{
					realtimeClock = SerialPortServices._realtimeClock;
				}
				return realtimeClock;
			}
			set
			{
				lock (SerialPortServices._lock)
				{
					SerialPortServices._realtimeClock = value;
				}
			}
		}

		public static string PortName
		{
			get
			{
				string portName2;
				lock (SerialPortServices._portName)
				{
					portName2 = SerialPortServices._portName;
				}
				return portName2;
			}
			set
			{
				lock (SerialPortServices._portName)
				{
					SerialPortServices._portName = value;
				}
			}
		}

		public static uint IPAddress
		{
			get
			{
				return SerialPortServices._ipAddress;
			}
			set
			{
				SerialPortServices._ipAddress = value;
			}
		}

		public static uint SubnetMask
		{
			get
			{
				return SerialPortServices._subnetMask;
			}
			set
			{
				SerialPortServices._subnetMask = value;
			}
		}

		public static uint DefaultIPAddress
		{
			get
			{
				return SerialPortServices._defaultIpAddress;
			}
			set
			{
				SerialPortServices._defaultIpAddress = value;
			}
		}

		public static uint DefaultSubnetMask
		{
			get
			{
				return SerialPortServices._defaultSubnetMask;
			}
			set
			{
				SerialPortServices._defaultSubnetMask = value;
			}
		}

		public static bool IsDefaultIPAddress
		{
			get
			{
				return SerialPortServices._isDefaultIPAddress;
			}
		}

		public static SerialPortServices.ErrorCode LastErrorCode
		{
			get
			{
				return SerialPortServices._lastErrorCode;
			}
		}

		public static string[] GetCOMPortNames()
		{
			return SerialPort.GetPortNames();
		}

		public static void RequestAbort()
		{
			if (!SerialPortServices._isFinished)
			{
				SerialPortServices._isAbortRequested = true;
			}
		}

		public static void SetRealtimeClock()
		{
			SerialPortServices.ResetFlags();
			if (!SerialPortServices.OpenSerialPort(SerialPortServices.PortName, SerialPortServices.DefaultPortBaudRate, SerialPortServices.DefaultParity, SerialPortServices.DefaultDataBits, SerialPortServices.DefaultStopBits, SerialPortServices.DefaultHandshake, SerialPortServices.DefaultReadTimeout, SerialPortServices.DefaultWriteTimeout))
			{
				SerialPortServices._isAborted = true;
				return;
			}
			if (!SerialPortServices.Login(SerialPortServices.UserName, SerialPortServices.Password))
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			DateTime dateTime = SerialPortServices.RealtimeClock.AddMilliseconds((double)SerialPortServices._processingDuration);
			SerialPortServices._processingDuration = 0;
			string lineToSend = string.Format(SerialPortServices.ShellCommand_SetLoggerDate, new object[]
			{
				dateTime.Day,
				dateTime.Month,
				dateTime.Year,
				dateTime.Hour,
				dateTime.Minute,
				dateTime.Second
			});
			SerialPortServices.SendLine("_lq");
			SerialPortServices.SendLine(lineToSend);
			SerialPortServices.RealtimeClock = dateTime.AddMilliseconds((double)SerialPortServices._processingDuration);
			if (SerialPortServices._isAbortRequested)
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			string text;
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			SerialPortServices.SendLine("_ll");
			string[] array = text.Split(new char[]
			{
				'\r',
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				if (!string.IsNullOrEmpty(text2) && text2.IndexOf(SerialPortServices.SetLoggerDateResultSuccess) >= 0)
				{
					SerialPortServices._isFinished = true;
					SerialPortServices.CloseSerialPort();
					return;
				}
			}
			SerialPortServices._isAborted = true;
			SerialPortServices.CloseSerialPort();
		}

		public static void ReadRealtimeClock()
		{
			SerialPortServices.ResetFlags();
			if (!SerialPortServices.OpenSerialPort(SerialPortServices.PortName, SerialPortServices.DefaultPortBaudRate, SerialPortServices.DefaultParity, SerialPortServices.DefaultDataBits, SerialPortServices.DefaultStopBits, SerialPortServices.DefaultHandshake, SerialPortServices.DefaultReadTimeout, SerialPortServices.DefaultWriteTimeout))
			{
				SerialPortServices._isAborted = true;
				return;
			}
			if (!SerialPortServices.Login(SerialPortServices.UserName, SerialPortServices.Password))
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			SerialPortServices.SendLine("_lq");
			SerialPortServices.SendLine(SerialPortServices.ShellCommand_ReadLoggerDate);
			if (SerialPortServices._isAbortRequested)
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			string text;
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			SerialPortServices.SendLine("_ll");
			string[] array = text.Split(new char[]
			{
				'\r',
				'\n'
			});
			int i = 0;
			while (i < array.Length)
			{
				string text2 = array[i];
				if (!string.IsNullOrEmpty(text2) && text2.IndexOf(SerialPortServices.ShellCommand_ReadLoggerDate) < 0 && text2.IndexOf(SerialPortServices.PromptLoggedIn) < 0)
				{
					DateTime realtimeClock;
					SerialPortServices._isAborted = !SerialPortServices.ParseDateCommandResult(text2, out realtimeClock);
					if (!SerialPortServices._isAborted)
					{
						SerialPortServices.RealtimeClock = realtimeClock;
						break;
					}
					break;
				}
				else
				{
					i++;
				}
			}
			SerialPortServices.CloseSerialPort();
			SerialPortServices._isFinished = true;
		}

		public static uint BuildCompleteIpAddress(uint val1, uint val2, uint val3, uint val4)
		{
			return (val1 & 255u) << 24 | (val2 & 255u) << 16 | (val3 & 255u) << 8 | (val4 & 255u);
		}

		public static void GetIpAddressBytes(uint completeIpAdr, out uint val1, out uint val2, out uint val3, out uint val4)
		{
			val1 = (completeIpAdr >> 24 & 255u);
			val2 = (completeIpAdr >> 16 & 255u);
			val3 = (completeIpAdr >> 8 & 255u);
			val4 = (completeIpAdr & 255u);
		}

		public static void SetIPAddress()
		{
			SerialPortServices.ResetFlags();
			if (!SerialPortServices.OpenSerialPort(SerialPortServices.PortName, SerialPortServices.DefaultPortBaudRate, SerialPortServices.DefaultParity, SerialPortServices.DefaultDataBits, SerialPortServices.DefaultStopBits, SerialPortServices.DefaultHandshake, SerialPortServices.DefaultReadTimeout, SerialPortServices.DefaultWriteTimeout))
			{
				SerialPortServices._isAborted = true;
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CommProblem;
				return;
			}
			if (!SerialPortServices.Login(SerialPortServices.UserName, SerialPortServices.Password))
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CommProblem;
				SerialPortServices._isAborted = true;
				return;
			}
			SerialPortServices.SendLine("_lq");
			SerialPortServices.SendLine(SerialPortServices.ShellCommand_IPAdrGetAdrEth1Def);
			if (SerialPortServices._isAbortRequested)
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			string text;
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			uint defaultIPAddress = 0u;
			uint defaultSubnetMask = 0u;
			SerialPortServices._isDefaultIPAddress = false;
			SerialPortServices.ParseResponse(text, ref defaultIPAddress, ref defaultSubnetMask);
			SerialPortServices.DefaultIPAddress = defaultIPAddress;
			SerialPortServices.DefaultSubnetMask = defaultSubnetMask;
			if (SerialPortServices.IPAddress == SerialPortServices.DefaultIPAddress)
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isDefaultIPAddress = true;
				SerialPortServices._isFinished = true;
				return;
			}
			SerialPortServices.SendLine(SerialPortServices.ShellCommand_IPAdrChangeDir);
			if (SerialPortServices._isAbortRequested)
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			if (!SerialPortServices.IsEmptyCommandResponse(SerialPortServices.ShellCommand_IPAdrChangeDir, text))
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CommProblem;
				SerialPortServices._isAborted = true;
				return;
			}
			uint num;
			uint num2;
			uint num3;
			uint num4;
			SerialPortServices.GetIpAddressBytes(SerialPortServices.IPAddress, out num, out num2, out num3, out num4);
			string text2 = string.Format(SerialPortServices.ShellCommand_IPAdrWriteAdr, new object[]
			{
				num,
				num2,
				num3,
				num4
			});
			SerialPortServices.SendLine(text2);
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			if (SerialPortServices._isAbortRequested || !SerialPortServices.IsEmptyCommandResponse(text2, text))
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CannotSetIPAddress;
				SerialPortServices._isAborted = true;
				return;
			}
			uint num5;
			uint num6;
			uint num7;
			uint num8;
			SerialPortServices.GetIpAddressBytes(SerialPortServices.SubnetMask, out num5, out num6, out num7, out num8);
			text2 = string.Format(SerialPortServices.ShellCommand_IPAdrWriteSubnetMask, new object[]
			{
				num5,
				num6,
				num7,
				num8
			});
			SerialPortServices.SendLine(text2);
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			if (SerialPortServices._isAbortRequested || !SerialPortServices.IsEmptyCommandResponse(text2, text))
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CannotSetSubnetMask;
				SerialPortServices._isAborted = true;
				return;
			}
			SerialPortServices.SendLine(SerialPortServices.ShellCommand_IPAdrChangeMod);
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			if (SerialPortServices._isAbortRequested || !SerialPortServices.IsEmptyCommandResponse(SerialPortServices.ShellCommand_IPAdrChangeMod, text))
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CommProblem;
				SerialPortServices._isAborted = true;
				return;
			}
			string text3 = string.Format(SerialPortServices.ShellCommand_IPAdrSetAdr, new object[]
			{
				num,
				num2,
				num3,
				num4
			});
			SerialPortServices.SendLine(text3);
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			if (SerialPortServices._isAbortRequested || !SerialPortServices.IsEmptyCommandResponse(text3, text))
			{
				SerialPortServices.SendLine("_ll");
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CannotSetIPAddress;
				SerialPortServices._isAborted = true;
				return;
			}
			string text4 = string.Format(SerialPortServices.ShellCommand_IPAdrSetSubnetMask, new object[]
			{
				num5,
				num6,
				num7,
				num8
			});
			SerialPortServices.SendLine(text4);
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			SerialPortServices.SendLine("_ll");
			Console.WriteLine(text);
			if (SerialPortServices._isAbortRequested || !SerialPortServices.IsEmptyCommandResponse(text4, text))
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CannotSetSubnetMask;
				SerialPortServices._isAborted = true;
				return;
			}
			SerialPortServices.CloseSerialPort();
			SerialPortServices._isFinished = true;
		}

		public static void ReadIPAddress()
		{
			SerialPortServices.ResetFlags();
			if (!SerialPortServices.OpenSerialPort(SerialPortServices.PortName, SerialPortServices.DefaultPortBaudRate, SerialPortServices.DefaultParity, SerialPortServices.DefaultDataBits, SerialPortServices.DefaultStopBits, SerialPortServices.DefaultHandshake, SerialPortServices.DefaultReadTimeout, SerialPortServices.DefaultWriteTimeout))
			{
				SerialPortServices._isAborted = true;
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CommProblem;
				return;
			}
			if (!SerialPortServices.Login(SerialPortServices.UserName, SerialPortServices.Password))
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._lastErrorCode = SerialPortServices.ErrorCode.CommProblem;
				SerialPortServices._isAborted = true;
				return;
			}
			SerialPortServices.SendLine("_lq");
			SerialPortServices.SendLine(SerialPortServices.ShellCommand_IPAdrGetAdrEth1);
			if (SerialPortServices._isAbortRequested)
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			string text;
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			Console.WriteLine(text);
			SerialPortServices.IPAddress = 0u;
			uint num = 0u;
			uint num2 = 0u;
			if (SerialPortServices.ParseResponse(text, ref num, ref num2) && num != 0u)
			{
				SerialPortServices.IPAddress = num;
				SerialPortServices.SubnetMask = num2;
			}
			SerialPortServices.SendLine(SerialPortServices.ShellCommand_IPAdrGetAdrEth1Def);
			if (SerialPortServices._isAbortRequested)
			{
				SerialPortServices.CloseSerialPort();
				SerialPortServices._isAborted = true;
				return;
			}
			lock (SerialPortServices._responseBuffer)
			{
				text = SerialPortServices._responseBuffer.ToString();
			}
			if (SerialPortServices.ParseResponse(text, ref num, ref num2))
			{
				SerialPortServices.DefaultIPAddress = num;
				SerialPortServices.DefaultSubnetMask = num2;
			}
			SerialPortServices.SendLine("_ll");
			SerialPortServices.CloseSerialPort();
			SerialPortServices._isFinished = true;
		}

		private static bool ParseResponse(string response, ref uint addr, ref uint subnetMask)
		{
			string[] array = response.Split(new char[]
			{
				'\r',
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				if (!string.IsNullOrEmpty(text))
				{
					int num = text.IndexOf(SerialPortServices.IPAdrGetAdrResult);
					if (num >= 0 && text.Length > SerialPortServices.IPAdrGetAdrResult.Length + num)
					{
						string commandResult = text.Substring(SerialPortServices.IPAdrGetAdrResult.Length + num);
						SerialPortServices._isAborted = !SerialPortServices.ParseIPAddress(commandResult, out addr);
						bool result;
						if (SerialPortServices._isAborted)
						{
							result = false;
						}
						else
						{
							num = text.IndexOf(SerialPortServices.IPAdrGetSubnetMaskResult);
							if (num < 0 || text.Length <= SerialPortServices.IPAdrGetSubnetMaskResult.Length + num)
							{
								break;
							}
							commandResult = text.Substring(SerialPortServices.IPAdrGetSubnetMaskResult.Length + num);
							SerialPortServices._isAborted = !SerialPortServices.ParseIPAddress(commandResult, out subnetMask);
							if (!SerialPortServices._isAborted)
							{
								break;
							}
							result = false;
						}
						return result;
					}
				}
			}
			return true;
		}

		public static bool IsValidSubnetMask(uint value1, uint value2, uint value3, uint value4)
		{
			return SerialPortServices.IsValidSubnetMaskByte(value1) && (value1 >= 255u || (value2 <= 0u && value3 <= 0u && value4 <= 0u)) && SerialPortServices.IsValidSubnetMaskByte(value2) && (value2 >= 255u || (value3 <= 0u && value4 <= 0u)) && SerialPortServices.IsValidSubnetMaskByte(value3) && (value3 >= 255u || value4 <= 0u) && SerialPortServices.IsValidSubnetMaskByte(value4);
		}

		private static bool IsValidSubnetMaskByte(uint val)
		{
			byte b = (byte)val;
			return b == 255 || b == 254 || b == 252 || b == 248 || b == 240 || b == 224 || b == 192 || b == 128 || b == 0;
		}

		private static bool OpenSerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits, Handshake handshake, int readTimeout, int writeTimeout)
		{
			if (SerialPortServices._serialPort != null)
			{
				return false;
			}
			SerialPortServices._serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
			SerialPortServices._serialPort.Handshake = handshake;
			SerialPortServices._serialPort.ReadTimeout = SerialPortServices.DefaultResponseTimeout;
			SerialPortServices._serialPort.WriteTimeout = SerialPortServices.DefaultResponseTimeout;
			SerialPortServices._serialPort.DtrEnable = true;
			SerialPortServices._serialPort.RtsEnable = true;
			SerialPortServices._serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPortServices._serialPort_ErrorReceived);
			SerialPortServices._serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPortServices._serialPort_DataReceived);
			try
			{
				SerialPortServices._serialPort.Open();
			}
			catch (Exception)
			{
				SerialPortServices._serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(SerialPortServices._serialPort_ErrorReceived);
				SerialPortServices._serialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialPortServices._serialPort_DataReceived);
				SerialPortServices._serialPort = null;
				return false;
			}
			SerialPortServices._responseBuffer = new StringBuilder();
			return true;
		}

		private static void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			string value = SerialPortServices._serialPort.ReadExisting();
			lock (SerialPortServices._responseBuffer)
			{
				SerialPortServices._responseBuffer.Append(value);
				SerialPortServices._responseBuffer.ToString();
			}
		}

		private static void _serialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
		{
			Console.WriteLine(e.ToString());
			SerialPortServices._isAbortRequested = true;
		}

		private static bool CloseSerialPort()
		{
			if (SerialPortServices._serialPort == null)
			{
				return false;
			}
			SerialPortServices._serialPort.Close();
			SerialPortServices._serialPort.ErrorReceived -= new SerialErrorReceivedEventHandler(SerialPortServices._serialPort_ErrorReceived);
			SerialPortServices._serialPort.DataReceived -= new SerialDataReceivedEventHandler(SerialPortServices._serialPort_DataReceived);
			SerialPortServices._serialPort = null;
			return true;
		}

		private static void SendLine(string lineToSend)
		{
			SerialPortServices.SendLine(lineToSend, SerialPortServices.DefaultResponseTimeout);
		}

		private static void SendLine(string lineToSend, int timeout)
		{
			lock (SerialPortServices._responseBuffer)
			{
				SerialPortServices._responseBuffer.Length = 0;
			}
			SerialPortServices._serialPort.WriteLine(lineToSend);
			Console.WriteLine("Send: " + lineToSend);
			SerialPortServices.Wait(timeout);
		}

		private static bool Login(string userName, string password)
		{
			if (SerialPortServices._serialPort == null)
			{
				return false;
			}
			bool flag = false;
			bool flag2 = false;
			SerialPortServices.SendLine("");
			while (!SerialPortServices._isAbortRequested)
			{
				string text;
				lock (SerialPortServices._responseBuffer)
				{
					text = SerialPortServices._responseBuffer.ToString();
				}
				if (!string.IsNullOrEmpty(text))
				{
					if (!flag && text.IndexOf(SerialPortServices.PromptUserName) >= 0)
					{
						SerialPortServices.SendLine(userName);
						flag = true;
					}
					else if (!flag2 && text.IndexOf(SerialPortServices.PromptPassword) >= 0)
					{
						SerialPortServices.SendLine(password);
						flag2 = true;
					}
					else
					{
						if (text.IndexOf(SerialPortServices.PromptLoggedIn) >= 0)
						{
							return true;
						}
						SerialPortServices.SendLine("");
					}
				}
			}
			return false;
		}

		private static bool IsEmptyCommandResponse(string command, string response)
		{
			string[] array = response.Split(new char[]
			{
				'\r',
				'\n'
			});
			List<string> list = new List<string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list.Count >= 2 && list[0].IndexOf(command) >= 0 && list[1].IndexOf(SerialPortServices.PromptLoggedIn) >= 0;
		}

		private static bool ParseDateCommandResult(string commandResult, out DateTime currentDateTime)
		{
			currentDateTime = default(DateTime);
			if (string.IsNullOrEmpty(commandResult))
			{
				return false;
			}
			if (commandResult.Length < SerialPortServices.ReadLoggerDateResultLen)
			{
				return false;
			}
			int day;
			if (!int.TryParse(commandResult.Substring(0, 2), out day))
			{
				return false;
			}
			int month;
			if (!int.TryParse(commandResult.Substring(2, 2), out month))
			{
				return false;
			}
			int year;
			if (!int.TryParse(commandResult.Substring(4, 4), out year))
			{
				return false;
			}
			int hour;
			if (!int.TryParse(commandResult.Substring(8, 2), out hour))
			{
				return false;
			}
			int minute;
			if (!int.TryParse(commandResult.Substring(10, 2), out minute))
			{
				return false;
			}
			int second;
			if (!int.TryParse(commandResult.Substring(12, 2), out second))
			{
				return false;
			}
			currentDateTime = new DateTime(year, month, day, hour, minute, second);
			return true;
		}

		private static bool ParseIPAddress(string commandResult, out uint ipAdr)
		{
			ipAdr = 0u;
			if (string.IsNullOrEmpty(commandResult))
			{
				return false;
			}
			string[] array = commandResult.Split(new char[]
			{
				'.',
				' '
			});
			if (array.Count<string>() < 4)
			{
				return false;
			}
			int num = 0;
			string[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				string s = array2[i];
				bool result;
				uint num2;
				if (num >= 4)
				{
					result = true;
				}
				else if (!uint.TryParse(s, out num2))
				{
					result = false;
				}
				else
				{
					if (num2 <= 255u)
					{
						ipAdr |= num2 << (3 - num) * 8;
						num++;
						i++;
						continue;
					}
					result = false;
				}
				return result;
			}
			return num == 4;
		}

		private static void Wait(int milliseconds)
		{
			Thread.Sleep(milliseconds);
			SerialPortServices._processingDuration += milliseconds;
		}

		private static void ResetFlags()
		{
			SerialPortServices._isAborted = false;
			SerialPortServices._isFinished = false;
			SerialPortServices._isAbortRequested = false;
			SerialPortServices._processingDuration = 0;
		}
	}
}
