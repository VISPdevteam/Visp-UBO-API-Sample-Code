import functions

customer_id = 917336
#get a token for our session. Using "Session" from the requests library, the token is stored on the session "object"
functions.get_token()
request = {
    "operationName": "customer",
    "query":"query customer($customer_id:Int!){\n customer(customer_id:$customer_id){\n package_instances {\n package_id\n package_name\n service_instances{\n service_details {\n ... on ServiceTypeWifi{\n service_number\n username\n up_speed\n down_speed\n up_speed_unit\n down_speed_unit\n}\n}\n}\n}\n}\n}",
    "variables": {
        "customer_id": customer_id
    }
};
#send request to API
response = functions.get_ubo_api(request)
#extract data from response
data = response["data"]["customer"]
for package_instance in data["package_instances"]:
    for service_instance in package_instance["service_instances"]:
        wifiService = service_instance["service_details"]
        if(wifiService!={} and wifiService != None):
            print("Service Username: %s" % wifiService["username"])
            print("Down speed: %s %s" % (wifiService["down_speed"],wifiService["down_speed_unit"]))
            print("Up speed: %s %s" % (wifiService["up_speed"],wifiService["up_speed_unit"]))
