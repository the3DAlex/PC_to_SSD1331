using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FTD2XX_NET;
using CH341;

namespace SSD1331
{
    public class SSD1331
    {
        // SSD1331 Commands
        private const byte SSD1331_CMD_DRAWLINE = 0x21;//!< Draw line
        private const byte SSD1331_CMD_DRAWRECT = 0x22;//!< Draw rectangle
        private const byte SSD1331_CMD_FILL = 0x26;//!< Fill enable/disable
        private const byte SSD1331_CMD_SETCOLUMN = 0x15;//!< Set column address
        private const byte SSD1331_CMD_SETROW = 0x75;//!< Set row adress
        private const byte SSD1331_CMD_CONTRASTA = 0x81;//!< Set contrast for color A
        private const byte SSD1331_CMD_CONTRASTB = 0x82;//!< Set contrast for color B
        private const byte SSD1331_CMD_CONTRASTC = 0x83;//!< Set contrast for color C
        private const byte SSD1331_CMD_MASTERCURRENT = 0x87;//!< Master current control
        private const byte SSD1331_CMD_SETREMAP = 0xA0;//!< Set re-map & data format
        private const byte SSD1331_CMD_STARTLINE = 0xA1;//!< Set display start line
        private const byte SSD1331_CMD_DISPLAYOFFSET = 0xA2;//!< Set display offset
        private const byte SSD1331_CMD_NORMALDISPLAY = 0xA4;//!< Set display to normal mode
        private const byte SSD1331_CMD_DISPLAYALLON = 0xA5;//!< Set entire display ON
        private const byte SSD1331_CMD_DISPLAYALLOFF = 0xA6;//!< Set entire display OFF
        private const byte SSD1331_CMD_INVERTDISPLAY = 0xA7;//!< Invert display
        private const byte SSD1331_CMD_SETMULTIPLEX = 0xA8;//!< Set multiplex ratio
        private const byte SSD1331_CMD_SETMASTER = 0xAD;//!< Set master configuration
        private const byte SSD1331_CMD_DISPLAYOFF = 0xAE;//!< Display OFF (sleep mode)
        private const byte SSD1331_CMD_DISPLAYON = 0xAF;//!< Normal Brightness Display ON
        private const byte SSD1331_CMD_POWERMODE = 0xB0;//!< Power save mode
        private const byte SSD1331_CMD_PRECHARGE = 0xB1;//!< Phase 1 and 2 period adjustment
        private const byte SSD1331_CMD_CLOCKDIV = 0xB3;//!< Set display clock divide ratio/oscillator frequency
        private const byte SSD1331_CMD_PRECHARGEA = 0x8A;//!< Set second pre-charge speed for color A
        private const byte SSD1331_CMD_PRECHARGEB = 0x8B;//!< Set second pre-charge speed for color B
        private const byte SSD1331_CMD_PRECHARGEC = 0x8C;//!< Set second pre-charge speed for color C
        private const byte SSD1331_CMD_PRECHARGELEVEL = 0xBB;//!< Set pre-charge voltage
        private const byte SSD1331_CMD_VCOMH = 0xBE;//!< Set Vcomh voltge

        private const int SCL = 0;
        private const int SDA = 1;
        private const int RES = 2;
        private const int DC = 4;
        private const int CS = 5;
        private static FTDI ft = new FTDI();
        private static CH341A ch = new CH341A();
        public bool FTnotCH = true;
        //------------------------------------------------------------------------------------
        public byte[] colorToBytes(Color c)
        {
            byte[] outV = new byte[2];
            outV[0] = (byte)((c.R & 0b11111000) | (c.G >> 5));
            outV[1] = (byte)(((c.G >> 2) & 0b00000111) | (c.B >> 3));
            return outV;
        }

        //------------------------------------------------------------------------------------
        public void sendBitmap(Bitmap bmpFile)
        {
            List<byte> data = new List<byte>();
            if (FTnotCH)
            {
                for (int y = 0; y < bmpFile.Height; y++)
                {
                    for (int x = 0; x < bmpFile.Width; x++)
                    {
                        var c = colorToBytes(bmpFile.GetPixel(x, y));
                        data.AddRange(prepareByteToSend(c[0], false));
                        data.AddRange(prepareByteToSend(c[1], false));
                    }
                }
                bmpFile.Dispose();

                uint bitsWritten = 0;
                byte[] dataSend = data.ToArray();
                ft.Write(dataSend, dataSend.Length, ref bitsWritten);
            }
            else
            {

                for (int y = 0; y < bmpFile.Height; y++)
                {
                    for (int x = 0; x < bmpFile.Width; x++)
                    {
                        data.AddRange(colorToBytes(bmpFile.GetPixel(x, y)));
                    }
                }
                int adder = bmpFile.Width * bmpFile.Height / 2;
                bmpFile.Dispose();

                for (int t = 0; t < 4; t++) //4 packets = full screen
                {
                    var part = data.GetRange(t * adder, adder);
                    byte[] output = part.ToArray();
                    ch.StreamSPI4(0x80, ref output);
                }
            }
        }

        //------------------------------------------------------------------------------------
        public void changeBit(ref int outByte, int bitNumber, int val )
        {
            outByte = outByte & ~(1 << bitNumber) | (val << bitNumber);
        }

        //------------------------------------------------------------------------------------
        public byte[] prepareByteToSend (byte b, bool isCommand = true)
        {
            int dataUnit = 0b00000100; //SCL - 0 bit, SDA - 1 bit, RES - 2 bit, DC - 4 bit, CS - 5bit
            if (!isCommand) changeBit(ref dataUnit, DC, 1);
            byte[] data = new byte[16];//8 bit data and 8 bit clock

            for (int i = 0; i < 16; i += 2)
            {
                //write data bit
                changeBit(ref dataUnit, SDA, ((b >> (7 - i / 2)) & 1));
                changeBit(ref dataUnit, SCL, 0);
                // change CS pin to hi at the end of package
                //if (i == 14) changeBit(ref dataUnit, CS, 1);
                //put dataUnit to data array
                data[i] = (byte)dataUnit;

                //write clock bit 
                changeBit(ref dataUnit, SCL, 1);
                data[i + 1] = (byte)dataUnit;
            }
            return data;
        }

        //------------------------------------------------------------------------------------
        public void sendCMD(byte command)
        {
            if (FTnotCH)
            {
                uint bitsWritten = 0;
                byte[] data = prepareByteToSend(command);
                ft.Write(data, data.Length, ref bitsWritten);
            }
            else
            {
                byte[] cm = { command };
                byte[] cm2 = { 0x00 };
                ch.StreamSPI5(0x80, 1, ref cm, ref cm2);
            }     
        }

        //------------------------------------------------------------------------------------
        public void resetDispl ()
        {
            uint bitsWritten = 0;
            byte[] d = { 0b00000000 }; //SCL - 0 bit, SDA - 1 bit, RES - 2 bit, DC - 4 bit, CS - 5bit
            ft.Write(d, d.Length, ref bitsWritten);
            System.Threading.Thread.Sleep(500);
            d[0] = 0b00000100;
            ft.Write(d, d.Length, ref bitsWritten);
        }

        //------------------------------------------------------------------------------------
        public String initDispl()
        {
            if (FTnotCH)
            {
                if(ft.OpenByDescription("FT232R USB UART") == FTDI.FT_STATUS.FT_OK)
                {
                    ft.SetBitMode(0b00111111, FTDI.FT_BIT_MODES.FT_BIT_MODE_ASYNC_BITBANG);
                    ft.SetBaudRate(3000000);
                    resetDispl();
                }
                else return "false";
            }
            else
            {
                if (ch.OpenDevice())
                {
                    ch.SetStream(0b10000011);
                    ch.SetDelaymS(0);
                }
                else return "false";
            }

            // Initialization Sequence
            sendCMD(SSD1331_CMD_DISPLAYOFF); // 0xAE
            sendCMD(SSD1331_CMD_SETREMAP);   // 0xA0
            sendCMD(0x72); // RGB Color
            sendCMD(SSD1331_CMD_STARTLINE); // 0xA1
            sendCMD(0x0);
            sendCMD(SSD1331_CMD_DISPLAYOFFSET); // 0xA2
            sendCMD(0x0);
            sendCMD(SSD1331_CMD_NORMALDISPLAY); // 0xA4
            sendCMD(SSD1331_CMD_SETMULTIPLEX);  // 0xA8
            sendCMD(0x3F);                      // 0x3F 1/64 duty
            sendCMD(SSD1331_CMD_SETMASTER);     // 0xAD
            sendCMD(0x8E);
            sendCMD(SSD1331_CMD_POWERMODE); // 0xB0
            sendCMD(0x1A);
            sendCMD(SSD1331_CMD_PRECHARGE); // 0xB1
            sendCMD(0x31);
            sendCMD(SSD1331_CMD_CLOCKDIV); // 0xB3
            sendCMD(0xF0); // 7:4 = Oscillator Frequency, 3:0 = CLK Div Ratio (A[3:0]+1 = 1..16)
            sendCMD(SSD1331_CMD_PRECHARGEA); // 0x8A
            sendCMD(0x64);
            sendCMD(SSD1331_CMD_PRECHARGEB); // 0x8B
            sendCMD(0x78);
            sendCMD(SSD1331_CMD_PRECHARGEC); // 0x8C
            sendCMD(0x64);
            sendCMD(SSD1331_CMD_PRECHARGELEVEL); // 0xBB
            sendCMD(0x3A);
            sendCMD(SSD1331_CMD_VCOMH); // 0xBE
            sendCMD(0x3E);
            sendCMD(SSD1331_CMD_MASTERCURRENT); // 0x87
            sendCMD(0x06);
            sendCMD(SSD1331_CMD_CONTRASTA); // 0x81
            sendCMD(0x91);
            sendCMD(SSD1331_CMD_CONTRASTB); // 0x82
            sendCMD(0x50);
            sendCMD(SSD1331_CMD_CONTRASTC); // 0x83
            sendCMD(0x7D);
            sendCMD(SSD1331_CMD_DISPLAYON); //--turn on oled panel
            sendCMD(0x2E);

            sendCMD(0x22);
            sendCMD(0);
            sendCMD(0);
            sendCMD(22);
            sendCMD(22);
            sendCMD(22);
            sendCMD(22);
            sendCMD(22);
            sendCMD(22);
            sendCMD(22);
            sendCMD(22);

            
            //clearWindow 
            sendCMD(0x25);
            sendCMD(0);
            sendCMD(0);
            sendCMD(95);
            sendCMD(63);

            // turn off scrolling
            sendCMD(0x26);
            sendCMD(1);
            
            return "true";
        }

        //------------------------------------------------------------------------------------
        public void closeDev()
        {
            if (FTnotCH) ft.Close(); else ch.CloseDevice();
        }

    }

}
