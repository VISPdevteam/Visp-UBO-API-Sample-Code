<?php

// Sample code on how to query customer's wireless service

require 'functions.php';

$customerId = 917336;

$data['body'] = '{"query":"{\n  customer(customer_id: '.$customerId.') {\n  package_instances {\n  package_id\n package_name\n  service_instances {\n service_details {\n  ... on ServiceTypeWifi {\n service_number\n username\n up_speed\n down_speed\n up_speed_unit\n down_speed_unit\n}\n}\n}\n}\n}\n}\n","operationName":null,"variables":{}}';
    
$data['request_type'] = 'POST';
$data = getUBOApi($data);
$data = json_decode($data);

foreach ($data->data->customer->package_instances as $package_instance) {
    foreach ($package_instance->service_instances as $service ) {
        if (is_object($service->service_details)) {
            if (property_exists($service->service_details, 'username')) {
                echo "Service Username: " . $service->service_details->username . "<br>";
                echo "Down speed: " . $service->service_details->down_speed . " " . $service->service_details->down_speed_unit . " <br>";
                echo "Up speed: " . $service->service_details->up_speed . " " . $service->service_details->up_speed_unit . " <br><br>";
            }
        }
    }
}

