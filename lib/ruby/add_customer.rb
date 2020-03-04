require_relative 'functions'

token = get_token() #retrieve a token

query = {
    "operationName": "addCustomer",
    "query": "mutation addCustomer($input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput, $input_package_id: Int) {
      addCustomer(input_customer: $input_customer, input_package_id: $input_package_id, input_customer_details: $input_customer_details){
            customer_id
            username
        }
      } ",
    "variables": {
        "input_customer": {
            "username": "jkazama27",
            "password": "kazama123!",
            "first_name": "Jin",
            "last_name": "Kazama",
            "emails": {
                "email_address": "jinkazama@example.com",
                "description": "personal"
            }
        },
        "input_customer_details": {
            "bill_first_name": "Jin",
            "bill_last_name": "Kazama",
            "main_address1": "123 Street",
            "bill_address1": "123 Street",
            "main_address2": "",
            "bill_address2": "",
            "main_city": "New York",
            "bill_city": "New York",
            "main_state": "NY",
            "bill_state": "NY",
            "main_zip": "10001",
            "bill_zip": "10001",
            "main_phone1": "1234567890",
            "bill_phone1": "1234567890",
            "bill_method": "Cash"
        },
        "input_package_id": 38915
    }
}

response = send_request(token, query)
#extract data
data = response["data"]["addCustomer"]
puts data["customer_id"]
puts data["username"]
