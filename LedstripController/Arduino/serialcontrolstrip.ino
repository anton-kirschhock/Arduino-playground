#define RED_PIN 5
#define GREEN_PIN 6
#define BLUE_PIN 7

int Red_Brightness = 255;
int Blue_Brightness = 0;
int Green_Brightness = 0;

String inString;

void setup(){
    Serial.begin(9600);
    pinMode(RED_PIN, OUTPUT);
    pinMode(GREEN_PIN, OUTPUT);
    pinMode(BLUE_PIN, OUTPUT);
    setColourFromHex("3cb371");
    writeColour();
    Serial.println("Ready for input!");
}  

void loop(){
    if (Serial.available() > 0) {
        inString = Serial.readString(); // read the incoming byte:

        Serial.print("Received: ");
        Serial.println(inString);
        setColourFromHex(inString);
        writeColour();

        delay(1000);
    }
}
void writeColour(){
    Serial.print("RGB(");
    Serial.print(Red_Brightness);
    Serial.print(", ");
    Serial.print(Green_Brightness);
    Serial.print(", ");
    Serial.print(Blue_Brightness);
    Serial.println(");");
}

void setColourFromHex(String hexColour){
    long number = strtol(&hexColour[0], NULL, 16);
    // Split them up into r, g, b va\lues
    int r = number >> 16;
    int g = number >> 8 & 0xFF;
    int b = number & 0xFF;
    Red_Brightness = r;
    Green_Brightness =g;
    Blue_Brightness = b;

    analogWrite(RED_PIN, Red_Brightness);
    analogWrite(GREEN_PIN, Green_Brightness);
    analogWrite(BLUE_PIN, Blue_Brightness);
}