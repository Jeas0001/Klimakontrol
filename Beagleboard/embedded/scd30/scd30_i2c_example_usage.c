/*
 * THIS FILE IS AUTOMATICALLY GENERATED
 *
 * Generator:    sensirion-driver-generator 0.9.0
 * Product:      scd30
 * Version:      None
 */
/*
 * Copyright (c) 2022, Sensirion AG
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 * * Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 *
 * * Neither the name of Sensirion AG nor the names of its
 *   contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */
#include "scd30_i2c.h" // Nødvendig
#include "sensirion_common.h" // Nødvendig
#include "sensirion_i2c_hal.h" // Nødvendig
#include <stdio.h>  // printf

#define sensirion_hal_sleep_us sensirion_i2c_hal_sleep_usec

int main(void) {
    int16_t error = NO_ERROR;
    sensirion_i2c_hal_init(); // Nødvendig
    init_driver(SCD30_I2C_ADDR_61); // Nødvendig

    // make sure the sensor is in a defined state (soft reset does not stop
    // periodic measurement)
    scd30_stop_periodic_measurement(); // Nødvendig
    scd30_soft_reset(); // Nødvendig
    sensirion_hal_sleep_us(2000000); // Nødvendig
    uint8_t major = 0;
    uint8_t minor = 0;
    error = scd30_read_firmware_version(&major, &minor);
    if (error != NO_ERROR) {
        printf("error executing read_firmware_version(): %i\n", error);
        return error;
    }
    printf("firmware version major: %u minor: %u\n", major, minor);
    error = scd30_start_periodic_measurement(0); // Nødvendig
    if (error != NO_ERROR) { // Nødvendig
        printf("error executing start_periodic_measurement(): %i\n", error);
        return error;
    }
    float co2_concentration = 0.0;
    float temperature = 0.0;
    float humidity = 0.0;
    uint16_t repetition = 0;
    for (repetition = 0; repetition < 1000; repetition++) {
        sensirion_hal_sleep_us(1500000);
        error = scd30_blocking_read_measurement_data(&co2_concentration,
                                                     &temperature, &humidity); // Nødvendig
        if (error != NO_ERROR) {
            printf("error executing blocking_read_measurement_data(): %i\n",
                   error);
            continue;
        }
        printf("co2_concentration: %.2f ", co2_concentration);
        printf("temperature: %.2f ", temperature);
        printf("humidity: %.2f\n", humidity);
    }

    error = scd30_stop_periodic_measurement(); // Nødvendig
    if (error != NO_ERROR) {
        printf("error executing stop_periodic_measurement(): %i\n", error);
        return error;
    }
    return NO_ERROR;
}
