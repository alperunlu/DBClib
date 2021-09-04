using System;
using System.IO;
using System.Text;

namespace DBCapi
{
    public class DBCclass
    {
        public String line;
        public static string minVal = "";
        public static string maxVal = "";
        public static string SIGunit = "";
        public static string MSGcycle = "";
        public static string DbcPeriod = "";
        public static string DbcMessages = "";
        public static string DbcCycles = "";
        public static string DbcSignals = "";
        private string[] DBCmessageList = new string[] { };
        private string[] MSGidList = new string[] { };
        private string[] SIGlist = new string[] { };

        //Signals
        public string[] GetSignalNames(string file, string message)
        {
            DBCmessageList = new string[] { };
            MSGidList = new string[] { };
            SIGlist = new string[] { };

            DBCload(file);

            string[] MSGsignalitems = DbcMessages.Split(new string[] { "SG_ " }, StringSplitOptions.None);

            for (int i = 1; i < MSGsignalitems.Length; i++)
            {
                string MSGsignal = MSGsignalitems[i];
                string SIGname = FindTextBetween(MSGsignal, "", ":");

                Array.Resize(ref SIGlist, SIGlist.Length + 1);
                SIGlist[SIGlist.Length - 1] = SIGname;
                
            }

            return SIGlist;
        }

        //Messages
        public string[] GetMessageNames(string file)
        {

            DBCmessageList = new string[] { };
            MSGidList = new string[] { };
            SIGlist = new string[] { };

            DBCload(file);


            string[] DBCmessageitems = DbcMessages.Split(new string[] { "BO_ " }, StringSplitOptions.None);

            for (int i = 1; i < DBCmessageitems.Length; i++)
            {
                string DBCMessage = DBCmessageitems[i];
                string MSGname = FindTextBetween(DBCMessage, " ", ":");
                string MSGid = DBCMessage.Substring(0, 10);

                Array.Resize(ref DBCmessageList, DBCmessageList.Length + 1);
                DBCmessageList[DBCmessageList.Length - 1] = MSGname;

            }

            return DBCmessageList;
        }

        public string[] GetMessageIDs(string file)
        {

            DBCmessageList = new string[] { };
            MSGidList = new string[] { };
            SIGlist = new string[] { };

            DBCload(file);
            GetMessageNames(file);

            string[] DBCmessageitems = DbcMessages.Split(new string[] { "BO_" }, StringSplitOptions.None);

            for (int i = 1; i < DBCmessageitems.Length; i++)
            {
                string DBCMessage = DBCmessageitems[i];
                string MSGname = FindTextBetween(DBCMessage, " ", ":");
                string MSGid = FindTextBetween(DBCMessage, " ", " "); ;

                Array.Resize(ref MSGidList, MSGidList.Length + 1);
                MSGidList[MSGidList.Length - 1] = MSGid;
            }

           return MSGidList;
        }

        public string GetMin(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length-1];    
            string minVal = FindTextBetween(DBCMessage, "[", "|");
            return minVal;
        }


        public string GetMax(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string maxVal = FindTextBetween(DBCMessage, ") [", @"""");
            maxVal = FindTextBetween(maxVal, "|", "]");
            return maxVal;
        }

        public string GetUnit(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string sigUnit = FindTextBetween(DBCMessage, @"] """, @"""");
            return sigUnit;
        }

        public string GetCycleTime(string file, string MSGid)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcCycles.Split(new string[] { "BA_ \"GenMsgCycleTime\" BO_ " + MSGid }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string MSGcycle = FindTextBetween(DBCMessage, " ", ";");
            return MSGcycle;
        }

        public void ClearLists()
        {
            DBCmessageList = new string[] { };
            MSGidList = new string[] { };
            SIGlist = new string[] { };
        }

        public string FindTextBetween(string text, string left, string right)
        {

            int beginIndex = text.IndexOf(left); 
            if (beginIndex == -1)
                return string.Empty; 

            beginIndex += left.Length;

            int endIndex = text.IndexOf(right, beginIndex); 
            if (endIndex == -1)
                return string.Empty; 

            return text.Substring(beginIndex, endIndex - beginIndex).Trim();
        }

        public void DBCload(string file)
        {
            if (file != null && file != "")
            {                
                using (StreamReader sr = new StreamReader(file))
                {
                    StringBuilder messages = new StringBuilder();
                    StringBuilder cycles = new StringBuilder();
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.StartsWith("BO_ "))
                        {
                            messages.Append(line);
                        }
                        else if (line.StartsWith(" SG_ "))
                        {
                            messages.Append(line);
                        }                        
                        else if (line.StartsWith("BA_ \"GenMsgCycleTime"))
                        {
                        cycles.Append(line);
                        }
                    }
                    DbcMessages = messages.ToString();
                    DbcCycles = cycles.ToString();
                }
            }
        }

    }
}
