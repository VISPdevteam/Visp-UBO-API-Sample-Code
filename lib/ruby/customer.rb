require_relative 'functions'

token = get_token() #retrieve a token

query = {
    "operationName": "customer",
    "query": 'query customer($customer_id:Int!){
     customer(customer_id:$customer_id){
        customer_id
        username
        first_name
        last_name
      }
      }',
    "variables": {
        "customer_id": 1432780
    }
};

response = send_request(token, query)
#extract data
data = response["data"]["customer"]
puts data["customer_id"]
puts data["username"]
