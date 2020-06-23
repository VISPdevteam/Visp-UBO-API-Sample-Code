package main

import (
  "fmt"
)

func main(){
  token := getToken();

  operationName := "customer"
  query := `query customer($customer_id:Int!){\n customer(customer_id:$customer_id){\n customer_id\n username\n first_name\n last_name\n}\n}`
  variables := `{"customer_id": 1432780}`

  req := `{"operationName": "`+operationName+`","query":"`+query+`","variables": `+variables+` }`
  data := UBOApiData{
    body: req,
    method: "POST",
    token: token,
  };

  response, err := sendApiRequest(data)
  if (err != nil) {
    fmt.Println(err)
  }
  customer, err := response["customer"].(map[string]interface{})
  //output response
  //convert from float to string with no decimal places, a bit hacky
  customer_id := fmt.Sprintf("%.0f", customer["customer_id"].(float64))
  fmt.Println("Customer_id: "+customer_id)

  username := customer["username"].(string)
  fmt.Println("Username: "+username)
}
