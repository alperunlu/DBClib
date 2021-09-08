using System;
using System.Globalization;
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
        public static string SIGorder = "";
        public static string SIGfactor = "";
        public static string SIGoffset = "";
        public static string SIGvalue = "";
        public static string SIGlength = "";
        public static string MSGcycle = "";
        public static string startBit = "";
        public static string endBit = "";
        public static string DbcPeriod = "";
        public static string DbcMessages = "";
        public static string DbcCycles = "";
        public static string DbcSignals = "";
        private string[] DBCmessageList = new string[] { };
        private string[] MSGidList = new string[] { };
        private string[] SIGlist = new string[] { };

        //Signals
        public string[] GetSignalNames(string file, string MSGid)
        {
            ClearLists();
            DBCload(file);
            
            DbcMessages = FindTextBetween(DbcMessages, "BO_ "+message, "|");
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

            ClearLists();
            DBCload(file);


            string[] DBCmessageitems = DbcMessages.Split(new string[] { "BO_ " }, StringSplitOptions.None);

            for (int i = 1; i < DBCmessageitems.Length; i++)
            {
                string DBCMessage = DBCmessageitems[i];
                string MSGname = FindTextBetween(DBCMessage, " ", ":");
                string MSGid = FindTextBetween(DBCMessage, " ", " ");

                Array.Resize(ref DBCmessageList, DBCmessageList.Length + 1);
                DBCmessageList[DBCmessageList.Length - 1] = MSGname;

            }

            return DBCmessageList;
        }

        public string[] GetMessageIDs(string file)
        {

            ClearLists();
            DBCload(file);
            GetMessageNames(file);

            string[] DBCmessageitems = DbcMessages.Split(new string[] { "BO_" }, StringSplitOptions.None);

            for (int i = 1; i < DBCmessageitems.Length; i++)
            {
                string DBCMessage = DBCmessageitems[i];
                string MSGname = FindTextBetween(DBCMessage, " ", ":");
                string MSGid = FindTextBetween(DBCMessage, " ", " ");

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
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
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
            string SIGunit = FindTextBetween(DBCMessage, @"] """, @"""");
            return SIGunit;
        }

        public string GetFactor(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string SIGfactor = FindTextBetween(DBCMessage, "(", ",");
            return SIGfactor;
        }

        public string GetOffset(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string SIGoffset = FindTextBetween(DBCMessage, ",", ")");
            return SIGoffset;
        }

        public string GetStartBit(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string startBit = FindTextBetween(DBCMessage, ": ", "|");
            return startBit;
        }

        public string GetEndBit(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            startBit = GetStartBit(file, signal);
            SIGorder = GetBitOrder(file, signal);
            string endBit = FindTextBetween(DBCMessage, "|", "@");

            if (SIGorder == "Intel")
            {
                endBit = (Int32.Parse(startBit) + Int32.Parse(endBit) - 1).ToString();
            }

            if (SIGorder == "Motorola")
            {
                endBit = (Int32.Parse(startBit) - Int32.Parse(endBit) + 1).ToString();
            }

            return endBit;
        }

        public string GetLength(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            startBit = GetStartBit(file, signal);
            SIGorder = GetBitOrder(file, signal);
            string SIGlength = FindTextBetween(DBCMessage, "|", "@");

            return SIGlength;
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

        public string GetBitOrder(string file, string signal)
        {

            ClearLists();
            DBCload(file);
            string[] DBCmessageitems = DbcMessages.Split(new string[] { "SG_ " + signal }, StringSplitOptions.None);
            string DBCMessage = DBCmessageitems[DBCmessageitems.Length - 1];
            string SIGorder = FindTextBetween(DBCMessage, "@", "-");
            if (SIGorder == "1") SIGorder = "Intel";
            if (SIGorder == "0") SIGorder = "Motorola";
            return SIGorder;
        }

        //Value

        public string GetValue(string payload, string file, string signal)
        {
            string factor = GetFactor(file, signal);
            string offset = GetOffset(file, signal);
            string order = GetBitOrder(file, signal);
            string start = GetStartBit(file, signal);
            string end = GetEndBit(file, signal);
            string length = GetLength(file, signal);
            payload = Reverse(payload);

            if (order == "Motorola")
            {
                string temp = start;
                start = end;
                end = temp;
            }

            string valueRawSTR = payload.Substring(Int32.Parse(start), Int32.Parse(length));

            if (order == "Motorola") valueRawSTR = Reverse(valueRawSTR);

            double valueRawINT = Convert.ToDouble(Convert.ToInt32(valueRawSTR, 2));
            double value = (valueRawINT) * double.Parse(factor, CultureInfo.InvariantCulture.NumberFormat);
            value = value + Int32.Parse(offset);

            return value.ToString();
        }


        public void ClearLists()
        {
            DBCmessageList = new string[] { };
            MSGidList = new string[] { };
            SIGlist = new string[] { };
        }

        public static string Reverse(string s) //Theodor Zoulias
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }


        public string FindTextBetween(string text, string left, string right) //Yeldar Kurmangaliyev
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
