using System;
using System.Collections;
using System.IO;

namespace LibMobileDeviceExample
{
	internal class LogString
	{
		public delegate void LogUpdateDelegate();

		private string m_strName;

		private string m_strLog = string.Empty;

		private bool m_bTimestamp = true;

		private bool m_bLineTerminate = true;

		private int m_nMaxChars = 32000;

		private bool m_bReverseOrder = true;

		private static Hashtable m_LogsTable = new Hashtable();

		private string FileName => m_strName + ".log";

		public bool Timestamp
		{
			get
			{
				return m_bTimestamp;
			}
			set
			{
				m_bTimestamp = value;
			}
		}

		public bool LineTerminate
		{
			get
			{
				return m_bLineTerminate;
			}
			set
			{
				m_bLineTerminate = value;
			}
		}

		public bool ReverseOrder
		{
			get
			{
				return m_bReverseOrder;
			}
			set
			{
				m_bReverseOrder = value;
			}
		}

		public int MaxChars
		{
			get
			{
				return m_nMaxChars;
			}
			set
			{
				m_nMaxChars = value;
			}
		}

		public string Log
		{
			get
			{
				string strLog = m_strLog;
				lock (strLog)
				{
					return m_strLog;
				}
			}
		}

		public string Name => m_strName;

		public event LogUpdateDelegate OnLogUpdate;

		private LogString(string name)
		{
			m_strName = name;
			ReadLog();
		}

		private void ReadLog()
		{
			if (!File.Exists(FileName))
			{
				return;
			}
			string strLog = m_strLog;
			lock (strLog)
			{
				using StreamReader streamReader = File.OpenText(FileName);
				m_strLog = streamReader.ReadToEnd();
				streamReader.Close();
			}
		}

		private void WriteLog()
		{
			if (File.Exists(FileName))
			{
				File.Delete(FileName);
			}
			if (m_strLog.Length == 0)
			{
				return;
			}
			string strLog = m_strLog;
			lock (strLog)
			{
				using StreamWriter streamWriter = File.CreateText(FileName);
				streamWriter.Write(m_strLog);
				streamWriter.Close();
			}
		}

		public static LogString GetLogString(string name)
		{
			if (m_LogsTable.ContainsKey(name))
			{
				return (LogString)m_LogsTable[name];
			}
			LogString logString = new LogString(name);
			m_LogsTable.Add(name, logString);
			return logString;
		}

		public static void RemoveLogString(string name)
		{
			if (m_LogsTable.ContainsKey(name))
			{
				LogString logString = (LogString)m_LogsTable[name];
				logString.Clear();
				m_LogsTable.Remove(name);
			}
		}

		public static void PersistAll()
		{
			ICollection values = m_LogsTable.Values;
			foreach (object obj in values)
			{
				LogString logString = (LogString)obj;
				logString.Persist();
			}
		}

		public static void ClearAll()
		{
			ICollection values = m_LogsTable.Values;
			foreach (object obj in values)
			{
				LogString logString = (LogString)obj;
				logString.Clear();
			}
		}

		public void Add(string str)
		{
			string text = "";
			if (m_bTimestamp)
			{
				text = text + DateTime.Now.ToString() + ": ";
			}
			text += str;
			if (m_bLineTerminate)
			{
				text += "\r\n";
			}
			string strLog = m_strLog;
			lock (strLog)
			{
				if (m_bReverseOrder)
				{
					m_strLog = text + m_strLog;
				}
				else
				{
					m_strLog += text;
				}
				if (m_strLog.Length > m_nMaxChars)
				{
					if (m_bReverseOrder)
					{
						m_strLog = m_strLog.Substring(0, m_nMaxChars);
					}
					else
					{
						m_strLog = m_strLog.Substring(m_strLog.Length - m_nMaxChars);
					}
				}
			}
			if (this.OnLogUpdate != null)
			{
				this.OnLogUpdate();
			}
		}

		public void Persist()
		{
			WriteLog();
		}

		public void Clear()
		{
			string strLog = m_strLog;
			lock (strLog)
			{
				m_strLog = string.Empty;
			}
			WriteLog();
			if (this.OnLogUpdate != null)
			{
				this.OnLogUpdate();
			}
		}
	}
}
