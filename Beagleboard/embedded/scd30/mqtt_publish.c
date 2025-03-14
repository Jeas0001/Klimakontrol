#include <stdio.h>
#include <mosquitto.h>
#include <string.h>
#include <stdlib.h>
#include <time.h>
#include "sensor_service.h"
#include "sensirion_i2c_hal.h" // Nødvendig


#define SUCCES 0
void on_connect1(struct mosquitto *mosq, void *obj, int result)
{
    int rc = MOSQ_ERR_SUCCESS;

    if(result == SUCCES){
        mosquitto_subscribe(mosq, NULL, "user/jeppe/sensor", 0);
    }else{
        fprintf(stderr, "%s\n", mosquitto_connack_string(result));
    }
}

#define sensirion_hal_sleep_us sensirion_i2c_hal_sleep_usec
int main() {

    int version[3];
    struct mosquitto *mosq1;
    char sensor_data[100];
    char temp[10];

    mosquitto_lib_init();
    mosquitto_lib_version(&version[0],&version[1],&version[2]);
    printf("Mosquitto library version. %i.%i.%i\n", version[0], version[1], version[2]);


    mosq1 = mosquitto_new(NULL, true, NULL);
    mosquitto_connect_callback_set(mosq1, on_connect1);
    mosquitto_connect(mosq1, "93.166.84.21", 1883, 60);

    sensor_init();
    while(1) {
        for(size_t i = 0; i < sizeof sensor_data; ++i){ sensor_data[i] = 0; }
        for(size_t i = 0; i < sizeof temp; ++i){ temp[i] = 0; }
        
        sprintf(temp, "%d", sensor_read_co2());
        strcat(sensor_data, "co2: ");
        strcat(sensor_data, temp);
        sprintf(temp, "%.2lf", sensor_read_temperature());
        strcat(sensor_data, " temperature: ");
        strcat(sensor_data, temp);
        sprintf(temp, "%.2lf", sensor_read_humidity());
        strcat(sensor_data, " humidity: ");
        strcat(sensor_data, temp);
        time_t unix_time = time(NULL);
        sprintf(temp, "%d", (int)unix_time);
        strcat(sensor_data, " time: ");
        strcat(sensor_data, temp);
        strcat(sensor_data, " identifier: XKPWhrM7ph");

        mosquitto_publish(mosq1, NULL, "user/jeppe/sensor", strlen(sensor_data), &sensor_data, 0, true);
        
        sensirion_hal_sleep_us(1500000);
    }
    mosquitto_destroy(mosq1);

    mosquitto_lib_cleanup();
}