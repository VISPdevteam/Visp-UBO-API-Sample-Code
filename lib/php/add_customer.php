<?php

// Sample code on how to add new customer and adding a package with package id 38915

require 'functions.php';

$data['body'] = '{"query":"mutation addCustomer($input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput, $input_package_id: Int) '
        . '{\n  addCustomer(input_customer: $input_customer, input_package_id: $input_package_id, input_customer_details: $input_customer_details) '
        . '{\n    customer_id\n    username\n}}",'
        . '"operationName":"addCustomer",'
        . '"variables":'
            . '{"input_customer":{"username":"jkazama26","password":"kazama123!","first_name":"Jin","last_name":"Kazama",'
            . '"emails":{"email_address":"jinkazama@gmail.com","description":"Personal"}},'
            . '"input_customer_details":{"bill_first_name":"Jin","bill_last_name":"Kazama","main_address1":"123 Street","bill_address1":"123 Street","main_address2":"",'
            . '"bill_address2":"","main_city":"New York","bill_city":"New York","main_state":"NY","bill_state":"NY","main_zip":"10001","bill_zip":"10001",'
            . '"main_phone1":"1234567890","bill_phone1":"1234567890","main_email":"jinkazama@gmail.com","bill_email":"jinkazama@gmail.com","bill_method":"Cash"},'
            . '"input_package_id":38915}}';

$data['request_type'] = 'POST';
$data = getUBOApi($data);
$data = json_decode($data);

/* 
 * returns data object
  
  {
   "data" : 
      "addCustomer": {
         "customer_id": 123456,
         "username": "jkazama22"
       }
  }
  
 * 
 * 
 */