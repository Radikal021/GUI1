using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Reflection.Emit;

namespace GUI1
{
    public class PortCom
    {
        SerialPort serial = new SerialPort();
        List<byte> list = new List<byte>();
        byte[] packet;
        bool FlagGetData;
        public event EventHandler<ClassOpcode> EventSerial;

        public void SETTINGSERIAL(int Baudrate, string com)
        {
            MainWindow mainWindow = new MainWindow();
            if (!serial.IsOpen)
            {
                serial.BaudRate = Baudrate;
                serial.Parity = Parity.None;
                serial.StopBits = StopBits.One;
                serial.PortName = com;
                serial.Open();
            }
            else if (serial.IsOpen)
            {
                serial.Close();
            }
        }
        public void reciveData()
        {
            try
            {
                byte[] ArrayData = new byte[1024];
                while (FlagGetData)
                {
                    int rec = serial.Read(ArrayData, 0, ArrayData.Length);
                    for (int i = 0; i < rec; i++)
                        list.Add(ArrayData[i]);
                    while (true)
                    {
                        int index = 0;
                        if (list.Count >= 20)
                        {
                            if (list[index] == 0x48 && list[index + 1] == 0x45 && list[index + 2] == 0x41 && list[index + 3] == 0x44)
                            {
                                index += 4;
                                short FrameCounter = BitConverter.ToInt16(list.ToArray(), index); index += 2;
                                byte DataType = list[index]; index++;
                                byte Source = list[index]; index++;
                                byte Destination = list[index]; index++;
                                byte Opcode = list[index]; index++;
                                int size = BitConverter.ToInt32(list.ToArray(), index); index += 4;
                                short data = BitConverter.ToInt16(list.ToArray(), index); index += 4;
                                index += 4;
                                list.RemoveRange(0, 20);
                                EventSerial?.Invoke(this, new ClassOpcode(data, Opcode));
                            }
                            else
                            {
                                index++;
                                list.RemoveAt(0);
                            }
                        }
                        else
                            break;
                    }
                }
            }
            catch
            {
                ;
            }
        }
        public void SendData(opCode opcode, short value)
        {
            packet = new byte[20];
            packet[0] = 0x48;//Header
            packet[1] = 0x45;//Header
            packet[2] = 0x41;//Header
            packet[3] = 0x44;//Header
            packet[4] = 0x00;//FrameCounter
            packet[5] = 0x00;//FrameCounter
            packet[6] = 0x0B;//DataType
            packet[7] = 0x10;//From
            packet[8] = 0x12;//To
            packet[9] = (byte)opcode; //Opcode
            packet[10] = 0x00;//Size
            packet[11] = 0x00;//Size
            packet[12] = 0x00;//Size
            packet[13] = 0x00;//Size
            byte[] temp = BitConverter.GetBytes(value);
            packet[14] = temp[0];//Data
            packet[15] = temp[1];//Data
            packet[17] = 0xAA;//Footer
            packet[16] = 0xBB;//Footer 
            packet[18] = 0xCC;//Footer
            packet[19] = 0xDD;//Footer
        }
    }
    public class ClassOpcode : EventArgs
    {
        public int Value;
        public int Opcode;
        public ClassOpcode(int value, int opcode)
        {
            this.Value = value;
            this.Opcode = opcode;
        }
    }
    public enum opCode
    {
        Att1 = 1,
        Att2 = 2,
        AttSum = 3,
        AttSwitch = 4,
        AttStep = 5,
        AttTimeStep = 6,
        AttStart = 7,
    }
}
