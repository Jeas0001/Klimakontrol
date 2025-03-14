#include "scd30_i2c.h" // Nødvendig
#include "sensirion_common.h" // Nødvendig
#include "sensirion_i2c_hal.h" // Nødvendig
#include <stdio.h>  // printf
#include <string.h>
#include <time.h>

#define sensirion_hal_sleep_us sensirion_i2c_hal_sleep_usec

void sensor_init() {
    int16_t error = NO_ERROR;
    sensirion_i2c_hal_init(); // Nødvendig
    init_driver(SCD30_I2C_ADDR_61); // Nødvendig

    scd30_stop_periodic_measurement();
    scd30_soft_reset();
    sensirion_hal_sleep_us(2000000);

    error = scd30_start_periodic_measurement(0); // Nødvendig
    if (error != NO_ERROR) { // Nødvendig
        printf("error executing start_periodic_measurement(): %i\n", error);
    }
}

int sensor_read_co2() {
    int16_t error = NO_ERROR;
    float co2_level = 0.0;
    float temperature = 0.0;
    float humidity = 0.0;
    
    sensirion_hal_sleep_us(1500000);
    
    error = scd30_blocking_read_measurement_data(&co2_level, &temperature, &humidity);

    if(co2_level == 0) {
        return -1;
    }
    return co2_level;
}

float sensor_read_humidity() {
    int16_t error = NO_ERROR;
    float co2_level = 0.0;
    float temperature = 0.0;
    float humidity = 0.0;
    
    sensirion_hal_sleep_us(1500000);
    
    error = scd30_blocking_read_measurement_data(&co2_level, &temperature, &humidity);

    return humidity;
}

float sensor_read_temperature() {
    int16_t error = NO_ERROR;
    float co2_level = 0.0;
    float temperature = 0.0;
    float humidity = 0.0;
    
    sensirion_hal_sleep_us(1500000);
    
    error = scd30_blocking_read_measurement_data(&co2_level, &temperature, &humidity);

    return temperature;
}

char sensor_data[100];

char* sensor_read() {
    int16_t error = NO_ERROR;
    float co2_level = 0.0;
    float temperature = 0.0;
    float humidity = 0.0;
    char temp[10];
    
    sensirion_hal_sleep_us(1500000);
    
    error = scd30_blocking_read_measurement_data(&co2_level, &temperature, &humidity);

    if(co2_level == 0) {
        return "-1";
    }

    sprintf(temp, "%.2lf", co2_level);
    strcat(sensor_data, "co2: ");
    strcat(sensor_data, temp);
    sprintf(temp, "%.2lf", temperature);
    strcat(sensor_data, "temperature: ");
    strcat(sensor_data, temp);
    sprintf(temp, "%.2lf", humidity);
    strcat(sensor_data, "humidity: ");
    strcat(sensor_data, temp);
    time_t unix_time = time(NULL);
    sprintf(temp, "%d", (int)unix_time);
    strcat(sensor_data, "time: ");
    strcat(sensor_data, temp);

    return sensor_data;
}
