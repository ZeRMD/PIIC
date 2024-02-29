#include <SPI.h>
#include <Ethernet.h>

#include <ArduinoJson.h>
#include <Arduino_FreeRTOS.h>
#include <task.h>
#include <queue.h>

///////////////////////////////////////////////
//Variáveis
byte mac[] = { 0x00, 0xAD, 0xCE, 0xEB, 0xFE, 0xED };
IPAddress ip(192, 168, 2, 20);

EthernetServer serverUnity(8888);  //Servidor para conecção com unity
EthernetClient clientUnity;  //Client que se conectou aka o unity do outo lado
int porto_I[1][5] = { -1 };
int porto_Q[2][8] = { 0 };

QueueHandle_t envio;
int saida;
bool digitalShadowON = false;

struct data_a_enviar {
  char pos_nome[5];  //Última posição tem de ser \0
  int high;
};

///////////////////////////////////////////////
//Inicializar portos
void inicializar_portos() {

  ///////////////////////////////////////////////
  //Inicializar os portos
  //Input
  pinMode(I0_1, INPUT);
  pinMode(I0_2, INPUT);
  pinMode(I0_3, INPUT);
  pinMode(I0_4, INPUT);

  //Output
  pinMode(Q1_4, OUTPUT);
  pinMode(Q1_5, OUTPUT);
  pinMode(Q1_6, OUTPUT);
  pinMode(Q1_7, OUTPUT);

}

///////////////////////////////////////////////
//Função que devolve o valor dos sensores
int sensor(int portIndex) {
  switch (portIndex) {
    case 1:
      return digitalRead(I0_1);
      break;
    case 2:
      return digitalRead(I0_2);
      break;
    case 3:
      return digitalRead(I0_3);
      break;
    case 4:
      return digitalRead(I0_4);
      break;
  }
}

///////////////////////////////////////////////
//Função que atualiza os atuadores
int atuador(int portIndex, int high) {

  switch (portIndex) {
    case 4:  //Conveyor E Rotate Clockwise
      digitalWrite(Q1_4, high);
      break;
    case 5:  //Conveyor E Rotate Clockwise
      digitalWrite(Q1_5, high);
      break;
    case 6:  //Conveyor C
      digitalWrite(Q1_6, high);
      break;
    case 7:  //Conveyor B
      digitalWrite(Q1_7, high);
      break;
  }

  ///////////////////////////////////////////////
  //Atualizar os portos locais e digitais
  Atualiza_Portos_Locais('Q', portIndex, high);

  if (digitalShadowON) {
    data_a_enviar data;
    data.pos_nome[0] = 'Q';
    data.pos_nome[1] = '1';
    data.pos_nome[2] = '_';
    data.pos_nome[3] = portIndex + '0';
    data.high = high;

    xQueueSend(envio, &data, portMAX_DELAY);
  }
}

///////////////////////////////////////////////
//Função que atualiza os Portos digitais enviando apenas um porto mudado e um novo valor
void Atualiza_Portos_Digital(char *pos_nome, int high) {

  ///////////////////////////////////////////////
  // Envio do documento
  StaticJsonDocument<1024> Gson;

  Gson["Port"] = pos_nome;
  Gson["Value"] = high;  //enviamos em int para ocupar o mm espaço, isto é importante para o unity conseguir seguementar os pedidos na stream

  // Serialize the JSON document to a string
  String GsonString;
  serializeJson(Gson, GsonString);

  // Print the JSON string to the Serial Monitor
  clientUnity.print(GsonString);
}

///////////////////////////////////////////////
//Função que atualiza os portos locais
void Atualiza_Portos_Locais(char porto, int portIndex, int high) {
  ///////////////////////////////////////////////
  //Colocar a nova informação nos bits
  switch (porto) {
    case 'I':
      porto_I[0][portIndex] = high;
      break;
    case 'Q':
      porto_Q[1][portIndex] = high;
      break;
  }
}

///////////////////////////////////////////////
//Funções de movimento
void transporte(bool left) {  //1 ->LEFT ; 0->RIGHT
  ///////////////////////////////////////////////
  //Rotina implementada
  if (left) {
    while (sensor(4)) {}
    atuador(7, 1);
    while (sensor(3)) {}
    atuador(7, 0);
  } else {
    while (sensor(3)) {}
    atuador(6, 1);
    while (sensor(4)) {}
    atuador(6, 0);
  }
  ///////////////////////////////////////////////
}

void punch(bool down) {  //1 ->DOWN ; 0->UP
  ///////////////////////////////////////////////
  //Rotina implementada
  if (down) {
    atuador(4, 1);
    while (!sensor(1)) {}
    atuador(4, 0);
  } else {
    atuador(5, 1);
    while (!sensor(2)) {}
    atuador(5, 0);
  }
  ///////////////////////////////////////////////
}

void tarefa() {
  ///////////////////////////////////////////////
  //Rotina implementada
  transporte(1);
  punch(1);
  delay(1400);
  punch(0);
  transporte(0);
  ///////////////////////////////////////////////
}

///////////////////////////////////////////////
//Tasks
void TaskTarefa(void *pvParameters) {
  Serial.println();
  for (;;)
  {
    Serial.print(".");
    ///////////////////////////////////////////////
    //Rotina implementada
    tarefa();

    vTaskDelay(1);
  }
}

void TaskEnvio(void *pvParameters) {
  data_a_enviar data;
  for (;;) {
    ///////////////////////////////////////////////
    //Envio do pedido para o unity
    xQueueReceive(envio, &data, portMAX_DELAY);  //Queue utilizada para conseguirmos acumular pedidos de atualização
    if (digitalShadowON) {  //Precisamos deste if para assegurar que após o unity se ter desligado não são enviados mais nenhuns updates de sensor
      Atualiza_Portos_Digital(data.pos_nome, data.high);
    }
    vTaskDelay(1);
  }
}

void TaskSensores(void *pvParameters) {

  data_a_enviar data;
  data.pos_nome[0] = 'I';
  data.pos_nome[2] = '_';

  for (;;) {
    if (digitalShadowON) {

      ///////////////////////////////////////////////
      //Sensores no porto I0
      data.pos_nome[1] = '0';

      ///////////////////////////////////////////////
      //I0_1
      saida = sensor( 1);
      if (porto_I[0][1] != saida) {

        Atualiza_Portos_Locais('I', 1, saida);

        data.high = saida;
        data.pos_nome[3] = '1';

        xQueueSend(envio, &data, portMAX_DELAY);
      }

      ///////////////////////////////////////////////
      //I0_2
      saida = sensor( 2);
      if (porto_I[0][2] != saida) {

        Atualiza_Portos_Locais('I', 2, saida);

        data.high = saida;
        data.pos_nome[3] = '2';

        xQueueSend(envio, &data, portMAX_DELAY);
      }

      ///////////////////////////////////////////////
      //I0_3
      saida = sensor( 3);
      if (porto_I[0][3] != saida) {

        Atualiza_Portos_Locais('I', 3, saida);

        data.high = saida;
        data.pos_nome[3] = '3';

        xQueueSend(envio, &data, portMAX_DELAY);
      }

      ///////////////////////////////////////////////
      //I0_4
      saida = sensor( 4);
      if (porto_I[0][4] != saida) {

        Atualiza_Portos_Locais('I', 4, saida);

        data.high = saida;
        data.pos_nome[3] = '4';

        xQueueSend(envio, &data, portMAX_DELAY);
      }
    }
    vTaskDelay(1);
  }
}

void TaskDigitalshadow(void *pvParameters) {
  for (;;) {
    ///////////////////////////////////////////////
    //Conecção com Unity
    if (digitalShadowON) {
      if (!clientUnity.connected()) {
        Serial.println("\nDigital Disconnected");

        //Quando desconectar avisar o programa que o digitalshadow está desligado
        digitalShadowON = false;
      }
    } else {
      clientUnity = serverUnity.accept();
      if (clientUnity) {
        Serial.println("\nDigital Connected");
        //Resetar informação local
        for (int i = 0; i < 1; i++) {
          for (int j = 0; j < 5; j++) {
            porto_I[i][j] = -1;
          }
        }
        //Avisar o programa que o digitalshadow está ligado
        digitalShadowON = true;
      }
    }
    vTaskDelay(1);
  }
}

///////////////////////////////////////////////
//Setup
void setup() {

  ///////////////////////////////////////////////
  //Pin Declaration
  inicializar_portos();

  ///////////////////////////////////////////////
  // Open serial communications and wait for port to open:
  Serial.begin(9600);
  Serial.println("Kit Tripa mas bem feito");

  ///////////////////////////////////////////////
  // start the Ethernet
  Ethernet.begin(mac, ip);

  //////////////////////////////////////////////
  // Check for Ethernet hardware present
  if (Ethernet.hardwareStatus() == EthernetNoHardware) {
    Serial.println("Ethernet shield was not found.  Sorry, can't run without hardware. :(");
    while (true) {
      delay(1);  // do nothing, no point running without Ethernet hardware
    }
  }
  if (Ethernet.linkStatus() == LinkOFF) {
    Serial.println("Ethernet cable is not connected.");
  }

  ///////////////////////////////////////////////
  //Começar os 2 servidores
  Serial.println("Starting Server");
  serverUnity.begin();
  Serial.print("Server at ");
  Serial.println(Ethernet.localIP());

  ///////////////////////////////////////////////
  //Inicializar Queue
  envio = xQueueCreate(30, sizeof(data_a_enviar));

  ///////////////////////////////////////////////
  //Task Creation
  xTaskCreate(TaskTarefa,  // Task function
              "Tarefa",    // Task name for humans
              200,
              NULL,  // Task parameter
              1,     // Task priority
              NULL);

  xTaskCreate(TaskDigitalshadow,  // Task function
              "DigitalShadow",    // Task name for humans
              256,
              NULL,  // Task parameter
              1,     // Task priority
              NULL);

  xTaskCreate(TaskSensores,  // Task function
              "Sensores",    // Task name for humans
              512,
              NULL,  // Task parameter
              1,     // Task priority
              NULL);

  xTaskCreate(TaskEnvio,  // Task function
              "Envio",    // Task name for humans
              512,
              NULL,  // Task parameter
              1,     // Task priority
              NULL);

  Serial.println("It is working, will start...");
}

///////////////////////////////////////////////
//Loop
void loop() {}