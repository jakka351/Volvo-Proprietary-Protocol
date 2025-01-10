# Volvo-Proprietary-Protocol
Decoding a Volvo Proprietary Protocol for flashing truck ECUs
***
## Flashing Process 
1. A standard UDS (ISO 14229) diagnostic session, security access and secondary bootloader upload.
2. A proprietary Volvo bootloader sequence, where the ECU apparently transitions away from standard UDS responses and into a custom command set using 0x07FF (request) and 0x07FE (response).
