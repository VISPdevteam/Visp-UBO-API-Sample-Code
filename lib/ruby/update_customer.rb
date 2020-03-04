require_relative 'functions'

token = get_token() #retrieve a token

customer_id = 1432780
query = {
    "operationName": "updateCustomer",
    "query": "mutation updateCustomer($customer_id: Int!, $input_customer: CustomerInput, $input_customer_details: CustomerDetailsInput) {
      updateCustomer(customer_id: $customer_id, input_customer: $input_customer, input_customer_details: $input_customer_details){
            customer_id
            username
            first_name
            last_name
        }
      }",
    "variables": {
          "customer_id": customer_id,
          "input_customer": {
              "username": "jkazama26",
              "password": "kazama123!",
              "first_name": "Jin",
              "last_name": "Kazama",
              "emails": {
                  "email_address": "jinkazama@example.com",
                  "description": "Example Email"
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
              "bill_phone1": "1234567890"
          }
      }
}

response = send_request(token, query)
#extract data
data = response["data"]["updateCustomer"]
puts data["customer_id"]
puts data["username"]
