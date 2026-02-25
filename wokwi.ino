#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <HTTPClient.h>
#include <DHT.h>

#define DHTPIN 4
#define DHTTYPE DHT22

DHT dht(DHTPIN, DHTTYPE);

const char* ssid = "Wokwi-GUEST";
const char* password = "";

//const char* serverUrl = "https://httpbin.org/post";
const char* serverUrl = "https://catarrhous-iridescently-anton.ngrok-free.dev/api/DHT";

unsigned long sendInterval = 5000;
unsigned long lastSend = 0;

void setup() {
  Serial.begin(115200);
  delay(1000);
  dht.begin();
  connectWiFi();
  delay(1000);
}

void loop() {
  if (millis() -lastSend > sendInterval) {
    lastSend = millis();

    float temperature;
    float humidity;

    if (readSensor(temperature, humidity)) {
      String jsonPayload = buildJson(temperature, humidity);

      sendHTTP(jsonPayload);
    }
  }
}

void connectWiFi() {
  Serial.println("Connectando ao WiFi...");
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("\nWiFi conectado!");
  Serial.print("IP: ");
  Serial.println(WiFi.localIP());
}

bool readSensor(float &temperature, float &humidity) {
  humidity = dht.readHumidity();
  temperature = dht.readTemperature();

  if (isnan(humidity) || isnan(temperature)) {
    Serial.println("Erro ao ler DHT22");
    return false;
  }

  Serial.print("Temperatura: ");
  Serial.println(temperature);
  
  Serial.print("Umidade: ");
  Serial.println(humidity);

  return true;
}

String buildJson(float temperature, float humidity) {
  unsigned long timestamp = millis();

  String json = "{";
  json += "\"device_id\":\"esp32-01\",";
  json += "\"timestamp\":" + String(timestamp) + ",";
  json += "\"temperature\":" + String(temperature, 2) + ",";
  json += "\"humidity\":" + String(humidity, 2);
  json += "}";

  Serial.println("\nJSON Gerado: ");
  Serial.println(json);

  return json;
}

void sendHTTP(String payload) {
  if (WiFi.status() == WL_CONNECTED) {

    WiFiClientSecure client;
    client.setInsecure();
    client.setHandshakeTimeout(30);
    client.setTimeout(30000);

    HTTPClient http;
    http.begin(client, serverUrl);
    http.setFollowRedirects(HTTPC_STRICT_FOLLOW_REDIRECTS);
    http.addHeader("Content-Type", "application/json");
    http.addHeader("User-Agent", "ESP32");

    int httpResponseCode = http.POST(payload);

    Serial.print("HTTP Response code: ");
    Serial.println(httpResponseCode);

    String response = http.getString();
    Serial.println("Resposta do servidor:");
    Serial.println(response);

    http.end();
  }
}
