require_relative 'functions'

token = get_token() #retrieve a token

customer_id = 917336
query = {
    "operationName": "customer",
    "query":"query customer($customer_id:Int!){
      customer(customer_id:$customer_id){
        package_instances {
          package_id
          package_name
          service_instances{
            service_details {
              ... on ServiceTypeWifi{
                service_number
                username
                up_speed
                down_speed
                up_speed_unit
                down_speed_unit
              }
            }
          }
        }
      }
    }",
    "variables": {
        "customer_id": customer_id
    }
}

response = send_request(token, query)
#extract data
data = response["data"]["customer"]
(data["package_instances"]).each do |package_instance|
  (package_instance["service_instances"]).each do |service_instance|
    wifiService = service_instance["service_details"]
    if (wifiService != {} && wifiService != nil)
      puts ("Service Username: #{wifiService["username"]}")
      puts ("Down speed: #{wifiService["down_speed"]} #{wifiService["down_speed_unit"]}")
      puts ("Up speed: #{wifiService["up_speed"]} #{wifiService["up_speed_unit"]}")
    end
  end
end
