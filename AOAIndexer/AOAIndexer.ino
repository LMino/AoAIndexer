#include <Arduino.h>
#include <HardwareSerial.h>

#define LEDG 4
#define LEDY 3
#define LEDR 5

//The LED definitions. Must match the AoaState class in the console application.
#define R_HIGH 1
#define G_HIGH 2
#define Y_HIGH 4
#define RG_HIGH 3
#define GY_HIGH 6
#define RY_HIGH 5
#define RGY_HIGH 7

//USB protocol definitions for the second byte
#define HANDSHAKE 128
#define LIGHTDATA 170
#define MESSAGEBEGIN 255

//Initializers for the incoming USB data
uint8_t inputByte_0;
uint8_t inputByte_1;
uint8_t inputByte_2;

void setup() {
	pinMode(LEDG, OUTPUT);
	pinMode(LEDY, OUTPUT);
	pinMode(LEDR, OUTPUT);

	digitalWrite(LEDR, HIGH);
	digitalWrite(LEDG, HIGH);
	digitalWrite(LEDY, HIGH);

	Serial.begin(115200);
}

void SendHandshake(){
	Serial.println(HANDSHAKE);
}

void AllLow() {
	digitalWrite(LEDR, LOW);
	digitalWrite(LEDG, LOW);
	digitalWrite(LEDY, LOW);
}

void ReadResponse() {
	if (Serial.available() >0)
	{
		//Read incoming USB data
		inputByte_0 = Serial.read();
		delay(100);
		inputByte_1 = Serial.read();
		delay(100);
		inputByte_2 = Serial.read();
	}
	//Check for start of Message - byte (255,X,X)
	if (inputByte_0 == MESSAGEBEGIN)
	{
		//Detect Command type
		switch (inputByte_1)
		{
		case LIGHTDATA: //Actions taken if second byte is AoAState change - (255,170,X)
			AllLow(); //Turn all of the LEDs off
			switch (inputByte_2) { //Determine which LEDs must be on based on the third byte.
			case R_HIGH:
				digitalWrite(LEDR, HIGH);
				break;
			case G_HIGH:
				digitalWrite(LEDG, HIGH);
				break;
			case Y_HIGH:
				digitalWrite(LEDY, HIGH);
				break;
			case RG_HIGH:
				digitalWrite(LEDR, HIGH);
				digitalWrite(LEDG, HIGH);
				break;
			case GY_HIGH:
				digitalWrite(LEDG, HIGH);
				digitalWrite(LEDY, HIGH);
				break;
			case RY_HIGH:
				digitalWrite(LEDR, HIGH);
				digitalWrite(LEDY, HIGH);
				break;
			case RGY_HIGH:
				digitalWrite(LEDR, HIGH);
				digitalWrite(LEDG, HIGH);
				digitalWrite(LEDY, HIGH);
				break;
			default: // Action to take if the third byte was none of the above
				AllLow();
				break;
			}
			break;
		case HANDSHAKE:
			//Say hello
			SendHandshake();
			break;
		default:
			break;
		}
		
	}
	//Clear Message bytes
	inputByte_0 = 0;
	inputByte_1 = 0;
	inputByte_2 = 0;
}

void loop() {
	ReadResponse();
}

