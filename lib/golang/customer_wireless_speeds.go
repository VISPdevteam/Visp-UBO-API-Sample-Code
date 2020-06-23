package main

import (
  "fmt"
)

func main(){
  token := getToken();

  operationName := "customer"
  query := `query customer($customer_id:Int!){\n customer(customer_id:$customer_id){\n package_instances {\n package_id\n package_name\n service_instances{\n service_details {\n ... on ServiceTypeWifi{\n service_number\n username\n up_speed\n down_speed\n up_speed_unit\n down_speed_unit\n}\n}\n}\n}\n}\n}`
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
    return
  }
  customer := response["customer"].(map[string]interface{})
  //output response
  packageInstances := customer["package_instances"]
  for _ , packageInstance := range packageInstances.([]interface {}) {
    packageInstance := packageInstance.(map[string]interface{})
    serviceInstances := packageInstance["service_instances"]
    for _, serviceInstance := range serviceInstances.([]interface {}){
      serviceInstance := serviceInstance.(map[string]interface{})
      serviceDetails := serviceInstance["service_details"]
      if serviceDetails != nil {
        serviceDetails := serviceDetails.(map[string]interface{})
        username := serviceDetails["username"].(string)
        downSpeed := fmt.Sprintf("%g",serviceDetails["down_speed"].(float64)) //convert to string immediately
        downSpeedUnit := serviceDetails["down_speed_unit"].(string)
        upSpeed := fmt.Sprintf("%g",serviceDetails["up_speed"].(float64))
        upSpeedUnit := serviceDetails["up_speed_unit"].(string)

        fmt.Println("Service Username: "+username)
        fmt.Println("Down Speed: "+downSpeed+" "+downSpeedUnit)
        fmt.Println("Up Speed: "+upSpeed+" "+upSpeedUnit)
      }
    }
  }
}
