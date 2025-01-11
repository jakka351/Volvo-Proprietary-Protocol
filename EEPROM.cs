#region Copyright (c) 2024, Jack Leighton
// /////     __________________________________________________________________________________________________________________
// /////
// /////                  __                   __              __________                                      __   
// /////                _/  |_  ____   _______/  |_  __________\______   \_______   ____   ______ ____   _____/  |_ 
// /////                \   __\/ __ \ /  ___/\   __\/ __ \_  __ \     ___/\_  __ \_/ __ \ /  ___// __ \ /    \   __\
// /////                 |  | \  ___/ \___ \  |  | \  ___/|  | \/    |     |  | \/\  ___/ \___ \\  ___/|   |  \  |  
// /////                 |__|  \___  >____  > |__|  \___  >__|  |____|     |__|    \___  >____  >\___  >___|  /__|  
// /////                           \/     \/            \/                             \/     \/     \/     \/      
// /////                                                          .__       .__  .__          __                    
// /////                               ____________   ____   ____ |__|____  |  | |__| _______/  |_                  
// /////                              /  ___/\____ \_/ __ \_/ ___\|  \__  \ |  | |  |/  ___/\   __\                 
// /////                              \___ \ |  |_> >  ___/\  \___|  |/ __ \|  |_|  |\___ \  |  |                   
// /////                             /____  >|   __/ \___  >\___  >__(____  /____/__/____  > |__|                   
// /////                                  \/ |__|        \/     \/        \/             \/                         
// /////                                  __                         __  .__                                        
// /////                   _____   __ ___/  |_  ____   _____   _____/  |_|__|__  __ ____                            
// /////                   \__  \ |  |  \   __\/  _ \ /     \ /  _ \   __\  \  \/ // __ \                           
// /////                    / __ \|  |  /|  | (  <_> )  Y Y  (  <_> )  | |  |\   /\  ___/                           
// /////                   (____  /____/ |__|  \____/|__|_|  /\____/|__| |__| \_/  \___  >                          
// /////                        \/                         \/                          \/                           
// /////                                                  .__          __  .__                                      
// /////                                       __________ |  |  __ ___/  |_|__| ____   ____   ______                
// /////                                      /  ___/  _ \|  | |  |  \   __\  |/  _ \ /    \ /  ___/                
// /////                                      \___ (  <_> )  |_|  |  /|  | |  (  <_> )   |  \\___ \                 
// /////                                     /____  >____/|____/____/ |__| |__|\____/|___|  /____  >                
// /////                                          \/                                      \/     \/                 
// /////                                   Tester Present Specialist Automotive Solutions
// /////     __________________________________________________________________________________________________________________
// /////      |--------------------------------------------------------------------------------------------------------------|
// /////      |       https://github.com/jakka351/| https://testerPresent.com.au | https://facebook.com/testerPresent        |
// /////      |--------------------------------------------------------------------------------------------------------------|
// /////      | Copyright (c) 2022/2023/2024 Benjamin Jack Leighton                                                          |          
// /////      | All rights reserved.                                                                                         |
// /////      |--------------------------------------------------------------------------------------------------------------|
// /////        Redistribution and use in source and binary forms, with or without modification, are permitted provided that
// /////        the following conditions are met:
// /////        1.    With the express written consent of the copyright holder.
// /////        2.    Redistributions of source code must retain the above copyright notice, this
// /////              list of conditions and the following disclaimer.
// /////        3.    Redistributions in binary form must reproduce the above copyright notice, this
// /////              list of conditions and the following disclaimer in the documentation and/or other
// /////              materials provided with the distribution.
// /////        4.    Neither the name of the organization nor the names of its contributors may be used to
// /////              endorse or promote products derived from this software without specific prior written permission.
// /////      _________________________________________________________________________________________________________________
// /////      THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES,
// /////      INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// /////      DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// /////      SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
// /////      SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,
// /////      WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE
// /////      USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// /////      _________________________________________________________________________________________________________________
// /////
// /////       This software can only be distributed with my written permission. It is for my own educational purposes and  
// /////       is potentially dangerous to ECU health and safety. Gracias a Gato Blancoford desde las alturas del mar de chelle.                                                        
// /////      _________________________________________________________________________________________________________________
// /////
// /////
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#endregion License
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Reflection.Emit;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using J2534;
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace OBD2
{
	public partial class D13 : Form
	{
		private async Task readEeprom()
		{
			try
			{
        ecuRxIdentifier1 = 0x07;
        ecuRxIdentifier2 = 0xFF;
        bool bootloaderFlag = false;
        using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					saveFileDialog.Title = "Volvo D13 Flasher";
					saveFileDialog.DefaultExt = "bin";
					saveFileDialog.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
					saveFileDialog.FileName = "Volvo EEPROM.bin";
					if (saveFileDialog.ShowDialog() == DialogResult.OK)
					{
            string filePath = saveFileDialog.FileName;
            FileStream fileStream = File.Open(filePath, FileMode.Create);
            //base hex  timestamps absolute
            //1251.105224 1 07FF       Rx   d 8 05 10 02 54 52 57 00 00 >> START DIAG SESSION
            //1251.106224 1 07FE       Rx   d 8 06 50 02 00 19 01 F4 00 //
            startDiagnosticSession(0x02); // Messaage might need to have more added to it to matchthe CAN log
						controlDtcSetting(0x02); // stop dtc logging for flash routine
						try
						{
							//1251.120224 1 07FF       Rx   d 8 04 22 F1 80 00 00 00 00 >> Mode 22 Request F180 //THIS NEEDS TO BE PARSED AND PUT TO TEXT BOX
							//1251.120224 1 07FE       Rx   d 8 10 13 62 F1 80 01 54 52 >> ??//
							//1251.123224 1 07FF       Rx   d 8 30 00 00 00 00 00 00 00 
							//1251.123224 1 07FE       Rx   d 8 21 57 20 42 4F 4F 54 2D 
							//1251.124224 1 07FE       Rx   d 8 22 30 31 2E 30 37 56 00 //
							byte[] mode22eeprom = { 0x00, 0x00, 0x07, 0xFF, 0x22, 0xF1, 0x80 };
							string hardware = sendPassThruMsg(mode22eeprom);
							hardware = hardware.Replace(" ", ""); hardware = hardware.Substring(0, 2); // T TRE 0.000
                        }
						//1251.228224 1 07FF       Rx   d 8 02 27 01 00 00 00 00 00 >> Request Security Access
						//1251.229224 1 07FE       Rx   d 8 05 67 01 00 00 00 00 00 << Seed 00 00 00
						//1251.231224 1 07FF       Rx   d 8 05 27 02 42 5F 6F 00 00 << Key 42 5F 6F
						//1251.232224 1 07FE       Rx   d 8 02 67 02 00 00 00 00 00 >> Security Access Granted//
						catch(Exception Ex) { Log("Mode 22 Error \r\n");  }
						if(requestSecurityAccess0x27())
						{
							try
							{
								//1251.333224 1 07FF       Rx   d 8 04 22 F1 80 00 00 00 00 >> Mode 22 Request F180 // PARSE THIS TO TEXTBOX ITS ASCII
								//1251.333224 1 07FE       Rx   d 8 10 13 62 F1 80 01 54 52 
								//1251.335224 1 07FE       Rx   d 8 21 57 20 42 4F 4F 54 2D 
								//1251.335224 1 07FF       Rx   d 8 30 00 00 00 00 00 00 00 
								//1251.336224 1 07FE       Rx   d 8 22 30 31 2E 30 37 56 00 //
                byte[] mode22eeprom = { 0x00, 0x00, 0x07, 0xFF, 0x22, 0xF1, 0x80 };
                sendPassThruMsg(mode22eeprom);
								//1251.541224 1 07FF       Rx   d 8 10 0B 34 00 44 40 00 80 >> Request Download
								//1251.542224 1 07FE       Rx   d 8 30 00 00 00 00 00 00 00 
								//1251.542224 1 07FF       Rx   d 8 21 00 00 00 3A 00 00 00 
								//1251.543224 1 07FE       Rx   d 8 04 74 20 0F FA 00 00 00 << Positive Response//
								byte[] eepromDownload = { 0x00, 0x00, 0x07, 0xFF, 0x34, 0x00, 0x44, 0x40, 0x00, 0x80, 0x00, 0x00, 0x00, 0x3A, 0x00, 0x00 };
								if(requestDownload(eepromDownload))
								{
		                            try
		                            {
                                        string bootloaderFile = "OBD2.lib.Firmware.bootloader.sbootloader.bin";
                                        byte[] bootloader = File.ReadAllBytes(bootloaderFile);
                                        //Upload Bootloader
                                        int blocksize = 0x3A;
                                        for (int i = 0x000; i <= 0xA6F; i += blocksize)
		                                {
		                                    transferData(bootloader, i, blocksize);
		                                    if (i == 0xA6F) { bootloaderFlag = true; break; }
		                                }
		                                if (bootloaderFlag)
		                                {
		                                    requestTransferExit();
		                                    Log("Secondary Bootloader has been uploaded\r\n");
		                                    Log("Activating Secondary Bootloader");
		                                    //Activate Routine
		                                    // 1183.00224 1 07FF       Rx   d 8 10 08 31 01 03 01 00 00 
		                                    // 1183.00224 1 07FE       Rx   d 8 30 00 00 00 00 00 00 00 
		                                    // 1183.01224 1 07FF       Rx   d 8 21 00 00 00 00 00 00 00 
		                                    // 1183.01224 1 07FE       Rx   d 8 06 71 01 03 01 00 00 00 
		                                    byte[] routineRequest = { 0x00, 0x00, 0x07, 0xFF, 0x31, 0x01, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00 };
		                                    string parseRoutineResponse = sendPassThruMsg(routineRequest);
		                                    parseRoutineResponse = parseRoutineResponse.Replace(" ", "");
		                                    parseRoutineResponse = parseRoutineResponse.Substring(8, 2);
		                                    int response = int.Parse(parseRoutineResponse, System.Globalization.NumberStyles.HexNumber);
		                                    switch (response)
		                                    {
		                                        case 0x7F:
		                                            Log("Failed to Activate Bootloader\r\n");
		                                            break;
		                                        case 0x71:
		                                            try //Disconnect ISO15765 and reconnect as CAN protocol for propietary volvo protocol
                                                    {
                                                        connectFlag = false;
                                                        connectSelectedJ2534Device();
                                                        connectFlag = true;
                                                        connectSelectedJ2534ToCAN(); // Set protocol to CAN
                            														byte[] data = new byte[0x3FC];
                            														// Step 2: Now switch to Volvo custom protocol
                                                        if (!volvoStartCommand()) return;
                                                        // Possibly do 0x4E, 0x41, etc. 
                                                        // For illustration, we do multiple "0x4B" with offsets from 0 to 0x100 in increments of 4.
                                                        for (uint addr = 0x0000; addr <= 0x03FC; addr += 4)
                                                        {
                                                            data = VolvoCommand4B(addr);
                                                            fileStream.Write(data, 0, data.Length);
                            															if(addr == 0x3FC) { break; }  // EEPROM is 3FC in length
                            														}
                                                        fileStream.Close();
                                                        Log("Volvo Proprietary Protocol Routine complete.");
                                                    }
                                                    catch (Exception Ex) { Log("Error Switching to CAN Protocol \r\n"); }
                                                    // Start Download of Firmare // Make Sense of Propietary Volvo Protocol
                                                    break;
		                                    }
		                                }
		                            }
		                            catch (Exception Ex) { Log("Request Download Error\r\n"); }
		                        }
		                        else { MessageBox.Show("Error Requesting EEPROM Download", "Volvo D13 Flasher");  }
							
		                    }
							catch(Exception Ex) { Log("Mode 22 Error\r\n");  }
		                }
		                else { MessageBox.Show("Error Requesting Security Access", "Volvo D13 Flasher"); }
		                //1251.547224 1 07FE       Rx   d 8 30 00 00 00 00 00 00 00 //
		                //1251.547224 1 07FF       Rx   d 8 10 F2 36 01 40 00 80 20 >> 0x36 Transfer Data datasize F2
		            }
				}
			}
			catch(Exception Ex) { Log("Read EEPROM Exception\r\n");  }		
		}		
	}
}
