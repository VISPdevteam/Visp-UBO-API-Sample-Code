package main

import (
  "fmt"
)

func main(){
  token := getToken();

  cust_id := "1432780"
  operationName := "updateCustomer"
  query := `mutation updateCustomer($customer_id: Int!, $input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput) {\nupdateCustomer(customer_id: $customer_id, input_customer: $input_customer, input_customer_details: $input_customer_details){\n customer_id\n username\n}\n}`
  variables := `{"customer_id": `+cust_id+`,"input_customer": {"username": "jkazama26","password": "kazama123!","first_name": "Jin","last_name": "Kazama","emails": {"email_address": "jinkazama@example.com","description": "Example Email"}},"input_customer_details": {"bill_first_name": "Jin","bill_last_name": "Kazama","main_address1": "123 Street","bill_address1": "123 Street","main_address2": "","bill_address2": "","main_city": "New York","bill_city": "New York","main_state": "NY","bill_state": "NY","main_zip": "10001","bill_zip": "10001","main_phone1": "1234567890","bill_phone1": "1234567890"}}`

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
  customer := response["updateCustomer"].(map[string]interface{})
  //output response
  customer_id := fmt.Sprintf("%.0f", customer["customer_id"].(float64))
  fmt.Println("Customer_id: "+customer_id)

  username := customer["username"].(string)
  fmt.Println("Username: "+username)
}
