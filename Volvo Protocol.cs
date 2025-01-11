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
using System.Collections;
using System.Windows.Forms;
using System.Globalization;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using J2534;
using Microsoft.Build.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Windows.Forms.AxHost;
using System.ComponentModel.Design;
// ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
namespace OBD2
{
    //Volvo Proprietary Protocol - format used in flashing the ECU via the 11 bit ID canbus.
	public partial class D13 : Form
	{
		bool volvoStartCommand()
		{
			try
			{
                // 6088.22928 1 07FF Rx   d 8 21 00 00 00 00 00 00 00 >> Havent Included this frame in the Volvo Protocol.cs file yet
                // 6092.76928 1 07FF Rx   d 8 40 00 00 00 00 00 00 00 >> Start Session
                // 6092.85928 1 07FE Rx   d 8 80 00 00 00 00 00 00 00 << Positive Response(+40)
                byte[] command21 = { 0x00, 0x00, 0x07, 0xFF, 0x21, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                sendPassThruMsg(command21);
                byte[] command40 = { 0x00, 0x00, 0x07, 0xFF, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                sendPassThruMsg(command40);
                // 6092.88928 1 07FF Rx   d 8 4E 00 00 00 00 00 00 00 >> Request
                // 6092.89928 1 07FE Rx   d 8 8E 00 00 00 57 77 E0 10 << Response that appears to be multiframe
                // 6092.91928 1 07FE Rx   d 8 CE 00 00 00 40 00 00 98 <<
                // 6092.92928 1 07FE Rx   d 8 0E 00 00 00 10 01 20 20 <<
                byte[] command4E = { 0x00, 0x00, 0x07, 0xFF, 0x4E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                sendPassThruMsg(command4E);
                // 6092.98928 1 07FF Rx   d 8 41 00 00 00 00 00 00 00 >>
                // 6093.00928 1 07FE Rx   d 8 81 54 47 71 82 13 04 3E <<
                byte[] command41 = { 0x00, 0x00, 0x07, 0xFF, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                sendPassThruMsg(command41);
                // 6093.02928 1 07FF Rx   d 8 42 33 95 6B 10 12 08 29 >>
                // 6093.04928 1 07FE Rx   d 8 82 33 95 6B 10 12 08 29 <<
                byte[] command212 = { 0x00, 0x00, 0x07, 0xFF, 0x42, 0x33, 0x95, 0x6B, 0x10, 0x12, 0x08, 0x29 };
                sendPassThruMsg(command212);
                // 6093.06928 1 07FF Rx   d 8 44 01 00 00 00 00 00 00 >>
                // 6093.07928 1 07FE Rx   d 8 84 01 55 00 00 00 00 00 >>
                byte[] command4401 = { 0x00, 0x00, 0x07, 0xFF, 0x44, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                sendPassThruMsg(command4401);
                // 6093.15928 1 07FF Rx   d 8 40 00 00 00 00 00 00 00 >>
                // 6093.15928 1 07FE Rx   d 8 80 00 00 00 00 00 00 00 << These frames will all need to be put into the protcol file.

                // 6093.18928 1 07FF Rx   d 8 4B 00 00 00 00 80 04 00 >> Start of Firmware Download from ECU
                // 6093.20928 1 07FE Rx   d 8 8B 80 BA BE 00 00 00 FF << 8B is positive response so our data bytes are the last 7 bytes
                // 6093.23928 1 07FF Rx   d 8 4B 00 00 04 00 80 04 00 << Increments by 4
                // 6093.23928 1 07FE Rx   d 8 8B 80 BA BE 00 00 00 FF
                // 6093.27928 1 07FF Rx   d 8 4B 00 00 08 00 80 04 00 << Increments by 4 to 8
                // 6093.27928 1 07FE Rx   d 8 8B 80 BA BE 00 00 00 FF
                byte[] command = { 0x00, 0x00, 0x07, 0xFF, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
			    string response = sendPassThruMsg(command);
			    // response = 00 00 07 FE 80 00 00 00 00 00 00 00
			    response = response.Replace(" ", ""); response = response.Substring(8, 2);
			    int resp = int.Parse(response, System.Globalization.NumberStyles.HexNumber);
			    switch(resp)
			    {
			    	case 0x00:
			    		return false;
			    	case 0x80:
			    		Log("Positive Response to Volvo Propietary Command\r\n");
			    		return true;
			    	default:
			    		return false;
			    }
                return false;
				
			}
			catch(Exception Ex) { Log("Volvo Propietary Command Error\r\n"); return false; }
		}
		public byte[] VolvoCommand4B(uint offset)
        {
            // Based on logs: 4B 00 00 [offsetLow] [offsetHigh] 80 04 00
            byte offsetLow  = (byte)(offset & 0xFF);
            byte offsetHigh = (byte)((offset >> 8) & 0xFF);
            byte[] nodat = {
                0x00, 0x00
            };
            byte[] cmd = {
                0x00, 0x00, 0x07, 0xFF, 0x4B, 0x00, 0x00, offsetLow, offsetHigh, 0x80, 0x04, 0x00
            };
            // response = 00 00 07 FE 80 00 00 00 00 00 00 00
            string response = sendPassThruMsg(cmd);
		    response = response.Replace(" ", ""); 
            string responseByte = response.Substring(8, 2);
		    int resp = int.Parse(responseByte, System.Globalization.NumberStyles.HexNumber);
            string firmwareByte1 = response.Substring(10, 2); string firmwareByte2 = response.Substring(12, 2);
            string firmwareByte3 = response.Substring(14, 2); string firmwareByte4 = response.Substring(16, 2);
            string firmwareByte5 = response.Substring(18, 2); string firmwareByte6 = response.Substring(20, 2);
            string firmwareByte7 = response.Substring(22, 2);
            byte firmware1 = Convert.ToByte(firmwareByte1, 16); byte firmware2 = Convert.ToByte(firmwareByte2, 16);
            byte firmware3 = Convert.ToByte(firmwareByte3, 16); byte firmware4 = Convert.ToByte(firmwareByte4, 16);
            byte firmware5 = Convert.ToByte(firmwareByte5, 16); byte firmware6 = Convert.ToByte(firmwareByte6, 16);
            byte firmware7 = Convert.ToByte(firmwareByte7, 16);
            byte[] firmwareArray = {
                firmware1, firmware2, firmware3, firmware4, firmware5, firmware6, firmware7
            };
            switch(resp)
            {
                case 0x00:
                    Log($"No response for 0x4B offset=0x{offset:X} \r\n");
                    return nodat;
                case 0x8B:
                    Log($"Offset 0x{offset:X}, success: {resp.ToString()} \r\n");
                    return firmwareArray;
                default:
                    Log($"Offset 0x{offset:X}, unexpected response: {resp.ToString()} \r\n");
                    return nodat;
            }
        }
 	}
}
