using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace XcpDll
{
    public class Class1
    {
        #region 变量
        FileStream A2lFile;
        StreamReader A2lReader;
        private string CommandLine;
        string tempStr;
        bool isVaild = false;
        int index = 0;
        int dimensionIndex = 0;
        int XY_index = 0;
        ReadClass ReadInformation;
        WriteClass WriteInformation;
        MetholdClass MetholdInformation;
        TableClass TableInformation;
        RecordLayoutClass RecordInformation;
        MemorySegmentClass MemoryInformation;
        PropertyClass PropertyInformation = new PropertyClass();
        List<ReadClass> ReadList = new List<ReadClass>();
        List<WriteClass> WriteList = new List<WriteClass>();
        List<MetholdClass> MetholdList = new List<MetholdClass>();
        List<TableClass> TableList = new List<TableClass>();
        List<RecordLayoutClass> RecordList = new List<RecordLayoutClass>();
        List<MemorySegmentClass> MemoryList = new List<MemorySegmentClass>();
        #endregion

        #region 读取文件，解析a2l文件
        /// <summary>
        /// 解析a2l文件，解析成功返回值1
        /// </summary>
        /// <param name="A2lPath">a2l绝对路径</param>
        /// <param name="Protocol">协议类型：1/2=calibrationcan；3/4=vehiclecan</param>
        /// <returns></returns>
        public int AnalyzeXcpFile(string A2lPath, int Protocol)
        {
            Init();
            if (!File.Exists(A2lPath))
            {
                return 0;
            }
            A2lFile = new FileStream(A2lPath, FileMode.Open);
            A2lReader = new StreamReader(A2lFile);

            #region read file
            while (!A2lReader.EndOfStream)
            {
                CommandLine = A2lReader.ReadLine();
                tempStr = CommandLine.ToLower().Replace(" ", "");
                switch (tempStr)
                {
                    case "/beginmod_common\"\"":
                        CommandLine = A2lReader.ReadLine();
                        tempStr = CommandLine.ToLower().Replace(" ", "");
                        if (tempStr.Contains("byte_order"))
                        {
                            if (tempStr.Contains("msb_last"))
                            {
                                //PropertyFileWriter.WriteLine("BYTE_ORDER:MSB_LAST");
                                PropertyInformation.byteOrder = "msb_last";
                            }
                            else if (tempStr.Contains("msb_first"))
                            {
                                //PropertyFileWriter.WriteLine("BYTE_ORDER:MSB_FIRST");
                                PropertyInformation.byteOrder = "msb_first";

                            }
                            else
                            {
                                FileClose();
                                return 1;
                            }
                        }
                        break;
                    case "/begin xcp_on_can":
                        string SendID = "";
                        string ReceiveID = "";
                        string Baudrate = "";
                        string EventChannel_sync = "";
                        string EventChannel_10 = "";
                        string EventChannel_100 = "";
                        if (isVaild == false)
                        {
                            while (tempStr != "/endxcp_on_can")
                            {
                                CommandLine = A2lReader.ReadLine();
                                tempStr = CommandLine.ToLower().Replace(" ", "");
                                if (tempStr.Contains("can_id_master0x"))
                                {
                                    tempStr = tempStr.Replace("can_id_master0x", "");
                                    int position = tempStr.IndexOf("/");
                                    SendID = tempStr.Substring(0, position);
                                }
                                else if (tempStr.Contains("can_id_slave0x"))
                                {
                                    tempStr = tempStr.Replace("can_id_slave0x", "");
                                    int position = tempStr.IndexOf("/");
                                    ReceiveID = tempStr.Substring(0, position);
                                }
                                else if (tempStr.Contains("baudrate") && (tempStr.Contains("/*baudrate[hz]*/")) && (!tempStr.Contains("_")))
                                {
                                    tempStr = tempStr.Replace("baudrate", "");
                                    int position = tempStr.IndexOf("/");
                                    Baudrate = tempStr.Substring(0, position);
                                }
                                else if (tempStr.Contains("seg_sync") && tempStr.Contains("event_channel_short_name"))
                                {
                                    CommandLine = A2lReader.ReadLine();
                                    tempStr = CommandLine.ToLower().Replace(" ", "");
                                    if (tempStr.Contains("/*event_channel_number*/"))
                                    {
                                        EventChannel_sync = tempStr.Substring(0, 1);
                                    }

                                }
                                else if (tempStr.Contains("10_ms") && tempStr.Contains("event_channel_short_name"))
                                {
                                    CommandLine = A2lReader.ReadLine();
                                    tempStr = CommandLine.ToLower().Replace(" ", "");
                                    if (tempStr.Contains("/*event_channel_number*/"))
                                    {
                                        EventChannel_10 = tempStr.Substring(0, 1);
                                    }

                                }
                                else if (tempStr.Contains("100_ms") && tempStr.Contains("event_channel_short_name"))
                                {
                                    CommandLine = A2lReader.ReadLine();
                                    tempStr = CommandLine.ToLower().Replace(" ", "");
                                    if (tempStr.Contains("/*event_channel_number*/"))
                                    {
                                        EventChannel_100 = tempStr.Substring(0, 1);
                                    }

                                }
                                else if (tempStr.Contains("transport_layer_instance"))
                                {
                                    if (Protocol == 1)
                                    {
                                        if ((tempStr.Contains("calibrationcan(appl)")) || (tempStr.Contains("calibrationcan(le)")))
                                        {
                                            isVaild = true;
                                        }
                                    }
                                    else if (Protocol == 2)
                                    {
                                        if ((tempStr.Contains("calibrationcan(appl)(ed-ram)")) || (tempStr.Contains("calibrationcan(appl)(ed-ram)")))
                                        {
                                            isVaild = true;
                                        }
                                    }
                                    else if (Protocol == 3)
                                    {
                                        if ((tempStr.Contains("vehiclecan(pt)")) || (tempStr.Contains("vehiclecan(appl)")))
                                        {
                                            isVaild = true;
                                        }
                                    }
                                    else if (Protocol == 4)
                                    {
                                        if ((tempStr.Contains("vehiclecan(pt)(ed-ram)")) || (tempStr.Contains("vehiclecan(pt)(ed-ram)")))
                                        {
                                            isVaild = true;
                                        }
                                    }
                                    else
                                    {
                                        FileClose();
                                        return 2;
                                    }
                                }

                            }
                            if ((isVaild == true) && (SendID != "") && (ReceiveID != "") && (Baudrate != "") && (EventChannel_sync != "") && (EventChannel_10 != "") && (EventChannel_100 != ""))
                            {
                                if (!UInt32.TryParse(Baudrate, out PropertyInformation.baudrate))
                                {
                                    FileClose();
                                    return 3;
                                }
                                if (!UInt16.TryParse(EventChannel_sync, out PropertyInformation.eventChannel_sync))
                                {
                                    FileClose();
                                    return 4;
                                }
                                if (!UInt16.TryParse(EventChannel_10, out PropertyInformation.eventChannel_10))
                                {
                                    FileClose();
                                    return 5;
                                }
                                if (!UInt16.TryParse(EventChannel_100, out PropertyInformation.eventChannel_100))
                                {
                                    FileClose();
                                    return 6;
                                }
                                try
                                {
                                    PropertyInformation.MasterCANID = Convert.ToUInt32(SendID, 16);
                                    PropertyInformation.SlaverCANID = Convert.ToUInt32(ReceiveID, 16);
                                }
                                catch
                                {
                                    FileClose();
                                    return 7;
                                }
                            }
                        }
                        break;
                    case "/beginmeasurement":
                        index = 0;
                        ReadInformation = new ReadClass();
                        while (tempStr.ToLower() != "/endmeasurement")
                        {
                            CommandLine = A2lReader.ReadLine();
                            tempStr = CommandLine.Replace(" ", "");
                            if ((tempStr != "") && (index == 0))
                            {
                                ReadInformation.name = tempStr;
                                index++;
                            }
                            else if ((tempStr.ToLower() == "ubyte") || (tempStr.ToLower() == "sbyte"))
                            {
                                ReadInformation.length = 1;
                                ReadInformation.type = tempStr.ToLower();
                                index++;
                            }
                            else if ((tempStr.ToLower() == "uword") || (tempStr.ToLower() == "sword"))
                            {
                                ReadInformation.length = 2;
                                ReadInformation.type = tempStr.ToLower();
                                index++;
                            }
                            else if ((tempStr.ToLower() == "ulong") || (tempStr.ToLower() == "slong"))
                            {
                                ReadInformation.length = 4;
                                ReadInformation.type = tempStr.ToLower();
                                index++;
                            }
                            else if (tempStr.ToLower() == "float32_ieee")
                            {
                                ReadInformation.length = 4;
                                ReadInformation.type = tempStr.ToLower();
                                index++;
                            }

                            else if ((tempStr != "") && (index == 2))
                            {
                                ReadInformation.expression = tempStr.ToLower();
                                index++;
                            }
                            else if (tempStr.ToLower().Contains("bit_mask0x"))
                            {
                                tempStr = tempStr.ToLower().Replace("bit_mask0x", "");
                                try
                                {
                                    ReadInformation.mask = Convert.ToUInt32(tempStr, 16);
                                }
                                catch
                                {
                                    FileClose();
                                    return 8;
                                }
                            }
                            else if (tempStr.ToLower().Contains("ecu_address0x"))
                            {
                                tempStr = tempStr.ToLower().Replace("ecu_address0x", "");
                                try
                                {
                                    ReadInformation.address = Convert.ToUInt32(tempStr, 16);
                                }
                                catch
                                {
                                    FileClose();
                                    return 9;
                                }
                            }
                        }
                        ReadList.Add(ReadInformation);
                        break;

                    case "/begincharacteristic":
                        index = 0;
                        dimensionIndex = 0;
                        XY_index = 0;
                        WriteInformation = new WriteClass();
                        while (tempStr.ToLower() != "/endcharacteristic")
                        {
                            CommandLine = A2lReader.ReadLine();
                            tempStr = CommandLine.Replace(" ", "");
                            if ((tempStr != "") && (index == 0))
                            {
                                WriteInformation.name = tempStr;
                                index++;
                            }
                            else if ((tempStr.ToLower() == "value"))
                            {
                                WriteInformation.dimension = 1;
                                WriteInformation.dimensiontype = "value";
                                index++;
                            }
                            else if ((tempStr.ToLower() == "val_blk"))
                            {
                                WriteInformation.dimension = 1;
                                WriteInformation.dimensiontype = "val_blk";
                                index++;
                            }
                            else if ((tempStr.ToLower() == "ascii"))
                            {
                                WriteInformation.dimension = 1;
                                WriteInformation.dimensiontype = "asii";
                                index++;
                            }
                            else if (tempStr.ToLower() == "curve")
                            {
                                WriteInformation.dimension = 2;
                                WriteInformation.dimensiontype = "curve";
                                index++;
                            }
                            else if (tempStr.ToLower() == "map")
                            {
                                WriteInformation.dimension = 3;
                                WriteInformation.dimensiontype = "map";
                                index++;
                            }
                            else if ((tempStr != "") && (index == 2))
                            {
                                if (tempStr.ToLower().Contains("0x"))
                                {
                                    tempStr = tempStr.ToLower().Replace("0x", "");
                                    try
                                    {
                                        WriteInformation.address = Convert.ToUInt32(tempStr, 16);
                                        index++;
                                    }
                                    catch
                                    {
                                        FileClose();
                                        return 10;
                                    }
                                }
                                else
                                {
                                    FileClose();
                                    return 11;
                                }
                            }

                            else if ((tempStr != "") && (index == 3))
                            {
                                WriteInformation.recordLayout = tempStr.ToLower();
                                index++;
                            }

                            else if ((tempStr != "") && (index == 4))
                            {
                                if (!float.TryParse(tempStr, out WriteInformation.V_range))
                                {
                                    FileClose();
                                    return 12;
                                }
                                index++;
                            }

                            else if ((tempStr != "") && (index == 5))
                            {
                                WriteInformation.V_Expression = tempStr.ToLower();
                                index++;
                            }

                            else if ((tempStr != "") && (index == 6))
                            {
                                if (!float.TryParse(tempStr, out WriteInformation.V_minValue))
                                {
                                    FileClose();
                                    return 13;
                                }
                                index++;
                            }

                            else if ((tempStr != "") && (index == 7))
                            {
                                if (!float.TryParse(tempStr, out WriteInformation.V_maxValue))
                                {
                                    FileClose();
                                    return 14;
                                }
                                index++;
                            }

                            else if (tempStr.ToLower().Contains("number") && (index == 8))
                            {
                                tempStr = tempStr.ToLower().Replace("number", "");
                                if (!UInt32.TryParse(tempStr, out WriteInformation.V_count))
                                {
                                    FileClose();
                                    return 15;
                                }
                                index++;
                            }

                            else if (tempStr.ToLower() == "/beginaxis_descr")
                            {
                                XY_index = 0;
                                while (tempStr.ToLower() != "/endaxis_descr")
                                {
                                    CommandLine = A2lReader.ReadLine();
                                    tempStr = CommandLine.Replace(" ", "");
                                    if (dimensionIndex == 0)
                                    {
                                        if ((tempStr != "") && (!tempStr.ToLower().Contains("_axis")) && (XY_index == 0))
                                        {
                                            WriteInformation.X_name = tempStr.ToLower();
                                            XY_index++;
                                        }
                                        else if ((tempStr != "") && (XY_index == 1))
                                        {
                                            WriteInformation.X_Expression = tempStr.ToLower();
                                            XY_index++;
                                        }
                                        else if ((tempStr != "") && (XY_index == 2))
                                        {
                                            if (!UInt32.TryParse(tempStr, out WriteInformation.X_count))
                                            {
                                                FileClose();
                                                return 16;
                                            }
                                            XY_index++;
                                        }
                                        else if ((tempStr != "") && (XY_index == 3))
                                        {
                                            if (!float.TryParse(tempStr, out WriteInformation.X_minValue))
                                            {
                                                FileClose();
                                                return 17;
                                            }
                                            XY_index++;
                                        }

                                        else if ((tempStr != "") && (XY_index == 4))
                                        {
                                            if (!float.TryParse(tempStr, out WriteInformation.X_maxValue))
                                            {
                                                FileClose();
                                                return 18;
                                            }
                                            XY_index++;
                                        }
                                    }
                                    else if (dimensionIndex == 1)
                                    {
                                        if ((tempStr != "") && (!tempStr.ToLower().Contains("_axis")) && (XY_index == 0))
                                        {
                                            WriteInformation.Y_name = tempStr.ToLower();
                                            XY_index++;
                                        }
                                        else if ((tempStr != "") && (XY_index == 1))
                                        {
                                            WriteInformation.Y_Expression = tempStr.ToLower();
                                            XY_index++;
                                        }
                                        else if ((tempStr != "") && (XY_index == 2))
                                        {
                                            if (!UInt32.TryParse(tempStr, out WriteInformation.Y_count))
                                            {
                                                FileClose();
                                                return 19;
                                            }
                                            XY_index++;
                                        }
                                        else if ((tempStr != "") && (XY_index == 3))
                                        {
                                            if (!float.TryParse(tempStr, out WriteInformation.Y_minValue))
                                            {
                                                FileClose();
                                                return 20;
                                            }
                                            XY_index++;
                                        }

                                        else if ((tempStr != "") && (XY_index == 4))
                                        {
                                            if (!float.TryParse(tempStr, out WriteInformation.Y_maxValue))
                                            {
                                                FileClose();
                                                return 21;
                                            }
                                            XY_index++;
                                        }
                                    }
                                }
                                dimensionIndex++;
                            }
                        }
                        WriteList.Add(WriteInformation);
                        break;

                    case "/begincompu_method":
                        index = 0;
                        MetholdInformation = new MetholdClass();
                        while (tempStr != "/endcompu_method")
                        {
                            CommandLine = A2lReader.ReadLine();
                            tempStr = CommandLine.ToLower().Replace(" ", "");
                            if ((tempStr == "") && (index == 0))
                            {
                                CommandLine = A2lReader.ReadLine();
                                tempStr = CommandLine.ToLower().Replace(" ", "");
                                MetholdInformation.name = tempStr;
                                index++;
                            }
                            else if ((tempStr != "") && (index == 0))
                            {
                                MetholdInformation.name = tempStr;
                                index++;
                            }
                            else if (tempStr == "tab_verb")
                            {
                                MetholdInformation.type = "tab_verb";
                            }
                            else if (tempStr == "rat_func")
                            {
                                MetholdInformation.type = "rat_func";
                            }
                            if ((MetholdInformation.type == "rat_func") && (index == 1))
                            {
                                while (!tempStr.Contains("coeffs"))
                                {
                                    CommandLine = A2lReader.ReadLine();
                                    tempStr = CommandLine.ToLower();
                                }
                                if (tempStr.Contains("coeffs"))
                                {
                                    MetholdInformation.express = tempStr.Replace("coeffs", "");
                                    index++;
                                }
                            }
                        }
                        MetholdList.Add(MetholdInformation);
                        break;
                    case "/begincompu_vtab":
                        index = 0;
                        TableInformation = new TableClass();
                        while (tempStr != "/endcompu_vtab")
                        {
                            CommandLine = A2lReader.ReadLine();
                            tempStr = CommandLine.ToLower().Replace(" ", "");
                            if ((tempStr != "") && (index == 0))
                            {
                                TableInformation.name = tempStr;
                                index++;
                            }
                            else if (tempStr == "tab_verb")
                            {
                                CommandLine = A2lReader.ReadLine();
                                tempStr = CommandLine.ToLower().Replace(" ", "");
                                int count = Convert.ToInt32(tempStr, 10);
                                for (int index = 0; index < count; index++)
                                {
                                    CommandLine = A2lReader.ReadLine();
                                    tempStr = CommandLine; //获取列表不能删除空格
                                    TableInformation.table = string.Concat(TableInformation.table, tempStr, "^&*");
                                }
                            }
                        }
                        TableList.Add(TableInformation);
                        break;

                    default:
                        if (tempStr.Contains("/beginrecord_layout"))
                        {
                            index = 0;
                            RecordInformation = new RecordLayoutClass();
                            RecordInformation.name = tempStr.Replace("/beginrecord_layout", "");
                            while (tempStr != "/endrecord_layout")
                            {
                                CommandLine = A2lReader.ReadLine();
                                tempStr = CommandLine.ToLower().Replace(" ", "");
                                if (tempStr.Contains("no_axis_pts_x"))
                                {
                                    if (tempStr.Contains("sbyte") || (tempStr.Contains("ubyte")))
                                    {
                                        RecordInformation.X_countLength = 1;
                                    }
                                    else if (tempStr.Contains("sword") || (tempStr.Contains("uword")))
                                    {
                                        RecordInformation.X_countLength = 2;
                                    }
                                    else if (tempStr.Contains("sword") || (tempStr.Contains("uword")))
                                    {
                                        RecordInformation.X_countLength = 4;
                                    }
                                }
                                else if (tempStr.Contains("no_axis_pts_y"))
                                {
                                    if (tempStr.Contains("sbyte") || (tempStr.Contains("ubyte")))
                                    {
                                        RecordInformation.Y_countLength = 1;
                                    }
                                    else if (tempStr.Contains("sword") || (tempStr.Contains("uword")))
                                    {
                                        RecordInformation.Y_countLength = 2;
                                    }
                                    else if (tempStr.Contains("sword") || (tempStr.Contains("uword")))
                                    {
                                        RecordInformation.Y_countLength = 4;
                                    }
                                }
                                else if (tempStr.Contains("axis_pts_x"))
                                {
                                    if (tempStr.Contains("sbyte"))
                                    {
                                        RecordInformation.X_elementLength = 1;
                                        RecordInformation.X_elementType = "sbyte";
                                    }
                                    else if (tempStr.Contains("ubyte"))
                                    {
                                        RecordInformation.X_elementLength = 1;
                                        RecordInformation.X_elementType = "ubyte";
                                    }
                                    else if (tempStr.Contains("sword"))
                                    {
                                        RecordInformation.X_elementLength = 2;
                                        RecordInformation.X_elementType = "sword";
                                    }

                                    else if (tempStr.Contains("uword"))
                                    {
                                        RecordInformation.X_elementLength = 2;
                                        RecordInformation.X_elementType = "uword";
                                    }

                                    else if (tempStr.Contains("slong"))
                                    {
                                        RecordInformation.X_elementLength = 4;
                                        RecordInformation.X_elementType = "slong";
                                    }

                                    else if (tempStr.Contains("ulong"))
                                    {
                                        RecordInformation.X_elementLength = 4;
                                        RecordInformation.X_elementType = "ulong";
                                    }
                                }

                                else if (tempStr.Contains("axis_pts_y"))
                                {
                                    if (tempStr.Contains("sbyte"))
                                    {
                                        RecordInformation.Y_elementLength = 1;
                                        RecordInformation.Y_elementType = "sbyte";
                                    }
                                    else if (tempStr.Contains("ubyte"))
                                    {
                                        RecordInformation.Y_elementLength = 1;
                                        RecordInformation.Y_elementType = "ubyte";
                                    }
                                    else if (tempStr.Contains("sword"))
                                    {
                                        RecordInformation.Y_elementLength = 2;
                                        RecordInformation.Y_elementType = "sword";
                                    }

                                    else if (tempStr.Contains("uword"))
                                    {
                                        RecordInformation.Y_elementLength = 2;
                                        RecordInformation.Y_elementType = "uword";
                                    }

                                    else if (tempStr.Contains("slong"))
                                    {
                                        RecordInformation.Y_elementLength = 4;
                                        RecordInformation.Y_elementType = "slong";
                                    }

                                    else if (tempStr.Contains("ulong"))
                                    {
                                        RecordInformation.Y_elementLength = 4;
                                        RecordInformation.Y_elementType = "ulong";
                                    }
                                }


                                else if (tempStr.Contains("fnc_values"))
                                {
                                    if (tempStr.Contains("sbyte"))
                                    {
                                        RecordInformation.V_elementLength = 1;
                                        RecordInformation.V_elementType = "sbyte";
                                    }
                                    else if (tempStr.Contains("ubyte"))
                                    {
                                        RecordInformation.V_elementLength = 1;
                                        RecordInformation.V_elementType = "ubyte";
                                    }
                                    else if (tempStr.Contains("sword"))
                                    {
                                        RecordInformation.V_elementLength = 2;
                                        RecordInformation.V_elementType = "sword";
                                    }

                                    else if (tempStr.Contains("uword"))
                                    {
                                        RecordInformation.V_elementLength = 2;
                                        RecordInformation.V_elementType = "uword";
                                    }

                                    else if (tempStr.Contains("slong"))
                                    {
                                        RecordInformation.V_elementLength = 4;
                                        RecordInformation.V_elementType = "slong";
                                    }

                                    else if (tempStr.Contains("ulong"))
                                    {
                                        RecordInformation.V_elementLength = 4;
                                        RecordInformation.V_elementType = "ulong";
                                    }
                                }
                            }
                            RecordList.Add(RecordInformation);
                        }
                        else if (tempStr.Contains("/beginmemory_segmentpst") || tempStr.Contains("/beginmemory_segmentdst") || tempStr.Contains("/beginmemory_segmentram"))
                        {
                            index = 0;
                            //XY_index = 0;
                            MemoryInformation = new MemorySegmentClass();
                            string[] strList = CommandLine.ToLower().Split(new char[1] { ' ' });
                            foreach (string str in strList)
                            {
                                if (str.Contains("0x") && (index == 0))
                                {
                                    index++;
                                    try
                                    {
                                        MemoryInformation.startAddress = Convert.ToUInt32(str.Replace("0x", ""), 16);
                                    }
                                    catch
                                    {
                                        FileClose();
                                        return 22;
                                    }
                                }
                                else if (str.Contains("0x") && (index == 1))
                                {
                                    index++;
                                    try
                                    {
                                        MemoryInformation.offset = Convert.ToUInt32(str.Replace("0x", ""), 16);
                                    }
                                    catch
                                    {
                                        FileClose();
                                        return 23;
                                    }
                                    break;
                                }
                            }
                            while (tempStr != "/endmemory_segment")
                            {
                                CommandLine = A2lReader.ReadLine();
                                tempStr = CommandLine.ToLower().Replace(" ", "");
                                if (tempStr.Contains("/*segmentlogicalnumber*/"))
                                {
                                    if (!byte.TryParse(tempStr.Replace("/*segmentlogicalnumber*/", ""), out MemoryInformation.segmentNumber))
                                    {
                                        FileClose();
                                        return 24;
                                    }
                                }
                                else if (tempStr.Contains("/*numberofpages*/"))
                                {
                                    tempStr = tempStr.Replace("/*numberofpages*/", "").Replace("0x", "");
                                    try
                                    {
                                        MemoryInformation.pageCount = Convert.ToByte(tempStr, 16);
                                    }
                                    catch
                                    {
                                        FileClose();
                                        return 25;
                                    }
                                }
                                else if (tempStr.Contains("/beginpage"))
                                {
                                    byte tempPageNumber = new byte();
                                    while (tempStr != "/endpage")
                                    {
                                        CommandLine = A2lReader.ReadLine();
                                        tempStr = CommandLine.ToLower().Replace(" ", "");
                                        if (tempStr.Contains("/*pagenumber*/"))
                                        {
                                            tempStr = tempStr.Replace("/*pagenumber*/", "").Replace("0x", "");
                                            try
                                            {
                                                tempPageNumber = Convert.ToByte(tempStr, 16);
                                            }
                                            catch
                                            {
                                                FileClose();
                                                return 26;
                                            }
                                        }
                                        else if (tempStr.Contains("xcp_write_access_not_allowed"))
                                        {
                                            MemoryInformation.readPageNumber = tempPageNumber;
                                        }
                                        else if (tempStr.Contains("xcp_write_access_with_ecu_only"))
                                        {
                                            MemoryInformation.writePageNumber = tempPageNumber;
                                        }
                                    }
                                }

                            }
                            MemoryList.Add(MemoryInformation);
                        }
                        break;
                }
            }
            #endregion

            #region 检查A2l是否是XCP类型，如果不是，则返回
            if (isVaild == false)
            {
                FileClose();
                return 27;
            }
            #endregion

            #region ReadList
            for (int index = 0; index < ReadList.Count(); index++)
            {
                MetholdClass tempMethold = MetholdList.Find(temp => temp.name == ReadList[index].expression);
                if (tempMethold == null)
                {
                    FileClose();
                    return 28;
                }
                if (tempMethold.type == "rat_func")
                {
                    if (tempMethold.express != null)
                    {
                        //ReadList[index].expression = tempMethold.express;
                        ReadList[index].metholdType = "rat_func";
                        if (!caculateReadParameter(tempMethold.express, index))
                        {
                            FileClose();
                            return 29;
                        }
                    }
                    else
                    {
                        FileClose();
                        return 30;
                    }
                }
                else if (tempMethold.type == "tab_verb")
                {
                    TableClass tempTable = TableList.Find(table => table.name == ReadList[index].expression);
                    if (tempTable == null)
                    {
                        FileClose();
                        return 31;
                    }
                    if (tempTable.table != null)
                    {
                        //ReadList[index].expression = tempTable.table;
                        ReadList[index].metholdType = "tab_verb";
                        if (!caculateReadTableDictionary(tempTable.table, index))
                        {
                            FileClose();
                            return 32;
                        }

                    }
                    else
                    {
                        FileClose();
                        return 33;
                    }
                }
                else
                {
                    FileClose();
                    return 34;
                }
            }
            #endregion

            #region WriteList
            for (int index = 0; index < WriteList.Count(); index++)
            {
                MetholdClass tempVMethold = MetholdList.Find(temp => temp.name == WriteList[index].V_Expression);
                if (tempVMethold == null)
                {
                    FileClose();
                    return 35;
                }
                if (tempVMethold.type == "rat_func")
                {
                    if (tempVMethold.express != null)
                    {
                        //WriteList[index].V_Expression  = tempVMethold.express;
                        WriteList[index].V_ExpressType = "rat_func";
                        if (!caculateWriteVParameter(tempVMethold.express, index))
                        {
                            FileClose();
                            return 36;
                        }
                    }
                    else
                    {
                        FileClose();
                        return 37;
                    }
                }
                else if (tempVMethold.type == "tab_verb")
                {
                    TableClass tempVTable = TableList.Find(table => table.name == WriteList[index].V_Expression);
                    if (tempVTable == null)
                    {
                        FileClose();
                        return 38;
                    }
                    if (tempVTable.table != null)
                    {
                        //WriteList[index].V_Expression = tempVTable.table;
                        WriteList[index].V_ExpressType = "tab_verb";
                        if (!caculateWriteVTableDictionary(tempVTable.table, index))
                        {
                            FileClose();
                            return 39;
                        }

                    }
                    else
                    {
                        FileClose();
                        return 40;
                    }
                }
                else
                {
                    FileClose();
                    return 41;
                }

                if (WriteList[index].dimension >= 2)
                {
                    MetholdClass tempXMethold = MetholdList.Find(temp => temp.name == WriteList[index].X_Expression);
                    if (tempXMethold == null)
                    {
                        FileClose();
                        return 42;
                    }
                    if (tempXMethold.type == "rat_func")
                    {
                        if (tempXMethold.express != null)
                        {
                            //WriteList[index].V_Expression  = tempVMethold.express;
                            WriteList[index].X_ExpressType = "rat_func";
                            if (!caculateWriteXParameter(tempXMethold.express, index))
                            {
                                FileClose();
                                return 43;
                            }
                        }
                        else
                        {
                            FileClose();
                            return 44;
                        }
                    }
                    else if (tempXMethold.type == "tab_verb")
                    {
                        TableClass tempXTable = TableList.Find(table => table.name == WriteList[index].X_Expression);
                        if (tempXTable == null)
                        {
                            FileClose();
                            return 45;
                        }
                        if (tempXTable.table != null)
                        {
                            //WriteList[index].V_Expression = tempVTable.table;
                            WriteList[index].X_ExpressType = "tab_verb";
                            if (!caculateWriteXTableDictionary(tempXTable.table, index))
                            {
                                FileClose();
                                return 46;
                            }

                        }
                        else
                        {
                            FileClose();
                            return 47;
                        }
                    }
                    else
                    {
                        FileClose();
                        return 48;
                    }

                }


                if (WriteList[index].dimension == 3)
                {
                    MetholdClass tempYMethold = MetholdList.Find(temp => temp.name == WriteList[index].Y_Expression);
                    if (tempYMethold == null)
                    {
                        FileClose();
                        return 49;
                    }
                    if (tempYMethold.type == "rat_func")
                    {
                        if (tempYMethold.express != null)
                        {
                            //WriteList[index].V_Expression  = tempVMethold.express;
                            WriteList[index].Y_ExpressType = "rat_func";
                            if (!caculateWriteYParameter(tempYMethold.express, index))
                            {
                                FileClose();
                                return 50;
                            }
                        }
                        else
                        {
                            FileClose();
                            return 51;
                        }
                    }
                    else if (tempYMethold.type == "tab_verb")
                    {
                        TableClass tempYTable = TableList.Find(table => table.name == WriteList[index].Y_Expression);
                        if (tempYTable == null)
                        {
                            FileClose();
                            return 52;
                        }
                        if (tempYTable.table != null)
                        {
                            //WriteList[index].V_Expression = tempVTable.table;
                            WriteList[index].Y_ExpressType = "tab_verb";
                            if (!caculateWriteYTableDictionary(tempYTable.table, index))
                            {
                                FileClose();
                                return 53;
                            }

                        }
                        else
                        {
                            FileClose();
                            return 54;
                        }
                    }
                    else
                    {
                        FileClose();
                        return 55;
                    }

                }

                RecordLayoutClass tempRecord = RecordList.Find(temp => temp.name == WriteList[index].recordLayout);
                if (tempRecord == null)
                {
                    FileClose();
                    return 56;
                }
                WriteList[index].V_elementLength = tempRecord.V_elementLength;
                WriteList[index].V_elementType = tempRecord.V_elementType;
                if (WriteList[index].dimension >= 2)
                {
                    WriteList[index].X_elementLength = tempRecord.X_elementLength;
                    WriteList[index].X_elementType = tempRecord.X_elementType;
                }

                if (WriteList[index].dimension == 3)
                {
                    WriteList[index].Y_elementLength = tempRecord.Y_elementLength;
                    WriteList[index].Y_elementType = tempRecord.Y_elementType;
                }

                MemorySegmentClass tempMemory = MemoryList.Find(temp => ((WriteList[index].address >= temp.startAddress) && (WriteList[index].address <= (temp.startAddress + temp.offset))));
                if (tempMemory == null)
                {
                    FileClose();
                    return 57;
                }
                if (tempMemory.pageCount < 2)
                {
                    FileClose();
                    return 58;
                }
                WriteList[index].pageCount = tempMemory.pageCount;
                WriteList[index].readPageNumber = tempMemory.readPageNumber;
                WriteList[index].writePageNumber = tempMemory.writePageNumber;
                WriteList[index].segmentNumber = tempMemory.segmentNumber;
            }
            #endregion

            FileClose();
            return 1;
        }
        #endregion 

        #region 获取属性结点
        /// <summary>
        /// 所有匹配量的集合，包含变量的地址，维（Map/Curve/Value/Val_BLK/ASCII），
        /// 每个维度的转换公式、数据类型、长度等信息
        /// </summary>
        public List<WriteClass> WriteListInterface
        {
            get { return WriteList; }
            set { }
        }

        /// <summary>
        /// 是所有测量量的集合，包含测量量的地址、长度、类型、转换公式、数值对照表等信息
        /// </summary>
        public List<ReadClass> ReadListInterface
        {
            get { return ReadList; }
            set { }
        }

        /// <summary>
        /// 包含了波特率、收发CANID、大小端序、EventChannel等
        /// </summary>
        public PropertyClass PropertyInterface
        {
            get { return PropertyInformation; }
            set { }
        }
        #endregion

        #region init 
        private void Init()
        {
            ReadList.Clear();
            WriteList.Clear();
            RecordList.Clear();
            MetholdList.Clear();
            TableList.Clear();
            MemoryList.Clear();
            PropertyInformation.baudrate = 0;
            PropertyInformation.byteOrder = "";
            PropertyInformation.eventChannel_10 = 0;
            PropertyInformation.eventChannel_100 = 0;
            PropertyInformation.eventChannel_sync = 0;
            PropertyInformation.MasterCANID = 0;
            PropertyInformation.SlaverCANID = 0;
            CommandLine = "";
            tempStr = "";
            isVaild = false;
            index = 0;
            dimensionIndex = 0;
            XY_index = 0;
        }
        #endregion

        #region 计算物理量
        //注意物理场景，不可能2个不同的物理值对应同一个CAN总现值的值，
        //因此不可能是一元二次方程，求解及防错可以简化
        //因为该函数在计算解析时使用量巨大，因此尽可能简化，所有防错做在其他函数中
        public float caculatePhyValue(List<float> tempLsit, float canValue)
        {
            float result = 0f;
            if ((canValue * tempLsit[4] - tempLsit[1]) != 0f)
            {
                result = (tempLsit[2] - (canValue * tempLsit[5])) / (canValue * tempLsit[4] - tempLsit[1]);
            }
            return result;
        }
        //注意物理场景，不可能2个不同的物理值对应同一个CAN总现值的值，因此不可能是一元二次方程，求解及防错可以简化
        //因为该函数在计算解析时使用量巨大，因此尽可能简化，所有防错做在其他函数中
        public float caculateCANValue(List<float> tempLsit, float phyValue)
        {
            float result = 0f;
            result = ((tempLsit[0] * phyValue * phyValue) + (tempLsit[1] * phyValue) + tempLsit[2]) / ((tempLsit[3] * phyValue * phyValue) + (tempLsit[4] * phyValue) + tempLsit[5]);
            return result;
        }
        #endregion

        #region 计算表格数据
        private bool caculateReadTableDictionary(string table, int index)
        {
            ReadList[index].tableDictionary = new Dictionary<UInt32, string>();
               string[]  strList =  table.Split(new string[1] {"^&*"}, StringSplitOptions.None);
            UInt32 keyValue = new UInt32();
            string tableContent = new string(new char[] { });
            foreach (string str in strList)
            {
                   string[] tempTable = str.Split(new string[1] {"\""}, StringSplitOptions.None);
                bool isKeyValue = false;
                foreach (string strEumn in tempTable)
                {
                    if ((strEumn.Replace(" ", "") != "") && (isKeyValue == false))
                    {
                        isKeyValue = true;
                        string Eumn = strEumn.Replace(" ", "");
                        if (!UInt32.TryParse(Eumn, out keyValue))
                        {
                            return false;
                        }
                    }
                    else if ((strEumn.Replace(" ", "") != "") && (isKeyValue == true))
                    {
                        tableContent = strEumn;
                        ReadList[index].tableDictionary.Add(keyValue, tableContent);
                        break;
                    }
                }
                //ReadList[index].tableDictionary.Add(keyValue, tableContent);
            }
            return true;

        }
        #endregion

        #region 计算写入X表格数据
        private bool caculateWriteXTableDictionary(string table, int index)
        {
            WriteList[index].X_tableDictionary = new Dictionary<UInt32, string>();
            string[] strList = table.Split(new string[1] { "^&*" }, StringSplitOptions.None);
            UInt32 keyValue = new UInt32();
            string tableContent = new string(new char[] { });
            foreach (string str in strList)
            {
                string[] tempTable = str.Split(new string[1] { "\"" }, StringSplitOptions.None);
                bool isKeyValue = false;
                foreach (string strEumn in tempTable)
                {
                    if ((strEumn.Replace(" ", "") != "") && (isKeyValue == false))
                    {
                        isKeyValue = true;
                        string Eumn = strEumn.Replace(" ", "");
                        if (!UInt32.TryParse(Eumn, out keyValue))
                        {
                            return false;
                        }
                    }
                    else if ((strEumn.Replace(" ", "") != "") && (isKeyValue == true))
                    {
                        tableContent = strEumn;
                        WriteList[index].X_tableDictionary.Add(keyValue, tableContent);
                        break;
                    }
                }
                //ReadList[index].tableDictionary.Add(keyValue, tableContent);
            }
            return true;

        }
        #endregion

        #region 计算写入Y表格数据
        private bool caculateWriteYTableDictionary(string table, int index)
        {
            WriteList[index].Y_tableDictionary = new Dictionary<UInt32, string>();
            string[] strList = table.Split(new string[1] { "^&*" }, StringSplitOptions.None);
            UInt32 keyValue = new UInt32();
            string tableContent = new string(new char[] { });
            foreach (string str in strList)
            {
                string[] tempTable = str.Split(new string[1] { "\"" }, StringSplitOptions.None);
                bool isKeyValue = false;
                foreach (string strEumn in tempTable)
                {
                    if ((strEumn.Replace(" ", "") != "") && (isKeyValue == false))
                    {
                        isKeyValue = true;
                        string Eumn = strEumn.Replace(" ", "");
                        if (!UInt32.TryParse(Eumn, out keyValue))
                        {
                            return false;
                        }
                    }
                    else if ((strEumn.Replace(" ", "") != "") && (isKeyValue == true))
                    {
                        tableContent = strEumn;
                        WriteList[index].Y_tableDictionary.Add(keyValue, tableContent);
                        break;
                    }
                }
                //ReadList[index].tableDictionary.Add(keyValue, tableContent);
            }
            return true;

        }
        #endregion

        #region 计算写入V表格数据
        private bool caculateWriteVTableDictionary(string table, int index)
        {
            WriteList[index].V_tableDictionary = new Dictionary<UInt32, string>();
            string[] strList = table.Split(new string[1] { "^&*" }, StringSplitOptions.None);
            UInt32 keyValue = new UInt32();
            string tableContent = new string(new char[] { });
            foreach (string str in strList)
            {
                string[] tempTable = str.Split(new string[1] { "\"" }, StringSplitOptions.None);
                bool isKeyValue = false;
                foreach (string strEumn in tempTable)
                {
                    if ((strEumn.Replace(" ", "") != "") && (isKeyValue == false))
                    {
                        isKeyValue = true;
                        string Eumn = strEumn.Replace(" ", "");
                        if (!UInt32.TryParse(Eumn, out keyValue))
                        {
                            return false;
                        }
                    }
                    else if ((strEumn.Replace(" ", "") != "") && (isKeyValue == true))
                    {
                        tableContent = strEumn;
                        WriteList[index].V_tableDictionary.Add(keyValue, tableContent);
                        break;
                    }
                }
                //ReadList[index].tableDictionary.Add(keyValue, tableContent);
            }
            return true;

        }
        #endregion

        #region 计算读取参数
        private bool caculateReadParameter(string Parameter, int index)
        {
            ReadList[index].parameter = new List<float>();
            string[] parameterList = Parameter.Split(new char[1] { ' ' });
            string temp;
            float result;
            foreach (string tempParamenter in parameterList)
            {
                temp = tempParamenter.Replace(" ", "");
                if (temp != "")
                {
                    if (float.TryParse(temp, out result))
                    {
                        ReadList[index].parameter.Add(result);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //注意物理场景，不可能2个不同的物理值对应同一个CAN总现值的值，因此不可能是一元二次方程，求解及防错可以简化
            if ((ReadList[index].parameter.Count != 6) || (ReadList[index].parameter[0] != 0f) || (ReadList[index].parameter[3] != 0f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 计算写入X参数
        private bool caculateWriteXParameter(string Parameter, int index)
        {
            WriteList[index].X_parameter = new List<float>();
            string[] parameterList = Parameter.Split(new char[1] { ' ' });
            string temp;
            float result;
            foreach (string tempParamenter in parameterList)
            {
                temp = tempParamenter.Replace(" ", "");
                if (temp != "")
                {
                    if (float.TryParse(temp, out result))
                    {
                        WriteList[index].X_parameter.Add(result);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //注意物理场景，不可能2个不同的物理值对应同一个CAN总现值的值，因此不可能是一元二次方程，求解及防错可以简化
            if ((WriteList[index].X_parameter.Count != 6) || (WriteList[index].X_parameter[0] != 0f) || (WriteList[index].X_parameter[3] != 0f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 计算写入Y参数
        private bool caculateWriteYParameter(string Parameter, int index)
        {
            WriteList[index].Y_parameter = new List<float>();
            string[] parameterList = Parameter.Split(new char[1] { ' ' });
            string temp;
            float result;
            foreach (string tempParamenter in parameterList)
            {
                temp = tempParamenter.Replace(" ", "");
                if (temp != "")
                {
                    if (float.TryParse(temp, out result))
                    {
                        WriteList[index].Y_parameter.Add(result);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //注意物理场景，不可能2个不同的物理值对应同一个CAN总现值的值，因此不可能是一元二次方程，求解及防错可以简化
            if ((WriteList[index].Y_parameter.Count != 6) || (WriteList[index].Y_parameter[0] != 0f) || (WriteList[index].Y_parameter[3] != 0f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 计算写入V参数
        private bool caculateWriteVParameter(string Parameter, int index)
        {
            WriteList[index].V_parameter = new List<float>();
            string[] parameterList = Parameter.Split(new char[1] { ' ' });
            string temp;
            float result;
            foreach (string tempParamenter in parameterList)
            {
                temp = tempParamenter.Replace(" ", "");
                if (temp != "")
                {
                    if (float.TryParse(temp, out result))
                    {
                        WriteList[index].V_parameter.Add(result);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //注意物理场景，不可能2个不同的物理值对应同一个CAN总现值的值，因此不可能是一元二次方程，求解及防错可以简化
            if ((WriteList[index].V_parameter.Count != 6) || (WriteList[index].V_parameter[0] != 0f) || (WriteList[index].V_parameter[3] != 0f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 关闭文件
        private void FileClose()
        {
            A2lFile.Close();
            A2lReader.Close();
        }
        #endregion
    }
}
