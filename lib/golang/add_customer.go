package main

import (
  "fmt"
)

func main(){
  token := getToken();

  operationName := "addCustomer"
  query := `mutation addCustomer($input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput, $input_package_id: Int) {\naddCustomer(input_customer: $input_customer, input_package_id: $input_package_id, input_customer_details: $input_customer_details){\n customer_id\n username\n}\n}`
  variables := `{"input_customer":{"username":"jkazama27","password":"kazama123!","first_name":"Jin","last_name":"Kazama","emails":{"email_address":"jinkazama@gmail.com","description":"Personal"}},"input_customer_details":{"bill_first_name":"Jin","bill_last_name":"Kazama","main_address1":"123 Street","bill_address1":"123 Street","main_address2":"","bill_address2":"","main_city":"New York","bill_city":"New York","main_state":"NY","bill_state":"NY","main_zip":"10001","bill_zip":"10001","main_phone1":"1234567890","bill_phone1":"1234567890","main_email":"jinkazama@gmail.com","bill_email":"jinkazama@gmail.com","bill_method":"Cash"},"input_package_id":38915}`

  req := `{"operationName": "`+operationName+`","query":"`+query+`","variables": `+variables+` }`
  data := UBOApiData{
    body: req,
    method: "POST",
    token: token,
  };

  response, err := sendApiRequest(data)
  if (err != nil) {
    fmt.Println(err)
    return;
  }
  customer := response["addCustomer"].(map[string]interface{})
  //output response
  customer_id := fmt.Sprintf("%.0f", customer["customer_id"].(float64))
  fmt.Println("Customer_id: "+customer_id)

  username := customer["username"].(string)
  fmt.Println("Username: "+username)
}
